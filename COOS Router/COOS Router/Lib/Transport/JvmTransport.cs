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

using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Util;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Org.Coos.Messaging.Transport
{
    //package org.coos.messaging.transport;

    //import org.coos.messaging.COOS;
    //import org.coos.messaging.COOSFactory;
    //import org.coos.messaging.Channel;
    //import org.coos.messaging.ChannelServer;
    //import org.coos.messaging.Message;
    //import org.coos.messaging.MessageContext;
    //import org.coos.messaging.Processor;
    //import org.coos.messaging.ProcessorException;
    //import org.coos.messaging.Transport;
    //import org.coos.messaging.impl.DefaultProcessor;
    //import org.coos.messaging.util.Log;
    //import org.coos.messaging.util.LogFactory;


    ///<summary>The JVM transport is used between COOS instances and Plugins (COOS to COOS and COOS to/from
    ///Plugin) residing in the same VM.
    ///</summary>
    ///<author>Knut Eilif Husa, Tellu AS</author>
    public class JvmTransport : DefaultProcessor, ITransport
    {
        private static readonly String PROPERTY_COOS_INSTANCE_NAME = "COOSInstanceName";
        private static readonly String PROPERTY_CHANNEL_SERVER_NAME = "ChannelServerName";
        private static readonly String PROPERTY_RETRY = "retry";
        private static readonly String PROPERTY_RETRY_TIME = "retryTime";

        protected static readonly ILog logger = LogFactory.getLog(typeof(JvmTransport).FullName);

        private IChannel channel;
        private IChannelServer channelServer;

        private InternalTransport intr;
        private IProcessor chainedProcessor;

        private IMessage storedConnectMsg = null;


        private bool running = false;
        protected int retryTime = 100;
        protected bool retry;
        
        
        public JvmTransport()
        {
            intr = new InternalTransport(this);
        }

        /**
         * Processes the message
         *
         * @param msg
         *            the message to be processed
         */
        public override void processMessage(IMessage msg) /*throws ProcessorException*/ {
            msg.setMessageContext(new MessageContext());

            if (intr.chainedProcessor != null)
            {

                //initialized
                if (!running)
                {
                    throw new ProcessorException("JVMTransport: " + name + " is stopped.");
                }

                intr.chainedProcessor.processMessage(msg);
            }
            else
            {

                //Not initialized
                storedConnectMsg = msg;
            }
        }

        /**
         * Sets the Processor that this Processor will call after finished
         * processing
         *
         * @param chainedProcessor
         */
        public void setChainedProcessor(IProcessor chainedProcessor)
        {
            this.chainedProcessor = chainedProcessor;
        }

        public void setChannel(IChannel channel)
        {
            this.channel = channel;
        }

        public void setChannelServer(IChannelServer channelServer)
        {
            this.channelServer = channelServer;
        }

        /**
         * Starts the service
         *
         * @throws Exception
         *             Exception thrown if starting of service fails
         */
        public void start() /*throws Exception */ {

          

            String retryStr = null;

            if (properties.ContainsKey(PROPERTY_RETRY))
                retryStr = (String)properties[PROPERTY_RETRY];


            if ((retryStr != null) && retryStr.Equals("true"))
            {
                retry = true;
            }
            else
            {
                retry = false;
            }

            if (properties.ContainsKey(PROPERTY_RETRY_TIME))
                retryTime = int.Parse((String)properties[PROPERTY_RETRY_TIME]);


            if (!running)
            {

                if (retry)
                { // must retry in a separate thread

                    var taskJvmTransport = Task.Factory.StartNew(() =>
#if NOTASK
                    Thread t = new Thread(() =>
#endif
                        {

                                     try
                                     {
                                         doStart(true);
                                     }
                                     catch (Exception e)
                                     {
                                         logger.warn("JvmTransport.start(). Exception ignored", e);
                                     }
                                 });

#if NOTASK
                    t.Start();
#endif
                }
                else
                {
                    doStart(false);
                }
            }
        }

        private void doStart(bool retry) /*throws Exception */ {

            if (channelServer == null)
            {
                String coosInstanceName = (String)properties[PROPERTY_COOS_INSTANCE_NAME];
                COOS coos;

                if (coosInstanceName != null)
                {
                    coos = COOSFactory.getCOOSInstance(coosInstanceName);

                    while (((coos = COOSFactory.getCOOSInstance(coosInstanceName)) == null) && retry)
                    {
                        logger.warn("Establishing transport to JVM coos " + coosInstanceName + " failed. Retrying in " + retryTime + " millisec.");
                        Thread.Sleep(retryTime);
                    }

                    if (coos == null)
                    {

                        throw new NullReferenceException("No COOS instance " + coosInstanceName + " defined in this vm!");
                    }
                }
                else
                { // COOSInstance is null
                    coos = COOSFactory.getDefaultCOOSInstance();

                    while (((coos = COOSFactory.getDefaultCOOSInstance()) == null) && retry)
                    {
                        logger.warn("Establishing transport to JVM defaultCOOS failed. Retrying in " + retryTime + " millisec.");
                        Thread.Sleep(retryTime);
                    }

                    if (coos == null)
                    {
                        throw new NullReferenceException("No defaultCOOS defined in this vm!");
                    }
                }

                String channelServerName = (String)properties[PROPERTY_CHANNEL_SERVER_NAME];

                if (channelServerName == null)
                {
                    channelServerName = "default";
                }

                channelServer = coos.getChannelServer(channelServerName);

                if (channelServer == null)
                {

                    if (!retry)
                        throw new NullReferenceException("ChannelServer: " + channelServerName + " is not declared within COOS instance: " +
                            coosInstanceName);

                    while (((channelServer = coos.getChannelServer(channelServerName)) == null) && retry)
                    {
                        Thread.Sleep(retryTime);
                        logger.warn("Establishing transport to JVM channelserver failed. Retrying in " + retryTime + " millisec.");
                    }
                }

                logger.debug("Established transport");
            }

            running = true;

            intr.start();

            channelServer.initializeChannel(intr);

            if (storedConnectMsg != null)
            {
                intr.chainedProcessor.processMessage(storedConnectMsg);
                storedConnectMsg = null;
            }
        }


        
        ///<summary>Stops the service</summary>
        ///<exception cref="Exception">Exception thrown if stopping of service fails</exception>
        public void stop() /*throws Exception */ {

            if (running)
            {
                running = false;
                channel.disconnect();
                intr.stop();
                channelServer = null;
            }
        }

        public override IProcessor copy()
        {
            JvmTransport transport = (JvmTransport)base.copy();
            transport.setChannel(channel);

            return transport;
        }

        private class InternalTransport : DefaultProcessor, ITransport
        {

            private bool running = false;
            public IProcessor chainedProcessor;
            IChannel channel;
            JvmTransport JvmTransportOuterInstance;

            /// <summary>
            /// Helper constructor for passing "this" JvmTransport from outer class, in Java available
            /// as class.this, but must be passed as argument to be available in inner class in C#
            /// </summary>
            /// <param name="outerTransportInstance"></param>
            public InternalTransport(JvmTransport outerTransportInstance)
            {
                JvmTransportOuterInstance = outerTransportInstance;
            }
            
            ///<summary>Processes the message</summary>
            ///<param name="msg">the message to be processed</param>
            public override void processMessage(IMessage msg) /*throws ProcessorException */ {

                if (!running)
                {
                    throw new ProcessorException("JVMTransport: " + name + " is stopped.");
                }

                msg.setMessageContext(new MessageContext());
                JvmTransportOuterInstance.chainedProcessor.processMessage(msg);
            }

            
            ///<summary>Sets the Processor that this Processor will call after finished processing</summary>
            ///<param name="chainedProcessor"></param>
            public void setChainedProcessor(IProcessor chainedProcessor)
            {
                this.chainedProcessor = chainedProcessor;
            }

            public void setChannel(IChannel channel)
            {
                this.channel = channel;
            }

            ///<summary>Starts the service</summary>
            ///<exception cref="Exception">Exception thrown if starting of service fails</exception>
            public void start() /*throws Exception */ {

                if (!running)
                {
                    running = true;
                }
            }

                  
            ///<summary>Stops the service</summary>
            ///<exception cref="Exception">Exception thrown if stopping of service fails</exception>
            public void stop() /*throws Exception */{

                if (running)
                {
                    running = false;

                    if (channel != null)
                    {
                        channel.disconnect();
                    }

                    JvmTransportOuterInstance.stop();
                }
            }

        }
    }
}
