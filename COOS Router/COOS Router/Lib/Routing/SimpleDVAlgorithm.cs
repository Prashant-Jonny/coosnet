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

using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Org.Coos.Messaging.Routing
{
//package org.coos.messaging.routing;

//import org.coos.messaging.Link;
//import org.coos.messaging.Message;
//import org.coos.messaging.impl.DefaultMessage;
//import org.coos.messaging.util.UuidHelper;

//import java.util.Collection;
//import java.util.HashMap;
//import java.util.HashSet;
//import java.util.Iterator;
//import java.util.LinkedList;
//import java.util.List;
//import java.util.Map;
//import java.util.Set;
//import java.util.Timer;
//import java.util.TimerTask;
//import java.util.Vector;


public class SimpleDVAlgorithm : DefaultRoutingAlgorithm , IHashMapCallback {

    public static readonly String ALG_NAME = "simpledv";
    public static readonly String REFRESH_INTERVAL = "refreshInterval";
    public static readonly String AGING_FACTOR = "agingFactor";

    private Timer timer;
    private int refreshInterval = 100; // Default value
    private int agingFactor = 5; // Default value

    public SimpleDVAlgorithm() {
    }

    public SimpleDVAlgorithm(IRouter router, String routerUuid) {
        init(routerUuid, router);
    }

    public override void init(String routerUuid, IRouter router) {

        // Setting the refreshinterval property
        
        if (properties.ContainsKey(REFRESH_INTERVAL))
            refreshInterval = int.Parse(properties[REFRESH_INTERVAL]);

        
        //Setting the aging factor property
        if (properties.ContainsKey(AGING_FACTOR)) 
          agingFactor = int.Parse(properties[AGING_FACTOR]);
        

        base.init(routerUuid, router);

    }

    //@SuppressWarnings("unchecked")
     public override void processRoutingInfo(IMessage routingInfo) {

        List<LinkCost> linkCosts = (List<LinkCost>) routingInfo.getBody();
        String s = "";

        foreach (TimedConcurrentHashMap<String, Link> routingTable in routingTables.Values) {

            for (int i = 0; i < linkCosts.Count; i++) {
                LinkCost linkCost = (LinkCost) linkCosts[i];
                s += linkCost.getFromUuid() + "<->" + linkCost.getToUuid() + ": " +
                    linkCost.getCost(Link.DEFAULT_QOS_CLASS) + ": " + linkCost.getAliases() + ", ";

                if (routingInfo.getMessageContext().getInBoundChannel() != null) {

                    //It is remote routingInfo
                    String toUuid = linkCost.getToUuid();
                    Link link = routingInfo.getMessageContext().getInBoundChannel().getOutLink();

                    if (linkCost.getCost(Link.DEFAULT_QOS_CLASS) < LinkCost.MAX_VALUE) {
                        ((TimedConcurrentHashMap<String,Link>) routingTable).put(toUuid, link,
                            agingFactor * refreshInterval, this);

                        LinkedList<String> aliases = linkCost.getAliases();

                        foreach (String alias in aliases) {
                            router.putAlias(alias, toUuid);
                        }
                    }
                } else {

                    //It is local routingInfo
                    String toUuid = linkCost.getToUuid();
                    Link link = links[linkCost.getLinkId()];

                    if ((linkCost.getCost(Link.DEFAULT_QOS_CLASS) < LinkCost.MAX_VALUE) &&
                            (link != null)) {
                        ((TimedConcurrentHashMap<String,Link>) routingTable).put(toUuid, link,
                            agingFactor * refreshInterval, this);

                        LinkedList<String> aliases = linkCost.getAliases();

                        foreach (String alias in aliases) {
                            router.putAlias(alias, toUuid);
                        }
                    }
                }
            }
        }

        logger.trace("Receiving on " + router.getCOOSInstanceName() + ", from " +
            routingInfo.getSenderEndpointUri() + " linkinfo: " + s);

        if (loggingEnabled) {

            foreach (String qos in routingTables.Keys) {
                printRoutingTable(routerUuid, qos, routingTables[qos], logger);
            }

            printAliasTable(routerUuid, aliasTable, logger);
        }
    }

    //@SuppressWarnings("unchecked")
     public override  void publishLink(Link link) {

        //from the local node
        LinkedList<Link> links = new LinkedList<Link>();
        links.AddLast(link);

        //Iterator<String> iter = routingTables.keySet().iterator();

        //while (iter.hasNext()) {
        //    String qos = iter.next();

            foreach (String qos in routingTables.Keys) {

            TimedConcurrentHashMap<string,Link> routingTable = routingTables[qos];

            if (link.getCost() < LinkCost.MAX_VALUE) {
                routingTable.put(link.getDestinationUuid(), link, agingFactor * refreshInterval, this);

                //This approach by broadcasting every new connection results in more traffic than
                //handling it on a scheduled basis. However, it might not be a problem.
                broadcastRoutingInfo(routingTable);
            }


        }

    }

     private void broadcastRoutingInfo(TimedConcurrentHashMap<string, Link> routingTable)
     {

        try {
            String s = "";

            foreach (Link link in routingTable.Values) {
                s += link.getDestinationUuid() + ": " + link.getAlises() + ", ";
            }

            // Send routinginfo to all router elements in routing table

            HashSet<String> uuids = new HashSet<String>();
            // Get routing table entries for all qos parameters


            foreach (Link link in routingTable.Values) {

                if ((link.getChannel() != null) && !link.getChannel().isReceiveRoutingInfo()) {
                    continue;
                }

                uuids.Add(link.getDestinationUuid());
            }

            foreach (String uuid in uuids) {

                // Only sending routerinfo to neighbour routers
                if (UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(uuid).Equals(segment) &&
                        UuidHelper.isRouterUuid(uuid)) {
                    sendRouterInfo(uuid, constructRoutingInfo(uuid, routingTable));
                    logger.trace("RouterInfo from: " + router.getCOOSInstanceName() + ", to:" +
                        uuid + ":: " + s);
                }
            }

            // Send info to self
            sendRouterInfo(routerUuid, constructLocalRoutingInfo(links.Values));
        } catch (Exception e) {
            logger.error("BroadcastRoutingInfo. Exception ignored.", e);
        }
    }

    //@SuppressWarnings("unchecked")
    private List<LinkCost> constructRoutingInfo(String receivingRouterUuid, TimedConcurrentHashMap<String, Link> routingTable) {
        List<LinkCost> routingInfo = new List<LinkCost>();

        foreach (String uuid in routingTable.Keys) {
            Link link = routingTable[uuid];
            String uuidSegment = UuidHelper.getSegmentFromSegmentOrEndpointUuid(uuid);

            LinkedList<String> broadCastAliases = new LinkedList<String>();
            ConcurrentDictionary<String, String> aliasTable = router.getAliasTable();

            foreach (String alias1 in aliasTable.Keys) {
                String uuid1 = aliasTable[alias1];

                //Do not propagate localcoos aliases to other coos nodes
                if (!alias1.StartsWith(IRouterPrimitives.LOCAL_SEGMENT) && uuid.Equals(uuid1)) {
                    broadCastAliases.AddLast(alias1);
                }

            }

            if (uuidSegment.Equals(segment) && !UuidHelper.isSegment(uuid) &&
                    !link.Equals(routingTable[receivingRouterUuid])) {

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

        routingInfo.Add(new LinkCost(routerUuid, routerUuid, null,
                new ConcurrentDictionary<String, int>(), new LinkedList<String>()));

        return routingInfo;
    }

    //@SuppressWarnings("unchecked")
    private List<LinkCost> constructLocalRoutingInfo(ICollection<Link> links) {
        List<LinkCost> routingInfoLocal = new List<LinkCost>();

        foreach (Link link in links) {
            String uuid = link.getDestinationUuid();
            String uuidSegment = UuidHelper.getSegmentFromSegmentOrEndpointUuid(uuid);

            if (uuidSegment.Equals(segment) && !UuidHelper.isSegment(uuid)) {
                routingInfoLocal.Add(new LinkCost(routerUuid, uuid, link.getLinkId(),link.getCostMap(), link.getAlises()));
            } else if (UuidHelper.isSegment(uuid) && !uuidSegment.Equals(segment)) {
                // Aliases are not part of cross segment linkcost to gateway
                // (local) topology map.

                // Aliases in cross segment linkcosts pointing at other
                // segment must not be added since this is where dico
                // aliases pointing
                // to uuids in own segment is located
                routingInfoLocal.Add(new LinkCost(routerUuid, uuid, link.getLinkId(), link.getCostMap(), null));
            }
        }

        return routingInfoLocal;
    }

    private void sendRouterInfo(String uuid, List<LinkCost> routingInfo) {

        try {
            DefaultMessage msg = new DefaultMessage();
            msg.setReceiverEndpointUri("coos://" + uuid);
            msg.setSenderEndpointUri("coos://" + routerUuid);
            msg.setHeader(IMessagePrimitives.SERIALIZATION_METHOD, IMessagePrimitives.SERIALIZATION_METHOD_JAVA);
            msg.setHeader(IMessagePrimitives.TYPE, IMessagePrimitives.TYPE_ROUTING_INFO);
            msg.setBody(routingInfo); // .NET problems here, does not have Java serialization
            router.processMessage(msg);
        } catch (Exception e) {
            logger.error("SendRouterInfo. Exception ignored.", e);
        }

    }

    private void broadcastRoutingInfoAndSchedule(object state) {





        broadcastRoutingInfo(routingTables[Link.DEFAULT_QOS_CLASS]);

      
        //try {
        //    timer.schedule(new TimerTask() {

        //            @Override public void run() {
        //                broadcastRoutingInfoAndSchedule();
        //            }
        //        }, refreshInterval);
        //} catch (IllegalStateException e) {
            // Nothing to be done. This situation occures when the timer is
            // cancelled in the stop routine
            // and the broadcastRoutingInfoAndSchedule is started. I.e. the
            // routing algorithm is about to stop.
        //}
    }


     public override void start() {
       // JAVA timer = new Timer("SimpleDVTimer", true);
         logger.info("Broadcast routing information refresh interval : " + refreshInterval.ToString() + " ms.");
         timer = new Timer(broadcastRoutingInfoAndSchedule, null, 0, refreshInterval);

      
    }

    public override  void stop() /*throws Exception */ {

        if (timer != null) {
            timer.Dispose();
        }

    }

     public bool remove<K,V>(K key, TimedConcurrentHashMap<K, V> routingTable) {
        
        
         
        V genericLink =  routingTable[key];
         Link link = genericLink as Link;

        
        if ((link != null) && (link.getChannel() != null) && link.getChannel().isDefaultGw() &&
                link.getDestinationUuid().Equals(key)) {
            return false;
        }

        foreach (String alias in aliasTable.Keys) {

            if (aliasTable[alias].Equals(key)) {
                String outAlias;
                aliasTable.TryRemove(alias,out outAlias);
            }
        }

        return true;
    }

    public override  String getAlgorithmName() {
        return ALG_NAME;
    }

}
}
