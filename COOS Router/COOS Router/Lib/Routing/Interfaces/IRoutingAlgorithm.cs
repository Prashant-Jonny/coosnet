//#define NETMICROFRAMEWORK
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
//package org.coos.messaging.routing;

//import java.util.Map;

//import org.coos.messaging.Link;
//import org.coos.messaging.Message;
//import org.coos.messaging.Service;

#if NETMICROFRAMEWORK
using System.Collections;
#endif
#if NET
using System.Collections.Generic;
using System.Collections.Concurrent;
#endif

namespace Org.Coos.Messaging.Routing
{

/**
 * @author Knut Eilif Husa, Tellu AS
 * 
 */
public interface IRoutingAlgorithm : IService {

    /**
     * Initialises the algorithm with the routerUuid (segment dependent) and the router
     * @param routerUuid the router uuid
     * @param router the router
     */
	 void init(string routerUuid, IRouter router);
	
	/**
	 * Sets the routing table into the algorithm
	 * @param routingTable
	 */
// JAVA	void setRoutingTables(Map<string, Link> routingTable);
#if NETMICROFRAMEWORK
    void setRoutingTables(Hashtable routingTable);
#endif
#if NET
     void setRoutingTables(ConcurrentDictionary<string, Link> routingTable);
#endif
	/**
	 * Returns the uuid of the router in the router segment that this algorithm serves
	 * @return the uuid of the router
	 */
	string getRouterUuid();
	
	/**
	 * 
	 * The method called with message that is destined towards this router
	 * @param routingInfo
	 */
	void processRoutingInfo(IMessage routingInfo);

	/**
	 * Enables logging info of this algorithm. Typically writers out the routing tables
	 * @param loggingEnabled
	 */
	void setLoggingEnabled(bool loggingEnabled);

	/**
	 * Sets the links used by the algorithm into the algorithm
	 * @param links the links
	 */
	// JAVA void setLinks(Map<string, Link> links);

#if NETMICROFRAMEWORK
    void setLinks(Hashtable links);
#endif
#if NET
    void setLinks(ConcurrentDictionary<string,Link> links);
#endif
	/**
	 * Publishes link into the segment that this algorithm serves
	 * @param link the link to be published by the algorithm
	 */
	void publishLink(Link link);
	
	/**
	 * Copies the algorithm
	 * @return a cloned instance of the algorithm
	 */
	IRoutingAlgorithm copy();
	
	/**
	 * Returns the algorithm name
	 * @return the algorithm name
	 */
	string getAlgorithmName();

}}
