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

using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

using Org.Coos.PluginXMLSchema;
using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;
using Org.Coos.Messaging.Impl;

namespace Org.Coos.Messaging
{
//package org.coos.messaging;

//import java.io.ByteArrayInputStream;
//import java.io.IOException;
//import java.io.InputStream;
//import java.io.InputStreamReader;

//import java.util.HashMap;
//import java.util.Hashtable;
//import java.util.Map;

//import org.coos.messaging.impl.DefaultChannel;
//import org.coos.messaging.serializer.ObjectJavaSerializer;
//import org.coos.messaging.util.URIHelper;
//import org.coos.messaging.util.UuidHelper;

//import org.coos.pluginXMLSchema.ChannelType;
//import org.coos.pluginXMLSchema.FilterType;
//import org.coos.pluginXMLSchema.InBoundType;
//import org.coos.pluginXMLSchema.OutBoundType;
//import org.coos.pluginXMLSchema.PluginType;
//import org.coos.pluginXMLSchema.PluginsDocument;
//import org.coos.pluginXMLSchema.PluginsType;
//import org.coos.pluginXMLSchema.ProcessorType;
//import org.coos.pluginXMLSchema.PropertyType;
//import org.coos.pluginXMLSchema.TransportType;

//import org.coos.util.macro.MacroSubstituteReader;


/**
 * Factory for Plugin instances
 *
 * @author Knut Eilif Husa, Tellu AS
 */
public class PluginFactory : COFactory {

    public static readonly string JVM_TRANSPORT_CLASS = "org.coos.messaging.transport.JvmTransport";
    public static readonly string PLUGIN_CHANNEL_CLASS = "Org.Coos.Messaging.Plugin.PluginChannel";

    private static Dictionary<string, IProcessor> sharedProcessors = new Dictionary<string, IProcessor>();
    private static Dictionary<string, processorType> processorTypes = new Dictionary<string, processorType>();


    private PluginFactory() {
#if IMPLEMENT_LATER
    	SerializerFactory.registerSerializer(IMessagePrimitives.SERIALIZATION_METHOD_JAVA, new ObjectJavaSerializer()); 
#endif
        }

#if TEST
    // Used in tests only
    public static Plugin createPlugin(string name, string className, string segment, string coosInstanceName, string channelServerName)
        /*throws Exception */{

        COContainer cl = new COContainer() {

                public Class<?> loadClass(string className) throws ClassNotFoundException {
                    return Class.forName(className);
                }

                public InputStream getResource(string resourceName) throws IOException {
                    InputStream is = COContainer.class.getResourceAsStream(resourceName);

                    return substitute(is);
                }

                public Object getObject(string name) {
                    return null;
                }

                public void start() {
                }

                public void stop() {
                }
            };

        Plugin plugin = new Plugin();

       Type pluginClass = PluginFactory.tryClass(cl, className);
        IEndpoint endpoint = (IEndpoint) Activator.CreateInstance(pluginClass);
        endpoint.setCoContainer(cl);
        if(name != null && Regex.IsMatch(name,".*\\."))
        {
    		throw new Exception("Name :"+ name + " not allowed. '.' is reserved for separating segments");
    	}
        endpoint.setEndpointUri("coos://" + name);
        endpoint.setProperties(new Dictionary<string, string>());
        plugin.setEndpoint(endpoint);

        URIHelper helper = new URIHelper(endpoint.getEndpointUri());

        if (helper.isEndpointUuid()) {
            endpoint.setEndpointUuid(name);
        }

        endpoint.setName(name);

        if (segment == null) {
            segment = ".";
        }

        Type channelClass = PluginFactory.tryClass(cl, PLUGIN_CHANNEL_CLASS);
        DefaultChannel channel = (DefaultChannel) Activator.CreateInstance(channelClass);
        channel.addProtocol("coos");
        channel.setCoContainer(cl);
        channel.setSegment(segment);
        plugin.addChannel(channel);

        Dictionary<string, string> properties = new Dictionary<string, string>();

        if (coosInstanceName != null)
            properties.Add("COOSInstanceName", coosInstanceName);

        properties.Add("ChannelServerName", channelServerName);

        Type transportClass = PluginFactory.tryClass(cl, JVM_TRANSPORT_CLASS);
        ITransport transport = (ITransport) Activator.CreateInstance(transportClass);
        transport.setProperties(properties);
        channel.setTransport(transport);

        return plugin;
    }


    // Used in tests only
    public static Plugin[] createPlugins(Stream config) /*throws Exception*/ {


        PluginsDocument doc = PluginsDocument.Factory.parse(config);
        COContainer cl = new COContainer() {

                public Class<?> loadClass(string className) throws ClassNotFoundException {
                    return Class.forName(className);
                }

                public InputStream getResource(string resourceName) throws IOException {
                    InputStream is = COContainer.class.getResourceAsStream(resourceName);

                    return substitute(is);
                }

                public Object getObject(string name) {
                    return null;
                }

                @Override public void start() {
                }

                @Override public void stop() {
                }
            };

        Plugin[] plugins = instantiate(doc.getPlugins(), cl);

        return plugins;

        }
#endif

#if IMPLEMENT_LATER

    private static InputStream substitute(InputStream is) /* throws IOException */ {
        InputStreamReader isr = new InputStreamReader(is);
        MacroSubstituteReader msr = new MacroSubstituteReader(isr);
        string substituted = msr.substituteMacros();
        is = new ByteArrayInputStream(substituted.getBytes());

        return is;
    }

#endif

    public static Plugin[] createPlugins(Stream config, ICOContainer container) /*throws Exception */{

        // XmlOptions options = new XmlOptions();
#if JAVA
        PluginsDocument doc = PluginsDocument.Factory.parse(config);

        Plugin[] plugins = instantiate(doc.getPlugins(), cl);
#endif
#if NET
        XmlSerializer serializer = new XmlSerializer(typeof(pluginsType));
        pluginsType pluginsType = (pluginsType) serializer.Deserialize(config);
        Plugin[] plugins = instantiate(pluginsType,container);
#endif
        return plugins;
    }

    private static Plugin[] instantiate(pluginsType model, ICOContainer container) /* throws Exception */  {
        Plugin[] res = new Plugin[model.plugin.Length];

        #region processors

        if (model.processor != null)
        {
            for (int i = 0; i < model.processor.Length; i++)
            {
                processorType processorType = model.processor[i];

                if ((processorType.name == null) || (processorType.@class == null))
                {
                    throw new Exception("Plugin processors must have a name and a implementing class. Must be defined!");
                }

                string name = processorType.name;
                processorTypes.Add(name, processorType);

                IProcessor processor = instantiateProcessor(processorType, container);

                if (processor.isShared())
                {
                    sharedProcessors.Add(name, processor);
                }
            }
        }

        #endregion
        Dictionary<string, ITransport> transportMap = new Dictionary<string, ITransport>();

#region transports
        if (model.transport != null)
        {
            for (int i = 0; i < model.transport.Length; i++)
            {
                transportType transportType = model.transport[i];

                if ((transportType.name == null) || (transportType.@class == null))
                {
                    throw new Exception("Plugin transports must have a name and an implementing class. Must be defined!");
                }

                string name = transportType.name;
                string className = transportType.@class;
                Dictionary<string, string> props = new Dictionary<string, string>();

                for (int j = 0; j < transportType.property.Length; j++)
                {
                    propertyType propertyType = transportType.property[j];

                    if ((propertyType.name == null) || (propertyType.value == null))
                    {
                        throw new Exception("Plugin properties must have a name and a value. Must be defined!");
                    }

                    props.Add(propertyType.name, propertyType.value);
                }

                Type transportClass = PluginFactory.tryClass(container, className);
                ITransport transport = (ITransport)Activator.CreateInstance(transportClass);
                transport.setName(name);
                transport.setCoContainer(container);
                transport.setProperties(new Dictionary<string, string>(props));
                transportMap.Add(name, transport);
            }
        }
#endregion

        #region channels
        Dictionary<string, IChannel> channelMap = new Dictionary<string, IChannel>();
        if (model.channel != null)
        {
            for (int i = 0; i < model.channel.Length; i++)
            {
                channelType channelType = model.channel[i];

                if ((channelType.name == null) || (channelType.@class == null))
                {
                    throw new Exception("Plugin channels must have a name and an implementing class. Must be defined!");
                }

                string name = channelType.name;
                string className = channelType.@class;
                string protocol = channelType.protocol1;
                string segment = channelType.segment;
                IChannel channel = null;
                Type channelClass;
                
                    channelClass = PluginFactory.tryClass(container, className);
                    if (channelClass.Module.Name.Contains("COOS Messaging"))
                        //channel = Activator.CreateInstance(channelClass) as IChannel;
                        channel = Activator.CreateInstanceFrom("COOS Messaging.dll", className).Unwrap() as IChannel;
                    else
                        channel = Activator.CreateInstance(null, className) as IChannel; // Currently executing assembly
                    

                channel.setCoContainer(container);

                if (protocol != null)
                {
                    channel.addProtocol(protocol);
                }
                else
                {
                    channel.addProtocol("coos"); //The default protocol
                }

                channel.setName(name);

                if (segment != null)
                {
                    channel.setSegment(segment);
                }

                Dictionary<string, string> props = new Dictionary<string, string>();

                if (channelType.property != null)
                {
                    for (int j = 0; j < channelType.property.Length; j++)
                    {
                        propertyType propertyType = channelType.property[j];

                        if ((propertyType.name == null) || (propertyType.value == null))
                        {
                            throw new Exception("Plugin properties must have a name and a value. Must be defined!");
                        }

                        props.Add(propertyType.name, propertyType.value);
                    }

                    channel.setProperties(new Dictionary<string, string>(props));
                }

                if (channelType.protocol != null)
                {
                    for (int j = 0; j < channelType.protocol.Length; j++)
                    {
                        string prot = channelType.protocol[j];
                        channel.addProtocol(prot);
                    }
                }

                string transportType = channelType.transport;

                if (transportType != null)
                {

                    if (!transportMap.ContainsKey(transportType))
                    {
                        throw new Exception("Transport " + transportType + " is not declared.");
                    }

                    channel.setTransport((ITransport)transportMap[transportType].copy());
                }
                else
                {
                    Type transportClass = PluginFactory.tryClass(container, JVM_TRANSPORT_CLASS);
                    ITransport transport = (ITransport)Activator.CreateInstance(transportClass);
                    transport.setCoContainer(container);
                    channel.setTransport(transport);
                }

                filterType[] outBoundType = channelType.outBound;

                if (outBoundType != null)
                {

                    for (int j = 0; j < outBoundType.Length; j++)
                    {
                        filterType filterType = outBoundType[j];
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

                channelMap.Add(name, channel);
            }
        }
        #endregion

        #region Plugins
        if (model.plugin != null)
        {
            for (int i = 0; i < model.plugin.Length; i++)
            {

                Plugin plugin = new Plugin();

                pluginType pluginType = model.plugin[i];

                if (pluginType.@class == null)
                {
                    throw new Exception("Plugin properties must have a name and a value. Must be defined!");
                }

                string className = pluginType.@class;
                Type pluginClass = PluginFactory.tryClass(container, className);
                IEndpoint endpoint = (IEndpoint)Activator.CreateInstance(pluginClass);

                endpoint.setCoContainer(container);

                string name = pluginType.name;
                string nameSegment = "";

                if (name != null && !name.Equals(""))
                {

                    if (name != null && Regex.IsMatch(name, ".*\\."))
                    {
                        throw new Exception("Name :" + name + " not allowed. '.' is reserved for separating segments");
                    }

                    if (UuidHelper.isUuid(name))
                    {
                        name = UuidHelper.getQualifiedUuid(name);
                        endpoint.setEndpointUuid(name);
                        endpoint.setEndpointUri("coos://" + name);
                        nameSegment = UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(name);
                    }
                    else
                    {
                        endpoint.setEndpointUri("coos://" + name);

                        URIHelper uHelper = new URIHelper(endpoint.getEndpointUri());
                        nameSegment = uHelper.getSegment();
                    }

                    endpoint.setName(name); // the alias
                }

                string channelName = pluginType.channel1;
                string startLevelStr = pluginType.startLevel;

                if (startLevelStr != null)
                {
                    plugin.setStartLevel(int.Parse(startLevelStr));
                }

                string[] aliases = pluginType.alias;

                if (aliases != null)
                {
                    for (int j = 0; j < aliases.Length; j++)
                    {
                        endpoint.addAlias(aliases[j]);
                    }
                }

                if ((channelName == null) && (pluginType.channel.Length == 0))
                {

                    // Always add the PluginChannel
                    Type channelClass1 = PluginFactory.tryClass(container, PLUGIN_CHANNEL_CLASS);
                    IChannel channel1 = (IChannel)Activator.CreateInstance(channelClass1);
                    channel1.addProtocol("coos");
                    channel1.setCoContainer(container);
                    channel1.setName("default");
                    channelMap.Add("default", channel1);

                    string transportType = pluginType.transport;

                    if (transportType != null)
                    {

                        if (!transportMap.ContainsKey(transportType))
                        {
                            throw new Exception("Transport " + transportType + " is not declared.");
                        }

                        channel1.setTransport(transportMap[transportType]);
                    }
                    else
                    {
                        Type transportClass1 = PluginFactory.tryClass(container, JVM_TRANSPORT_CLASS);
                        ITransport transport1 = (ITransport)Activator.CreateInstance(transportClass1);
                        transport1.setCoContainer(container);
                        channel1.setTransport(transport1);
                    }

                    plugin.addChannel((IChannel)channelMap["default"].copy());
                }

                if (channelName != null)
                {
                    addChannel(channelMap, plugin, name, nameSegment, channelName);
                }

                if (pluginType.channel != null)
                {
                    for (int j = 0; j < pluginType.channel.Length; j++)
                    {
                        channelName = pluginType.channel[j];
                        addChannel(channelMap, plugin, name, nameSegment, channelName);
                    }
                }

                Dictionary<string, string> props = new Dictionary<string, string>();

                if (pluginType.property != null)
                {
                    for (int k = 0; k < pluginType.property.Length; k++)
                    {
                        propertyType propertyType = pluginType.property[k];

                        if ((propertyType.name == null) || (propertyType.value == null))
                        {
                            throw new Exception("Plugin properties must have a name and a value. Must be defined!");
                        }

                        props.Add(propertyType.name, propertyType.value);
                    }
                }

                endpoint.setProperties(new Dictionary<string, string>(props));
                plugin.setEndpoint(endpoint);

                res[i] = plugin;
            }
        }
        #endregion

        return res;
    }

    private static void addChannel(Dictionary<string, IChannel> channelMap, Plugin plugin, string name, string nameSegment, string channelName)
        /*throws Exception */ {

        if (!channelMap.ContainsKey(channelName)) {
            throw new Exception("Channel " + channelName + " is not declared for plugin: " + name);
        }

        IChannel channel = (IChannel) channelMap[channelName].copy();

        if (!nameSegment.Equals("")) {

            if (channel.getSegment().Equals("")) {

                //use namesegment as channelsegment
                channel.setSegment(nameSegment);
            } else if (!nameSegment.Equals("dico") && !nameSegment.Equals("localcoos") 
            		&& !channel.getSegment().Equals(nameSegment)) {

                //if different they must match
                throw new Exception("Channel " + channelName + " with segment '" + channel.getSegment() +
                    "' does not match segment declared for plugin: " + name);
            }
        }

        plugin.addChannel(channel);
    }

    private static IProcessor instantiateProcessor(processorType processorType, ICOContainer cl) /*throws Exception */{
        string className = processorType.@class;
        bool isShared = processorType.shared;
        string name = processorType.name;
        Dictionary<string, string> props = new Dictionary<string, string>();

        for (int j = 0; j < processorType.property.Length; j++) {
            propertyType propertyType = processorType.property[j];

            if ((propertyType.name == null) || (propertyType.value == null)) {
                throw new Exception("COOS properties must have a name and a value. Must be defined!");
            }

            props.Add(propertyType.name, propertyType.value);
        }

        Type procClass = PluginFactory.tryClass(cl, className);
        IProcessor processor = (IProcessor) Activator.CreateInstance(procClass);
        processor.setCoContainer(cl);
        processor.setName(name);
        processor.setProperties(new Dictionary<string, string>(props));
        processor.setShared(isShared);

        return processor;
    }
}
}