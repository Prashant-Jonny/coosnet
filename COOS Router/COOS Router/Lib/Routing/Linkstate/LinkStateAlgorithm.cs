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
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Text;

using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;

using javaNETintegration;

namespace Org.Coos.Messaging.Routing
{
//package org.coos.messaging.routing;

//import org.coos.messaging.Link;
//import org.coos.messaging.Message;
//import org.coos.messaging.impl.DefaultMessage;
//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.UuidHelper;

//import java.io.StringWriter;

//import java.util.Collection;
//import java.util.HashMap;
//import java.util.HashSet;
//import java.util.Iterator;
//import java.util.LinkedList;
//import java.util.List;
//import java.util.Map;
//import java.util.Random;
//import java.util.Set;
//import java.util.Timer;
//import java.util.TimerTask;
//import java.util.Vector;


/**
 * @author Knut Eilif Husa, Tellu AS
 *
 */
public class LinkStateAlgorithm : DefaultRoutingAlgorithm, ITopologyMapListener {

    public static readonly string ALG_NAME = "linkstate";
    public static readonly string REFRESH_INTERVAL = "refreshInterval";
    public static readonly string AGING_FACTOR = "agingFactor";
    public static readonly string EMPTY_SEG_FACTOR = "emptyDynamicSegmentFactor";


    private readonly Random r = new Random();
    private TopologyMap topologyMap;
    private Timer timer;
    private int refreshInterval = 100; // Default value
    static private int agingFactor = 5; // Default value
    private long emptyDynamicSegmentFactor = 2 * agingFactor; // Default value

    public LinkStateAlgorithm() {
    }

    public LinkStateAlgorithm(IRouter router, string routerUuid) {
        init(routerUuid, router);
    }

     public override  void init(string routerUuid, IRouter router) {

        // Setting the refreshinterval property
         string refIntvStr = null;
         if (properties.ContainsKey(REFRESH_INTERVAL))
           refIntvStr = properties[REFRESH_INTERVAL];
         
         if (refIntvStr != null) {
            refreshInterval = int.Parse(refIntvStr);
        }

        // Setting the aging factor property
         string agefactStr = null;

         if (properties.ContainsKey(AGING_FACTOR))
             agefactStr = AGING_FACTOR;

        if (agefactStr != null) {
            agingFactor = int.Parse(agefactStr);
        }

        // Setting the aging factor for the dynamic segment property
        string emptySegfactStr = null;

        if (properties.ContainsKey(EMPTY_SEG_FACTOR))
            emptySegfactStr = properties[EMPTY_SEG_FACTOR];

        if (emptySegfactStr != null) {
            emptyDynamicSegmentFactor = int.Parse(emptySegfactStr);
        }

        topologyMap = new TopologyMap(routerUuid, refreshInterval, agingFactor * refreshInterval);
        topologyMap.addListener(this);
        topologyMap.start();
         
        base.init(routerUuid, router);

    }

    public TopologyMap getTopologyMap() {
        return topologyMap;
    }

    public void setTopologyMap(TopologyMap topologyMap) {
        this.topologyMap = topologyMap;
    }

    public override void publishLink(Link link) {
        List<Link> links = new List<Link>();
        links.Add(link);
        broadcastRoutingInfo(links);
    }

    // @SuppressWarnings("unchecked")
    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void processRoutingInfo(IMessage routingInfo) {
        List<LinkCost> linkCosts = (List<LinkCost>) routingInfo.getBody();
        string s = "";

        for (int i = 0; i < linkCosts.Count; i++) {
            LinkCost linkCost = linkCosts[i];
            s += linkCost.getFromUuid() + "<->" + linkCost.getToUuid() + ": " +
                linkCost.getCost(Link.DEFAULT_QOS_CLASS) + ": " + linkCost.getAliases() + ", ";
        }

        logger.trace("Receiving on " + router.getCOOSInstanceName() + ", from " +
            routingInfo.getSenderEndpointUri() + " linkinfo: " + s);
        topologyMap.update(linkCosts);
    }

    private void calculateOptimalPaths() {
        QoSClasses = router.getQoSClasses();

        foreach (string qos in QoSClasses) {
            calculateOptimalPaths(qos);
        }
    }

    private void calculateOptimalPaths(string qos) {
        logger.debug(this.router.getCOOSInstanceName() + ": Calculating optimal paths for: " +
            topologyMap.getRouterUuid() + " QoS: " + qos);

        Dictionary<string, LinkCost> optimalPath = new Dictionary<string, LinkCost>();
        HashSet<string> uuids = topologyMap.getNodeUuids();
        uuids.Remove(topologyMap.getRouterUuid());

        // initialize optimal path with neighbours

        foreach(string uuid in uuids)
            optimalPath.Add(uuid,new LinkCost(topologyMap.getLinkCost(uuid)));
        

        while (!(uuids.Count == 0)) {
            LinkCost minimalCost = null;

            foreach(string uuid in optimalPath.Keys)
            {
            
            // identify node with smallest cost
            
                if (uuids.Contains(uuid)) {

                    if ((minimalCost == null) ||
                            ((optimalPath[uuid]).getCost(qos) < minimalCost.getCost(qos))) {
                        minimalCost = optimalPath[uuid];
                    }
                }
            }

            string minimalCostUuid = minimalCost.getToUuid();
            uuids.Remove(minimalCostUuid);


            foreach(string nodeUuid in uuids) {
            //iter = uuids.iterator();

            //while (iter.hasNext()) {
            //    string nodeUuid = iter.next();

                if (topologyMap.isNeighbourNode(minimalCostUuid, nodeUuid)) {
                    int candidateCost = minimalCost.getCost(qos) +
                        topologyMap.getLinkCost(minimalCostUuid, nodeUuid).getCost(qos);
                    int currentCost;

                    if (optimalPath[nodeUuid] != null) {
                        currentCost = (optimalPath[nodeUuid]).getCost(qos);
                    } else {
                        currentCost = topologyMap.getLinkCost(nodeUuid).getCost(qos); // return
                                                                                      // large
                                                                                      // number
                    }

                    if (candidateCost < currentCost) {
                        LinkCost linkCost = optimalPath[nodeUuid];
                        linkCost.setCost(qos, candidateCost);
                        linkCost.setNextLinkCost(optimalPath[minimalCostUuid]);

                    }
                }
            }
        }

        IEnumerator<LinkCost> valIter = optimalPath.Values.GetEnumerator();
      

        //Iterator<LinkCost> valIter = optimalPath.values().iterator();

        //// populate routing table with smallest costs paths.
        while (valIter.MoveNext()) {
            LinkCost linkCost = valIter.Current;
            string toUuid = linkCost.getToUuid();

            while (linkCost.getNextLink() != null) {
                linkCost = linkCost.getNextLink();
            }
            // Now linkCost points to the link from the router

            Link l = links[linkCost.getLinkId()];

            if (linkCost.getCost(qos) < LinkCost.MAX_VALUE) {

                // insert Link into routing table
                if (l != null) {
                    routingTables[qos].put(toUuid, links[linkCost.getLinkId()]);

                    foreach (string alias in topologyMap.getAliases(toUuid)) {
                        router.putAlias(alias, toUuid);
                    }
                }
            } else {

                // remove Link from routing table
                routingTables[qos].remove(toUuid);

                //Always try to remove any aliases
                foreach (string alias in topologyMap.getAliases(toUuid)) {
                    router.removeAlias(alias);
                }

                if ((l != null) && (l.getChannel() != null) && !l.getChannel().isDefaultGw()) {

                    //Never remove the default gateway
                    Link outLink;
                    links.TryRemove(linkCost.getLinkId(),out outLink);

                    if (loggingEnabled) {
                        logger.debug(routerUuid + " removing from routerTable Link to: " +
                            linkCost.getToUuid());
                    }
                }
            }
        }

        if (loggingEnabled) {
            printRoutingTable(routerUuid, qos, routingTables[qos], logger);
            printAliasTable(routerUuid, aliasTable, logger);
            printOptimalPath(routerUuid, qos, optimalPath, logger);
        }

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private static  void printOptimalPath(string routerUuid, string qos,
        Dictionary<string, LinkCost> optimalPath, ILog logger) {
        StringBuilder writer = new StringBuilder();
        writer.Append("-------------optimal paths for Qos: " + qos + " in router: " + routerUuid +
            "------------\n");

        //Iterator<string> keys = optimalPath.keySet().iterator();

        //while (keys.hasNext()) {
        //    string uuid = keys.next();

        foreach (string uuid in optimalPath.Keys) {

            writer.Append("'" + uuid + "': ");

            LinkCost linkCost = optimalPath[uuid];

            while (linkCost != null) {
                writer.Append("'" + linkCost.getFromUuid() + "', '" + linkCost.getToUuid() + "': " +
                    linkCost.getCost(qos));
                linkCost = linkCost.getNextLink();

                if (linkCost != null) {
                    writer.Append(" --> ");
                }
            }

            writer.Append("\n");
        }

        writer.Append("-------------------------\n");

        logger.debug(writer.ToString());

    }

    public void notifyChanged(TopologyMap topologyMap) {

        try {
            calculateOptimalPaths();
        } catch (Exception e) {
            logger.warn("Exception occured in " + topologyMap.getRouterUuid(), e);
        }

        if (topologyMap.isEmpty() && router.getSegment(segment).isDynamicSegment()) {
            RouterSegment rs = router.getSegment(segment);
            long now = Time.currentTimeMillis();

            if (rs.getTimestamp() == 0) {
                rs.setTimestamp(now);
            } else if ((rs.getTimestamp() + (emptyDynamicSegmentFactor * refreshInterval)) > now) {
                router.removeSegment(segment);

                foreach (TimedConcurrentHashMap<string, Link> routingTable in routingTables.Values) {
                    routingTable.remove(segment);
                }

                stop();
                logger.info("Removing dynamic segment: " + segment);
            }
        }
    }

    public override void start() {
        //timer = new Timer("LinkStateThread", true);
         timer = new Timer(broadcastRoutingInfoAndSchedule,null,0,refreshInterval +r.Next((int)(0.1*refreshInterval)));
        broadcastRoutingInfoAndSchedule(null);
    }

    
    private void broadcastRoutingInfoAndSchedule(object state) {
      
        broadcastRoutingInfo(links.Values);

        //try {
           

        //    timer.schedule(new TimerTask() {

        //            @Override public void run() {
        //                broadcastRoutingInfoAndSchedule();
        //            }
        //        }, refreshInterval + r.nextInt((int) (0.1 * refreshInterval)));
        //} catch (IllegalStateException e) {
            // Nothing to be done. This situation occures when the timer is
            // cancelled in the stop routine
            // and the broadcastRoutingInfoAndSchedule is started. I.e. the
            // routing algorithm is about to stop.
       // }
    }

    private void broadcastRoutingInfo(ICollection<Link> links) {

        try {
            string s = "";
           
            foreach (Link link in links) {
                s += link.getDestinationUuid() + ": " + link.getAlises() + ", ";
            }

            // Send routinginfo to all router elements in routing table

            HashSet<string> uuids = new HashSet<string>();
            // Get routing table entries for all qos parameters

            foreach (TimedConcurrentHashMap<string, Link> routingTable in routingTables.Values) {

                foreach (string uuid in routingTable.Keys) {

                    if ((routingTable[uuid] != null && routingTable[uuid].getChannel() != null) &&
                            !routingTable[uuid].getChannel().isReceiveRoutingInfo()) {
                        continue;
                    }

                    uuids.Add(uuid);
                }

            }

            /*for (Map routingTable : routingTables.values()) {
                    uuids.addAll(routingTable.keySet());
            }*/

            foreach(string uuid in uuids) {

                // Only sending routerinfo to routers
                if (UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(uuid).Equals(segment) &&
                        UuidHelper.isRouterUuid(uuid)) {
                    sendRouterInfo(uuid, constructRoutingInfo(uuid, links));
                    logger.trace("RouterInfo from: " + router.getCOOSInstanceName() + ", to:" +
                        uuid + ":: " + s);
                }
            }

            // Send info to self
            sendRouterInfo(routerUuid, constructLocalRoutingInfo(links));
        } catch (Exception e) {
            logger.error("Exception ignored.", e);
        }
    }

    //@SuppressWarnings("unchecked")
    private List<LinkCost> constructRoutingInfo(string receivingRouterUuid,
        ICollection<Link> links) {
        List<LinkCost> routingInfo = new List<LinkCost>();

        foreach (Link link in links) {
            string uuid = link.getDestinationUuid();
            string uuidSegment = UuidHelper.getSegmentFromSegmentOrEndpointUuid(uuid);

            //Do not propagate localcoos aliases to other coos nodes
            LinkedList<string> aliases = link.getAlises();
            LinkedList<string> broadCastAliases = new LinkedList<string>();

            foreach (string alias in aliases) {

                if (!alias.StartsWith(IRouterPrimitives.LOCAL_SEGMENT)) {
                    broadCastAliases.AddLast(alias);
                }
            }

            if (uuidSegment.Equals(segment) && !UuidHelper.isSegment(uuid)) {

                //The endpoints belonging to the router in the segment except from localcoos
                routingInfo.Add(new LinkCost(routerUuid, uuid, link.getLinkId(),
                        link.getCostMap(), broadCastAliases));
            } else if (UuidHelper.isSegment(uuid) && !uuidSegment.Equals(segment) &&
                    UuidHelper.isInParentChildRelation(uuidSegment,
                        UuidHelper.getSegmentFromSegmentOrEndpointUuid(receivingRouterUuid))) {

                //Other segments
                routingInfo.Add(new LinkCost(routerUuid, uuid, link.getLinkId(),
                        link.getCostMap(), link.getAlises()));
            }
        }

        return routingInfo;
    }

   // @SuppressWarnings("unchecked")
    private List<LinkCost> constructLocalRoutingInfo(
        ICollection<Link> links) {
        List<LinkCost> routingInfoLocal = new List<LinkCost>();

        foreach (Link link in links) {
            string uuid = link.getDestinationUuid();
            string uuidSegment = UuidHelper.getSegmentFromSegmentOrEndpointUuid(uuid);

            if (uuidSegment.Equals(segment) && !UuidHelper.isSegment(uuid)) {
                routingInfoLocal.Add(new LinkCost(routerUuid, uuid, link.getLinkId(),
                        link.getCostMap(), link.getAlises()));
            } else if (UuidHelper.isSegment(uuid) && !uuidSegment.Equals(segment)) {
                // Aliases are not part of cross segment linkcost to gateway
                // (local) topology map.

                // Aliases in cross segment linkcosts pointing at other
                // segment must not be added since this is where dico
                // aliases pointing
                // to uuids in own segment is located
                routingInfoLocal.Add(new LinkCost(routerUuid, uuid, link.getLinkId(),
                        link.getCostMap(), null));
            }
        }

        return routingInfoLocal;
    }

    private void sendRouterInfo(string uuid, List<LinkCost> routingInfo) {

        try {
            DefaultMessage msg = new DefaultMessage();
            msg.setReceiverEndpointUri("coos://" + uuid);
            msg.setSenderEndpointUri("coos://" + routerUuid);
            msg.setHeader(IMessagePrimitives.SERIALIZATION_METHOD, IMessagePrimitives.SERIALIZATION_METHOD_JAVA);
            msg.setHeader(IMessagePrimitives.TYPE, IMessagePrimitives.TYPE_ROUTING_INFO);
            msg.setBody(routingInfo);
            router.processMessage(msg);
        } catch (Exception e) {
            logger.error("SendRouterInfo. Exception ignored.", e);
        }

    }

    public override void stop() {
        topologyMap.stop();

        if (timer != null) {
            timer.Dispose();
        }
    }

     public override  string getAlgorithmName() {
        return ALG_NAME;
    }


}
}