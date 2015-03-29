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
using System.Collections;
using Microsoft.SPOT;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Util.Serialize
{
//package org.coos.util.serialize;

//import java.io.ByteArrayOutputStream;
//import java.io.DataInputStream;
//import java.io.DataOutputStream;
//import java.util.Hashtable;
//import java.util.Vector;

/**
 * User: Knut Eilif
 * 
 * 
 */

public class ObjectHelper {

   
	public static sbyte[] persist(object obj) /* throws Exception */{
        //Debug.GC(true);

	   MemoryStream  bout = new MemoryStream();
		DataOutputStream dout = new DataOutputStream(bout);
		persist(obj, dout);
		return DataOutputStream.ToSByteArray(bout.ToArray());

	}

	public static void persist(object obj, DataOutputStream dout) /* throws Exception */ {
        //Debug.GC(true);

		if (obj is Hashtable) {
			dout.writeByte(1);
			dout.write(HashtableHelper.persist((Hashtable) obj));
		} else if (obj is string) {
			dout.writeByte(2);
			dout.write(StringHelper.persist((string) obj));
		} else if (obj is ArrayList) {
			dout.writeByte(3);
			dout.write(VectorHelper.persist((ArrayList) obj));
		} else if (obj is byte[] || obj is string[]
				|| obj is  int[] || obj is AFSerializer[]) {
			dout.writeByte(4);
			dout.write(ArrayHelper.persist(obj));
		} else if (obj is AFSerializer) {
            throw new NotImplementedException("AFSerializer not implemeneted!");
#if AFSERIALIZER
			dout.writeByte(5);
			if (obj == null) {
				dout.writeInt(-1);
			} else {

				dout.writeUTF(obj.getClass().getName());
				byte[] data = ((AFSerializer) obj).serialize();
				dout.writeInt(data.Length);
				if (data.Length > 0) {
					dout.write(data);
				}
			}
			dout.flush();
#endif

		} else if (obj is int) {
			dout.writeByte(6);
			IntegerHelper.persist((int) obj, dout);
		} else {
			throw new Exception("Cannot serialize obj of type " + obj.GetType().FullName);
		}
	}

	public static object resurrect(DataInputStream din, AFClassLoader loader) /* throws Exception */ {
		byte type = din.readByte();
		switch (type) {
		case 1:
			return HashtableHelper.resurrect(din, loader);
		case 2:
			return StringHelper.resurrect(din);
		case 3:
			return VectorHelper.resurrect(din, loader);
		case 4:
			return ArrayHelper.resurrect(din, loader);
		case 5:
            throw new NotImplementedException("AF deserialization not implemented!");
#if AFSERIALIZER
			string cname = din.readUTF();
			int len = din.readInt();
			byte[] tmp = new byte[len];
			din.readFully(tmp);

			Class cl;
               
			object o;
			if (loader == null) {
				cl = Class.forName(cname);
				o = cl.newInstance();
				((AFSerializer) o).deSerialize(tmp, loader);
			} else {
				cl = loader.loadClass(cname);
				o = cl.newInstance();
				((AFSerializer) o).deSerialize(tmp, loader);
			}
			return o;
#endif
		case 6:
			return IntegerHelper.resurrect(din);
		default:
			throw new Exception("Can not resurrect obj of type " + type);

		}
	}
}
}
