//#define JMX
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

//import java.util.Map;
//import java.util.concurrent.ConcurrentHashMap;

//import org.coos.messaging.jmx.ManagedObject;
//import org.coos.messaging.jmx.ManagementFactory;
//import org.coos.messaging.routing.Router;
//import org.coos.messaging.routing.RouterChannel;
//import org.coos.messaging.routing.RouterSegment;
//import org.coos.messaging.routing.RoutingAlgorithm;
//import org.coos.messaging.serializer.ObjectJavaSerializer;
//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;

using System.Collections;

using Org.Coos.Messaging.Routing;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging
{
/**
 * COOS class.
 * <p>
 * Description needed.
 * <p>
 * The COOS instance has a reference to the COOS-container in which it is running.
 *
 * @author Knut Eilif Husa, Tellu AS
 * @author Robert Bjarum, Tellu AS
 *
 */
public class COOS {

    private static readonly string COOS_INSTANCE_KEY = "COOSInstance";
    private static readonly ILog LOG = LogFactory.getLog(typeof(COOS).FullName);

    private string name;
    private IRouter router;
#if JAVA
    private Map<string, Transport> transports = new ConcurrentHashMap<string, Transport>();
    private Map<string, RouterChannel> channels = new ConcurrentHashMap<string, RouterChannel>();
    private Map<string, Processor> processors = new ConcurrentHashMap<string, Processor>();
    private Map<string, ChannelServer> channelServers = new ConcurrentHashMap<string, ChannelServer>();
    private Map<string, RouterSegment> segmentMap = new ConcurrentHashMap<string, RouterSegment>();
    private Map<string, RoutingAlgorithm> routingAlgorithmMap = new ConcurrentHashMap<string, RoutingAlgorithm>();
#endif
#if NETMICROFRAMEWORK
    private Hashtable transports = new Hashtable();
    private Hashtable channels = new Hashtable();
    private Hashtable processors = new Hashtable();
    private Hashtable channelServers = new Hashtable();
    private Hashtable segmentMap = new Hashtable();
    private Hashtable routingAlgorithmMap = new Hashtable();
#endif

    
    private COContainer coosContainer;
    private bool started;

#if JMX
    /*
     * The object used for Monitoring and Management of the COOS instance (i.e. using JMX)
     */
    private ManagedObject managedObject = null;

#endif

    protected COOS() {
        SerializerFactory.registerSerializer(Message.SERIALIZATION_METHOD_JAVA, new ObjectJavaSerializer());

        /*
         * We do not want to see the context of the other loggers on the same thread in this logger
         */
        LOG.setInheritMDC(false);

    }

    public string getName() {
        return name;
    }

    public void setName(string name) {
        this.name = name;
        LOG.putMDC(COOS_INSTANCE_KEY, getName());
    }

    /**
     * Return reference to the COOS Container in which this COOS instance is running.
     *
     * @return COOS Container (COContainer)
     */
    public COContainer getCoosContainer() {
        return coosContainer;
    }

    public void setCoosContainer(COContainer coosContainer) {
        this.coosContainer = coosContainer;

        foreach (ITransport transport in transports.Values) {
            transport.setCoContainer(coosContainer);
        }

        foreach (IChannel channel in channels.Values) {
            channel.setCoContainer(coosContainer);
        }

        foreach (IProcessor processor in processors.Values) {
            processor.setCoContainer(coosContainer);
        }
    }

    public bool isStarted() {
        return started;
    }

    public IRouter getRouter() {
        return router;
    }

    public void setRouter(IRouter router) {
        this.router = router;
        this.router.setCOOS(this);
    }

    public void addTransport(string name, ITransport transport) {
        transports.Add(name, transport);
        transport.setCoContainer(coosContainer);
    }

    public ITransport getTransport(string name) {
        return transports[name] as ITransport;
    }

    public void addChannel(string name, RouterChannel channel) {
        channels.Add(name, channel);
        channel.setCoContainer(coosContainer);
    }

    public void removeChannel(string name) {
        channels.Remove(name);
    }

    public RouterChannel getChannel(string name) {
        return channels[name] as RouterChannel;
    }

    public void addProcessor(string name, IProcessor processor) {
        processors.Add(name, processor);
        processor.setCoContainer(coosContainer);
    }

    public IProcessor getProcessor(string name) {
        return (IProcessor)processors[name];
    }

   // JAVA  public Map<string, Transport> getTransports() {
    public Hashtable getTransports() {
        return transports;
    }

   // JAVA public Map<string, RouterChannel> getChannels() {
    public Hashtable getChannels() {
    
        return channels;
    }

    // JAVA public Map<string, Processor> getProcessors() {
    public Hashtable getProcessors() {
    
        return processors;
    }

    // JAVA public Map<string, ChannelServer> getChannelServers() {
    public Hashtable getChannelServers() {
   
        return channelServers;
    }

    public void addChannelServer(string name, IChannelServer server) {
        channelServers.Add(name, server);
    }

    public IChannelServer getChannelServer(string name) {
        return channelServers[name] as IChannelServer;
    }


    // JAVA public Map<string, RouterSegment> getSegmentMap() {
    public Hashtable getSegmentMap() {
          
    return segmentMap;
    }

    // JAVA public void setSegmentMap(Map<string, RouterSegment> segmentMap) {
    public void setSegmentMap(Hashtable segmentMap) {
    
        this.segmentMap = segmentMap;
    }

    public void addSegment(RouterSegment routerSegment) {
        segmentMap.Add(routerSegment.getName(), routerSegment);
    }

    public RouterSegment getSegment(string segmentName) {
        return segmentMap[segmentName] as RouterSegment;
    }

    public void addRoutingAlgorithm(string algorithmName, IRoutingAlgorithm routingAlgorithm) {
        routingAlgorithmMap.Add(algorithmName, routingAlgorithm);
    }

    public IRoutingAlgorithm getRoutingAlgorithm(string algorithmName) {
        return routingAlgorithmMap[algorithmName] as IRoutingAlgorithm;
    }

// JAVA    public void start() throws Exception {
        public void start()  {

        if (coosContainer == null) {
            LOG.warn("The COOS container property (COContainer) has not been set.");
        }

        LOG.info("Starting COOS " + name);
        LOG.info("Starting processors");

        foreach (IProcessor processor in processors.Values) {

            if (processor is IService) {
                ((IService) processor).start();
            }
        }

        LOG.info("Starting channel servers");

        foreach (IChannelServer channelServer in channelServers.Values) {

            if (channelServer is IService) {
                ((IService) channelServer).start();
            }
        }

        LOG.info("Initializing channels");

        foreach (IChannel channel in channels.Values) {

            if (channel.getTransport() != null) {
                channel.connect(router);
            }
        }

        LOG.info("Starting Router");

        // router.setLoggingEnabled(true);
        router.start();

#if JMX
        /*
         * Register COOS for monitoring. Need to cast, since the method registerCoos() is not defined in interface.
         */
        managedObject = ManagementFactory.getPlatformManagementService().registerCoos(this);

#endif
        LOG.info("COOS " + name + " successfully started");
        started = true;
    }

    // JAVA public void stop() throws Exception {
    public void stop()  {
    
        LOG.info("Stopping COOS " + name);
        LOG.info("Stopping Router");

        // router.setLoggingEnabled(true);
        router.stop();

        LOG.info("Stopping channels");

        foreach (IChannel channel in channels.Values) {
            channel.disconnect();
        }

        LOG.info("Stopping channel servers");

        foreach (IChannelServer channelServer in channelServers.Values) {
            channelServer.stop();
        }

        LOG.info("Stopping processors");

        foreach (IProcessor processor in processors.Values) {

            if (processor is IService) {
                ((IService) processor).stop();
            }
        }

        LOG.info("COOS " + name + " stopped");
        started = false;

#if JMX
        if (managedObject != null) {
            ManagementFactory.getPlatformManagementService().unregister(managedObject);
        }

#endif
    }

}
}

