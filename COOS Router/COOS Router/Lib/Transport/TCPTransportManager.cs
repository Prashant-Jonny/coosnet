/**
 * COOS - Connected Objects Operating System (www.connectedobjects.org).
 *
 * Copyright (C) 2009 Telenor ASA and Tellu AS. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * You may also contact one of the following for additional information:
 * Telenor ASA, Snaroyveien 30, N-1331 Fornebu, Norway (www.telenor.no)
 * Tellu AS, Hagalokkveien 13, N-1383 Asker, Norway (www.tellu.no)
 */

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging.Transport
{
//package org.coos.messaging.transport;

//import java.net.ServerSocket;
//import java.net.Socket;
//import java.net.SocketException;
//import java.net.SocketTimeoutException;

//import java.util.Collections;
//import java.util.HashSet;
//import java.util.Set;

//import org.coos.messaging.LinkManager;
//import org.coos.messaging.Transport;
//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;


/**
 * A TCP transport manager
 *
 * @author Knut Eilif Husa, Tellu AS
 */
public class TCPTransportManager : DefaultChannelServer {

    static readonly String PROPERTY_LISTEN_PORT = "port";
    static readonly String PROPTERY_HOST = "host";

    private static readonly ILog logger = LogFactory.getLog(typeof(TCPTransportManager).FullName);

    private string host = "127.0.0.1";
    private IPAddress firstIPv4Adr;
    private int listenPort = 15666;

    private Socket serverSocket;
    private ConcurrentBag<ITransport> transports = new ConcurrentBag<ITransport>();
    private bool running;
    
    //Thread threadTCPManager;

    bool stopping = false;

    public TCPTransportManager() {
    }

    public TCPTransportManager(int listenPort, ILinkManager linkManager) {
        this.listenPort = listenPort;
        setLinkManager(linkManager);
    }

    public int getListenPort() {
        return listenPort;
    }

    public override void start() /*throws Exception */ {

        if (properties.ContainsKey(PROPERTY_LISTEN_PORT)) 
          listenPort = int.Parse(properties[PROPERTY_LISTEN_PORT]);


        if (properties.ContainsKey(PROPTERY_HOST))
            host = properties[PROPTERY_HOST];
        else
        {
            host = "127.0.0.1";
            logger.info("Host property not defined; using " + host);
        }



        serverSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
       
        // Find host addressess
        IPAddress[] hostAddresses = Dns.GetHostAddresses(host); // Will also return IPV6 adr.
        if (hostAddresses.Length == 0)
        {
            logger.error("DNS found no valid host addresses for : " + host);
            return;
        }

        //logger.info("Possible IP adresses for server");
        //foreach (IPAddress ipAdr in hostAddresses)
        //    if (ipAdr.AddressFamily == AddressFamily.InterNetworkV6)
        //        logger.info("IPv6 : " + ipAdr.ToString());
        //    else if (ipAdr.AddressFamily == AddressFamily.InterNetwork)
        //        logger.info("IPv4 : " + ipAdr.ToString());
        //    else
        //        logger.info("Address : " + ipAdr.ToString() + " address family : " + ipAdr.AddressFamily.ToString());
                

        foreach (IPAddress ipAdr in hostAddresses)
            if (ipAdr.AddressFamily == AddressFamily.InterNetwork)
            {

                firstIPv4Adr = ipAdr;
                break;
            }

        //logger.info("Using the first IPv4 adr. by default");

         IPEndPoint localEndPoint = new IPEndPoint(firstIPv4Adr, listenPort);
        
        
        serverSocket.ReceiveTimeout = 500;
        serverSocket.SendTimeout = 500;
        // JAVA serverSocket.setSoTimeout(500);

        serverSocket.Bind(localEndPoint);
        serverSocket.Listen(Int32.MaxValue); // Max. avail backlog og queued requests
        
        
        logger.info("Listening on "+firstIPv4Adr.ToString()+":" + listenPort);
        

        running = true;

       var taskTCPManager = Task.Factory.StartNew(() =>  {

                        try {

                            while (running) {

                                try {
                                    Socket socket = serverSocket.Accept();
                                    TCPTransport transport = new TCPTransport(socket, this);
                                    initializeChannel(transport);
                                    transport.start();

                                   
                                        transports.Add(transport);
                                    

                                //} catch ( SocketTimeoutException e) {

                                //    if (!running) {

                                //        if ((serverSocket != null) && !serverSocket.isClosed()) {
                                //            serverSocket.close();
                                //        }

                                //        return;
                                //    }
                                } catch (SocketException e) {

                                    //if (e.g.equals("socket closed")) {
                                    //    logger.info("Server connection closing");
                                        running = false;
                                    }
                                }
                            }
                         catch (Exception e1) {
                            logger.error("TCPTransportManager. Exception ignored", e1);
                        }
                    }
                );

      // threadTCPManager.Start();
    }

    public override void stop() /*throws Exception */ {

        if (running) {
            running = false;
            serverSocket.Close();
            //thread.Abort();
            stopTransports();
        }
    }

    public void stopTransports() /* throws Exception */ {

        
            stopping = true;

            foreach (ITransport transport in transports) 
                transport.stop();
            

            transports = new ConcurrentBag<ITransport>(); // Let GC "clear" collection

            stopping = false;
        
    }

    protected void disconnected(ITransport transport) {

        if (stopping)
            return;

        bool result = transports.TryTake(out transport);
        
        
    }

}
}