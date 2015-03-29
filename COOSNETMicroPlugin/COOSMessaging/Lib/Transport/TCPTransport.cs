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
using System.Runtime.CompilerServices;


using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Util;
using java.lang; // IRunnable interface compability

#endregion



namespace Org.Coos.Messaging.NETMicro.Transport
{
    
    // Services offered by transport layer
    //  - connection establishment to router with retry
    //  - prioritized mailbox of messages (processMessage)
    //  - serialization and send (writer)
    //  - deserialization and receive (reader)

    public class TCPTransport : DefaultProcessor, ITransport, IService
    {

         const string PROPERTY_HOST = "host";
        const string PROPERTY_PORT = "port";
         const string PROPERTY_RETRY = "retry";
         const string PROPERTY_RETRY_TIME = "retryTime";
         const int CONNECT_TIMEOUT = 4000;

       

        // Server adr.
        protected string hostName;
        protected int hostPort = 0;
        static protected int retryTime = 10000;
        /// <summary>
        /// Determines timeout for giving up socket connect attempt to router, pluginchannel also has a
        /// separate timeout for connecting the channel to transport
        /// </summary>
        protected Int32 GIVEUP_CONNECT_RETRY_TIMEOUT =  11*retryTime; // Allows 10 retries
        protected bool retry = false;

        // C# inner class can only access static members of outer class, hence socket is static
        
       /* static*/ protected Socket socket;

        /// <summary>
        ///  Reader reads incoming/inbound network msg, auto starts as thread
        /// </summary>
        public Reader reader;
        
        /// <summary>
        ///  Writer write outbound network msg., auto starts as a thread
        /// </summary>
        public Writer writer;

        /// <summary>
        /// The mailbox contains a sorted list of messages based on priority, most urgent messages are sent first
        /// </summary>
        static protected ArrayList mailbox = new ArrayList();

        /// <summary>
        /// Next processor that takes further action on message, the reader thread deserialze incoming data
        /// (in DefaultMessage(networkStream)) into a message that is forwarded to the transportProcessor
        /// </summary>
        static public IProcessor transportProcessor; 
#if LOGGING
        private static Org.Coos.Messaging.Util.ILog logger = LogFactory.getLog(Type.GetType("Org.Coos.Messaging.NETMicro.Transport.TCPTransport").FullName);
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

        /// <summary>
        /// Signals wheter endpoint is connected to router
        /// </summary>
        static AutoResetEvent connectedToServerEvent = new AutoResetEvent(false);


        public static Thread threadConnectSocket;

        //http://msdn.microsoft.com/en-us/library/system.net.sockets.networkstream.aspx
        static protected Stream networkStream;
        // Network stream allows read/write for different threads (one thread for reading) and one thread for writing
        // without need for synchronization, so instead of having two networkstreams as in java, only one should be
        // enough for .NET


        public TCPTransport()
        {
        }

        public TCPTransport(string hostIP, int hostPort)
        {
            this.hostName = hostIP;
            this.hostPort = hostPort;
        }

        //TCPTransport(SocketConnection socket) {
        //    this.socket = socket;
        //}

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

        public Reader getReader()
        {
            return reader;
        }

        public Writer getWriter()
        {
            return writer;
        }

        /// <summary>
        /// The TCPTransport processor puts msg in a mailbox
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void start()
        {


                // Well if we are running, why start, just return
                if (running)
                {
                    return;
                }


                running = true;

                if (socket != null)
                {
                    // Socket already exist, Start reader and writer
                    startReaderWriter();

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

                string retryStr = (string)properties[PROPERTY_RETRY];

                if ((retryStr != null) && retryStr.Equals("true"))
                {
                    retry = true;
                }
                else
                {
                    retry = false;
                }

                if (properties[PROPERTY_RETRY_TIME] != null)
                {
                    retryTime = int.Parse((String)properties[PROPERTY_RETRY_TIME]);
                }

                logger.info("Establishing transport to " + hostName + ":" +
                hostPort);

                // Start connection process on a thread
                Thread connectThread = new Thread(() => connectToRouter(retryStr));
                connectThread.Start();

                // Wait for connect signal
                bool connectSignalReceived = connectedToServerEvent.WaitOne(GIVEUP_CONNECT_RETRY_TIMEOUT, false);
                if (!connectSignalReceived)
                {
                    if (connectThread.IsAlive)  // Stop this thread please, we timed out, this thread will not be aborted by channel that is also waiting for connection to transport
                        connectThread.Abort();

                    throw new ConnectingException("Could not connect to COOS router, timeout");

                }

            
        }

        /// <summary>
        /// Create client socket to server
        /// </summary>
        /// <param name="serverHost"></param>
        /// <param name="port"></param>
        /// <returns></returns>
         public Socket createClient(String serverHost, int port)  {
        // JAVA SVN rev. 1034 
        //Socket socket = new Socket();
        //socket.connect(new InetSocketAddress(hostName, hostPort), CONNECT_TIMEOUT);

        //return socket;

        // No timeout on connect in .NET Micro framework

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
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
        //socket.Bind(ipEndp);
        socket.Connect(ipEndp);

        connectedToServerEvent.Set();

        return socket;
    }

        /// <summary>
        /// Connect to router 
        /// </summary>
        /// <param name="retryStr"></param>
        /// <returns></returns>
        private void connectToRouter(string retryStr)
        {
            try
            {

                if (socket == null)
                {

                    bool connecting = true;

                    while (connecting && running)
                    {
                        try
                        {
                            socket = createClient(hostName, hostPort);

                            connecting = false;
                        }
                        catch (Exception e)
                        {
                            logger.info("Establishing transport to " +
                            hostName + ":" + hostPort +
                           " failed. Retrying in " + retryTime.ToString() + " millisec.");

                            if (retryStr == null || retryStr.Equals("true"))
                            {
                                Thread.Sleep(retryTime);
                            }
                            else
                            {
                                connecting = false;
                            }
                        }
                    }


                    if (!connecting && running)
                    {
                        //logger.info("Transport from " +
                        //socket.LocalEndPoint.ToString() + " to " +
                        //socket.RemoteEndPoint.ToString() + " established.");

                    }
                }

                startReaderWriter();
            }
            catch (ThreadAbortException e)
            {
                logger.info("Connect to router thread aborted.");
            }
        }

        protected virtual void startReaderWriter()
        {
            networkStream = new NetworkStream(socket, false);

            reader = new Reader(socket,networkStream, channel,retry,retryTime);
            writer = new Writer(socket,networkStream, channel, mailbox, msgInMailBoxEvent);
           
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
                if (socket != null)
                {
                   
                    socket.Close();
                }

                socket = null;
            
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


    public class Reader : IRunnable
    {
        Thread readerThread;
        // JAVA InputStream is;
        Stream incomingStream;
        Socket socket;
        IChannel channel;

        bool running = true, failed, stopped, retry;
        int retryTime;

        
         protected const  int MAX_LENGTH = (16 * 1024); // Room for large headers
         protected const  int MAX_BODY_LENGTH = (8 * 1024);


        public Reader(Socket socket,Stream incomingStream, IChannel channelReader,  bool retry, int retryTime)
        {
            this.incomingStream = incomingStream;
            this.socket = socket;
            this.channel = channelReader;
            this.retry = retry;
            this.retryTime = retryTime;

            readerThread = new Thread(() => { this.run(); });
            readerThread.Start();
            //logger.info("Reader started on :" +
            //socket.LocalEndPoint.ToString());
            failed = false;
        }

        public virtual void stop()
        {
            stopped = true;

            running = false;

            if (!failed)
                retry = false;

           // readerThread.Abort();

        }

        public virtual void run()
        {
            try
            {
                if (incomingStream == null)
                    Debug.Print("Got null in inputstream!!!!!!!!!");

                DataInputStream din = new DataInputStream(incomingStream);
                IMessage msg = null;

                while (running)
                {

                    try
                    {
                        if (socket.Poll(30*1000000, SelectMode.SelectRead))

                            if (socket.Available >= 4) // Beging reading if available data from network buffer is at least 4 bytes message length
                            // if (networkStream.DataAvailable)
                            {
                               // Debug.Print("Reader: " + socket.Available.ToString() + " bytes received. Deserializing...");



                                int size = din.readInt();

                                if (size + 4 > MAX_LENGTH)
                                    throw new IOException("Packet too big, length=" + ((int)(size + 4)).ToString());


                                byte[] buf = new byte[size + 1];

                                // Reads directly
                                #region Read message in one operation

                                MemoryStream bos = new MemoryStream();
                                DataOutputStream dos = new DataOutputStream(bos);
                                dos.writeInt(size);

                                // Direct ingest of entire message in one operation
                                int bytesRead = incomingStream.Read(buf, 0, size);
                                if (bytesRead < size)
                                    throw new IOException("Could not read entire message of " + size.ToString() + " bytes from network, only got " + bytesRead.ToString() + " bytes.");

                                dos.write(DataOutputStream.ToSByteArray(buf), 0, bytesRead);


                                bos.Position = 0; // Deserialization should start at first byte
                                DataInputStream di = new DataInputStream(bos);
                                #endregion

                                // Derserialization can now be read directly from memory
                                msg = new DefaultMessage(di);


                                Debug.Print("Reader: received msg.: " + msg.getName());

                                if ((msg.getSerializedBody() == null) ||
                                   ((msg.getSerializedBody() != null) &&
                                    (msg.getSerializedBody().Length <= MAX_BODY_LENGTH)))
                                   
                                    TCPTransport.transportProcessor.processMessage(msg);
                                else
                                    throw new Exception("Body too big, length=" + msg.getSerializedBody().Length);


                            }
                            else
                            {
                               // Debug.Print("Reader: Waiting for message ....");
                               // Thread.Sleep(0);  
                               
                            }
                                //} catch (EOFException e) {
                        //    // logger.info("Connection closing EOF");
                        //    running = false;
                    }
                    catch (System.Exception e)
                    {
                        //e.printStackTrace();
                        //logger.fatal("Error in Message deserialization. Aborting");
                        running = false;
                        failed = true;
                    }
                }

                incomingStream.Close();

               

                if (channel != null)
                { 
                    if (!stopped)
                    {
                       channel.disconnect(); //-> stop
                    }

                    if (retry)
                    {
                        Thread.Sleep(retryTime);
                        channel.connect(channel.getLinkManager());
                    }

                }
            }
            catch (System.Exception e)
            {
                //logger.error("Unkown error in reader",e.Message);
            }
        }
    }


    public class Writer : IRunnable
    {
        Thread writerThread;
        Stream outgoingStream; // SslStream or Networkstream 
        bool running = true;
        static IChannel channel;
        ArrayList mailbox;
        AutoResetEvent msgInMailBoxEvent;
        Socket socket;

        public Writer(Socket socket,Stream networkStream, IChannel channelWriter, ArrayList mailbox, AutoResetEvent msgInMailBoxEvent)
        {
            outgoingStream = networkStream;
            channel = channelWriter;
            this.mailbox = mailbox;
            this.msgInMailBoxEvent = msgInMailBoxEvent;
            this.socket = socket;

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



                        //    IMessage msg = new DefaultMessage(IChannelPrimitives.CONNECT);
                        mailbox.Remove(msg);
                        try
                        {
                            byte[] serialMsg = msg.serialize(); // Serialization

                            Debug.Print("Writer sending: " + msg.getName());

                            // Check first at least 1 sec. for write possibility on socket
                            // if (socket.Poll(1 * 1000000, SelectMode.SelectWrite))
                            //{
                            //lock (outgoingStream) - Synchronization with reader-thread should not be necessary, at least on networkstream
                            //{
                                outgoingStream.Write(serialMsg, 0, serialMsg.Length); // Send to network

                           // }
                            //   outgoingStream.Flush();
                            //}


                            // networkStream.Write(serialMsg, 0, serialMsg.Length);

                            // networkStream.Flush(); // MSDN documentation -> reserved for future use 31 oct 2010
                        }
                        catch (SocketException e) // Network error
                        {
                            running = false;
                            
                        }
                        catch (System.Exception e) // General error
                        {
                            //e.printStackTrace();
                            // logger.fatal("Error in Message writing. Aborting");
                            running = false;
                        }

                        #endregion
                    }
                }


                // Close transport

                outgoingStream.Close();

                // Disconnect channel
                if (channel != null)
                {
                    channel.disconnect();
                }
            }
            catch (System.Exception e)
            {
                //e.printStackTrace();
            }
        }
    }
}
