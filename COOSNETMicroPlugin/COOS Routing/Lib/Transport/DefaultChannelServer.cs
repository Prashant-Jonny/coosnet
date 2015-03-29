#define NETMICROFRAMEWORK
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

using Org.Coos.Messaging.Util;
using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Routing;


using System.Collections;
using System;

namespace Org.Coos.Messaging.Transport
{
    //package org.coos.messaging.transport;

    //import java.util.HashMap;
    //import java.util.Hashtable;
    //import java.util.Iterator;
    //import java.util.Map;
    //import java.util.Vector;

    //import org.coos.messaging.COOS;
    //import org.coos.messaging.Channel;
    //import org.coos.messaging.ChannelServer;
    //import org.coos.messaging.ConnectingException;
    //import org.coos.messaging.Link;
    //import org.coos.messaging.LinkManager;
    //import org.coos.messaging.Message;
    //import org.coos.messaging.ProcessorException;
    //import org.coos.messaging.Transport;
    //import org.coos.messaging.impl.DefaultChannel;
    //import org.coos.messaging.impl.DefaultMessage;
    //import org.coos.messaging.impl.DefaultProcessor;
    //import org.coos.messaging.routing.Router;
    //import org.coos.messaging.routing.RouterChannel;
    //import org.coos.messaging.routing.RouterSegment;
    //import org.coos.messaging.util.Log;
    //import org.coos.messaging.util.LogFactory;
    //import org.coos.messaging.util.UuidGenerator;
    //import org.coos.messaging.util.UuidHelper;


    /**
     * @author Knut Eilif Husa, Tellu AS A default channel server
     */
    public class DefaultChannelServer : DefaultProcessor, IChannelServer
    {
        private static  ILog logger = LogFactory.getLog(typeof(DefaultChannelServer).FullName);
#if JAVA
    private Map<String, RouterChannel> channelMappings = new HashMap<String, RouterChannel>();
#endif
#if NETMICROFRAMEWORK
        private Hashtable channelMappings = new Hashtable();
#endif
     // C#: hides inherited member from DefaultProcessor   protected Hashtable properties = new Hashtable();


        private ILinkManager linkManager;

        private ITransport transport;

        private UuidGenerator uuidGenerator = new UuidGenerator();

        private COOS coosInstance;
#if JAVA
   public void setChannelMappings(Hashtable<string, RouterChannel> channelMappings) {
#endif
#if NETMICROFRAMEWORK
        public void setChannelMappings(Hashtable channelMappings)
        {
#endif
            this.channelMappings = channelMappings;
        }

        public void addChannelMapping(string uuid, RouterChannel channel)
        {
            this.channelMappings.Add(uuid, channel);
        }


        public void setCOOSInstance(COOS coosInstance)
        {
            this.coosInstance = coosInstance;
        }


        public void setLinkManager(ILinkManager linkManager)
        {
            this.linkManager = linkManager;
        }

        public void initializeChannel(ITransport transport)
        {
            this.transport = transport;
            transport.setChainedProcessor(this);
        }

        /**
         * Starts the service
         *
         * @throws Exception
         *             Exception thrown if starting of service fails
         */
        public void start()
        {

        }

        /**
         * Stops the service
         *
         * @throws Exception
         *             Exception thrown if stopping of service fails
         */
        public void stop()
        {

        }

        #region Properties

        public override void setProperties(Hashtable properties)
        {
            this.properties = properties;
        }

        public override Hashtable getProperties()
        {
            return properties;
        }

        public override string getProperty(string key)
        {
            return (string)properties[key];
        }

        public override void setProperty(string key, string value)
        {
            if (!properties.Contains(key))
                properties.Add(key, value);

        }
        #endregion


        /// <summary>
        /// Process connect message
        /// </summary>
        /// <param name="msg"></param>
        public override void processMessage(IMessage msg)
        {

            if (msg.getHeader(IMessagePrimitives.MESSAGE_NAME).Equals(DefaultChannel.CONNECT))
            {

                string conUuid = msg.getHeader(DefaultChannel.CONNECT_UUID);
                string conSeg = msg.getHeader(DefaultChannel.CONNECT_SEGMENT);
                string routingAlgName = msg.getHeader(DefaultChannel.ROUTING_ALGORITHM);

                IMessage reply = new DefaultMessage(DefaultChannel.CONNECT_ACK);
                RouterSegment seg;
                RouterChannel channel;
                try
                {

                    if ((conUuid != null) && (routingAlgName == null))
                    {

                        // this happens when endpoints with already allocated uuid connect
                        conSeg = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(conUuid);

                        // the routerUuids for the various segments
                        seg = linkManager.getSegment(conSeg);

                        if (seg == null)
                        {
                            throwUUIDConnectingException(conUuid);
                        }

                        reply.setHeader(DefaultChannel.CONNECT_UUID, seg.getRouterUUID());
                        reply.setHeader(DefaultChannel.CONNECT_ROUTER_UUID, seg.getRouterUUID());
                    }
                    else if ((conUuid != null) && (routingAlgName != null))
                    {
                        conSeg = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(conUuid);

                        // this happens when routers connect
                        // the routerUuids for the various segments
                        seg = linkManager.getSegment(conSeg);

                        if (seg == null)
                        {

                            // parentsegment must be present and static
                            string parentSeg = UuidHelper.getParentSegment(conSeg);
                            seg = linkManager.getSegment(parentSeg);

                            if (seg == null)
                            {
                                throwSegmentConnectingException(conUuid, routingAlgName);
                            }

                            linkManager.addDynamicSegment(conSeg, routingAlgName);
                            seg = linkManager.getSegment(conSeg);
                        }
                        else
                        {

                            if (!seg.getRoutingAlgorithmName().Equals(routingAlgName))
                            {
                                throwSegmentConnectingException(conUuid, routingAlgName);
                            }
                        }

                        reply.setHeader(DefaultChannel.CONNECT_UUID, seg.getRouterUUID());
                        reply.setHeader(DefaultChannel.CONNECT_ROUTER_UUID, seg.getRouterUUID());
                    }
                    else
                    {

                        // This happens when endpoint attach to the bus and gets
                        // uuid allocated
                        if ((conSeg == null) || conSeg.Equals("") || conSeg.Equals(IRouterPrimitives.LOCAL_SEGMENT) || conSeg.Equals(IRouterPrimitives.DICO_SEGMENT))
                        {

                            // If no segment id specified we pick the default from the
                            // segment mapping
                            foreach (RouterSegment rs in linkManager.getSegmentMap().Values)
                            {

                                if (rs.isDefaultSegment())
                                {
                                    conSeg = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(rs.getRouterUUID());

                                    break;
                                }
                            }
                        }

                        seg = linkManager.getSegment(conSeg);

                        if (seg == null)
                        {
                            throwSegmentConnectingException(conSeg, "n/a");
                        }

                        if (!conSeg.Equals("."))
                        {
                            conSeg += ".";
                            // JAVA = new StringBuilder(conSeg).append(".").toString();
                        }

                        conUuid += uuidGenerator.generateId();
                        // JAVA =   new StringBuilder().append(conSeg).append(uuidGenerator.generateId()).toString();

                        reply.setHeader(DefaultChannel.CONNECT_ALLOCATED_UUID, conUuid);
                        reply.setHeader(DefaultChannel.CONNECT_ROUTER_UUID, seg.getRouterUUID());
                    }

                    IChannel prototypeChannel = null;

                    foreach (string key in channelMappings.Keys)
                    {

                        if (conUuid.Equals(key))
                        {
                            prototypeChannel = channelMappings[key] as IChannel;
                            logger.debug("Matched channel with key: " + key + " to channel: " + prototypeChannel.getName());
                        }
                    }

                    if (prototypeChannel == null)
                    {
                        logger.debug("Allocating new channel!");
                        prototypeChannel = new RouterChannel();
                    }

                    channel = (RouterChannel)prototypeChannel.copy();

                    if (coosInstance != null)
                    {
                        coosInstance.addChannel(conUuid, channel);
                    }

                    channel.getOutLink().setChainedProcessor(transport);

                    transport.setChannel(channel);

                    transport.setChainedProcessor(channel.getInLink());
                    channel.getInLink().setChainedProcessor(linkManager.getDefaultProcessor()); // The
                    // router
                    channel.getInLink().setDestinationUuid(seg.getRouterUUID());
                    channel.setLinkManager(linkManager);
                    channel.setTransport(transport);

                    linkManager.addLink(conUuid, channel.getOutLink());

                    // Retrieve the aliases
                    // They are not set on the link directly since we want to handle aliases
                    // a common way whether they arrive at registration or at later stages
                    Hashtable props = msg.getBodyAsProperties();

                    if (props != null)
                    {
                        ArrayList aliases = (ArrayList)props[Link.ALIASES];
                        linkManager.setLinkAliases(aliases, channel.getOutLink());
                    }

                }
                catch (Exception e)
                {
                    logger.warn("Exception caught. Removing link " + conUuid, e);
                    linkManager.removeLink(conUuid);
                    transport.setChainedProcessor(null);
                    coosInstance.removeChannel(conUuid);
                    reply.setHeader(IMessagePrimitives.MESSAGE_NAME, DefaultChannel.CONNECT_NACK);
                    reply.setHeader(IMessagePrimitives.TYPE, IMessagePrimitives.TYPE_ERROR);
                    reply.setHeader(IMessagePrimitives.ERROR_REASON, e.Message);
                    transport.processMessage(reply);

                    return;
                }

                transport.processMessage(reply);
                channel.setConnected(true);


            }
            else
            {
                throw new ProcessorException("ChannelServer: Cannot process message: " + msg.ToString());
            }
        }

#if JAVA
     private void throwUUIDConnectingException(string conUuid) throws ConnectingException {
#endif
#if NETMICROFRAMEWORK
        private void throwUUIDConnectingException(string conUuid)
        {
#endif
            string segmentListStr = "";

            foreach (string segment in linkManager.getSegmentMap().Keys)
                segmentListStr += (segment + ", ");

            
            string s = "Connection to endpoint/coos instance with uuid: " + conUuid + " failed. " +
                "This coos instance is declared to only connect to endpoints/coos instances in segment(s):" + segmentListStr;
            
            logger.error(s);

            throw new ConnectingException(s);
        }

#if JAVA
        private void throwSegmentConnectingException(string conSeg, string routingAlg) throws ConnectingException {
#endif
#if NETMICROFRAMEWORK
        private void throwSegmentConnectingException(string conSeg, string routingAlg)
        {
#endif
            string segmentListStr = "";

            foreach (string segment in linkManager.getSegmentMap().Keys)
            {
                RouterSegment seg = linkManager.getSegmentMap()[segment] as RouterSegment;
                segmentListStr += seg.getName() + ":" + seg.getRoutingAlgorithmName();
                segmentListStr += ", ";
           
            }

            string s = "Connection to endpoint/coos instance with segment: '" + conSeg + ", routingAlgorithm: " + routingAlg + "' failed. " +
                "This coos instance is declared to only connect to endpoints/coos instances in segment(s):" + segmentListStr;
          
            logger.error(s);

            throw new ConnectingException(s);
        }
    }
}
