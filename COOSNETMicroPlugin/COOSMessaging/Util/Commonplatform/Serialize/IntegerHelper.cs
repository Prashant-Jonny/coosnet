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
using System.IO;
using System;

namespace Org.Coos.Util.Serialize
{
//package org.coos.util.serialize;

//import java.io.*;

/**
 * Telenor FoU 2004 User: t537929
 * 
 * 
 */
public class IntegerHelper {

	public static sbyte[] persist(int i) /*throws IOException*/ {
		MemoryStream bout = new MemoryStream();
		DataOutputStream dout = new DataOutputStream(bout);
		persist(i, dout);
		return DataOutputStream.ToSByteArray(bout.ToArray());
	}

	public static void persist(int i, DataOutputStream dout) /*throws IOException */{
#if IMPLEMENT_NULL_CHECK
		if (i == null) {
			dout.writeBoolean(false);
		} else 
#endif
        {
			dout.writeBoolean(true);
			dout.writeInt(i);
		}

		dout.flush();
	}

	public static int resurrect(DataInputStream din) /*throws IOException */ {
		if (din.readBoolean()) {
			// The String is not null
			return din.readInt();
		} else
            throw new NotImplementedException("Resurrect of integer failed, is null");
#if IMPLEMENT_NULL_CHECK
        else {
			return null;
		}
#endif
	}

	public static int resurrect(sbyte[] data) /*throws IOException*/ {
		DataInputStream din = new DataInputStream(new MemoryStream(DataOutputStream.ToByteArray(data)));

		return IntegerHelper.resurrect(din);
	}
}
}
