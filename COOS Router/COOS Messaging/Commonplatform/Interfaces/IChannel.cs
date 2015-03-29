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


using System.Collections.Generic;

namespace Org.Coos.Messaging
{
  


//import java.util.Vector;


    // Note JAVA to C# conversion : interfaces in c# cannot contain fields -> converter to properties
    /// <summary>
    /// The interface for a Channel
    /// </summary>
    /// <author>Knut Eilif Husa, Tellu AS</author>
public interface IChannel : IProcessor {


    /// <summary>
    /// Returns the inLink of the Channel. The InLink direction is from this
    /// Channel such that messages processed by the channel will be delivered
    /// from the InLink
    /// </summary>
    /// <returns>the InLink</returns>
    Link getInLink();


    /// <summary>
    /// Sets the inLink of the Channel. The InLink direction is from this Channel
    /// such that messages processed by the channel will be delivered from the
    /// InLink
    /// </summary>
    /// <param name="inLink"></param>
    void setInLink(Link inLink);


    /// <summary>
    /// Returns the outLink of the Channel. The OutLink direction is into the
    /// Channel such that messages inserted into the OutLink will be processed by
    /// the Channel
    /// </summary>
    /// <returns>the Outlink</returns>
    Link getOutLink();


    /// <summary>
    /// Sets the outLink of the Channel. The OutLink direction is into the
    /// Channel such that messages inserted into the OutLink will be processed by
    /// the Channel
    /// </summary>
    /// <param name="outLink"></param>
    void setOutLink(Link outLink);
        

//// JAVA	void connect(Connectable linkManager) throws ConnectingException;
    /// <summary>
    /// Connects the Channel to the LinkManager. Connecting involves the
    /// following actions: Initiate the defined transport mechanism and send the
    /// "connect" message over the transport. Receive the For PluginChannels:
    /// CONNECT_ALLOCATED_UUID (Unique id for the Plugin allocated by the distant
    /// party) Receive the For RouterChannels: CONNECT_UUID (Unique id of the
    /// distant party)
    /// 
    /// </summary>
    /// <param name="linkManager">Manager handling links</param>
    /// <exception cref="ConnectingException"> if connection fails, e.g. due to timeout</exception>
    void connect(IConnectable linkManager);


    /// <summary>
    /// Disconnects the channel
    /// </summary>
    void disconnect();


    /// <summary>
    /// Returns the transport mechanism
    /// </summary>
    /// <returns>the transport mechanism</returns>
    ITransport getTransport();


    /// <summary>
    /// Sets the Transport mechanism
    /// </summary>
    /// <param name="transport">transport</param>
    void setTransport(ITransport transport);


    /// <summary>
    /// Flag indicating whether this Channel side (Channels always exits in peer
    /// relations) shall initiate connection
    /// </summary>
    /// <returns>boolean</returns>
    bool isInit();


    /// <summary>
    /// Sets indicating whether this Channel side (Channels always exits in peer
    /// relations) shall initiate connection
    /// </summary>
    /// <param name="init"></param>
    void setInit(bool init);


    /// <summary>
    /// Sets the LinkManager
    /// </summary>
    /// <param name="linkmanager"></param>
    void setLinkManager(IConnectable linkmanager);


    /// <summary>
    /// Returns the LinkManager of this Channel
    /// </summary>
    /// <returns></returns>
   IConnectable getLinkManager();


    /// <summary>
   /// Adds a protocol that this Channel handles. For PluginChannel and
   /// RouterChannel this is defaulted to coos
    /// </summary>
    /// <param name="protocol"></param>
    void addProtocol(string protocol);


    /// <summary>
    /// The protocol that this Channel can handle
    /// </summary>
    /// <returns></returns>
    List<string> getProtocols();


    /// <summary>
    /// Sets the Segment that this channel will connect to on the distant side
    /// </summary>
    /// <param name="segment"></param>
    void setSegment(string segment);


    /// <summary>
    /// Returns the segment of this Channel
    /// </summary>
    /// <returns></returns>
    string getSegment();


    /// <summary>
    /// Sets a Vector of protocols that this Channel shall handle
    /// </summary>
    /// <param name="protocols"></param>
    void setProtocols(List<string> protocols);

     bool isDefaultGw();
	
   void setDefaultGw(bool defaultGw);

    bool isConnected();
	
    void setConnected(bool connected);
	
    bool isReceiveRoutingInfo();
	
    void setReceiveRoutingInfo(bool receiveRoutingInfo);

}

}
