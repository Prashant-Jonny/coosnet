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
//package org.coos.messaging.routing;

//import java.util.Collection;
//import java.util.Map;

//import org.coos.messaging.COOS;
//import org.coos.messaging.Link;
//import org.coos.messaging.LinkManager;
//import org.coos.messaging.Message;
//import org.coos.messaging.Processor;
//import org.coos.messaging.Service;

using System.Collections;

namespace Org.Coos.Messaging.Routing
{

/**
 * @author Knut Eilif Husa, Tellu AS The router routes messages between links
 */
public interface IRouter : IProcessor, ILinkManager, IService {

	

	/**
	 * The routing action. This method searches through the routing table and
	 * finds the correct link If Link for UUID is not present the method
	 * searches through the routing for the segment part of the UUID. If Link
	 * for segment part of UUID is not present the method searches through the
	 * routing for the parents segment the UUID If a default gateway link is
	 * present it is returned If Link is not present the method returns null.
	 * 
	 * @param uuid
	 *            The UUID with segments
	 * @param msg
	 *            The message to be routed
	 * @param routingTable
	 *            The correct routing table according to QoS requirements
	 * @return Link or null if not present
	 */
#if JAVA
	 Link route(string uuid, IMessage msg, Map<string, Link> routingTable);
#endif
#if NETMICROFRAMEWORK
    Link route(string uuid, IMessage msg, Hashtable routingTable);
#endif
	
	string resolveAlias(IMessage msg);

	/**
	 * This method returns an error message to the msg sender
	 * 
	 * @param msg
	 *            Message that shall be responded with an error message
	 * @param message
	 *            The error message string
	 */
	void replyErrorReason(IMessage msg, string message);

	/**
	 * This method adds a routing algorithm to the router and attaches it so a
	 * segment
	 * 
	 * @param segment
	 *            the segment name
	 * @param routingAlgorithm
	 *            the routing algorithm
	 */
	void addRoutingAlgorithm(string segment, IRoutingAlgorithm routingAlgorithm);

	/**
	 * This method retrieves the RoutingAlgorithm according to segment
	 * 
	 * @param segment
	 *            the segment in question
	 * @return the Routing algorithm
	 */
	IRoutingAlgorithm getRoutingAlgorithm(string segment);

	/**
	 * This method returns all routing tables of the router as a Map
	 * 
	 * @return map of the routing tables of the router
	 */
#if JAVA
	Map<string, TimedConcurrentHashMap<string, Link>> getRoutingTables();
#endif
#if NETMICROFRAMEWORK
	Hashtable getRoutingTables();
#endif
	/**
	 * This method returns routing table according to qos indicated
	 * 
	 * @param qos
	 *            the qos in question
	 * @return the Routing table
	 */
#if JAVA
	Map<string, Link> getRoutingTable(string qos);
#endif
#if NETMICROFRAMEWORK
	Hashtable getRoutingTable(string qos);
#endif
	/**
	 * Returns a map of the unique linkUUIDs and the Links
	 * 
	 * @return Map of Links
	 */
#if JAVA
	Map<string, Link> getLinks();
#endif
#if NETMICROFRAMEWORK
    Hashtable getLinks();
#endif
	/**
	 * Returns Link based on unique linkUUID
	 * 
	 * @param uuid
	 * @return Link
	 */
	Link getLinkById(string uuid);

	/**
	 * Enables or disables the router
	 * 
	 * @param enabled
	 */
	void setEnabled(bool enabled);

	/**
	 * Enables or disables logging for this router
	 * 
	 * @param loggingEnabled
	 */
	void setLoggingEnabled(bool loggingEnabled);

	/**
	 * Adds a QoS class that this router shall maintain routing table for
	 * 
	 * @param QoSClass
	 *            The qoS class
	 * @param isDefaultQosClass
	 *            true indicates that the router shall route on this QoS class
	 *            if none indicated in the message
	 */
	void addQoSClass(string QoSClass, bool isDefaultQosClass);

	/**
	 * Returns the Default QoS class
	 * 
	 * @return default QoS class
	 */
	 string getDefaultQoSClass();

	/**
	 * returns all QoS classes of the router
	 * 
	 * @return QoS classes
	 */
#if JAVA
	Collection<string> getQoSClasses();
#endif
#if NETMICROFRAMEWORK
    ArrayList getQosClasses();
#endif
	/**
	 * Adds a RouterProcessor as a preprocessor. This processor will be called
	 * prior to the routing step
	 * 
	 * @param the
	 *            preProcessor
	 */
	void addPreProcessor(RouterProcessor preProcessor);

	/**
	 * Adds a RouterProcessor as a postprocessor. This processor will be called
	 * after the routing step
	 * 
	 * @param the
	 *            postProcessor
	 */
	void addPostProcessor(RouterProcessor postProcessor);

	/**
	 * Sets the InstanceName of the RouterNode. Same as Coos instance namee
	 * 
	 * @param instanceName
	 */
	void setCOOS(COOS coos);
	
	COOS getCOOS();

	/**
	 * Returns the COOS instance name
	 * 
	 * @return the instanceName
	 */
	string getCOOSInstanceName();

	/**
	 * Returns the alias table
	 * 
	 * @return the alias table
	 */
#if JAVA
	Map<string, string> getAliasTable();
#endif
#if NETMICROFRAMEWORK
    Hashtable getAliasTable();
#endif
	/**
	 * Puts an alias into the alias table
	 * 
	 * @param alias the alias
	 * 
	 * @param toUuid the uuid to associate the alias with
	 */
    void putAlias(string alias, string toUuid);

	/**
	 * Remove an alias from the alias table
	 * 
	 * @param alias the alias to remove
	 */
	void removeAlias(string alias);


	void setCOOSInstanceName(string str);


	void removeSegment(string segment);
	
	 Link getDefaultGw();

}
}