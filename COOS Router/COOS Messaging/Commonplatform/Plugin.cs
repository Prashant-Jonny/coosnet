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

//#define MANAGMENT

using System.Collections;


namespace Org.Coos.Messaging
{

    //import java.util.Vector;

    //import org.coos.messaging.jmx.ManagedObject;
    //import org.coos.messaging.jmx.ManagementFactory;


    /**
     * The Plugin, A holder class for all information associated with a plugin.
     *
     * @author Knut Eilif Husa, Tellu AS
     */
    public class Plugin
    {

        public static int DEFAULT_STARTLEVEL = 10;

        private IEndpoint endpoint;
        private ArrayList channels = new ArrayList();
        private int startLevel = DEFAULT_STARTLEVEL;

        /*
         * The object used for Monitoring and Management of the COOS instance (i.e. using JMX)
         */
#if MANAGMENT 
        private ManagedObject managedObject = null;
#endif

        public string getName()
        {
            return endpoint.getName();
        }

        public IEndpoint getEndpoint()
        {
            return endpoint;
        }

        public void setEndpoint(IEndpoint endpoint)
        {
            this.endpoint = endpoint;
            endpoint.setPlugin(this);
        }

        public string getEndpointState()
        {

            if (endpoint != null)
            {
                return endpoint.getEndpointState();
            }

            return null;
        }

        public int getStartLevel()
        {
            return startLevel;
        }

        public void setStartLevel(int startLevel)
        {
            this.startLevel = startLevel;
        }

        public void addChannel(IChannel channel)
        {
            channels.Add(channel);
        }

        public ArrayList getChannels()
        {
            return channels;
        }

        public void removeChannel(IChannel channel)
        {
            channels.Remove(channel);
        }

        /**
         * Connects all channels. Then starts the endpoint.
         * @throws ConnectingException Exception thrown if any of the
         * channels fails in connecting
         */
        // JAVA public void connect() throws ConnectingException {

        public void connect()
        {

            foreach (IChannel channel in channels)
                channel.connect(endpoint);
            
            endpoint.initializeEndpoint();

#if MANAGMENT
            /*
             * Register plugin for management.
             */
            managedObject = ManagementFactory.getPlatformManagementService().registerPlugin(this);
#endif
            }

        /**
         * Connects the provided Channel. Does NOT start the endpoint
         * @param channel the channel to connect
         * @throws ConnectingException thrown if the channel fails in connecting
         */

        // JAVA public void connect(Channel channel) throws ConnectingException {
        public void connect(IChannel channel)
        {

            channel.connect(endpoint);
        }

        /**
         * Shuts down a specific channel. Does not bring down the endpoint
         * @param channel
         */
        public void disconnect(IChannel channel)
        {
            channel.disconnect();
        }

        /**
         * Shuts down all channels connected to this endpoint. Shuts down the
         * endpoint
         */
        public void disconnect()
        {
            endpoint.shutDownEndpoint();

            for (int i = 0; i < channels.Count; i++)
            {
                IChannel channel = (IChannel)channels[i];
                channel.disconnect();
            }

#if MANAGMENT
            /*
             * Unregister plugin from management.
             */
            if (managedObject != null)
            {
                ManagementFactory.getPlatformManagementService().unregister(managedObject);
            }
#endif

        }

        #region Connected
        public bool Connected
        {
            get { return isConnected(); }
        }

        public bool isConnected()
        {
            bool connected = true;

            for (int i = 0; i < channels.Count; i++)
            {
                IChannel channel = (IChannel)channels[i];
                connected = connected && channel.isConnected();
            }

            return connected;
        }

        #endregion
    }
}
