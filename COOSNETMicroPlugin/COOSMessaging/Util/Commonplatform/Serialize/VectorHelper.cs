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

using Org.Coos.Messaging.Util;
using System.Collections;
using System.IO;
using System;


namespace Org.Coos.Util.Serialize
{

//package org.coos.util.serialize;

//import java.io.MemoryStream;
//import java.io.DataInputStream;
//import java.io.DataOutputStream;
//import java.io.IOException;
//import java.util.Hashtable;
//import java.util.ArrayList;

/**
 * Use this class to serialize ArrayList's
 * 
 * 
 * 
 */
public class VectorHelper {

	public static readonly int NULL = 0;
	public static readonly int INTEGER = 1;
	public static readonly int STRING = 2;
	public static readonly int LONG = 3;
	public static readonly int BOOLEAN = 4;
	public static readonly int AFSERIALIZABLE = 5;
	public static readonly int VECTOR = 6;
	public static readonly int HASHTABLE = 7;
	public static readonly int BYTEARRAY = 8;
	public static readonly int FLOAT = 9;
	public static readonly int STRINGARRAY = 10;
	public static readonly int DOUBLE = 11;
	public static readonly int DOUBLEARRAY = 12;

	public static sbyte[] persist(ArrayList v) /*throws IOException*/ {
		MemoryStream bout = new MemoryStream();
		DataOutputStream dout = new DataOutputStream(bout);
		persist(v, dout);
		return DataOutputStream.ToSByteArray(bout.ToArray());

	}

	public static void persist(ArrayList v, DataOutputStream dout) /*throws IOException*/ {

		if (v == null) {
			dout.writeInt(-1);
		} else {
			int n = v.Count;
			dout.writeInt(n);

			for (int i = 0; i < n; ++i) {
				object o = v[i];
				if (o is string) {
					dout.writeByte((byte)STRING);
					dout.writeUTF((string) o);
				} else if (o is int) {
					dout.writeByte((byte)INTEGER);
					dout.writeInt((int) o);

				} else if (o is double) {
					dout.writeByte((byte)DOUBLE);
					dout.writeDouble(((double) o));
				} else if (o is long) {
					dout.writeByte((byte)LONG);
				  
                  dout.writeLong((long)o);
				} else if (o is float) {
					// BUG dout.writeFloat((byte)FLOAT);
                    dout.writeByte((byte)FLOAT);
           		    dout.writeFloat(((float) o));

				} else if (o is bool) {
					dout.writeByte((byte)BOOLEAN);
					dout.writeBoolean((bool) o);
				} else if (o is ArrayList) {
					dout.writeByte((byte)VECTOR);
					VectorHelper.persist((ArrayList) o, dout);
				} else if (o is Hashtable) {
					dout.writeByte((byte)HASHTABLE);
					HashtableHelper.persist((Hashtable) o, dout);
				} else if (o is AFSerializer) {
                    throw new NotImplementedException("AFSerializer not implemented");
				
#if IMPLEMENT_LATER
					dout.writeByte((byte)AFSERIALIZABLE);
					dout.writeUTF(o.getClass().getName());
					byte[] data = ((AFSerializer) o).serialize();
					dout.writeInt(data.Length);
					if (data.Length > 0) {
						dout.write(data);
					}
#endif
				} else if (o is byte[]) {
					dout.writeByte((byte)BYTEARRAY);
					dout.writeInt(((byte[]) o).Length);
					dout.write(DataOutputStream.ToSByteArray((byte[]) o));
				} else if (o is String[]) {
					dout.writeByte((byte)STRINGARRAY);
					String[] tmp = (String[]) o;
					dout.writeInt(tmp.Length);
					for (int x = 0; x < tmp.Length; x++) {
						dout.writeUTF(tmp[x]);
					}
				} else if (o is double[]) {
                    
					dout.writeByte((byte)DOUBLEARRAY);
					double[] tmp = (double[]) o;
					dout.writeInt(tmp.Length);
					for (int x = 0; x < tmp.Length; x++) {
						dout.writeDouble(tmp[x]);
					}

				} else if (o == null) {

					dout.writeByte((byte)NULL);

				} else {
					throw new IOException("Cannot persist " + "object of type " + o.GetType().FullName);
				}
			}
		}
		dout.flush();
	} // persist

	// public static ArrayList resurrect(byte[] persisted) throws IOException {
	// ByteArrayInputStream bin = new ByteArrayInputStream(persisted);

	// DataInputStream din = new DataInputStream(bin);

	public static ArrayList resurrect(DataInputStream din, AFClassLoader loader) /*throws IOException */ {
		ArrayList v = new ArrayList();
		int n = din.readInt();
		if (n == -1) {
			v = null;
		} else {
			for (int i = 0; i < n; ++i) {
				int type = din.readByte();
				if (type == NULL) {
					v.Add(null);
				} else if (type == INTEGER) {
					v.Add(din.readInt());
				} else if (type == DOUBLE) {
					v.Add(din.readDouble());
				} else if (type == STRING) {
					v.Add(din.readUTF());
				} else if (type == LONG) {
                    v.Add(din.readLong());
				} else if (type == FLOAT) {
					v.Add(din.readFloat());
				} else if (type == BOOLEAN) {
					v.Add(din.readBoolean());
				} else if (type == VECTOR) {
					v.Add(VectorHelper.resurrect(din, loader));
				} else if (type == HASHTABLE) {
					v.Add(HashtableHelper.resurrect(din, loader));
				} else if (type == AFSERIALIZABLE) {
                    throw new NotImplementedException("Can not resurrect object of type " + type.ToString());
#if IMPLEMENT_LATER
                    String cname = din.readUTF();
					int len = din.readInt();
					byte[] tmp = new byte[len];
					din.readFully(tmp);
					try {
						Class cl;
						Object o;
						if (loader == null) {
							cl = Class.forName(cname);
							o = cl.newInstance();
							((AFSerializer) o).deSerialize(tmp, loader);
						} else {
							cl = loader.loadClass(cname);
							o = cl.newInstance();
							((AFSerializer) o).deSerialize(tmp, loader);
						}
						/*
						 * o = cl.newInstance(); ((AFSerializer)
						 * o).deSerialize(tmp);
						 */
						v.addElement(o);
					} catch (IOException e) {
						throw e;
					} catch (Exception e) {
						throw new IOException("Exception " + e.toString());
					}
#endif
				} else if (type == BYTEARRAY) {
					byte[] ba = new byte[din.readInt()];
					din.readFully(ba);
					v.Add(ba);
				} else if (type == STRINGARRAY) {
					String[] st = new String[din.readInt()];
					for (int x = 0; x < st.Length; x++)
						st[x] = din.readUTF();
					v.Add(st);
				} else if (type == DOUBLEARRAY) {


                    int length = din.readInt();
                    double[] tmp = new double[length];
                    for (int x = 0; x < length; x++)
                    {
                        tmp[x] = din.readDouble();
                    }
                    v.Add(tmp);
                }
                else {
					throw new IOException("Unknown " + "type " + type);
				}
			}
		}
		return v;
	}
}
}