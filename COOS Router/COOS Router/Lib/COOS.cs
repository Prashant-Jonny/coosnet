//#define JMX
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
using System.Collections.Concurrent;

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

#if NET
     private  ConcurrentDictionary<string, ITransport> transports = new ConcurrentDictionary<string, ITransport>();
    private  ConcurrentDictionary<string, RouterChannel> channels = new ConcurrentDictionary<string, RouterChannel>();
    private  ConcurrentDictionary<string, IProcessor> processors = new  ConcurrentDictionary<string, IProcessor>();
    private  ConcurrentDictionary<string, IChannelServer> channelServers = new  ConcurrentDictionary<string, IChannelServer>();
    private  ConcurrentDictionary<string, RouterSegment> segmentMap = new  ConcurrentDictionary<string, RouterSegment>();
    private ConcurrentDictionary<string, IRoutingAlgorithm> routingAlgorithmMap = new ConcurrentDictionary<string, IRoutingAlgorithm>();
#endif

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

    
    private ICOContainer coosContainer;
    private bool started;

#if JMX
    /*
     * The object used for Monitoring and Management of the COOS instance (i.e. using JMX)
     */
    private ManagedObject managedObject = null;

#endif

    public COOS() {
#if JAVA
        SerializerFactory.registerSerializer(Message.SERIALIZATION_METHOD_JAVA, new ObjectJavaSerializer());
#endif
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
    public ICOContainer getCoosContainer() {
        return coosContainer;
    }

    public void setCoosContainer(ICOContainer coosContainer) {
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
        bool result = transports.TryAdd(name, transport);
        transport.setCoContainer(coosContainer);
    }

    public ITransport getTransport(string name) {
        return transports[name] as ITransport;
    }

    public void addChannel(string name, RouterChannel channel) {
        bool result = channels.TryAdd(name, channel);
        channel.setCoContainer(coosContainer);
    }

    public void removeChannel(string name) {
        RouterChannel rchannel = new RouterChannel();
        bool result = channels.TryRemove(name,out rchannel);
    }

    public RouterChannel getChannel(string name) {
        return channels[name] as RouterChannel;
    }

    public void addProcessor(string name, IProcessor processor) {
#if NETMICROFRAMEWORK || JAVA
        processors.Add(name, processor);
#endif
#if NET
        bool results = processors.TryAdd(name, processor);
#endif
        processor.setCoContainer(coosContainer);
    }

    public IProcessor getProcessor(string name) {
        return (IProcessor)processors[name];
    }

#if JAVA
     public Map<string, Transport> getTransports() 
#endif
#if NETMICROFRAMEWORK
    public Hashtable getTransports() 
#endif
#if NET
    public ConcurrentDictionary<string, ITransport> getTransports()
#endif
    {
        return transports;
    }

#if JAVA
   JAVA public Map<string, RouterChannel> getChannels() {
#endif
#if NETMICROFRAMEWORK
    public Hashtable getChannels() 
#endif
#if NET
    public ConcurrentDictionary<string, RouterChannel> getChannels()
#endif
    {
    
        return channels;
    }
#if JAVA
    public Map<string, Processor> getProcessors() {
#endif
#if NETMICROFRAMEWORK
    public Hashtable getProcessors() {
#endif
#if NET
    public ConcurrentDictionary<string, IProcessor> getProcessors()
#endif
    {
    
        return processors;
    }

#if JAVA
      public Map<string, ChannelServer> getChannelServers() {
#endif
#if NETMICROFRAMEWORK
    public Hashtable getChannelServers() {
#endif
#if NET
    public ConcurrentDictionary<string, IChannelServer> getChannelServers() {
#endif
   
        return channelServers;
    }

    public void addChannelServer(string name, IChannelServer server) {
#if NETMICROFRAMEWORK || JAVA
        channelServers.Add(name, server);
#endif
#if NET
        channelServers.TryAdd(name,server);
#endif
    }

    public IChannelServer getChannelServer(string name) {
        return channelServers[name] as IChannelServer;
    }

#if JAVA
     public Map<string, RouterSegment> getSegmentMap() {
#endif
#if NETMICROFRAMEWORK
    public Hashtable getSegmentMap() {
#endif
#if NET
    public ConcurrentDictionary<string, RouterSegment> getSegmentMap() {
#endif
          
    return segmentMap;
    }

#if JAVA
    JAVA public void setSegmentMap(Map<string, RouterSegment> segmentMap) {
#endif
#if NETMICROFRAMEWORK
    public void setSegmentMap(Hashtable segmentMap) {
#endif
#if NET
    public void setSegmentMap(ConcurrentDictionary<string, RouterSegment> segmentMap) {
#endif
    
        this.segmentMap = segmentMap;
    }

    public void addSegment(RouterSegment routerSegment) {
#if NETMICROFRAMEWORK || JAVA
        segmentMap.Add(routerSegment.getName(), routerSegment);
#endif
#if NET
        bool result = segmentMap.TryAdd(routerSegment.getName(),routerSegment);
#endif
    }

    public RouterSegment getSegment(string segmentName) {
        return segmentMap[segmentName] as RouterSegment;
    }

    public void addRoutingAlgorithm(string algorithmName, IRoutingAlgorithm routingAlgorithm) {
#if NETMICROFRAMEWORK || JAVA
        routingAlgorithmMap.Add(algorithmName, routingAlgorithm);
#endif
#if NET
        bool result = routingAlgorithmMap.TryAdd(algorithmName, routingAlgorithm);
#endif

        }

    public IRoutingAlgorithm getRoutingAlgorithm(string algorithmName) {
        return routingAlgorithmMap[algorithmName] as IRoutingAlgorithm;
    }

#if JAVA
   public void start() throws Exception {
#endif
#if NETMICROFRAMEWORK || NET
    public void start()  {
#endif

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

#if JAVA
    public void stop() throws Exception {
#endif
#if NETMICROFRAMEWORK || NET
    public void stop()  {
#endif
    
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

