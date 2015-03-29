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
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging.Routing
{
//package org.coos.messaging.routing;

//import java.util.Collection;
//import java.util.Hashtable;
//import java.util.Iterator;
//import java.util.Map;
//import java.util.Vector;
//import java.util.concurrent.ConcurrentHashMap;
//import java.util.concurrent.ConcurrentLinkedQueue;

//import org.coos.messaging.COOS;
//import org.coos.messaging.ConnectingException;
//import org.coos.messaging.Link;
//import org.coos.messaging.Message;
//import org.coos.messaging.Processor;
//import org.coos.messaging.ProcessorException;
//import org.coos.messaging.ProcessorInterruptException;
//import org.coos.messaging.impl.DefaultProcessor;
//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;
//import org.coos.messaging.util.URIHelper;
//import org.coos.messaging.util.UuidGenerator;
//import org.coos.messaging.util.UuidHelper;


/**
 * The DefaultRouter (actually default point to point router) contains the point
 * to point routing mechanisms
 *
 * @author Knut Eilif Husa, Tellu AS
 */
public class DefaultRouter : DefaultProcessor, IRouter, IHashMapCallback {

    /// <summary>
    /// Default value for link timeout
    /// </summary>
    private static readonly String LINK_TIMEOUT_DEFAULT = "60000"; 
    public static readonly String LINK_TIMEOUT = "linkTimeout";

   
    private ConcurrentBag<RouterProcessor> preProcessors = new ConcurrentBag<RouterProcessor>();
    private ConcurrentBag<RouterProcessor> postProcessors = new ConcurrentBag<RouterProcessor>();
    /// <summary>
    /// Seems like the first parameter is quality of service, so we have a routing table for each quality of service
    /// </summary>
    private ConcurrentDictionary<String, TimedConcurrentHashMap<String, Link>> routingTables = new ConcurrentDictionary<String, TimedConcurrentHashMap<String, Link>>();
    private ConcurrentDictionary<String, String> aliasTable = new ConcurrentDictionary<String, String>();

    // todo let the links be handled by the coos instance
    private ConcurrentDictionary<String, Link> links = new ConcurrentDictionary<String, Link>();
    /// <summary>
    /// Seems like first parameter is router UUID, a specific routing algorithm for each router uuid
    /// </summary>
    private ConcurrentDictionary<String, IRoutingAlgorithm> routingAlgorithms = new ConcurrentDictionary<String, IRoutingAlgorithm>();
    private ConcurrentDictionary<String, RouterSegment> segmentMapping = new ConcurrentDictionary<String, RouterSegment>();

   
    /// <summary>
    /// The uuid of the router in the different segments it participates in. If more than one uuid, this router is a gateway router
    /// </summary>
    private List<String> routerUuids = new List<String>();

    private ConcurrentBag<String> QoSClasses = new ConcurrentBag<String>();
    
    private String defaultQoSClass;
    
    private String COOSInstanceName;
    
    private bool running = false;
    private bool enabled = true;
    private bool loggingEnabled = false;

    private Link defaultGw = null;


    private readonly ILog logger = LogFactory.getLog(typeof(DefaultRouter).FullName, false);
   
    private COOS COOS;

    private long linkTimeout;

    // This constructor is only used by tests
    public DefaultRouter(String routerUuid) {
        COOSInstanceName = routerUuid;
        new LinkStateAlgorithm(this, routerUuid);
        addQoSClass(Link.DEFAULT_QOS_CLASS, true);
        addSegmentMapping(UuidHelper.getSegmentFromSegmentOrEndpointUuid(routerUuid), routerUuid, "linkstate");
    }

    public DefaultRouter() {
        //addQoSClass(Link.DEFAULT_QOS_CLASS, true);
    }

    public void setEnabled(bool enabled) {
        this.enabled = enabled;
    }

    public void removeRouterUuid(String routerUuid) {
        bool res = routerUuids.Remove(routerUuid);
    }

    public void setLoggingEnabled(bool loggingEnabled) {
        this.loggingEnabled = loggingEnabled;

        foreach (IRoutingAlgorithm routingAlgorithm in routingAlgorithms.Values) {
            routingAlgorithm.setLoggingEnabled(loggingEnabled);
        }
       
    }

   
	public override void processMessage(IMessage msg) {
       // logger.info("DefaultRouter processing msg. : " + msg.ToString());
      

        // Router preprocessors
        foreach (RouterProcessor routerProcessor in preProcessors) {

            try {
                routerProcessor.processMessage(msg);
            } catch (ProcessorInterruptException e) {
                return;
            } catch (ProcessorException e) {
                logger.error("ProcessorException caught.", e);
            } catch (Exception e) {
                logger.error("Unknown Exception caught.", e);
            }
        }

        if (msg.getReceiverEndpointUri() == null) {
            logger.warn("Message from " + msg.getSenderEndpointUri() + " missing receiver address");
            replyErrorReason(msg, IMessagePrimitives.ERROR_NO_RECEIVER);

            return;
        }

        String qosClass = msg.getHeader(IMessagePrimitives.QOS_CLASS);

        if (qosClass == null) {
            qosClass = defaultQoSClass;
        }

        /// Routing table for specified QOS class
        TimedConcurrentHashMap<String, Link> routingTable = null;

        if (routingTables.ContainsKey(qosClass))
            routingTable = routingTables[qosClass];
        else
            logger.warn("Found no routing table for QoS class : " + qosClass);

        String hops = msg.getHeader(IMessagePrimitives.HOPS);

        if (hops == null) {
            hops = "1";
        } else {
            hops = (int.Parse(hops) + 1).ToString();
        }

        msg.setHeader(IMessagePrimitives.HOPS, hops);

        if (int.Parse(hops) > 244) {
            logger.warn("Message from " + msg.getSenderEndpointUri() + ", to: " + msg.getReceiverEndpointUri() + " too many hops");

            return;
        }

        String uuid = resolveAlias(msg);
       // logger.info("Uuid after resolveAlias: "+uuid);

        if (uuid == null) {

            // will never occur except for localcoos
            replyErrorReason(msg, IMessagePrimitives.ERROR_NO_ALIAS + ":" + msg.getReceiverEndpointUri());

            return;
        }

        uuid = UuidHelper.getQualifiedUuid(uuid);
        //logger.info("Qualified Uuid : "+uuid);

        Link link;


        
        // check if the message is destined towards this router (routing info, aliases)
       

        if (routerUuids.Contains(uuid)) {
            

            logger.trace("Message destined towards this router, uuid of msg.: "+uuid+" type: "+msg.getType());
            logger.putMDC(IRouterPrimitives.UUID_PREFIX, uuid);

            if (msg.getType().Equals(IMessagePrimitives.TYPE_ROUTING_INFO)) {
                routingAlgorithms[UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(uuid)].processRoutingInfo(msg);
            } else if (msg.getType().Equals(IMessagePrimitives.TYPE_ALIAS)) {

                try {
                    setLinkAliases((List<string>) msg.getBody(), msg.getMessageContext().getInBoundChannel().getOutLink());
                } catch (ProcessorException e) {
                    logger.error("ProcessorException caught while stopping endpoint.", e);
                }
            }
        } else {

            logger.trace("Finding route to uuid: "+uuid);
            // The routing step
            link = route(uuid, msg, routingTable);

            if (link != null) {
                msg.getMessageContext().setNextLink(link);
            }

            // Router postprocessors
            foreach (RouterProcessor routerProcessor in postProcessors) {

                try {
                    routerProcessor.processMessage(msg);
                } catch (ProcessorInterruptException e) {
                    return;
                } catch (ProcessorException e) {
                    logger.error("ProcessorException caught.", e);
                } catch (Exception e) {
                    logger.error("Unknown Exception caught.", e);
                }
            }

            // The sending step
            if (link != null) {

                if (msg.getHeader(IMessagePrimitives.TRACE_ROUTE) != null) {
                    String trace = msg.getHeader(IMessagePrimitives.TRACE);

                    if (trace == null) {
                        trace = COOSInstanceName;
                    }

                    msg.setHeader(IMessagePrimitives.TRACE, trace + " -> " + link.getDestinationUuid());
                }

                try {
                    link.processMessage(msg);
                } catch (ProcessorException e) {
                    replyErrorReason(msg, e.Message);
                }
            } else {
                String errorMsg;
                URIHelper helper = new URIHelper(msg.getReceiverEndpointUri());
                String alias = helper.getEndpoint();

                if (uuid.Equals("null")) {
                    errorMsg = COOSInstanceName + ": No uuid for alias " + alias + ", No route from " + COOSInstanceName;
                } else {

                    if (uuid.Equals(alias)) {
                        errorMsg = COOSInstanceName + ": No route to: " + alias + " from " + COOSInstanceName;
                    } else {
                        errorMsg = COOSInstanceName + ": No route to: " + alias + " / " + uuid + " from " + COOSInstanceName;
                    }
                }

                logger.warn(errorMsg);
                replyErrorReason(msg, IMessagePrimitives.ERROR_NO_ROUTE);
            }
        }
    }

    
    /// <summary>
    /// This method dynamiclly registers and unregisters aliases to link
    /// </summary>
    /// <param name="regAliases">An vector containing all aliases to be registered</param>
    /// <param name="outlink">The link to register the aliases with. This link is always directed towards an endpoint</param>
    public void setLinkAliases(List<String> regAliases, Link outlink) /*throws ProcessorException*/ {
        String segment = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(outlink.getDestinationUuid());

        if (!segment.Equals(".")) {
            segment += ".";
        }

        
        // Qualify aliases
        for (int i = 0; i < regAliases.Count; i++) {
            String alias = (String) regAliases[i];

            if (!alias.StartsWith(IRouterPrimitives.DICO_SEGMENT + ".") && !alias.StartsWith(IRouterPrimitives.LOCAL_SEGMENT + ".") && !alias.StartsWith(segment)) {

                // unqualified aliases are qualified by prefixing with segment
                // name
                String qualifiedAlias = segment + alias;
                regAliases.Remove(alias);
                regAliases.Add(qualifiedAlias);
            }
        }

        LinkedList<String> curAliases = outlink.getAlises();
        //Iterator itCurAliases = curAliases.iterator();

        //// remove aliases that are not present anymore
        //while (itCurAliases.hasNext()) {
        //    String alias = (String) itCurAliases.next();
        foreach(String alias in curAliases)

            if (!regAliases.Contains(alias)) {
                curAliases.Remove(alias);
                removeAlias(alias);
            }
        

        // Add all aliases
        // JAVA Iterator itRegAliases = regAliases.iterator();
        List<string>.Enumerator itRegAliases = regAliases.GetEnumerator();

       
        while (itRegAliases.MoveNext())
        {
            String alias = (String)itRegAliases.Current;
        
            String oldToUuid = null;
            if (aliasTable.ContainsKey(alias))  // Defensive indexing, otherwise "key not found"-exception
              oldToUuid = aliasTable[alias];

            if ((oldToUuid != null) && !oldToUuid.Equals(outlink.getDestinationUuid())) {
                List<string>.Enumerator itRegAliases2 = regAliases.GetEnumerator();

                while (itRegAliases2.MoveNext()) {
                    removeAlias((String) itRegAliases2.Current);
                }

                throw new ProcessorException("Can not register alias:" + alias + " since this alias is occupied for endpoint with uuid " +
                    outlink.getDestinationUuid());
            }

            putAlias(alias, outlink.getDestinationUuid());
        }
    }

    
    ///<summary>This method resolves receiver URIs into uuids. It handles both URIs containing aliases and uuids
    ///</summary>
    ///<param name="msg">the message to resolve alias for</param>
    public String resolveAlias(IMessage msg) {
        URIHelper helper = new URIHelper(msg.getReceiverEndpointUri());
        String uuid = null;
        String alias;
        alias = helper.getEndpoint();

        String segment;

        if ((alias != null) && !helper.isEndpointUuid()) {

            // routing on alias

            if (!helper.isEndpointQualified()) {

                // To account for that not all messages arrives the router via an
                // incoming link.
                // I.e. the routingInfo from this router
                if ((msg.getMessageContext().getInBoundLink() != null) && (msg.getMessageContext().getInBoundLink().getDestinationUuid() != null)) {
                    segment = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(msg.getMessageContext().getInBoundLink().getDestinationUuid());
                } else {
                    segment = ".";
                }

                if (!segment.Equals(".")) {
                    segment += ".";
                }

                // the alias is unqualified, first try to route in segment
                // namespace
                String qualifiedAlias = segment + alias;
                if (aliasTable.ContainsKey(qualifiedAlias))
                  uuid = aliasTable[qualifiedAlias];

                if (uuid == null) {

                    // If not found in segment namespace, then try to route in
                    // dico namespace
                    qualifiedAlias = IRouterPrimitives.DICO_SEGMENT + "." + alias;
                    if (aliasTable.ContainsKey(qualifiedAlias))
                      uuid = aliasTable[qualifiedAlias];
                }
            } else {

                // If fully qualified alias then lookup in aliasTable
                if (aliasTable.ContainsKey(alias))
                   uuid = aliasTable[alias];
            }

            if ((uuid == null) && !helper.getSegment().Equals(IRouterPrimitives.LOCAL_SEGMENT) && !helper.getSegment().Equals(IRouterPrimitives.DICO_SEGMENT)) {

                // we return with the segmented alias in order to allow for
                // routing on segments
                uuid = alias;
            }
        } else {
            uuid = alias;
        }

        return uuid;
    }

    private void showRoutingTable(TimedConcurrentHashMap<String, Link> routingTable)
    {
        logger.info("Routing table:");
        foreach (KeyValuePair<string,Link> kvp in routingTable)
            logger.info(kvp.Key + "--->" + kvp.Value.ToString());

    }

    /// <summary>
    /// This is the core of the routing algorithm
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="msg"></param>
    /// <param name="routingTable"></param>
    /// <returns></returns>
	public Link route(String uuid, IMessage msg, TimedConcurrentHashMap<String, Link> routingTable) {


        showRoutingTable(routingTable);

        URIHelper helper = new URIHelper(msg.getSenderEndpointUri());
  
          
        IChannel inboundChannel = msg.getMessageContext().getInBoundChannel();

        if (inboundChannel != null)
            logger.info("Inbound channel : " + inboundChannel.ToString());
        else
            logger.warn("Inbound channel : null -> no channel will process message");

        if (msg.getMessageContext().getInBoundChannel() != null) {

            // Only populate routingtable with senderSegment/senderUuid if the
            // message enters the router through an incoming channel
            String destUuid = msg.getMessageContext().getInBoundLink().getDestinationUuid();
            String curSegment = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(destUuid);

            String senderEndpointUuid = helper.getEndpoint();
            String senderSegment = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(senderEndpointUuid);

            if (senderSegment.Equals(curSegment)) 

                // If senderEndpointUuid belongs to the same segment as the
                // incoming channel
                // populate routingtable with senderUuid
                //if (!routingTable.ContainsKey(senderEndpointUuid)) 
                    routingTable.put(senderEndpointUuid, msg.getMessageContext().getInBoundChannel().getOutLink(), linkTimeout, this);
                
             else 

                // If senderEndpointUuid not belongs to the same segment as the
                // incoming channel
                // populate routingtable with segment of the senderUuid
                //if (!routingTable.ContainsKey(senderSegment)) 
                     routingTable.put(senderSegment, msg.getMessageContext().getInBoundChannel().getOutLink(), linkTimeout, this);
                
            
        }

         Link link = null;
        // Possible bug: uuid has "." prefix
        if (uuid.StartsWith(".")) {
            logger.info("Possible bug: uuid starts with ., removing it to index routing table");
                uuid = uuid.Substring(1);
        }

         if (routingTable.ContainsKey(uuid))
         {
             link = routingTable[uuid];
             if (link != null)
                 logger.debug("Found link in routing table: " + link.ToString());
         }

        // route down
        if (link == null) {
            String toSegment = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(uuid);
            Link inboundLink = msg.getMessageContext().getInBoundLink();

            if (inboundLink == null) {
                // The message has not arrived this router through an inboundLink. It is invalid and can not be routed
                return null; 
            }

            String destUuid = inboundLink.getDestinationUuid();

            if (destUuid == null) {
                logger.warn(COOSInstanceName + ":destinationUuid is null on incoming link : " + inboundLink.getLinkId());
            }

            String curSegment = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(destUuid);

            if (!toSegment.Equals(curSegment)) {

                // route down
                while (!toSegment.Equals("")) {
                    link = routingTable[toSegment];

                    if (link != null) {
                        break;
                    } else {
                        toSegment = UuidHelper.getParentSegment(toSegment);
                    }
                }

                // route up
                /*if (link == null) {
                        if (curSegment != null) {
                                String parentSegment = UuidHelper.getParentSegment(curSegment);
                                if (parentSegment != null) {
                                        link = routingTable.get(parentSegment);
                                }
                        }
                }*/
            }
        }

        // route along defaultGw if set. Must be different from the incoming link
        if (link == null) {

            if ((defaultGw != null) && (defaultGw != msg.getMessageContext().getInBoundChannel().getOutLink())) {
                link = defaultGw;
            } else {

                if (defaultGw == null) {
                    logger.warn("Defaultgw is not set!");
                } else if (defaultGw == msg.getMessageContext().getInBoundChannel().getOutLink()) {
                    logger.warn("Incoming link is defaultgw!");
                }
            }
        }

        return link;
    }

    public void replyErrorReason(IMessage msg, String message) {

        // Only send error indication on type msg. i.e. from an endpoint
        if (msg.getHeader(IMessagePrimitives.TYPE).Equals(IMessagePrimitives.TYPE_MSG)) {
            msg.setReceiverEndpointUri(msg.getSenderEndpointUri());
            msg.setHeader(IMessagePrimitives.TYPE, IMessagePrimitives.TYPE_ERROR);
            msg.setHeader(IMessagePrimitives.HOPS, "1");
            msg.setHeader(IMessagePrimitives.ERROR_REASON, message);
            msg.setHeader(IMessagePrimitives.ERROR_CODE, "504");
            processMessage(msg);
        }
    }

    public IProcessor getDefaultProcessor() {
        return this;
    }

    
    ///<summary>
    ///Adding a link to the router. Can either be a link to an endpoint/router, or a link to another segment
    ///<param name="link"></param>
    ///<param name="routerUuid"></param>
    ///</summary>
    public void addLink(String routerUuid, Link link) /* throws ConnectingException */{

        if (UuidHelper.isUuid(routerUuid)) {
            // It is a router or endpoint

            String seg = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(routerUuid);
            IRoutingAlgorithm algorithm = routingAlgorithms[seg];

            if (algorithm == null) {
                throw new ConnectingException("Router is not attached to segment: " + seg);
            }

            RouterSegment rs = segmentMapping[seg];
            rs.setTimestamp(0);

            link.setDestinationUuid(routerUuid);
            bool result = links.TryAdd(link.getLinkId(), link);

            if ((link.getChannel() != null) && link.getChannel().isDefaultGw()) {
                defaultGw = link;
                logger.debug("Setting defaultgw " + link);
            }

            algorithm.publishLink(link);
            logger.debug(getCOOSInstanceName() + ": Adding link: " + link);
        } else {

            // it is a segment
            link.setDestinationUuid(routerUuid);
            bool result = links.TryAdd(link.getLinkId(), link);
            logger.debug(getCOOSInstanceName() + ": Adding link: " + link);
            routingAlgorithms[routerUuid].publishLink(link);
        }

        
    }

    public Link getLink(String destinationUuid) {

        if (destinationUuid != null) {

            foreach (Link link in links.Values) {

                if (link.getDestinationUuid().Equals(destinationUuid)) {
                    return link;
                }
            }
        }

        return null;
    }

    public void removeLinkById(String linkId) {
        Link link = links[linkId];

        if (link != null) {
            publishAndCleanUpAfterLinkRemoval(link);
        }
    }

    public void removeLink(String destinationUuid) {

        foreach (Link link in links.Values) {

            if (link.getDestinationUuid().Equals(destinationUuid)) {
                publishAndCleanUpAfterLinkRemoval(link);
            }
        }
    }


    private void publishAndCleanUpAfterLinkRemoval(Link link) {
        link.setCost(LinkCost.MAX_VALUE);
        routingAlgorithms[UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(link.getDestinationUuid())].publishLink(link);

        // remove from routing table can be done here.
        foreach (TimedConcurrentHashMap<String, Link> routingTable in routingTables.Values) {

            foreach (String uuidKey in routingTable.Keys) {
                Link l = routingTable[uuidKey];

                if ((l != null) && l.Equals((link))) {

                    //remove link to uuidKey
                    routingTable.remove(uuidKey);

                    //remove all aliases pointing at uuidKey
                    foreach (String aliasKey in aliasTable.Keys) {
                        String uuid = aliasTable[aliasKey];

                        if (uuidKey.Equals(uuid)) {
                            removeAlias(aliasKey);
                        }
                    }
                }
            }

            if (COOS != null)
                COOS.removeChannel(link.getDestinationUuid());
        }

        //Check if the destinationUuid was referenced in the alias table
        foreach (String aliasKey in aliasTable.Keys) {
            String uuid = aliasTable[aliasKey];

            if (link.getDestinationUuid().Equals(uuid)) {
                removeAlias(aliasKey);
            }
        }

        //Eventually remove the link
        if ((link.getChannel() != null) && !link.getChannel().isDefaultGw()) {
            Link outLink;
            bool result = links.TryRemove(link.getLinkId(),out outLink);
        }
    }


    public void addQoSClass(String QoSClass, bool isDefaultQoSClass) {
        this.QoSClasses.Add(QoSClass);

        if ((defaultQoSClass == null) || isDefaultQoSClass) {
            defaultQoSClass = QoSClass;
        }

        
        // create routing table for QoSclass
        if (!routingTables.ContainsKey(QoSClass)) {
            routingTables.TryAdd(QoSClass, new TimedConcurrentHashMap<String, Link>());
        }
    }

    public ConcurrentBag<String> getQoSClasses() {
        return this.QoSClasses;
    }

    public void addPreProcessor(RouterProcessor preProcessor) {
        preProcessor.setRouter(this);
        preProcessors.Add(preProcessor);
    }

    public void addPostProcessor(RouterProcessor postProcessor) {
        postProcessor.setRouter(this);
        postProcessors.Add(postProcessor);
    }

    public String getCOOSInstanceName() {
        return this.COOSInstanceName;
    }

    public void setCOOSInstanceName(String COOSInstanceName) {
        this.COOSInstanceName = COOSInstanceName;

    }

    public void setCOOS(COOS coos) {
        this.COOSInstanceName = coos.getName();
        this.COOS = coos;
    }

    public COOS getCOOS() {
        return COOS;
    }

    public void setSegmentMappings(ConcurrentDictionary<String, RouterSegment> segmentMapping) {
        this.segmentMapping = new ConcurrentDictionary<String, RouterSegment>(segmentMapping);
    }

    public void addSegmentMapping(String segment, String routerUUID, String routerAlgorithm) {
        this.segmentMapping.TryAdd(segment, new RouterSegment(segment, routerUUID, routerAlgorithm, false));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public  void addRoutingAlgorithm(String routerUuid, IRoutingAlgorithm routingAlgorithm) {

        
        if (!UuidHelper.isRouterUuid(routerUuid)) {
            throw new ArgumentOutOfRangeException("Router uuid must start with prefix " + IRouterPrimitives.ROUTER_UUID_PREFIX);
        }

        IProcessor processor = null;

        bool result = this.routingAlgorithms.TryAdd(UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(routerUuid), routingAlgorithm);
        routerUuids.Add(routerUuid);

        if (routerUuids.Count > 1) {

            foreach (String uuid in routerUuids) {

                if (!routerUuid.Equals(uuid)) {
                    Link link = new Link(0);
                    link.addFilterProcessor(processor);
                    link.setChainedProcessor(this);

                    try {
                        addLink(UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(uuid), link);
                    } catch (Exception e) {
                        logger.error("Unknown Exception caught.", e);
                    }

                    link = new Link(0);
                    link.addFilterProcessor(processor);
                    link.setChainedProcessor(this);

                    try {
                        addLink(UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(routerUuid), link);
                    } catch (Exception e) {
                        logger.error("Unknown Exception caught.", e);
                    }
                }
            }
        }

        if (running) {

            try {
                routingAlgorithm.start();
            } catch (Exception e) {
                logger.error("Unknown Exception caught.", e);
            }
        }
    }

    
    /// <summary>
    /// Takes care of updating
    /// </summary>
	private void aliasesUpdated() {

        // remove global aliases
        foreach (Link link in links.Values) {
            LinkedList<string> aliases = link.getAlises();

            lock (aliases) {

                //Synchronizes on alias since concurrent access to this member can cause exceptions,
                //Can not use ConcurrentHashmap cause of java 1.3
                //for (Iterator iterator = aliases.iterator(); iterator.hasNext();) {
                //    String alias = (String) iterator.next();

                foreach(string alias in aliases)
                    if (alias.StartsWith(IRouterPrimitives.DICO_SEGMENT + ".") && !aliasTable.ContainsKey(alias)) {
                       aliases.Remove(alias);
                    }
                
            }
        }

        // add global aliases in crossegment links
        foreach (String alias in aliasTable.Keys) {

            if (alias.StartsWith(IRouterPrimitives.DICO_SEGMENT + ".")) {
                String aliasUuid = aliasTable[alias];
                String aliasSegment = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(aliasUuid);

                foreach (Link link in links.Values) {
                    String segment = UuidHelper.getSegmentFromSegmentOrEndpointUuid(link.getDestinationUuid());

                    if (aliasSegment.Equals(segment) && UuidHelper.isSegment(link.getDestinationUuid())) {
                        link.addAlias(alias);
                    }
                }
            }
        }
    }

    public String getDefaultQoSClass() {
        return defaultQoSClass;
    }

    public IRoutingAlgorithm getRoutingAlgorithm(String segment) {
        return routingAlgorithms[segment];
    }

    public ConcurrentDictionary<String, TimedConcurrentHashMap<String, Link>> getRoutingTables() {
        return routingTables;
    }

    public ConcurrentDictionary<String, Link> getRoutingTable(String qos) {
        return routingTables[qos];
    }

    public ConcurrentDictionary<String, Link> getLinks() {
        return links;
    }

    public Link getLinkById(String id) {

        return links[id];
    }

    public void start() /*throws Exception */{

        String linkTimeoutStr = getProperty(LINK_TIMEOUT, LINK_TIMEOUT_DEFAULT);
        linkTimeout = 0; 

        
        if (linkTimeoutStr != null) {
            linkTimeout = int.Parse(linkTimeoutStr);
        }

        logger.info("Link timeout : " + linkTimeout.ToString() +" ms");
       

        // Start routing tables
        foreach (TimedConcurrentHashMap<string,Link> routingTable in routingTables.Values) {
            routingTable.start();
        }

        // Start routing algorithms
        if (!running && enabled) {

            foreach (IRoutingAlgorithm routingAlgorithm in routingAlgorithms.Values) {
                routingAlgorithm.start();
            }

            running = true;
        }
    }

   
	public void stop() /*throws Exception */ {

        // Stop routing algorithms
        foreach (IRoutingAlgorithm routingAlgorithm in routingAlgorithms.Values) {
            routingAlgorithm.stop();
        }

        // Stop routing tables
        foreach (TimedConcurrentHashMap<string,Link> routingTable in routingTables.Values) {
            routingTable.stop();
        }

        running = false;
    }

    public override String ToString() {
        return "Router " + COOSInstanceName;
    }

    public ConcurrentDictionary<String, String> getAliasTable() {
        return aliasTable;
    }

    public void putAlias(String alias, String toUuid) {
       
        String oldToUuid = null;

        // Do we have a previous uuid for alias
        if (aliasTable.ContainsKey(alias))
            oldToUuid = aliasTable[alias];

        // If yes then possible conflict
        if ((oldToUuid != null) && !oldToUuid.Equals(toUuid)) {
            logger.warn("Possible alias conflict for alias: " + alias + ". Was pointing to : " + oldToUuid + ". Now pointing to :" + toUuid + ".");
        }

        string newValue = aliasTable.AddOrUpdate(alias, toUuid, (aliasX, toUuidY) =>  toUuid );
      
        aliasesUpdated();
    }

    public void removeAlias(String alias) {
        String outAlias;
       bool removed = aliasTable.TryRemove(alias,out outAlias);
       
        aliasesUpdated();
    }

     public void addDynamicSegment(String segmentName, String routingAlg) /*throws ConnectingException */ {
        IRoutingAlgorithm prototype = COOS.getRoutingAlgorithm(routingAlg);

        if (prototype == null) {
            throw new ConnectingException("Routingalgorithm: " + routingAlg + " not defined. Refusing dynamic segment allocation: " + segmentName);
        }

        IRoutingAlgorithm algorithm = prototype.copy();
        UuidGenerator uuidGenerator = new UuidGenerator(IRouterPrimitives.ROUTER_UUID_PREFIX);
        String routerUuid = segmentName + "." + uuidGenerator.generateSanitizedId();
        bool result = segmentMapping.TryAdd(segmentName, new RouterSegment(segmentName, routerUuid, routingAlg, false, true));
        algorithm.init(routerUuid, this);
    }

    public RouterSegment getSegment(String segmentName) {
        if (segmentMapping.ContainsKey(segmentName))
            return segmentMapping[segmentName];
        else
            return null;
    }

    public  ConcurrentDictionary<String, RouterSegment> getSegmentMap() {
        return segmentMapping;
    }

    public void removeSegment(String segmentName) {
        RouterSegment outRouterSegment;
        IRoutingAlgorithm outRoutingAlgorithm;

        segmentMapping.TryRemove(segmentName, out outRouterSegment);
        routingAlgorithms.TryRemove(segmentName, out outRoutingAlgorithm);
        removeLink(segmentName);
    }

    public Link getDefaultGw() {
        return defaultGw;
    }

    public bool remove<K,V>(K key, TimedConcurrentHashMap<K, V> routingTable) {
        V genericlink = default(V);

        if (routingTable.ContainsKey(key))
            genericlink = routingTable[key];
        

        Link link = genericlink as Link;

        // not remove if default gw
        if ((link != null) && (link.getChannel() != null) && link.getChannel().isDefaultGw() && link.getDestinationUuid().Equals(key)) {
            return false;
        }

        //remove alias for key
        foreach (String alias in aliasTable.Keys) {

            if (aliasTable[alias].Equals(key)) {
                String outAlias;
               bool removed = aliasTable.TryRemove(alias, out outAlias);
            }
        }

        return true;
    }


}
}