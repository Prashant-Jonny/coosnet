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
//package org.coos.messaging.cldc.transport;

//import org.coos.messaging.*;
//import org.coos.messaging.impl.DefaultMessage;
//import org.coos.messaging.impl.DefaultProcessor;

//import java.io.*;
//import java.util.Vector;
//import javax.microedition.io.Connector;
//import javax.microedition.io.SocketConnection;

#define LOGGING
#region Using
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;
using Microsoft.SPOT;
using System.IO;
using System.Runtime.CompilerServices; // For synchronization


using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Util;
using java.lang; // IRunnable interface compability

#endregion



namespace Org.Coos.Messaging.NETMicro.Transport
{


    public class UDPTransport : DefaultProcessor, ITransport, IService
    {

        const string PROPERTY_HOST = "host";
        const string PROPERTY_PORT = "port";
    /* Should not need to retry connection, since UDP is connection-less
        const string PROPERTY_RETRY = "retry";
        const string PROPERTY_RETRY_TIME = "retryTime"; */
        
        public const Int32 MAX_UDP_SIZE = 2 << 16; // Max length for 1 udp packet, length is in 2 bytes



        // Server adr.
        protected string hostName;
        protected int hostPort = 0;
/*        static protected int retryTime = 10000;
        protected bool retry = false;

        /// <summary>
        /// Determines timeout for giving up socket connect attempt to router, pluginchannel also has a
        /// separate timeout for connecting the channel to transport
        /// </summary>
        protected Int32 GIVEUP_CONNECT_RETRY_TIMEOUT = 11 * retryTime; // Allows 10 retries

        // C# inner class can only access static members of outer class, hence socket is static
        */
        /* static*/

        protected Socket udpSocket;
        protected IPEndPoint udpServerEndpoint;
        protected IPEndPoint udpClientEndpoint;
       

        /// <summary>
        ///  Reader reads incoming/inbound network msg, auto starts as thread
        /// </summary>
        public UDPReader reader;

        /// <summary>
        ///  Writer write outbound network msg., auto starts as a thread
        /// </summary>
        public UDPWriter writer;

        /// <summary>
        /// The mailbox contains a sorted list of messages based on priority, most urgent messages are sent first
        /// </summary>
        protected ArrayList mailbox = new ArrayList();

        /// <summary>
        /// Next processor that takes further action on message, the reader thread deserialze incoming data
        /// (in DefaultMessage(networkStream)) into a message that is forwarded to the transportProcessor
        /// </summary>
         public IProcessor transportProcessor;
#if LOGGING
        public static Org.Coos.Messaging.Util.ILog logger = LogFactory.getLog(typeof(UDPTransport).FullName);
#endif

        protected bool running = false;

        static protected IChannel channel;




        /// <summary>
        /// This event is used to synchronize writer/reader-thread
        ///     - it is raised when a new message is ready  for transmission across network (mailbox), so that writer
        ///       thread that is waiting for a message is resumed
        ///     - follows Java wait/notify-paradigm
        ///           - WaitOne() = wait()
        ///           - Set() = notify()
        /// </summary>
        static protected AutoResetEvent msgInMailBoxEvent = new AutoResetEvent(false);

       

        public static Thread threadConnectSocket;

        //http://msdn.microsoft.com/en-us/library/system.net.sockets.networkstream.aspx
        static protected Stream networkStream;
        // Network stream allows read/write for different threads (one thread for reading) and one thread for writing
        // without need for synchronization, so instead of having two networkstreams as in java, only one should be
        // enough for .NET


        public UDPTransport()
        {
        }

        public UDPTransport(string hostIP, int hostPort)
        {
            this.hostName = hostIP;
            this.hostPort = hostPort;
        }

        public UDPTransport(Socket socket, string hostIP, int hostPort)
        {
            this.udpSocket = socket;
            this.hostName = hostIP;
            this.hostPort = hostPort;
        }
        
        
        public IProcessor getTransportProcessor()
        {
            return transportProcessor;
        }

        /// <summary>
        /// Most likely the transportProcessor will be the channel processor which takes care of connection handshanke
        /// After connect in pluginchannel the next processor to process msg. will be inlink
        /// </summary>
        /// <param name="transportProcessor"></param>
        public void setChainedProcessor(IProcessor newTransportProcessor)
        {
            transportProcessor = newTransportProcessor;
        }

        public UDPReader getReader()
        {
            return reader;
        }

        public UDPWriter getWriter()
        {
            return writer;
        }

        /// <summary>
        /// The UDPTransport processor puts msg in a mailbox
        ///     - the message is inserted into the mailbox in prioritized order
        ///     - a new message arrival from channel is notified to writer thread with aid of autoresetevent-mechanism in .NET, that forwards message to router
        /// </summary>
        /// <param name="msg"></param>
        public override void processMessage(IMessage msg)
        {

            if (!running)
                throw new ProcessorException("Transport :" + name + " is stopped");

            string newMsgpriStr = msg.getHeader(IMessagePrimitives.PRIORITY);
            if (newMsgpriStr != null)
            {
                int newMsgpri = int.Parse(newMsgpriStr);
                int idx = 0;

                for (int i = 0; i < mailbox.Count; i++)
                {
                    IMessage message = (IMessage)mailbox[i];
                    string pr = message.getHeader(IMessagePrimitives.PRIORITY);
                    if (pr != null)
                    {
                        int currentMsgPri = int.Parse(pr);
                        if (newMsgpri < currentMsgPri)
                        {
                            mailbox.Insert(idx, msg);

                            msgInMailBoxEvent.Set(); // Signal to writer-thread that new message has arrived
                            break;
                        }
                    }
                    idx++;
                }
            }
            else
            {
                mailbox.Add(msg);

                // Signal writer thread - that transport messages over the network
                msgInMailBoxEvent.Set();
            }
        }




         //Info: http://www.informit.com/guides/content.aspx?g=dotnet&seqNum=599
        // Accessed: 14 nov 2010
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void start()
        {

           
                // Well if we are running, why start, just return
                if (running)
                {
                    UDPTransport.logger.debug("UDP transport got request to start, but is already running");
                    return;
                }


                running = true;

                // Socket exsists
                if (udpSocket != null)
                {
                    if (reader == null)
                        reader = new UDPReader(udpSocket, new IPEndPoint(IPAddress.Parse(hostName),hostPort), channel, transportProcessor);

                    if (writer == null)
                        writer = new UDPWriter(udpSocket, new IPEndPoint(IPAddress.Parse(hostName), hostPort), channel, mailbox, msgInMailBoxEvent);

                    return;

                }

                // socket does not exist, must be established before starting reader and
                // writer
                if (hostName == null)
                {
                    hostName = (string)properties[PROPERTY_HOST];
                }

                if (hostPort == 0 && properties[PROPERTY_PORT] != null)
                {
                    hostPort = int.Parse((string)properties[PROPERTY_PORT]);
                }

                logger.info("Establishing UDP transport to " + hostName + ":" + hostPort);

              
            // UDP writer write data to server using this socket
              udpSocket = createClient(hostName, hostPort);
              udpServerEndpoint = new IPEndPoint(IPAddress.Parse(hostName),hostPort);


                // UDP reader read data from this socket
                Socket pluginSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                udpClientEndpoint = new IPEndPoint(IPAddress.Loopback, 16000);   
                pluginSocket.Bind(udpClientEndpoint);

                reader = new UDPReader(pluginSocket, udpClientEndpoint , channel, transportProcessor);
                writer = new UDPWriter(udpSocket, udpServerEndpoint, channel, mailbox, msgInMailBoxEvent);
          
        }

        /// <summary>
        /// Create client socket to server
        /// </summary>
        /// <param name="serverHost"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public Socket createClient(String serverHost, int port)
        {
            
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPHostEntry hostEntry = Dns.GetHostEntry(serverHost);

            // Pick first adr. that is not null
            IPAddress ipAdr = null;
            for (int i = 0; i < hostEntry.AddressList.Length; i++)
            {
                ipAdr = hostEntry.AddressList[i];
                if (ipAdr != null)
                    break;
            }

            IPEndPoint ipEndp = new IPEndPoint(ipAdr, port);


            //udpSocket.Bind(ipEndp);
            //udpSocket.Connect(ipEndp);
            
            return udpSocket;
        }

       
       
        //Info accessed 4 nov 2010: http://stackoverflow.com/questions/541194/c-version-of-javas-synchronized-keyword
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void stop()
        {
           

                // If we are not running, no need to stop
                if (!running)
                {
                    return;
                }


                logger.info("Closing transport: " + hostName + ":" + hostPort);

                running = false;

                // Stop reader
                if (reader != null)
                {
                    reader.stop();
                }

                // Stop writer
                if (writer != null)
                {
                    writer.stop();
                }

                // Close socket - session with server is over now
                if (udpSocket != null)
                {
                    udpSocket.Close();
                }

                udpSocket = null;
            
        }

        public int getQueueSize()
        {
            return mailbox.Count;
        }

        public void setChannel(IChannel newChannel)
        {
            channel = newChannel;
        }


    }


    public class UDPReader : IRunnable
    {
        Thread readerThread;
       
        Socket udpSocket;
        IPEndPoint udpServerEndpoint;

        IChannel channel;

        /// <summary>
        /// Running indicates that UDP reader is trying to read data from socket 
        /// </summary>
        bool running = true;

        private IProcessor transportProcessor;
     


        protected const int MAX_LENGTH = (16 * 1024); // Room for large headers
        protected const int MAX_BODY_LENGTH = (8 * 1024);


        public UDPReader(Socket socket, IPEndPoint ipEndp, IChannel channel,IProcessor transportProcessor)
        {
           
            this.udpSocket = socket;
            this.udpServerEndpoint = ipEndp;

            this.channel = channel;
            this.transportProcessor = transportProcessor;
            
          

            readerThread = new Thread(() => { this.run(); });
            readerThread.Start();
            //logger.info("Reader started on :" +
            //socket.LocalEndPoint.ToString());
            

            
        }

        public virtual void stop()
        {
            

            running = false;

            // readerThread.Abort();

        }

        /// <summary>
        /// Trim off exceeding bytes at end of array, include bytes up to length
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] trimByteArray(byte[] data, int length)
        {
            if (length <= 0)
                return null;

            byte[] buffer = new byte[length];

            for (int bNr = 0; bNr < length; bNr++)
                buffer[bNr] = data[bNr];

           
            return buffer;

        }

        public void receivedMessage(byte[] data, int length)
        {

            try
            {
                DataInputStream din = new DataInputStream(new MemoryStream(trimByteArray(data, length)));
                IMessage msg = null;

                msg = new DefaultMessage(din);
                transportProcessor.processMessage(msg);
            }
            catch (SocketException e)
            {
                UDPTransport.logger.info("Connection closing");
                running = false;
            }
            //catch (EOFException e)
            //{
            //    logger.info("Connection closing EOF");
            //    running = false;
            //}
            catch (Exception e)
            {
                UDPTransport.logger.error("Error in Message deserialization. Aborting", e);
                running = false;
            }
        }


        public virtual void run()
        {
           byte[] buffer = new byte[MAX_LENGTH]; // Reduce size to MAX_LENGTH, was MAX_UDP_SIZE -> might get "out of memory" on small device
            
            udpSocket.ReceiveTimeout = -1;
            

             System.Net.EndPoint serverEndpoint = (EndPoint) udpServerEndpoint;

            
                while (running)
                {
                    try {
                        
                            int receivedBytes = udpSocket.ReceiveFrom(buffer, ref serverEndpoint);

                            receivedMessage(buffer, receivedBytes);
                        }
                    catch (SocketException e)
                       {
                            Debug.Print("UDPReader: Socket exception, error code: "+e.ErrorCode.ToString());
                            running = false;
                        }
                  }
                

                if (channel != null)
                {
                    channel.disconnect();
                }
  
                      
         }
 }

    public class UDPWriter : IRunnable
    {
        Thread writerThread;
        
        bool running = true;
        IChannel channel;
        ArrayList mailbox;
        AutoResetEvent msgInMailBoxEvent;
        Socket udpSocket;
        IPEndPoint udpEndpoint;

        public UDPWriter(Socket socket, IPEndPoint udpEndp,IChannel channelWriter, ArrayList mailbox, AutoResetEvent msgInMailBoxEvent)
        {
            this.channel = channelWriter;
            this.mailbox = mailbox;
            this.msgInMailBoxEvent = msgInMailBoxEvent;
            this.udpSocket = socket;
            this.udpEndpoint = udpEndp;

            writerThread = new Thread(this.run);
            writerThread.Start();
            //     logger.info("Writer started on :" +
            //     socket.LocalEndPoint.ToString());
        }

        public virtual void stop()
        {
            running = false;
            // JAVA writerThread.interrupt();
            // writerThread.Abort();
        }

        public virtual void run()
        {
            try
            {


                while (running)
                {
                    #region Wait for message in mailbox
                    // We will wait for message to arrive in mailbox
                    if (mailbox.Count == 0)
                    {
                        try
                        {
                            msgInMailBoxEvent.WaitOne();

                        }
                        catch (System.Threading.ThreadAbortException e)
                        {
                            if (!running)
                                return;
                        }
                    }
                    #endregion
                    // Take first message from mailbox, serialize it and send it 
                    else
                    {


                        #region Write message to network via socket
                        IMessage msg = (IMessage)mailbox[0];
                        mailbox.Remove(msg);
                          
                        Debug.Print("Writer : " + msg.getName());
                        
                        try
                        {
                            udpSocket.SendTo(msg.serialize(),this.udpEndpoint);
                        }
                        catch (SocketException e)
                        {
                            running = false;
                        }
                        catch (System.Exception e)
                        {
                            //e.printStackTrace();
                            // logger.fatal("Error in Message writing. Aborting");
                            running = false;
                        }

                        #endregion
                    }
                }


                // Disconnect channel 

                if (channel != null)
                {
                    channel.disconnect();
                }

            }
            catch (System.Exception e)
            {
                running = false;
            }
        }
    }
}
