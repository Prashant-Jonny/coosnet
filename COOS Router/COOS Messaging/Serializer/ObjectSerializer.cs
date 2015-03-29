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
using System.IO;

using Org.Coos.Messaging.Util;
using Org.Coos.Util.Serialize;
using Org.Coos.Messaging;


namespace Org.Coos.Messaging.Serializer
{
//package org.coos.messaging.serializer;

//import org.coos.util.serialize.AFClassLoader;
//import org.coos.util.serialize.ObjectHelper;

//import java.io.DataInputStream;
//import java.io.ByteArrayInputStream;

//import org.coos.messaging.Serializer;


/**
 * @author Knut Eilif Husa, Tellu AS Serialization by java object serialization
 */
public class ObjectSerializer : ISerializer {
    public sbyte[] serialize(Object obj) /* throws Exception */ {
        return ObjectHelper.persist(obj);
    }

    public Object deserialize(sbyte[] bytes) /* throws Exception */ {
        DataInputStream dis = new DataInputStream(new MemoryStream(DataOutputStream.ToByteArray(bytes)));

        return ObjectHelper.resurrect(dis, null);
    }

    public Object deserialize(sbyte[] bytes, AFClassLoader cl) /* throws Exception */ {
        DataInputStream dis = new DataInputStream(new MemoryStream(DataOutputStream.ToByteArray(bytes)));

        return ObjectHelper.resurrect(dis, cl);
    }
}
}