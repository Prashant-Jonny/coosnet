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
//package org.coos.messaging;

//import java.util.Hashtable;

//import org.coos.messaging.routing.RouterChannel;

using System.Collections;
using Org.Coos.Messaging.Routing;

namespace Org.Coos.Messaging
{

    ///<summary>This class is responsible for receiving requests for establishing new Channels</summary>
    ///<author>Knut Eilif Husa, Tellu AS</author>
    public interface IChannelServer : IService
    {

        ///<summary>Called during the initialization phase to set the LinkedProcessors (i.e Links and transports) 
        ///that requests to connect</summary>
        void initializeChannel(ITransport transport);

#if JAVA
        void setChannelMappings(Hashtable<String, RouterChannel> channelMappings);
#endif

#if NETMICROFRAMEWORK
          ///<summary>
        /// The channel mappings contains the type of channel that is instantiated on
        /// a channel connect request. The mapping is based on a regexp on the uuid.
        /// In case the bus allocates uuid (which often will be the case for
        /// connecting plugins), the regexp should be based on the segment that the
        /// channel connects on
          /// </summary>
          /// <param name="channelMappings">the channel Mappings</param>
          void setChannelMappings(Hashtable channelMappings);
#endif
        /**
         * Method for adding a channel Mapping
         *
         * @param uuidRegexp
         *            Regular expression for mathing a uuid to a channel type
         * @param channel
         *            The channel prototype
         */
        void addChannelMapping(string uuidRegexp, RouterChannel channel);

        /**
         * Method setting the linkManager. The Linkmanager recevies the connecting
         * links
         *
         * @param linkManager
         *            the linkmanager
         */
        void setLinkManager(ILinkManager linkManager);


        /**
         * Sets the COOS instance name of the instance this Channel Server is
         * contained within
         *
         * @param coosInstance
         *            the coos instance name
         */
        void setCOOSInstance(COOS coosInstance);

    }
}
