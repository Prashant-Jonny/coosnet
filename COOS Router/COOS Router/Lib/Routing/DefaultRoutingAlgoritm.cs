#define NET
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

using System;
#if NETMICROFRAMEWORK
using System.Collections;
#endif
#if NET
using System.Collections.Generic;
using System.Collections.Concurrent;
#endif
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.IO;

namespace Org.Coos.Messaging.Routing
{
    //package org.coos.messaging.routing;

    //import org.coos.messaging.util.Log;
    //import org.coos.messaging.util.LogFactory;
    //import org.coos.messaging.Link;
    //import org.coos.messaging.util.UuidHelper;

    //import java.io.StringWriter;

    //import java.util.*;


    /**
     * This router contains a set of methods that must be implemented by all routing algorithms
     *
     * @author Knut Eilif Husa, Tellu AS
     */
    public abstract class DefaultRoutingAlgorithm : IRoutingAlgorithm
    {
        protected ConcurrentDictionary<string, TimedConcurrentHashMap<string, Link>> routingTables;
        protected ConcurrentDictionary<string,string> aliasTable;
        protected Dictionary<string,string> properties = new Dictionary<string,string>();
        protected ConcurrentDictionary<string, Link> links;
        protected ConcurrentBag<string> QoSClasses;

        protected bool loggingEnabled = false;
        protected IRouter router;
        protected string routerUuid;
        protected string segment;
        
        protected readonly ILog logger = LogFactory.getLog(typeof(DefaultRoutingAlgorithm).Name);
        
        
        public virtual void init(string routerUuid, IRouter router)
        {
            this.router = router;
            this.routerUuid = routerUuid;
            segment = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(routerUuid);
            links = router.getLinks();
            QoSClasses = router.getQoSClasses();
            routingTables = router.getRoutingTables();
            aliasTable = router.getAliasTable();
            this.router.addRoutingAlgorithm(routerUuid, this);
        }

        //@SuppressWarnings("unchecked")
        public virtual void setRoutingTables(ConcurrentDictionary<string, TimedConcurrentHashMap<string, Link>> routingTables)
        {
            this.routingTables = routingTables;
        }

        public virtual string getRouterUuid()
        {
            return routerUuid;
        }

        public virtual void setLoggingEnabled(bool loggingEnabled)
        {
            this.loggingEnabled = loggingEnabled;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static void printRoutingTable(string routerUuid, string qos, TimedConcurrentHashMap<string, Link> routingTable, ILog logger)
        {

            StringWriter writer = new StringWriter();

            writer.Write("-------------Routing table for QoS:" + qos + " in router: " + routerUuid + "------------\n");

            foreach (string key in routingTable.Keys)

                //Iterator<string> keys = routingTable.keySet().iterator();

                //while (keys.hasNext()) {
                //    string uuid = keys.next();
                writer.Write("'" + key + "' --> '" + routingTable[key] + "'\n");


            writer.Write("-------------------------\n");

            logger.debug(writer.ToString());
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected   static void printAliasTable(string routerUuid, ConcurrentDictionary<string,string> aliasTable, ILog logger)
        {
            StringWriter writer = new StringWriter();

            writer.Write("-------------Alias table for router: " + routerUuid + "------------\n");

            //Iterator<string> keys = aliasTable.keySet().iterator();

            //while (keys.hasNext()) {
            //    string alias = keys.next();
            foreach (string key in aliasTable.Keys)
                writer.Write("'" + key + "' --> '" + aliasTable[key] + "'\n");


            writer.Write("-------------------------\n");

            logger.debug(writer.ToString());
        }

        public virtual void setLinks(ConcurrentDictionary<string, Link> links)
        {
            this.links = links;
        }


        //@SuppressWarnings("unchecked")
        public virtual void setProperties(Dictionary<string,string> properties)
        {
            this.properties = properties;
        }

        public virtual Dictionary<string,string> getProperties()
        {
            return new Dictionary<string,string>(properties);
        }

        public virtual string getProperty(string key)
        {
            return (string)properties[key];
        }

        public virtual void setProperty(string key, string value)
        {
            properties.Add(key, value);

        }

        public virtual IRoutingAlgorithm copy()
        {

            IRoutingAlgorithm algorithm = null;
           

        try {
#if JAVA
            algorithm = this.getClass().newInstance();
#endif
#if NET
            algorithm = Activator.CreateInstance(this.GetType()) as IRoutingAlgorithm;
#endif              
            algorithm.setProperties(new Dictionary<string,string>(properties));
#if JAVA
        } catch (InstantiationException e) {
            logger.error("InstantiationException ignored.", e);
        } catch (IllegalAccessException e) {
            logger.error("IllegalAccessException ignored.", e);
        }
#endif
#if NET
        } catch (Exception e)
        {
            logger.error("Exception ignored.", e);
        }
#endif
            return algorithm;

        }


        public virtual void stop() {}
        public virtual void start() {}

        public virtual string getAlgorithmName() {
            return String.Empty;
        }

        public virtual void publishLink(Link link) { }
        public virtual void processRoutingInfo(IMessage routingInfo) { }
        public virtual void setRoutingTables(ConcurrentDictionary<string, Link> routingTable) { }

        /// <summary>
        /// Helper method for value collection, convert to List of Link
        /// </summary>
        /// <param name="vcollcetion"></param>
        /// <returns></returns>
        //protected List<Link> valueCollectionToList(ConcurrentDictionary<string, Link>.ValueCollection vcollcetion)
        //{
        //    List<Link> links = new List<Link>();
        //    links.AddRange(vcollcetion);
        //    return links;

        //}
    }
}
