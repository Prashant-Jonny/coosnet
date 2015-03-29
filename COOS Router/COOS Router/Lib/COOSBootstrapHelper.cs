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

using System;
using System.Collections.Concurrent;
#if NETMICROFRAMEWORK
using System.Collections;
#endif
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Org.Coos.Messaging.Routing;
using Org.Coos.Messaging.Util;
using Org.Coos.Messaging.Transport;
using Org.Coos.CoosXMLSchema;

namespace Org.Coos.Messaging
{
//package org.coos.messaging;

//import java.io.InputStream;

//import java.util.HashMap;
//import java.util.Hashtable;
//import java.util.Map;
//import java.util.concurrent.ConcurrentHashMap;

//import org.coos.coosXMLSchema.ChannelMappingType;
//import org.coos.coosXMLSchema.ChannelType;
//import org.coos.coosXMLSchema.ChannelserverType;
//import org.coos.coosXMLSchema.CoosDocument;
//import org.coos.coosXMLSchema.CoosType;
//import org.coos.coosXMLSchema.CostType;
//import org.coos.coosXMLSchema.FilterType;
//import org.coos.coosXMLSchema.InBoundType;
//import org.coos.coosXMLSchema.OutBoundType;
//import org.coos.coosXMLSchema.ProcessorType;
//import org.coos.coosXMLSchema.PropertyType;
//import org.coos.coosXMLSchema.QosclassType;
//import org.coos.coosXMLSchema.RouteralgorithmType;
//import org.coos.coosXMLSchema.RouterprocessorType;
//import org.coos.coosXMLSchema.SegmentType;
//import org.coos.coosXMLSchema.TransportType;

//import org.coos.messaging.routing.Router;
//import org.coos.messaging.routing.RouterChannel;
//import org.coos.messaging.routing.RouterProcessor;
//import org.coos.messaging.routing.RouterSegment;
//import org.coos.messaging.routing.RoutingAlgorithm;
//import org.coos.messaging.transport.DefaultChannelServer;
//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;
//import org.coos.messaging.util.UuidGenerator;


/**
 * Factory for COOS instances
 *
 * @author Knut Eilif Husa, Tellu AS
 */
public class COOSFactory : COFactory {

    private static readonly ILog logger = LogFactory.getLog(typeof(COOSFactory).FullName);

    protected static Dictionary<string, IProcessor> sharedProcessors = new Dictionary<string, IProcessor>();
    protected static Dictionary<string, processorType> processorTypes = new Dictionary<string, processorType>();

    private static ConcurrentDictionary<string, COOS> coosInstances = new ConcurrentDictionary<string, COOS>();
    private static string defaultCoosInstanceName = null;

    private COOSFactory() {

    }

    public static COOS getCOOSInstance(string COOSInstanceName) {
        return coosInstances[COOSInstanceName];
    }

    /**
     * Default COOS instance for testing.
     *
     * @return A COOS instance
     */
    public static COOS getDefaultCOOSInstance() {

        if (defaultCoosInstanceName == null) {

            foreach (string s in coosInstances.Keys) {
                defaultCoosInstanceName = s;

                break;
            }
        }

        return coosInstances[defaultCoosInstanceName];
    }

    public static void clear() {
        coosInstances.Clear();
        defaultCoosInstanceName = null;
    }

    /**
     * Default COOS instance for testing.
     *
     * @param coos - a COOS instance
     */
    public static void setDefaultCOOSInstanceName(string coosName) {
        defaultCoosInstanceName = coosName;
    }

    /**
     * Create COOS instance.
     *
     * @deprecated use <code>createCOOS(InputStream, COContainer)</code>
     * @param config XML configuration
     * @return COOS instance
     * @throws Exception if errors in configuration is found
     */
    //@Deprecated public static COOS createCOOS(InputStream config) throws Exception {
    //    return createCOOS(config, null);
    //}

    /**
     * Create COOS instance.
     *
     * @param config XML configuration
     * @param container COContainer in which COOS instance is started
     * @return COOS instance
     * @throws Exception if errors in configuration is found
     */
   
    public static COOS createCOOS(Stream config, ICOContainer container) /*throws Exception */{

        //XmlOptions options = new XmlOptions();
#if JAVA
        CoosDocument doc = CoosDocument.Factory.parse(config);
#endif
        XmlSerializer serializer = new XmlSerializer(typeof(coosType));
        coosType doc = (coosType) serializer.Deserialize(config);


        COOS coos = instantiate(doc, container);
        coos.setCoosContainer(container);

        return coos;
    }

    private static COOS instantiate(coosType model, ICOContainer container) /*throws Exception */{
        
        COOS coos = new COOS();

        verifyRouterSettings(model, coos);

      
#if JAVA
          Class<?> cl = COOSFactory.tryClass(container, model.router.@class);
        IRouter router = (IRouter) cl.newInstance();
#endif
#if NET
        Type cl = COOSFactory.tryClass(container,model.router.@class);
        IRouter router = (IRouter) Activator.CreateInstance(cl) as IRouter;
#endif


        instantiateRouterProperties(model, router,coos);

        instantiateRouterProcessor(model, container, coos);

        instantiatePreprocessor(model, coos, router);

        instantiatePostprocessor(model, coos, router);

        instantiateQoSclass(model, router);

        instantiateRouterAlgorithm(model, container, coos);

        instantiateRouterSegment(model, coos, router);

        instantiateProcessor(model, container, coos);

        instantiateTransport(model, container, coos);

        instantiateChannel(model, container, coos, router);

        instantiateChannelServer(model, container, coos, router);

        bool result = coosInstances.TryAdd(model.name, coos);

        return coos;

    }

    private static void verifyRouterSettings(coosType model, COOS coos)
    {
        if (model.name == null)
        {
            throw new Exception("COOS instance has no name. Must be defined!");
        }

        coos.setName(model.name);


        if (model.router == null)
        {
            throw new Exception("COOS must contain a router. Must be defined!");
        }

        if (model.router.@class == null)
        {
            throw new Exception("COOS router must have a implementing class. Must be defined!");
        }
    }

    private static void instantiateChannelServer(coosType model, ICOContainer container, COOS coos, IRouter router)
    {
        #region channelserver

        // Default Channelserver always present
        DefaultChannelServer defaultChannelServer = new DefaultChannelServer();
        defaultChannelServer.setCOOSInstance(coos);
        defaultChannelServer.setLinkManager(router);
        coos.addChannelServer("default", defaultChannelServer);

        if (model.channelserver != null)
        {
            for (int i = 0; i < model.channelserver.Length; i++)
            {
                channelserverType channelserverType = model.channelserver[i];

                if ((channelserverType.name == null) || (channelserverType.@class == null))
                {
                    throw new Exception("COOS channel servers must have a name and a implementing class. Must be defined!");
                }

                string name = channelserverType.name;
                string className = channelserverType.@class;

                Dictionary<string, string> props = parseChannelTypeProperties(channelserverType);

                Type serverClass = COOSFactory.tryClass(container, className);
                IChannelServer server = (IChannelServer)Activator.CreateInstance(serverClass);
                server.setCOOSInstance(coos);
                server.setProperties(new Dictionary<string, string>(props));
                server.setLinkManager(router);

                Dictionary<string, RouterChannel> channelMap = parseChannelMapping(coos, channelserverType);

                server.setChannelMappings(channelMap);
                coos.addChannelServer(name, server);
            }
        }
        #endregion
    }

    private static Dictionary<string, string> parseChannelTypeProperties(channelserverType channelserverType)
    {
        #region Properties -- could probably be optimized with generic method,  code pattern appears serveral times

        Dictionary<string, string> props = new Dictionary<string, string>();

        if (channelserverType.property != null)
        {


            for (int j = 0; j < channelserverType.property.Length; j++)
            {
                propertyType propertyType = channelserverType.property[j];

                if ((propertyType.name == null) || (propertyType.value == null))
                {
                    throw new Exception("COOS properties must have a name and a value. Must be defined!");
                }

                props.Add(propertyType.name, propertyType.value);
            }
        }
        #endregion
        return props;
    }

    private static Dictionary<string, RouterChannel> parseChannelMapping(COOS coos, channelserverType channelserverType)
    {
        #region channel-mapping
        Dictionary<string, RouterChannel> channelMap = new Dictionary<string, RouterChannel>();

        if (channelserverType.channelmapping != null)
        {
            for (int j = 0; j < channelserverType.channelmapping.Length; j++)
            {
                channelmappingType channelMappingType = channelserverType.channelmapping[j];

                if ((channelMappingType.uuid == null) || (channelMappingType.channel == null))
                {
                    throw new Exception("COOS channel mappings must define a relation between a uuid regexp pattern and a channel.");
                }

                string uuid = channelMappingType.uuid;
                string channel = channelMappingType.channel;

                if (coos.getChannel(channel) == null)
                {
                    throw new Exception("COOS channel: " + channel + " referenced from channel mappings is not defined.");
                }

                channelMap.Add(uuid, coos.getChannel(channel));
            }
        }
        #endregion
        return channelMap;
    }

    private static void instantiateChannel(coosType model, ICOContainer container, COOS coos, IRouter router)
    {
        #region channels
        if (model.channel != null)
        {
            for (int i = 0; i < model.channel.Length; i++)
            {
                channelType channelType = model.channel[i];

                if (channelType.name == null)
                {
                    throw new Exception("COOS channels must have a name. Must be defined!");
                }

                string name = channelType.name;
                bool init = channelType.init;
                bool defaultGw = channelType.defaultgw;
                bool receiveRoutingInfo = channelType.receiveroutinginfo;
                string transport = channelType.transport;
                RouterChannel channel = new RouterChannel();
                channel.setName(name);
                channel.setInit(init);
                channel.setDefaultGw(defaultGw);
                channel.setReceiveRoutingInfo(receiveRoutingInfo);

                string segment = channelType.segment;

                if (init && ((segment == null) || segment.Equals("")))
                {
                    segment = ".";
                }

                if (segment != null)
                {

                    if (coos.getRouter().getRoutingAlgorithm(segment) == null)
                    {
                        throw new Exception("COOS channel: " + channel + " is defined to connect to segment: " + segment + " which the Coos instance: " +
                            coos.getName() + " does not maintain.");
                    }

                    channel.setConnectingPartyUuid(coos.getRouter().getRoutingAlgorithm(segment).getRouterUuid());
                }

                if (transport != null)
                {
                    channel.setTransport(coos.getTransport(transport));
                }

                if ((segment != null) && (coos.getSegment(segment) != null))
                {
                    channel.setRoutingAlgorithmName(coos.getSegment(segment).getRoutingAlgorithmName());
                }

                channel.setLinkManager(router);

                Link inLink = new Link();
                channel.setInLink(inLink);

                filterType[] inBoundType = channelType.inBound;

                if (inBoundType != null)
                {

                    for (int j = 0; j < inBoundType.Length; j++)
                    {
                        filterType filterType = inBoundType[j];
                        string processor = filterType.processor;
                        processorType procType = processorTypes[processor];

                        if (procType == null)
                        {
                            throw new Exception("Processor " + processor + " is not declared.");
                        }

                        if (procType.shared)
                        {
                            channel.getInLink().addFilterProcessor(sharedProcessors[processor]);
                        }
                        else
                        {
                            channel.getInLink().addFilterProcessor(instantiateProcessor(procType, container));
                        }
                    }
                }

                Link outLink = new Link();
                channel.setOutLink(outLink);

                outBoundType outBoundType = channelType.outBound;

                if (outBoundType != null)
                {

                    if (outBoundType.filter != null)
                    {
                        for (int j = 0; j < outBoundType.filter.Length; j++)
                        {
                            filterType filterType = outBoundType.filter[j];
                            string processor = filterType.processor;
                            processorType procType = processorTypes[processor];

                            if (procType == null)
                            {
                                throw new Exception("Processor " + processor + " is not declared.");
                            }

                            if (procType.shared)
                            {
                                channel.getOutLink().addFilterProcessor(sharedProcessors[processor]);
                            }
                            else
                            {
                                channel.getOutLink().addFilterProcessor(instantiateProcessor(procType, container));
                            }
                        }
                    }

                    if (outBoundType.cost != null)
                    {
                        for (int j = 0; j < outBoundType.cost.Length; j++)
                        {
                            costType costType = outBoundType.cost[j];
                            outLink.setCost(costType.name, costType.value);
                        }
                    }
                }

                coos.addChannel(name, channel);
            }
        }
        #endregion
    }

    private static void instantiateRouterProperties(coosType model, IRouter router, COOS coos)
    {
        #region Router properties
        Dictionary<string, string> properties = new Dictionary<string, string>();

        if (model.router.property != null)
        {

            for (int j = 0; j < model.router.property.Length; j++)
            {
                propertyType propertyType = model.router.property[j];
                properties.Add(propertyType.name, propertyType.value);
            }
            router.setProperties(properties);
        }
        #endregion

        coos.setRouter(router);
    }

    private static void instantiateRouterProcessor(coosType model, ICOContainer container, COOS coos)
    {
        if (model.router.routerprocessor != null)
        {

            // routerprocessors
            for (int i = 0; i < model.router.routerprocessor.Length; i++)
            {
                processorType processorType = model.router.routerprocessor[i];

                if ((processorType.name == null) || (processorType.@class == null))
                {
                    throw new Exception("COOS processors must have a name and a implementing class. Must be defined!");
                }

                string name = processorType.name;
                string className = processorType.@class;
                bool isShared = processorType.shared;
                Dictionary<string, string> props = new Dictionary<string, string>();

                for (int j = 0; j < processorType.property.Length; j++)
                {
                    propertyType propertyType = processorType.property[j];
                    props.Add(propertyType.name, propertyType.value);
                }
#if JAVA
            Class<?> processorClass = COOSFactory.tryClass(container, className);
            IProcessor processor = (IProcessor) processorClass.newInstance();
#endif
#if NET
                Type processorClass = COOSFactory.tryClass(container, className);
                IProcessor processor = (IProcessor)Activator.CreateInstance(processorClass);
#endif
                processor.setShared(isShared);
                processor.setProperties(new Dictionary<string, string>(props));
                coos.addProcessor(name, processor);
            }
        }
    }

    private static void instantiatePreprocessor(coosType model, COOS coos, IRouter router)
    {
        if (model.router.preprocessor != null)
        {
            // preProcessors
            for (int i = 0; i < model.router.preprocessor.Length; i++)
            {
                routerprocessorType routerprocessorType = model.router.preprocessor[i];

                if (coos.getProcessor(routerprocessorType.routerprocessor) == null)
                {
                    throw new Exception("COOS processors with name: " + routerprocessorType.routerprocessor +
                        " is not defined. referenced from router preprocessors!");
                }

                router.addPreProcessor((RouterProcessor)coos.getProcessor(routerprocessorType.routerprocessor));
            }
        }
    }

    private static void instantiatePostprocessor(coosType model, COOS coos, IRouter router)
    {
        if (model.router.postprocessor != null)
        {
            #region postProcessors
            for (int i = 0; i < model.router.postprocessor.Length; i++)
            {
                routerprocessorType routerprocessorType = model.router.postprocessor[i];

                if (coos.getProcessor(routerprocessorType.routerprocessor) == null)
                {
                    throw new Exception("COOS processors with name: " + routerprocessorType.routerprocessor +
                        " is not defined. referenced from router postprocessors!");
                }

                router.addPostProcessor((RouterProcessor)coos.getProcessor(routerprocessorType.routerprocessor));
            }
            #endregion
        }
    }

    private static void instantiateQoSclass(coosType model, IRouter router)
    {
        router.addQoSClass(Link.DEFAULT_QOS_CLASS, true);

        #region qosclass

        if (model.router.qosclass != null)
        {
            for (int i = 0; i < model.router.qosclass.Length; i++)
            {
                qosclassType qosclassType = model.router.qosclass[i];

                if (qosclassType.name == null)
                {
                    throw new Exception("COOS QoS types must have a name. Must be defined!");
                }

                router.addQoSClass(qosclassType.name, Boolean.Parse(qosclassType.@default));
            }
        }
        #endregion
    }

    private static void instantiateTransport(coosType model, ICOContainer container, COOS coos)
    {
        #region transports
        if (model.transport != null)
        {
            for (int i = 0; i < model.transport.Length; i++)
            {
                transportType transportType = model.transport[i];

                if ((transportType.name == null) || (transportType.@class == null))
                {
                    throw new Exception("COOS transports must have a name and an implementing class. Must be defined!");
                }

                string name = transportType.name;
                string className = transportType.@class;
                Dictionary<string, string> props = new Dictionary<string, string>();

                for (int j = 0; j < transportType.property.Length; j++)
                {
                    propertyType propertyType = transportType.property[j];

                    if ((propertyType.name == null) || (propertyType.value == null))
                    {
                        throw new Exception("COOS properties must have a name and a value. Must be defined!");
                    }

                    props.Add(propertyType.name, propertyType.value);
                }

                Type transportClass = COOSFactory.tryClass(container, className);
                ITransport transport = (ITransport)Activator.CreateInstance(transportClass);
                transport.setProperties(new Dictionary<string, string>(props));
                transport.setName(name);
                coos.addTransport(name, transport);
            }
        }
        #endregion
    }

    private static void instantiateProcessor(coosType model, ICOContainer container, COOS coos)
    {
        #region processors
        if (model.processor != null)
        {
            for (int i = 0; i < model.processor.Length; i++)
            {
                processorType processorType = model.processor[i];

                if ((processorType.name == null) || (processorType.@class == null))
                {
                    throw new Exception("COOS processors must have a name and a implementing class. Must be defined!");
                }

                string name = processorType.name;
                processorTypes.Add(name, processorType);

                IProcessor processor = instantiateProcessor(processorType, container);

                if (processor.isShared())
                {
                    sharedProcessors.Add(name, processor);
                }

                coos.addProcessor(name, processor);
            }
        }
        #endregion
    }

    private static void instantiateRouterAlgorithm(coosType model, ICOContainer container, COOS coos)
    {
        #region RoutingAlgorithms
        if (model.router.routeralgorithm != null)
        {
            for (int j = 0; j < model.router.routeralgorithm.Length; j++)
            {
                routeralgorithmType routeralgorithmType = model.router.routeralgorithm[j];
                string algName = routeralgorithmType.name;

                if ((algName == null) || (routeralgorithmType.@class == null))
                {
                    throw new Exception("COOS router algorithms must have a name and a implementing class. Must be defined!");
                }

                string className = routeralgorithmType.@class;
                Type algorithmClass = COOSFactory.tryClass(container, className);
                #region Algorithm properties
                Dictionary<string, string> props = new Dictionary<string, string>();

                for (int k = 0; k < routeralgorithmType.property.Length; k++)
                {
                    propertyType propertyType = routeralgorithmType.property[k];
                    props.Add(propertyType.name, propertyType.value);
                }
                #endregion

                IRoutingAlgorithm algorithm = (IRoutingAlgorithm)Activator.CreateInstance(algorithmClass);

                algorithm.setProperties(new Dictionary<string, string>(props));

                coos.addRoutingAlgorithm(algName, algorithm);

            }
        }
        #endregion
    }

    private static void instantiateRouterSegment(coosType model, COOS coos, IRouter router)
    {
        #region segment
        bool defaultSegmentDefined = false;
        if (model.router.segment != null)
        {
            for (int i = 0; i < model.router.segment.Length; i++)
            {
                segmentType segmentType = model.router.segment[i];
                string segmentName = segmentType.name;

                if ((segmentName == null) || segmentName.Equals(""))
                {
                    segmentName = ".";
                }

                if (segmentName.Equals(IRouterPrimitives.LOCAL_SEGMENT) || segmentName.Equals(IRouterPrimitives.DICO_SEGMENT) || (segmentName.IndexOf('/') != -1) ||
                        (segmentName.IndexOf('\\') != -1) || (segmentName.IndexOf(' ') != -1) || (segmentName.IndexOf('-') != -1))
                {
                    throw new Exception("Invalid COOS segment: " + segmentName +
                        " Can not contain: blanks, \\, /, - or the reserved segment 'localcoos' and 'dico'.");
                }

                string routerUuid;

                if ((segmentType.routeruuid != null) && !segmentType.routeruuid.Equals(""))
                {
                    routerUuid = IRouterPrimitives.ROUTER_UUID_PREFIX + segmentType.routeruuid;
                }
                else
                {
                    routerUuid = IRouterPrimitives.ROUTER_UUID_PREFIX + coos.getName();
                }

                if (routerUuid == null)
                {
                    throw new Exception("COOS Segments must have a segment unique identification of the Router uuid. Must be defined!");
                }

                if (routerUuid.Contains("."))
                {
                    throw new Exception("Router uuid cannot contain '.', The segment is explicitly declared in segment attribute.");
                }

                if (segmentName.Equals("."))
                {
                    routerUuid = segmentName + routerUuid;
                }
                else
                {
                    routerUuid = segmentName + "." + routerUuid;
                }

                UuidGenerator gen = new UuidGenerator(routerUuid);
                routerUuid = gen.generateId();

                string routerAlgorithm = segmentType.routeralgorithm;

                if (segmentType.routeralgorithm == null)
                {
                    throw new Exception("COOS Segments must have a router algorithm. Must be defined!");
                }

                string defaultSegmentStr = segmentType.defaultSegment;
                bool defaultSegment = (defaultSegmentStr != null) && defaultSegmentStr.ToLowerInvariant().Equals("true");

                if (defaultSegment)
                {
                    defaultSegmentDefined = true;
                }

                IRoutingAlgorithm prototypeAlg = coos.getRoutingAlgorithm(routerAlgorithm);

                if (prototypeAlg == null)
                {
                    throw new Exception("COOS router algorithm: " + routerAlgorithm + " is not defined. Must be defined!");
                }

                IRoutingAlgorithm algorithm = prototypeAlg.copy();

                if (segmentType.logging != null)
                {
                    algorithm.setLoggingEnabled(segmentType.logging.ToLowerInvariant().Equals("true"));
                }

                algorithm.init(routerUuid, router);
                coos.addSegment(new RouterSegment(segmentName, routerUuid, algorithm.getAlgorithmName(), defaultSegment));
            }

            if (!defaultSegmentDefined)
            {

                //First segment in list is defined as default
                foreach (RouterSegment rs in coos.getSegmentMap().Values)
                {
                    rs.setDefaultSegment(true);

                    break;
                }
            }


            router.setSegmentMappings(new ConcurrentDictionary<string, RouterSegment>(coos.getSegmentMap()));
        }
        #endregion
    }

    protected static IProcessor instantiateProcessor(processorType processorType, ICOContainer container) /* throws Exception */{
        string typeName = processorType.@class;
        bool isShared = processorType.shared;
        string name = processorType.name;

        #region Properties
        Dictionary<string,string> props = new Dictionary<string,string>();

        if (processorType.property != null)
        {
            for (int j = 0; j < processorType.property.Length; j++)
            {
                propertyType propertyType = processorType.property[j];

                if ((propertyType.name == null) || (propertyType.value == null))
                {
                    throw new Exception("COOS properties must have a name and a value. Must be defined!");
                }

                props.Add(propertyType.name, propertyType.value);
            }
        }
        #endregion

        Type procClass = PluginFactory.tryClass(container, typeName);
        IProcessor processor = (IProcessor) Activator.CreateInstance(procClass);
        processor.setCoContainer(container);
        processor.setName(name);
        processor.setProperties(new Dictionary<string,string>(props));
        processor.setShared(isShared);

        return processor;
    }

}}
