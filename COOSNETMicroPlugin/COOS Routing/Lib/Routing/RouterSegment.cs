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

namespace Org.Coos.Messaging.Routing
{
public class RouterSegment {

    private string name;

    private string routerUUID;

    private string routingAlgorithmName;

    private bool defaultSegment;

    private bool dynamicSegment;

    private long timestamp;


    public RouterSegment(string name, string routerUUID, string routingAlgorithmName, bool defaultSegment) : base() {
       
       
        this.name = name;
        this.routerUUID = routerUUID;
        this.routingAlgorithmName = routingAlgorithmName;
        this.defaultSegment = defaultSegment;
        this.dynamicSegment = false;
    }


    public RouterSegment(string name, string routerUUID, string routingAlgorithmName, bool defaultSegment, bool dynamicSegment) : base() {
        
        this.name = name;
        this.routerUUID = routerUUID;
        this.routingAlgorithmName = routingAlgorithmName;
        this.defaultSegment = defaultSegment;
        this.dynamicSegment = dynamicSegment;
    }


    public string getName() {
        return name;
    }

    public void setName(string name) {
        this.name = name;
    }

    public string getRouterUUID() {
        return routerUUID;
    }

    public void setRouterUUID(string routerUUID) {
        this.routerUUID = routerUUID;
    }

    public bool isDefaultSegment() {
        return defaultSegment;
    }

    public void setDefaultSegment(bool defaultSegment) {
        this.defaultSegment = defaultSegment;
    }


    public string getRoutingAlgorithmName() {
        return routingAlgorithmName;
    }

    public void setRoutingAlgorithmName(string routingAlgorithmName) {
        this.routingAlgorithmName = routingAlgorithmName;
    }


    public bool isDynamicSegment() {
        return dynamicSegment;
    }


    public void setDynamicSegment(bool dynamicSegment) {
        this.dynamicSegment = dynamicSegment;
    }


    public long getTimestamp() {
        return timestamp;
    }


    public void setTimestamp(long timestamp) {
        this.timestamp = timestamp;
    }


     public override string ToString() {
        return name;
    }


}}
