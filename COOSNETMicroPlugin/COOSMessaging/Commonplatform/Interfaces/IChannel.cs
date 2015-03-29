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
using Microsoft.SPOT;

using System.Collections;

namespace Org.Coos.Messaging
{
  


//import java.util.Vector;

/**
 * The interface for a Channel.
 * 
 * @author Knut Eilif Husa, Tellu AS
 * 
 */

    // Note JAVA to C# conversion : interfaces in c# cannot contain fields -> converter to properties
public interface IChannel : IProcessor {

// //    /**
//     * Returns the inLink of the Channel. The InLink direction is from this
//     * Channel such that messages processed by the channel will be delivered
//     * from the InLink
//     * 
//     * @return the InLink
//     */
    Link getInLink();

//    /**
//     * Sets the inLink of the Channel. The InLink direction is from this Channel
//     * such that messages processed by the channel will be delivered from the
//     * InLink
//     * 
//     * @param inLink
//     */
    void setInLink(Link inLink);

//    /**
//     * Returns the outLink of the Channel. The OutLink direction is into the
//     * Channel such that messages inserted into the OutLink will be processed by
//     * the Channel
//     * 
//     * @return the Outlink
//     */
    Link getOutLink();

//    /**
//     * Sets the outLink of the Channel. The OutLink direction is into the
//     * Channel such that messages inserted into the OutLink will be processed by
//     * the Channel
//     * 
//     * @param outLink
//     */
    void setOutLink(Link outLink);

//    /**
//     * Connects the Channel to the LinkManager. Connecting involves the
//     * following actions: Initiate the defined transport mechanism and send the
//     * "connect" message over the transport. Receive the For PluginChannels:
//     * CONNECT_ALLOCATED_UUID (Unique id for the Plugin allocated by the distant
//     * party) Receive the For RouterChannels: CONNECT_UUID (Unique id of the
//     * distant party)
//     * 
//     * @param linkManager
//     *            Manager handling links
//     * @throws ConnectingException
//     *             if connection fails, e.g. due to timeout
//     */
//// JAVA	void connect(Connectable linkManager) throws ConnectingException;
    void connect(IConnectable linkManager);

//    /**
//     * Disconnects the channel
//     */
    void disconnect();

//    /**
//     * Returns the transport mechanism
//     * 
//     * @return the transport mechanism
//     */
    ITransport getTransport();

//    /**
//     * Sets the Transport mechanism
//     * 
//     * @param transport
//     */
    void setTransport(ITransport transport);

//    /**
//     * Flag indicating whether this Channel side (Channels always exits in peer
//     * relations) shall initiate connection
//     * 
//     * @return boolean
//     */
    bool isInit();

//    /**
//     * Sets indicating whether this Channel side (Channels always exits in peer
//     * relations) shall initiate connection
//     * 
//     * @param init
//     */
    void setInit(bool init);

//    /**
//     * Sets the LinkManager
//     * 
//     * @param linkmanager
//     */
    void setLinkManager(IConnectable linkmanager);

//    /**
//     * Returns the LinkManager of this Channel
//     * 
//     * @return the LinkManager
//     */
   IConnectable getLinkManager();

//    /**
//     * Adds a protocol that this Channel handles. For PluginChannel and
//     * RouterChannel this is defaulted to coos
//     * 
//     * @param protocol
//     */
    void addProtocol(string protocol);

//    /**
//     * The protocol that this Channel can handle
//     * 
//     * @return
//     */
    ArrayList getProtocols();

//    /**
//     * Sets the Segment that this channel will connect to on the distant side
//     * 
//     * @param segment
//     */
    void setSegment(string segment);

//    /**
//     * Returns the segment of this Channel
//     * 
//     * @return
//     */
    string getSegment();

//    /**
//     * Returns a copy of this Channel
//     */
// hides inherited copy from Iprocessor    IProcessor copy();

//    /**
//     * Sets a Vector of protocols that this Channel shall handle
//     * 
//     * @param protocols
//     */
    void setProtocols(ArrayList protocols);

     bool isDefaultGw();
	
   void setDefaultGw(bool defaultGw);

    bool isConnected();
	
    void setConnected(bool connected);
	
    bool isReceiveRoutingInfo();
	
    void setReceiveRoutingInfo(bool receiveRoutingInfo);

}

}
