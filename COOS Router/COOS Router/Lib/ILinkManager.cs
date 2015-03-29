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

//package org.coos.messaging;

//import java.util.Hashtable;
//import java.util.Map;
//import java.util.Vector;

//import org.coos.messaging.routing.RouterSegment;


using System.Collections.Generic;
using System.Collections.Concurrent;

using Org.Coos.Messaging.Routing;

namespace Org.Coos.Messaging
{

public interface ILinkManager : IConnectable {

    /**
     * Sets a set of aliases on a link.
     *
     * @param linkAliases
     *            - A vector of alias strings
     * @param link
     *            - the link to set the aliases on
     * @throws ProcessorException
     */
   
    void setLinkAliases(List<string> linkAliases, Link link);

    /**
     * The segment mappings contains the uuid of the COOS that this
     * channelserver belongs to based on the segment that the channel is
     * connecting
     *
     * @param segmentMapping
     *            the segment mappings
     */
// JAVA    void setSegmentMappings(Hashtable<String, RouterSegment> segmentMapping);

    void setSegmentMappings(ConcurrentDictionary<string, RouterSegment> segmentMapping);

    /**
     * Add a segment mapping
     *
     * @param segment
     *            the segment
     * @param segmentUuid
     *            the uuid of the coos in this segment
     */
    void addSegmentMapping(string segment, string segmentUuid, string routingAlgorithm);

    /**
     * Returns the RouterSegment matching the segmentName
     * @param segmentName
     * @return the RouterSegment
     */
    RouterSegment getSegment(string segmentName);

    /**
     * Returns the map of RouterSegments
     * @return map of RouterSegments
     */
    // JAVA Map<String, RouterSegment> getSegmentMap();
    ConcurrentDictionary<string, RouterSegment> getSegmentMap();

    /**
     * Adds dynamic segments
     * @param segmentName
     * @param routingAlg
     * @throws ConnectingException
     */
    void addDynamicSegment(string segmentName, string routingAlg);

}
}
