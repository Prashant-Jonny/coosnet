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
using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;
using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Ping;
using System.Collections;
using Org.Coos.Messaging.NETMicro.Transport;

namespace Org.Coos.Messaging
{


    /**
     * Factory for Plugin instances
     *
     * @author Knut Eilif Husa, Tellu AS
     */
    public class PluginFactory /* extends COFactory */ {

        public static string JVM_TRANSPORT_CLASS = "org.coos.messaging.transport.JvmTransport";
        public static string PLUGIN_CHANNEL_CLASS = "org.coos.messaging.plugin.PluginChannel";

        //private static Map<String, Processor> sharedProcessors = new HashMap<String, Processor>();
        //private static Map<String, ProcessorType> processorTypes = new HashMap<String, ProcessorType>();


        private PluginFactory()
        {
    //        SerializerFactory.registerSerializer(IMessagePrimitives.SERIALIZATION_METHOD_JAVA, new ObjectJavaSerializer());
        }

        // Used in tests only
        public static Plugin createPlugin(string name, IEndpoint endpoint, IChannel channel, ITransport transport, string segment, string host, string port, string retry)
         {

        //COContainer cl = new COContainer() {

        //        public Class<?> loadClass(String className) throws ClassNotFoundException {
        //            return Class.forName(className);
        //        }

        //        public InputStream getResource(String resourceName) throws IOException {
        //            InputStream is = COContainer.class.getResourceAsStream(resourceName);

        //            return substitute(is);
        //        }

        //        public Object getObject(String name) {
        //            return null;
        //        }

        //        public void start() {
        //        }

        //        public void stop() {
        //        }
        //    };

        Plugin plugin = new Plugin();

        // JAVA Class<?> pluginClass = PluginFactory.tryClass(cl, className);
        //IEndpoint endpoint = (IEndpoint)new PingEndpoint();
        // endpoint.setCoContainer(cl);
        //if(name != null && name.matches(".*\\.")){
        //    throw new Exception("Name :"+ name + " not allowed. '.' is reserved for separating segments");
        //}
        endpoint.setEndpointUri("coos://" + name);
        endpoint.setProperties(new Hashtable());
        plugin.setEndpoint(endpoint);

        URIHelper helper = new URIHelper(endpoint.getEndpointUri());

        if (helper.isEndpointUuid()) {
            endpoint.setEndpointUuid(name);
        }

        endpoint.setName(name);

        if (segment == null) {
            segment = ".";
        }

        //Class<?> channelClass = PluginFactory.tryClass(cl, PLUGIN_CHANNEL_CLASS);
        //PluginChannel channel =  new PluginChannel();
        channel.addProtocol("coos");
        //channel.setCoContainer(cl);
        channel.setSegment(segment);
        plugin.addChannel(channel);
       
        Hashtable properties = new Hashtable();
        transport.setProperty("host", host);
        transport.setProperty("port", port);
        transport.setProperty("retry", retry);

        //if (coosInstanceName != null)
        //    properties.Add("COOSInstanceName", coosInstanceName);

        //properties.Add("ChannelServerName", channelServerName);

        //Class<?> transportClass = PluginFactory.tryClass(cl, JVM_TRANSPORT_CLASS);
        //ITransport transport = (ITransport) 
        transport.setProperties(properties);
        channel.setTransport(transport);

        

        return plugin;
    }


    }
}
