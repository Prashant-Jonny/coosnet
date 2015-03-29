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

using System.Collections;

using Org.Coos.Messaging.Impl;

using Org.Coos.Messaging.Serializer;

namespace Org.Coos.Messaging {


//import java.util.Hashtable;

//import org.coos.messaging.serializer.ObjectSerializer;


/**
 * @author Knut Eilif Husa, Tellu AS
 * Factory for serialization utility
 */
public class SerializerFactory {

    /// <summary>
    /// Holds all registered serializers
    /// </summary>
    private static Hashtable serializers = new Hashtable();
    
    /// <summary>
    /// The default (ObjectSerializer)
    /// </summary>
    private static ISerializer defaultSerializer = new ObjectSerializer();

    /// <summary>
    /// It seems like we register two serializers AF and Default (Java)
    /// </summary>
    static SerializerFactory() {
        registerSerializer(IMessagePrimitives.SERIALIZATION_METHOD_AF, defaultSerializer);
        registerSerializer(IMessagePrimitives.SERIALIZATION_METHOD_DEFAULT, defaultSerializer);
    }

    public static ISerializer getDefaultSerializer() {
        return defaultSerializer;
    }

    public static void registerSerializer(string method, ISerializer serializer) {
        serializers.Add(method, serializer);
    }

    public static ISerializer getSerializer(string method) {
        return (ISerializer) serializers[method];
    }

}
}
