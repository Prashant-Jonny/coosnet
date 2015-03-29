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

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

using javaNETintegration;

namespace Org.Coos.Messaging.Routing
{
//package org.coos.messaging.routing;

//import java.util.Collections;
//import java.util.HashMap;
//import java.util.LinkedList;
//import java.util.List;
//import java.util.Map;
//import java.util.Set;
//import java.io.Serializable;

//import org.coos.messaging.Link;

/**
 * @author Knut Eilif Husa, Tellu AS This class holds the link information used
 *         by the routing algorithm
 */
public class LinkCost /*: Serializable */ {

	/**
	 * 
	 */
	private static readonly long serialVersionUID = 5695860173037233991L;
	private string fromUuid;
	private string toUuid;
    // Info: List<> vs LinkedList<> performance
    // http://stackoverflow.com/questions/169973/when-should-i-use-a-list-vs-a-linkedlist
	private LinkedList<string> aliases = new LinkedList<string>();
	private ConcurrentDictionary<string, int> costMap = new ConcurrentDictionary<string, int>();
	private bool link = false;
	private LinkCost nextLink;
	private string linkId;

	private /*transient = java do not serialize this field*/ long timeStamp;
	public static readonly int MAX_VALUE = 10000000;

	public LinkCost(string fromUri, string toUri, string linkId, int cost) {
		
        this.fromUuid = fromUri;
		this.toUuid = toUri;
		this.linkId = linkId;

		if (cost >= MAX_VALUE) 
			link = false;
	    else 
			link = true;
		
		bool result = this.costMap.TryAdd(Link.DEFAULT_QOS_CLASS, cost);
		this.timeStamp = Time.currentTimeMillis();
		
	}

    public LinkCost(string fromUri, string toUri, string linkId, ConcurrentDictionary<string, int> costMap, LinkedList<string> aliases) {
		this.fromUuid = fromUri;
		this.toUuid = toUri;
		this.linkId = linkId;
		this.costMap = costMap;
        
        if (costMap.Values.Contains(MAX_VALUE)) {
			link = false;
		} else {
			link = true;
		}
		if (aliases != null && !(aliases.Count == 0)) {
			this.aliases = aliases;
		}
        this.timeStamp = Time.currentTimeMillis();
	}

	public string getKey() {
		StringBuilder buf = new StringBuilder();
		buf.Append(fromUuid);
		buf.Append(",");
		buf.Append(toUuid);
		return buf.ToString();
	}

	public LinkCost(LinkCost linkCost) {
		fromUuid = linkCost.getFromUuid();
		toUuid = linkCost.getToUuid();
		costMap = new ConcurrentDictionary<string, int>(linkCost.costMap);
		link = linkCost.link;
		nextLink = linkCost.getNextLink();
		linkId = linkCost.getLinkId();
		aliases = new LinkedList<string>(linkCost.getAliases());
		this.timeStamp = Time.currentTimeMillis();
	}

	public LinkedList<string> getAliases() {
		return aliases;
	}

	public long getTimeStamp() {
		return timeStamp;
	}

	public void setTimeStamp(long timeStamp) {
		this.timeStamp = timeStamp;
	}

	public ICollection<string>  getQoSClasses() {
        return costMap.Keys;
	}

	public string getFromUuid() {
		return fromUuid;
	}

	public void setFromUuid(string fromUuid) {
		this.fromUuid = fromUuid;
	}

	public string getToUuid() {
		return toUuid;
	}

	public void setToUuid(string toUuid) {
		this.toUuid = toUuid;
	}

	public int getCost(string qos) {
		if (!link) {
			return MAX_VALUE;
		}
        try
        {
            int cost = costMap[qos];
            return cost;
        }
        catch (System.Exception e)
        {
            return 0;
        }
	}

	public void setCost(string qos, int cost) {
		if (cost < MAX_VALUE) {
			link = true;
		}
		bool result = costMap.TryAdd(qos, cost);
	}

	public LinkCost getNextLink() {
		return nextLink;
	}

	public void setNextLinkCost(LinkCost nextLink) {
		this.nextLink = nextLink;
	}

	public string getLinkId() {
		return linkId;
	}

	public void setLinkId(string linkId) {
		this.linkId = linkId;
	}

	public void setLink(bool link) {
		this.link = link;
	}

	public bool isLink() {
		return link;
	}

    /// <summary>
    /// Allows .NET Equals to call java lowercase equals
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
   {
 	   return equals(obj);
   }

	public bool equals(object o) {
		
        if (this == o)
			return true;

		if (o == null || this.GetType() != o.GetType())
			return false;

		LinkCost linkCost = (LinkCost) o;

		if (!fromUuid.Equals(linkCost.fromUuid))
			return false;
		if (!linkId.Equals(linkCost.linkId))
			return false;
		if (!toUuid.Equals(linkCost.toUuid))
			return false;

		return true;
	}

    /// <summary>
    /// Allows .NET GetHashCode to call Java hashCode
    /// </summary>
    /// <returns></returns>
    public override int  GetHashCode()
    {
 	 return hashCode();
    }

	public int hashCode() {
		int result;
		result = fromUuid.GetHashCode();
		result = 31 * result + toUuid.GetHashCode();
		result = 31 * result + linkId.GetHashCode();
		return result;
	}
}
}