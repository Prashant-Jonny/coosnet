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

using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

using java.util;
using javaNETintegration;

namespace Org.Coos.Messaging.Routing
{
//package org.coos.messaging.routing;

//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;
//import org.coos.messaging.util.UuidHelper;

//import java.util.HashSet;
//import java.util.Iterator;
//import java.util.LinkedList;
//import java.util.List;
//import java.util.Map;
//import java.util.Set;
//import java.util.Timer;
//import java.util.TimerTask;
//import java.util.Vector;
//import java.util.concurrent.ConcurrentHashMap;


/**
 * @author Knut Eilif Husa, Tellu AS
 *
 * Topology map of the COOS network
 */
public class TopologyMap {

    private readonly string routerUuid;
    static private ConcurrentDictionary<string, LinkCost> curLinkCosts = new ConcurrentDictionary<string, LinkCost>();
    private  LinkedList<ITopologyMapListener> listeners = new LinkedList<ITopologyMapListener>();
    private long notificationInterval = 200;
    static private long maxAge = 200;
    private Timer timer;
    static private bool dirty = false;

    static private readonly ILog logger = LogFactory.getLog(typeof(TopologyMap).FullName);

    public TopologyMap(string routerUri) {
        this.routerUuid = routerUri;
    }

    public TopologyMap(string routerUuid, long notificationInterval, long maxAgep) {
        this.routerUuid = routerUuid;
        this.notificationInterval = notificationInterval;
        maxAge = maxAgep;
    }

    public void update(List<LinkCost> linkCosts) {
        // Todo optimize serialization of linkCosts
        // System.out.println("Updating topology map router: " + routerUri);

        foreach (LinkCost linkCost in linkCosts) {
            
            LinkCost currentCost = curLinkCosts[linkCost.getKey()];

            if (currentCost != null) {

                if (!linkCost.getLinkId().Equals(currentCost.getLinkId())) {

                    // New link
                    LinkCost lc = new LinkCost(linkCost);
                    bool result = curLinkCosts.TryAdd(linkCost.getKey(), lc);

                    //logger.trace("New Link: " + linkCost.getKey() + " caused topologymap change.");
                    dirty = true;
                } else {

                    // check change in cost to trigger a recalculation of
                    // optimal paths
                    foreach (string qos in currentCost.getQoSClasses()) {
                        int curCost = currentCost.getCost(qos);
                        int candCost = linkCost.getCost(qos);

                        if (curCost != candCost) {

                            //logger.trace("Link: " + linkCost.getKey() + " caused topologymap change," +
                            //              "current cost: "+curCost + ", candidate cost: "+candCost);
                            dirty = true;

                            break;
                        }
                    }

                    foreach (string qos in linkCost.getQoSClasses()) {

                        // check change in cost
                        int curCost = currentCost.getCost(qos);
                        int candCost = linkCost.getCost(qos);

                        if (curCost != candCost) {

                            //logger.trace("Link: " + linkCost.getKey() + " caused topologymap change," +
                            //              "current cost: "+curCost + ", candidate cost: "+candCost);
                            dirty = true;

                            break;
                        }
                    }

                    // check change in alias to trigger a recalculation of
                    // optimal paths
                    // Not necessary, but else we need to call a separate
                    // interface to deal with this
                    foreach (string alias in currentCost.getAliases()) {

                        if (!linkCost.getAliases().Contains(alias)) {

                            //logger.trace("Link: " + linkCost.getKey() + " caused topologymap change," +
                            //              "new alias: "+alias);
                            dirty = true;

                            break;
                        }
                    }

                    foreach (string alias in linkCost.getAliases()) {

                        if (!currentCost.getAliases().Contains(alias)) {

                            //logger.trace("Link: " + linkCost.getKey() + " caused topologymap change," +
                            //              "new alias: "+alias);
                            dirty = true;

                            break;
                        }
                    }

                    LinkCost lc = new LinkCost(linkCost);
                    bool result = curLinkCosts.TryAdd(linkCost.getKey(), lc);

                }

            } else {
                LinkCost lc = new LinkCost(linkCost);
                bool result = curLinkCosts.TryAdd(linkCost.getKey(), lc);
                dirty = true;
            }
        }

        if (dirty) {

            // Call listeners
           
            foreach(ITopologyMapListener listener in listeners)
                listener.notifyChanged(this);

            // remove infinitePaths after listeners have had a chance to react
            // on infinite path links
            //IEnumerable<LinkCost> iter = curLinkCosts.Values.GetEnumerator();
            
            foreach(LinkCost linkCost in curLinkCosts.Values)
            //{

            //while (iter.hasNext()) {
            //    LinkCost linkCost = iter.next();

                if (!linkCost.isLink()) {
                    logger.trace("Removing link: " + linkCost.getKey() + " from topologyMap");
                    //iter.remove();
                    LinkCost lcost;
                    bool result = curLinkCosts.TryRemove(linkCost.getKey(),out lcost);
                }
            

            dirty = false;
        }

    }

    public void update(LinkCost linkCost) {
        List<LinkCost> arg = new List<LinkCost>();
        arg.Add(linkCost);
        update(arg);
    }

    public LinkedList<string> getAliases(string uuid) {
        LinkedList<string> res = new LinkedList<string>();
        
        foreach (LinkCost lc in curLinkCosts.Values) {

            if (lc.getToUuid().Equals(uuid) && !(lc.getAliases().Count == 0)) 
                foreach(string alias in lc.getAliases())
                  res.AddLast(alias);
            
        }

        return res;
    }

    public void addListener(ITopologyMapListener listener) {
        listeners.AddLast(listener);
    }

    public void removeListener(ITopologyMapListener listener) {
        listeners.Remove(listener);
    }

    public string getRouterUuid() {
        return routerUuid;
    }

    public bool isNeighbourNode(string uuid) {
        return isNeighbourNode(routerUuid, uuid);

    }

    public bool isNeighbourNode(string fromUuid, string toUuid) {
        StringBuilder buf = new StringBuilder();
        buf.Append(fromUuid);
        buf.Append(",");
        buf.Append(toUuid);

        return curLinkCosts[buf.ToString()] != null;
    }

    public HashSet<string> getNodeUuids() {

        // return new HashSet(nodeUris);
        HashSet<string> nodeUuid = new HashSet<string>();

        foreach (LinkCost lc in curLinkCosts.Values) {
            bool resultFromUuid = nodeUuid.Add(lc.getFromUuid()); // true, if added, false otherwise
            bool resultToUuid = nodeUuid.Add(lc.getToUuid());
        }

        return nodeUuid;
    }

    public LinkCost getLinkCost(string fromUuid, string toUuid) {
        StringBuilder buf = new StringBuilder();
        buf.Append(fromUuid);
        buf.Append(",");
        buf.Append(toUuid);

        LinkCost lc = curLinkCosts[buf.ToString()];

        if (lc != null) {
            return lc;
        } else {
            return new LinkCost(fromUuid, toUuid, "-1", LinkCost.MAX_VALUE);
        }
    }

    public LinkCost getLinkCost(string toUuid) {
        return getLinkCost(routerUuid, toUuid);
    }

    public void start() {
        NotificationTask task = new NotificationTask();

        // JAVA timer = new Timer("TopologyMapTimer", true);
        timer = new Timer(task.NETrun, null, 0, notificationInterval);
        // JAVA timer.schedule(new NotificationTask(), 0, notificationInterval);
    }

    public void stop() {

        if (timer != null) {
            timer.Dispose();
        }
    }

    public bool isEmpty() {

        foreach (LinkCost linkCost in curLinkCosts.Values) {

            if (UuidHelper.isUuid(linkCost.getToUuid())) {
                return false;
            }
        }

        return true;
    }

    private class NotificationTask : TimerTask {

        public override bool cancel()
        {
            throw new System.NotImplementedException();
        }

         public override void run() {

            // Check for aging
            foreach (LinkCost linkCost in curLinkCosts.Values) {
                long now = Time.currentTimeMillis();

                if ((linkCost.getTimeStamp() + maxAge) < now) {
                    linkCost.setLink(false);
                    dirty = true;
                    logger.trace("Link: " + linkCost.getKey() + " expired");
                }
            }

        }

         public void NETrun(object state)
         {
             run();
         }
    }
}
}