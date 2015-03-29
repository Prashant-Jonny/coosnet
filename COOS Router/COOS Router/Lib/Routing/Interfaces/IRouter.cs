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

//import java.util.Collection;
//import java.util.Map;

//import org.coos.messaging.COOS;
//import org.coos.messaging.Link;
//import org.coos.messaging.LinkManager;
//import org.coos.messaging.Message;
//import org.coos.messaging.Processor;
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


    ///<summary>The router routes messages between links</summary>
    ///<author>Knut Eilif Husa, Tellu AS</author>
public interface IRouter : IProcessor, ILinkManager, IService {

#if JAVA
	 Link route(string uuid, IMessage msg, Map<string, Link> routingTable);
#endif
#if NETMICROFRAMEWORK
    Link route(string uuid, IMessage msg, Hashtable routingTable);
#endif
#if NET
     ///<summary>
    ///The routing action. This method searches through the routing table and
    ///finds the correct link If Link for UUID is not present the method
    ///searches through the routing for the segment part of the UUID. If Link
    ///for segment part of UUID is not present the method searches through the
    ///routing for the parents segment the UUID If a default gateway link is
    ///present it is returned If Link is not present the method returns null.
     ///</summary>
     ///<param name="uuid">The UUID with segments</param>
     ///<param name="msg">The message to be routed</param>
     ///<param name="routingTable">The correct routing table according to QoS requirements</param>
     ///<returns>Link or null if not present</returns>
     Link route(string uuid, IMessage msg, TimedConcurrentHashMap<string, Link> routingTable);
#endif

	string resolveAlias(IMessage msg);

	
    ///<summary>This method returns an error message to the msg sender</summary>
    ///<param name="message">The error message string</param>
    ///<param name="msg">Message that shall be responded with an error message</param>
	void replyErrorReason(IMessage msg, string message);

	
    ///<summary>This method adds a routing algorithm to the router and attaches it a segment</summary>
    ///<param name="routingAlgorithm">tge routing algorithm</param>
    ///<param name="segment">the segment name</param>
	void addRoutingAlgorithm(string segment, IRoutingAlgorithm routingAlgorithm);

	
	///<summary>This method retrieves the RoutingAlgorithm according to segment</summary>
    ///<param name="segment">the segment in question</param>
    ///<returns>the Routing algorithm</returns>
    IRoutingAlgorithm getRoutingAlgorithm(string segment);

	
#if JAVA
	Map<string, TimedConcurrentHashMap<string, Link>> getRoutingTables();
#endif
#if NETMICROFRAMEWORK
	Hashtable getRoutingTables();
#endif
#if NET
    ///<summary>Returns all routing tables of the router</summary>
    ConcurrentDictionary<string, TimedConcurrentHashMap<string, Link>> getRoutingTables();
#endif
	
#if JAVA
	Map<string, Link> getRoutingTable(string qos);
#endif
#if NETMICROFRAMEWORK
	Hashtable getRoutingTable(string qos);
#endif
#if NET
    ///<summary>Returns routing table according to qos indicated</summary>
    ///<param name="qos">the qos in action</param>
    ConcurrentDictionary<string, Link> getRoutingTable(string qos);
#endif
	
#if JAVA
	Map<string, Link> getLinks();
#endif
#if NETMICROFRAMEWORK
    Hashtable getLinks();
#endif
#if NET
    ///<summary>Unique linkUUIDs and the Links</summary>
    ConcurrentDictionary<string, Link> getLinks();
#endif
	
    ///<summary>Link based on unique linkUUID</summary>
    ///<param name="uuid"></param>
	Link getLinkById(string uuid);
    
    ///<summary>Enables or disables the router</summary>
    ///<param name="enabled"></param>
	void setEnabled(bool enabled);

	
    ///<summary>Enables or disables logging for this router</summary>
    ///<param name="loggingEnabled"></param>
	void setLoggingEnabled(bool loggingEnabled);

	
    ///<summary>Adds a QoS class that this router shall maintain routing table for</summary>
    ///<param name="QoSClass">the QoS class</param>
    ///<param name="isDefaultQosClass">True indicates that the router shall route on this Qos class if none indicated in the message</param>
	void addQoSClass(string QoSClass, bool isDefaultQosClass);

	/**
	 * Returns the Default QoS class
	 * 
	 * @return default QoS class
	 */
    ///<summary>The Default QoS class</summary>
	 string getDefaultQoSClass();

	
#if JAVA
	Collection<string> getQoSClasses();
#endif
#if NETMICROFRAMEWORK 
    ArrayList getQoSClasses();
#endif
#if NET
    ///<summary>All QoS classes of the router</summary>
     ConcurrentBag<string> getQoSClasses();
#endif
	
     ///<summary>Adds a RouterProcessor as a preprocessor. This processor will be called prior to the routing step</summary>
     ///<param name="preProcessor">preProcessor</param>
	void addPreProcessor(RouterProcessor preProcessor);

	
    ///<summary>Adds a RouterProcessor as a postprocessor. This processor will be called after the routing step</summary>
    ///<param name="postProcessor">postProcessor</param>
	void addPostProcessor(RouterProcessor postProcessor);

	
    ///<summary>Sets the instance name of the RouterNode. Same as Coos instance name</summary>
    ///<param name="coos">instance name</param>
	void setCOOS(COOS coos);
	
	COOS getCOOS();

	
    ///<summary>Returns the COOS instance name</summary>
	string getCOOSInstanceName();

	
#if JAVA
	Map<string, string> getAliasTable();
#endif
#if NETMICROFRAMEWORK 
    Hashtable getAliasTable();
#endif
#if NET
    ///<summary>Returns the alias table</summary>
    ConcurrentDictionary<string, string> getAliasTable();
#endif
	
    ///<summary>Puts an alias into the alias table</summary>
    ///<param name="alias">the alias</param>
    ///<param name="toUuid">the uuid to associate the alias with</param>
    void putAlias(string alias, string toUuid);

	
    ///<summary>Remove an alias from the alias table</summary>
    ///<param name="alias">the alias to remove</param>
	void removeAlias(string alias);


	void setCOOSInstanceName(string str);


	void removeSegment(string segment);
	
	 Link getDefaultGw();

}
}