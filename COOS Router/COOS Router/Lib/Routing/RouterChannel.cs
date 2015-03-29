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
//package org.coos.messaging.routing;

//import org.coos.messaging.Connectable;
//import org.coos.messaging.ConnectingException;
//import org.coos.messaging.Message;
//import org.coos.messaging.impl.DefaultChannel;
//import org.coos.messaging.impl.DefaultMessage;
//import org.coos.messaging.impl.DefaultProcessor;
//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;

using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;
using Org.Coos.Messaging.Impl;

using System;

namespace Org.Coos.Messaging.Routing
{

    ///<summary>Router Channel class. Contains one inlink and one outlink and a transport 
    /// This Channel type connects COOS instances
    ///</summary>
    ///<author>Knut Eilif Husa, Tellu AS</author>
    public class RouterChannel : DefaultChannel
    {

        private ILog logger = LogFactory.getLog(typeof(RouterChannel).FullName, true);

        private String routingAlgorithm;


        public RouterChannel()
        {
            protocols.Add("coos");
        }

        public String getRoutingAlgorithm()
        {
            return routingAlgorithm;
        }

        public void setRoutingAlgorithmName(String routingAlgorithm)
        {
            this.routingAlgorithm = routingAlgorithm;
        }

        public String getConnectingPartyUuid()
        {
            return connectingPartyUuid;
        }

        public void setConnectingPartyUuid(String connectingPartyUuid)
        {
            this.connectingPartyUuid = connectingPartyUuid;
        }

        /// <summary>
        /// Process CONNECT_ACK from connecting party
        /// </summary>
        /// <param name="msg"></param>
        public  override void processMessage(IMessage msg)
        {

            if (msg.getHeader(IMessagePrimitives.MESSAGE_NAME).Equals(CONNECT_ACK))
            {
                String conUuid = msg.getHeader(CONNECT_UUID);

                if (conUuid != null)
                {
                    outLink.setDestinationUuid(conUuid);
                    outLink.setChainedProcessor(transport);

                    inLink.setChainedProcessor(connectable.getDefaultProcessor());
                    inLink.setDestinationUuid(connectingPartyUuid);
                    transport.setChainedProcessor(inLink);
                    connected = true;

                    try
                    {
                        connectable.addLink(conUuid, outLink);
                    }
                    catch (Exception e)
                    {
                        disconnect();
                        logger.warn("Connection failed.", e);
                    }

                }
                else
                {
                    logger.warn("Can not process msg: " + msg);
                    disconnect();
                }
            }
            else
            {
                logger.warn("Connection failed: " + msg);
                disconnect();
            }
        }

        
        ///<summary>Method that connects the channel if the Channel is init property set to true</summary>
        public  override void connect(IConnectable linkManager)
        {
            this.connectable = linkManager;

            if (!isInit())
            {
                return;
            }

            try
            {

                transport.setChainedProcessor(this); // Process CONNECT request in router channel processMessage

                IMessage msg = new DefaultMessage(CONNECT);
                msg.setHeader(CONNECT_UUID, connectingPartyUuid);
                msg.setHeader(ROUTING_ALGORITHM, routingAlgorithm);
                transport.start();
                outLink.start();
                inLink.start();
                transport.processMessage(msg);

            }
            catch (Exception e)
            {
                logger.error("Router channel connect. Exception ignored.", e);
            }
        }

        
        ///<summary>Disconnects the Channel</summary>
        public override void disconnect()
        {

            // todo disconnect protocol is missing towards the peer. Will only
            // disconnect on this channel side
            connected = false;

            try
            {

                if (connectable != null)
                {
                    connectable.removeLinkById(outLink.getLinkId());
                }

                outLink.stop();
                inLink.stop();

                if (transport != null)
                {
                    transport.stop();
                }
            }
            catch (Exception e)
            {
                logger.error("Router channel disconnect. Exception ignored.", e);
            }
        }

        
        /// <summary>Makes a copy of the channel. Based on the shared property of the processors the Channel will be populated with unique or shared instances</summary>
        ///<returns>the copied channel</returns>
        public new DefaultChannel copy()
        {
            RouterChannel ch = (RouterChannel)base.copy();
            ch.setConnectingPartyUuid(connectingPartyUuid);

            return ch;
        }


    }
}
