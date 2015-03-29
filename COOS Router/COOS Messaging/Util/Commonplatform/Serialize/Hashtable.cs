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
using System;
using System.IO;

using Org.Coos.Messaging.Util;

namespace Org.Coos.Util.Serialize
{
//package org.coos.util.serialize;

//import java.io.ByteArrayOutputStream;
//import java.io.DataInputStream;
//import java.io.DataOutputStream;
//import java.io.IOException;
//import java.util.Enumeration;
//import java.util.Hashtable;
//import java.util.Vector;

/**
 * Hashtable helper for persitance and resurrection.
 * 
 * @author Geir Melby, Tellu AS
 */
public class HashtableHelper {
	/**
	 * Helper for Hashtable serialization.
	 * 
	 * @param hTable
	 * @return
	 * @throws IOException
	 */

	public static sbyte[] persist(Hashtable hTable) /*throws IOException*/ {
		MemoryStream bout = new MemoryStream();
		DataOutputStream dout = new DataOutputStream(bout);
		persist(hTable, dout);
		return DataOutputStream.ToSByteArray(bout.ToArray());

	}

	public static void persist(Hashtable hTable, DataOutputStream dout) /*throws IOException*/ {

		if (hTable == null) {
			dout.writeBoolean(false);
		} else {
			dout.writeBoolean(true);
			// write the vector values.
			VectorHelper.persist(getValues(hTable), dout);

			// write the key table
			ArrayList keys = new ArrayList();
			
            foreach (object eKeys in hTable.Keys)
                keys.Add(eKeys);

			
			VectorHelper.persist(keys, dout);
		}

		dout.flush();

	}

	public static Hashtable resurrect(DataInputStream din, AFClassLoader cl) /*throws IOException*/ {
		if (din.readBoolean()) {
			// There exists a hastable.
			Hashtable ht = new Hashtable();
			ArrayList values = VectorHelper.resurrect(din, cl);
			ArrayList keys = VectorHelper.resurrect(din, cl);

			if (keys.Count != values.Count) {
				throw (new IOException("Hashtable resurrect: keys and values length does not match")); // should not happen.
			}

			for (int i = 0; i < keys.Count; i++) {
				// fill the hashtable
				ht.Add(keys[i], values[i]);
			}

			return ht;
		}

		return null;
	}

	private static ArrayList getValues(Hashtable h1) {
		ArrayList v1 = new ArrayList();
		foreach (object v in h1.Values)
            v1.Add(v);

		return v1;
	}
}
}