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
//package org.coos.util.serialize;

//import java.io.*;

/**
 * Use this class to serialize Vector's
 * 
 * 
 * 
 */

using System.IO;
using System;

using Org.Coos.Messaging.Util;


namespace Org.Coos.Util.Serialize
{
public class ArrayHelper {

	public static sbyte[] persist(object obj) /*throws IOException*/ {
		MemoryStream bout = new MemoryStream();
		DataOutputStream dout = new DataOutputStream(bout);
		persist(obj, dout);

		return DataOutputStream.ToSByteArray(bout.ToArray());

	}

	public static void persist(object obj, DataOutputStream dout) /*throws IOException*/ {

		if (obj == null) {
			dout.writeInt(-1);
		} else {
			if (obj is string[]) {
				// String Array helper
				string[] stringArray = (string[]) obj;
				int n = stringArray.Length;
				dout.writeInt(n);
				StringHelper.persist("StringArray", dout);

				for (int i = 0; i < n; i++) {
					string s = stringArray[i];
					dout.write(StringHelper.persist(s));
				}
			} else if (obj is byte[]) {
				// sbyte array helper
				byte[] byteArray = (byte[]) obj;
				int n = byteArray.Length;
				dout.writeInt(n);
				StringHelper.persist("ByteArray", dout);
				dout.write(DataOutputStream.ToSByteArray(byteArray), 0, n);
			} else if (obj is int[]) {
				// sbyte array helper
				int[] intArray = (int[]) obj;
				int n = intArray.Length;
				dout.writeInt(n);
				StringHelper.persist("IntArray", dout);

				for (int i = 0; i < intArray.Length; i++) {
					int data = intArray[i];
					dout.writeInt(data);
				}
			} 
            else if (obj is AFSerializer[]) {
                throw new NotImplementedException("AFSerializer not implemented!");                
#if AFSERIALIZER
                
                AFSerializer[] afArray = (AFSerializer[]) obj;
				int n = afArray.Length;
				dout.writeInt(n);
				StringHelper.persist("AFArray", dout);

				AFSerializer obj;
				for (int i = 0; i < n; i++) {
					obj = afArray[i];
					if (obj == null) {
						dout.writeUTF("null");
					} else {
						dout.writeUTF(obj.getClass().getName());
						sbyte[] data = obj.serialize();
						dout.writeInt(data.length);
						if (data.length > 0) {
							dout.write(data);
						}
					}
				}
#endif
                }

		}

		dout.flush();

	} // persist

	/**
	 * Resurrect array of String or sbyte
	 * 
	 * @param din
	 *            data input stream
	 * @return reference to array
	 * @throws IOException
	 */
	public static object resurrect(DataInputStream din) /*throws IOException*/ {
		object result = null;
		int n = din.readInt();

		if (n != -1) {
			string s = StringHelper.resurrect(din);

			if (s.Equals("StringArray")) {
				string[] stringArray = null;
				stringArray = new string[n];

				for (int i = 0; i < n; i++) {
					string sin = StringHelper.resurrect(din);
					stringArray[i] = sin;
				}

				result = stringArray;
			} else if (s.Equals("ByteArray")) {
				byte[] byteArray = new byte[n];
				din.readFully(byteArray, 0, n);
				result = byteArray;
			} else if (s.Equals("IntArray")) {
				int[] intArray = new int[n];

				for (int i = 0; i < intArray.Length; i++) {
					intArray[i] = din.readInt();
				}

				result = intArray;

			} // else if (s.equals("null")) returns null

		}

		return result;
	} // resurrect

	/**
	 * Resurrect array of String or sbyte
	 * 
	 * @param din
	 *            data input stream
	 * @return reference to array
	 * @throws IOException
	 */
	public static object resurrect(DataInputStream din, AFClassLoader loader) /*throws IOException */{
		object result = null;
		int n = din.readInt();

		if (n != -1) {
			string s = StringHelper.resurrect(din);

			if (s.Equals("StringArray")) {
				string[] stringArray = null;
				stringArray = new string[n];

				for (int i = 0; i < n; i++) {
					string sin = StringHelper.resurrect(din);
					stringArray[i] = sin;
				}

				result = stringArray;
			} else if (s.Equals("ByteArray")) {
				byte[] byteArray = new byte[n];
				din.readFully(byteArray, 0, n);
				result = byteArray;
			} else if (s.Equals("IntArray")) {
				int[] intArray = new int[n];

				for (int i = 0; i < intArray.Length; i++) {
					intArray[i] = din.readInt();
				}

				result = intArray;
			} else if (s.Equals("AFArray")) {
                throw new NotImplementedException("AF deserialization not implemented");
#if AFSERIALIZER
                
				AFSerializer[] afArray = new AFSerializer[n];
				for (int i = 0; i < n; i++) {
					string cname = din.readUTF();
					if (cname.equals("null")) {
						afArray[i] = null;
					} else {
						int len = din.readInt();
						sbyte[] tmp = new sbyte[len];
						din.readFully(tmp);
						try {
							Class cl;
							AFSerializer obj;
							if (loader == null) {
								cl = Class.forName(cname);
								obj = (AFSerializer) cl.newInstance();
								obj.deSerialize(tmp, loader);
							} else {
								cl = loader.loadClass(cname);
								obj = (AFSerializer) cl.newInstance();
								obj.deSerialize(tmp, loader);
							}
							afArray[i] = obj;
						} catch (IOException e) {
							throw e;
						} catch (Exception e) {
							throw new IOException("Exception " + e.toString());
						}
					}
				}
				result = afArray;
#endif
			}

		}

		return result;
	} // resurrect
}
}