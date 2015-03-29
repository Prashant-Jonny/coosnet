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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging.Impl
{

//package org.coos.messaging.plugin;

//import java.util.Hashtable;

//import org.coos.messaging.Channel;
//import org.coos.messaging.ConnectingException;
//import org.coos.messaging.Endpoint;
//import org.coos.messaging.Link;
//import org.coos.messaging.Connectable;
//import org.coos.messaging.Message;
//import org.coos.messaging.Processor;
//import org.coos.messaging.ProcessorException;
//import org.coos.messaging.Transport;
//import org.coos.messaging.impl.DefaultChannel;
//import org.coos.messaging.impl.DefaultMessage;
//import org.coos.messaging.impl.DefaultProcessor;
//import org.coos.messaging.util.LogFactory;
//import org.coos.messaging.util.UuidHelper;

/**
 * A channel connecting plugins to coos instances (router nodes)
 * 
 * @author Knut Eilif Husa, Tellu AS
 * 
 */


   
public class PluginChannel : DefaultChannel {

	private static string PROPERTY_CONNECTION_TIMEOUT = "connectionTimeout";
	private static string PROPERTY_STARTUP_ORDERED = "startupOrdered";

    /// <summary>
    /// Plugin channel has a timeout for channel connect with router
    /// </summary>
    private int connectionTimeout = 10000; // Default value

	private bool startupOrdered = true; // Default value

    private string connectionErrorCause = null;

    /// <summary>
    /// Signals if channel has processed connect response
    /// </summary>
    public AutoResetEvent ConnectResponseEvent = new AutoResetEvent(false); 


    IEndpoint endpoint;

	public PluginChannel()  {
	}

	public long getConnectionTimeout() {
		return connectionTimeout;
	}

	public void setConnectionTimeout(int connectionTimeout) {
		this.connectionTimeout = connectionTimeout;
	}

	public bool isStartupOrdered() {
		return startupOrdered;
	}

	public void setStartupOrdered(bool startupOrdered) {
		this.startupOrdered = startupOrdered;
	}

	public void addInFilter(IProcessor filter) {
		inLink.addFilterProcessor(filter);
	}

	public void addOutFilter(IProcessor filter) {
		outLink.addFilterProcessor(filter);
	}

	public override void  setProperties(Dictionary<string,string> properties)  {
		
        base.setProperties(properties);

        if (properties.ContainsKey(PROPERTY_CONNECTION_TIMEOUT))
        {
            if (properties[PROPERTY_CONNECTION_TIMEOUT] != null)
            {
                connectionTimeout = int.Parse((string)properties[PROPERTY_CONNECTION_TIMEOUT]);
            }
        }

		/*String startupOrderedStr = (String) properties.get(PROPERTY_STARTUP_ORDERED);
		if (startupOrderedStr != null && startupOrderedStr.equals("false")) {
			startupOrdered = false;
		}*/

	}

    [MethodImpl(MethodImplOptions.Synchronized)]
	public override void connect(IConnectable connectable)  {
// Only one connect allowed on this specific PluginChannel instance at a time, others have to wait
		
			if (!(connectable is IEndpoint)) {
				throw new ConnectingException("This channel can only be connected to Endpoints.");
			}
			
			if (connected) {
				throw new ConnectingException("This channel is already connected.");
			}
			
			this.connectable = connectable;
			endpoint = (IEndpoint) connectable;
			
			try {

                // If no transport is defined, then use Jvm transport
				if (transport == null) {
                //    string className = "org.coos.messaging.transport.JvmTransport";
                //    Class transportClass = Class.forName(className);
                //    transport = (Transport) transportClass.newInstance();
                //    transport.setChannel(this);
                    throw new NotImplementedException("Not implemented transport==null, no JVM transport");
                }

             
                // Set up transport layer to process CONNECT response from the router to our chained processor
				transport.setChainedProcessor(this);

            

// Setup connect message to router

				DefaultMessage msg = new DefaultMessage(DefaultChannel.CONNECT);
				if (endpoint.getEndpointUuid() == null) {
					// must allocate a uuid
					msg.setHeader(DefaultChannel.CONNECT_SEGMENT, segment);
				} else {
					// has predefined uuid
					msg.setHeader(DefaultChannel.CONNECT_UUID, endpoint.getEndpointUuid());
				}

				// Set the aliases of the endpoint as a property in the body of
				// the message
				Hashtable props = new Hashtable();
				props.Add(Link.ALIASES, endpoint.getAliases());
				msg.setBody(props);

               
				inLink.setChainedProcessor(endpoint);

                foreach (string protocol in protocols)
                    endpoint.addLink(protocol,outLink);

				

                outLink.setChainedProcessor(transport);
				
               
                transport.start(); // Start connection to router = establish socket and start reader/writer-thread
				
                outLink.start();
				inLink.start();

				transport.processMessage(msg); // Send connect request message to router, will raise/set ConnectResponseEvent

				if (startupOrdered) {
// this particular instance of Plugin channel is now put in suspended mode for a time interval of connectionTimeout
                    // lock is released 
					//  JAVA this.wait(connectionTimeout);
                    // REAL LIFE : ConnectResponseEvent.WaitOne(connectionTimeout,false);
                    
                    // Debug, wait() forever
                    // Wait for ACK from router until timeout
                    ConnectResponseEvent.WaitOne(connectionTimeout,false);
					
                    if(!connected){
						throwConnectingException();
					} 
				}
				
			} catch (Exception e) {
				disconnect();
				throw new ConnectingException("Connecting channel: " + name + " failed, cause: "
						+ e.Message);

			}
		
	}

	private void throwConnectingException()  {
		if(connectionErrorCause == null){
			throw new ConnectingException("Timeout, no response from connecting coos router");
		} else {
			throw new ConnectingException(connectionErrorCause);
		}
	}

	public override void disconnect() {
		try {
			connected = false;
			
			if (connectable != null) {
				connectable.removeLinkById(outLink.getLinkId());
			}
			outLink.stop();
			inLink.stop();
			if (transport != null) {
				transport.stop();
			}

		} catch (Exception e) {
#if LOGGING
		    LogFactory.getLog(this.getClass()).warn("Exception when diconnecting", e);
#endif		
}
	}

	public override bool isConnected() {
		return connected;
	}


    public override void processMessage(IMessage msg)
    {

        // 1. Check if error on connect
        if (msg.getType().Equals(IMessagePrimitives.TYPE_ERROR))
        {
            endpoint.setEndpointState(IEndpointPrimitives.STATE_STARTUP_FAILED);
            connectionErrorCause = msg.getHeader(IMessagePrimitives.ERROR_REASON);
        }
        // 2. Process Connect ACK from router
        else if (msg.getName().Equals(DefaultChannel.CONNECT_ACK))
        {
            string allocUuid = msg.getHeader(DefaultChannel.CONNECT_ALLOCATED_UUID);
            string routerUuid = msg.getHeader(DefaultChannel.CONNECT_ROUTER_UUID);

            // Next processor in channel layer is inLink


            transport.setChainedProcessor(inLink);

            // Outlink destionation Uuid is router
            outLink.setDestinationUuid(routerUuid);

            if (allocUuid != null)
            {
                // We got an allocated UUid from router, set inlink destination Uuid to the allocated one
                inLink.setDestinationUuid(allocUuid);
                // Our endpoint gets allocated Uuid also
                endpoint.setEndpointUuid(allocUuid);
                if (endpoint.getEndpointUri() == null)
                {
                    endpoint.setName(allocUuid);
                    endpoint.setEndpointUri("coos://" + allocUuid);
                }
                //set the segment here if not allocated
                if (segment.Equals(""))
                {
                    segment = UuidHelper.getSegmentFromSegmentOrEndpointUuid(allocUuid);
                }
            }


            connected = true;
            
            
        }

        ConnectResponseEvent.Set();
   
        // Now create a new thread that notify all PluginChannel-instance objects that waits,
        // if StartupOrdered = true for pluginchannel, then after sending connect request
        // pluginchannel will be in suspended mode for a timeout interval, this wakes up
        // pluginchannel waiting for a response from router. Why use a thread for running this here?

        //new Thread(new Runnable(){
        //    public void run() {
        //        synchronized (PluginChannel.this) {
        //            PluginChannel.this.notifyAll();
        //        }
        //    }
        //}).start();

        if (!connected && !startupOrdered)
        {
            throw new ConnectingException();
        }
    }
}

}
