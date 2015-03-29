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

using Microsoft.SPOT.Net.Security;
using System.IO;
using System;
using System.Text;

using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging.NETMicro.Transport
{

//package org.coos.messaging.transport;

//import java.io.FileInputStream;
//import java.net.InetSocketAddress;
//import java.net.Socket;
//import java.security.KeyManagementException;
//import java.security.KeyStore;
//import java.security.KeyStoreException;
//import java.security.NoSuchAlgorithmException;
//import java.security.SecureRandom;
//import java.security.cert.CertificateException;

//import javax.net.ssl.KeyManager;
//import javax.net.ssl.SSLContext;
//import javax.net.ssl.SSLSocket;
//import javax.net.ssl.SSLSocketFactory;
//import javax.net.ssl.TrustManager;
//import javax.net.ssl.TrustManagerFactory;

//import org.coos.messaging.Service;
//import org.coos.messaging.Transport;
//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;


/**
 * @author sverre
 * SecureTCPTransport is a transport for COOS that implements the
 *         secure tcp.
 *
 *         This class is implemented to provide secure tcp communication in the
 *         COOS framework.
 * */


public class SecureTCPTransport : TCPTransport,ITransport, IService {
    private static  ILog logger = LogFactory.getLog(typeof(SecureTCPTransport).FullName);

    string keyStore = "serverkeys";
    char[] keystoprepass = "hellothere".ToCharArray();
    char[] keypassword = "hiagain".ToCharArray();

    SslStream sslStream;
    
    public SecureTCPTransport() {
    }

    /**
     *
     * Constructor - creates a new instance of SecureTCPTransport
     *
     * @param hostIp
     *            - <code>String</code> indicating the ip of the host/server
     * @param hostPort
     *            - <code>Integer</code> indicating the host/server port
     *
     * */
    public SecureTCPTransport(string hostIP, int hostPort) : base(hostIP,hostPort) {
       
    }

    /**
     * Constructor used by the server to create a new transport on response to a
     * TCP request
     *
     * @param socket
     *            - <code>SSLSocket</code> to communicate with the server
     * */
    //SecureTCPTransport(SSLSocket socket, SecureTCPTransportManager tm) {
    //    super(socket, tm);
    //}

    /**
     * creates a SSLSocket
     *
     * @param port
     *            - <code>Integer</code> indicating the port of the socket
     * @param serverHost
     *            - <code>String</code> indicating the ip of the server/host
     *
     * @return a <code>SSLSocket</code>
     * @throws KeyStoreException
     * @throws CertificateException
     * @throws NoSuchAlgorithmException
     * @throws KeyManagementException
     * */
    //public Socket createClient(string serverHost, int port)  {
    //}

     protected override void startReaderWriter()
        { 

         
         // Authenticate server before starting reader and writer
            try
            {
                networkStream = authenticateClient();

                reader = new Reader(socket, networkStream, channel,  retry, retryTime);
                writer = new Writer(socket, networkStream, channel, mailbox, msgInMailBoxEvent);

            }
            catch (Exception e)
            {
                logger.error("Did not start reader/writer due to authentication failure with router " + e.Message);
            }
        }

     private SslStream authenticateClient()
     {
         
         // Device: Could use CertificateStore
         
         sslStream = new SslStream(socket);


         byte[] certificate = Org.Coos.Messaging.Resource1.GetBytes(Resource1.BinaryResources.COOSFakeCA);

         
         X509Certificate rootCACert = new X509Certificate(certificate, "NetMF");
         // sslStream.AuthenticateAsClient("COOS fake router", SslProtocols.TLSv1);

         try
         {
             sslStream.AuthenticateAsClient("COOS fake router", null, new X509Certificate[] { rootCACert }, SslVerification.CertificateRequired, SslProtocols.TLSv1);
         }
         catch (Exception e)
         {
             logger.error("Could not authenticate client with router " + e.Message);
             throw e; // let caller know the exception
            
         }

         logger.info("Runs SSL without client certificate, with server certificate check");

         return sslStream;
     }
#if JAVA

        // set the context to SSL 3
        SSLContext sslcontext = SSLContext.getInstance("TLSv1");

        // use lines bellow to override default truststore
        if (properties.contains("truststore") && properties.contains("trustpw")) {

            KeyStore ks = KeyStore.getInstance("JKS");
            ks.load(new FileInputStream((String) properties.get("truststore")), ((String) properties.get("trustpw")).toCharArray());

            TrustManagerFactory tmf = TrustManagerFactory.getInstance("SunX509");
            tmf.init(ks);

            sslcontext.init(null, tmf.getTrustManagers(), null);
        } else if (properties.containsKey("nosslcheck") && properties.get("nosslcheck").equals("true")) {
            TrustManager[] tm = new TrustManager[] { new NaiveTrustManager() };
            sslcontext.init(new KeyManager[0], tm, new SecureRandom());
        } else { // SSL without client certificate, with server certificate check
            sslcontext.init(null, null, null);
        }

        Socket socket = new Socket();
        socket.connect(new InetSocketAddress(hostName, hostPort), CONNECT_TIMEOUT);

        SSLSocketFactory factory = sslcontext.getSocketFactory(); // (SSLSocketFactory)SSLSocketFactory.getDefault();
        SSLSocket client = (SSLSocket) factory.createSocket(socket, hostName, hostPort, true);

        return client;

        /*
         * KeyStore ks = KeyStore.getInstance("JKS"); ks.load(new
         * FileInputStream("serverkeys"), "Wakka1337".toCharArray());
         * KeyManagerFactory kmf = KeyManagerFactory.getInstance("SunX509");
         * kmf.init(ks, "Smorka".toCharArray());
         *
         * // set the context to SSL 3 SSLContext sslcontext =
         * SSLContext.getInstance("SSLv3");
         * sslcontext.init(kmf.getKeyManagers(), null, null);
         *
         * // create the socket SSLSocketFactory ssf =
         * sslcontext.getSocketFactory(); SSLSocket socket = (SSLSocket)
         * ssf.createSocket("127.0.0.1", port);
         *
         *
         * return socket;
         */

#endif

    }
}
