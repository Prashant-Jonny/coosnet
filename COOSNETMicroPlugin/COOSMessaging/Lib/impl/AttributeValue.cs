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

using System.IO;

using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging.Impl
{
    //package org.coos.messaging.impl;

    //import java.io.ByteArrayInputStream;
    //import java.io.ByteArrayOutputStream;
    //import java.io.DataInputStream;
    //import java.io.DataOutputStream;
    //import java.io.IOException;
    //import java.io.Serializable;

    //import org.coos.util.serialize.AFClassLoader;
    //import org.coos.util.serialize.AFSerializer;


    public class AttributeValue : AFSerializer, ISerializable
    {


        /** <em>null</em> type, the default type of an attribute */
        public const int NULL = 0;

        /** string of bytes */
        public const int BYTEARRAY = 1;

        /**
         * string of bytes
         *
         **/
        public const int STRING = 1;

        /**
         * integer type.
         *
         * corresponds to the Java <code>long</code> type.
         **/
        public const int LONG = 2;

        /**
         * integer type.
         *
         * corresponds to the Java <code>int</code> type.
         **/
        public const int INT = 2;

        /**
         * double type.
         *
         * corresponds to the Java <code>double</code> type.
         **/
        public const int DOUBLE = 3;

        /**
         * bool type.
         *
         * corresponds to the Java <code>bool</code> type.
         **/
        public const int BOOL = 4;

        private int type;

        private byte[] sval;
        private long ival;
        private double dval;
        private bool bval;

        public AttributeValue()
        {
            type = NULL;
        }

        public AttributeValue(AttributeValue x)
        {

            if (x == null)
            {
                type = NULL;

                return;
            }

            type = x.type;

            switch (type)
            {

                case INT:
                    ival = x.ival;

                    break;

                case BOOL:
                    bval = x.bval;

                    break;

                case DOUBLE:
                    dval = x.dval;

                    break;

                case BYTEARRAY:
                    sval = cloneByteArray((byte[])x.sval);

                    break;
            }
        }

        public AttributeValue(string s)
        {

            if (s == null)
            {
                type = NULL;

                return;
            }

            type = BYTEARRAY;
            sval = s.getBytes();
        }

        public AttributeValue(byte[] s)
        {
            type = BYTEARRAY;
            sval = cloneByteArray((byte[])s);
        }

        public AttributeValue(long i)
        {
            type = LONG;
            ival = i;
            sval = null;
        }

        public AttributeValue(bool b)
        {
            type = BOOL;
            bval = b;
            sval = null;
        }

        public AttributeValue(double d)
        {
            type = DOUBLE;
            dval = d;
            sval = null;
        }

        private byte[] cloneByteArray(byte[] target)
        {
            byte[] retb = new byte[target.Length];

            for (int i = 0; i < target.Length; i++)
            {
                retb[i] = target[i];
            }

            return retb;
        }

        //
        // other types here ...work in progress...
        //

        public int getType()
        {
            return type;
        }

        public int intValue()
        {

            switch (type)
            {

                case LONG:
                    return (int)ival;

                case BOOL:
                    return bval ? 1 : 0;

                case DOUBLE:
                    return (int)dval;

                case BYTEARRAY:
                    return Integer.parseInt(new string(sval));

                default:
                    return 0;
            }
        }

        public long longValue()
        {

            switch (type)
            {

                case LONG:
                    return ival;

                case BOOL:
                    return bval ? 1 : 0;

                case DOUBLE:
                    return (int)dval;

                case BYTEARRAY:
                    return Long.parseLong(new string(sval));

                default:
                    return 0;
            }
        }

        public double doubleValue()
        {

            switch (type)
            {

                case LONG:
                    return ival;

                case BOOL:
                    return bval ? 1 : 0;

                case DOUBLE:
                    return dval;

                case BYTEARRAY:
                    return Double.valueOf(new string(sval)).doubleValue();

                default:
                    return 0;
            }
        }

        public bool boolValue()
        {

            switch (type)
            {

                case LONG:
                    return ival != 0;

                case BOOL:
                    return bval;

                case DOUBLE:
                    return dval != 0;

                case BYTEARRAY:
                    return new bool(new string(sval).equalsIgnoreCase("true")).boolValue();

                default:
                    return false;
            }
        }

        public string stringValue()
        {

            switch (type)
            {

                case LONG:
                    return string.valueOf(ival);

                case BOOL:
                    return string.valueOf(bval);

                case DOUBLE:
                    return string.valueOf(dval);

                case BYTEARRAY:
                    return new string(sval);

                default:
                    return "";
            }
        }

        public byte[] byteArrayValue()
        {

            switch (type)
            {

                case LONG:
                    return string.valueOf(ival).getBytes();

                case BOOL:
                    return string.valueOf(bval).getBytes();

                case DOUBLE:
                    return string.valueOf(dval).getBytes();

                case BYTEARRAY:
                    return sval;

                default:
                    return null;
            }
        }

        public bool isEqualTo(AttributeValue x)
        {

            switch (type)
            {

                case BYTEARRAY:

                    if (sval.Length != x.sval.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < sval.Length; i++)
                    {

                        if (sval[i] != x.sval[i])
                        {
                            return false;
                        }
                    }

                    return true;

                case LONG:
                    return ival == x.longValue();

                case DOUBLE:
                    return dval == x.doubleValue();

                case BOOL:
                    return bval == x.boolValue();

                default:
                    return false;
            }
        }

        public override string ToString()
        {
            string s = "Attribute: Type: ";

            switch (type)
            {

                case LONG:
                    return s + "long, Value: " + stringValue();

                case BOOL:
                    return s + "bool, Value: " + stringValue();

                case DOUBLE:
                    return s + "double, Value: " + stringValue();

                case BYTEARRAY:
                    return s + "string, Value: " + stringValue();

                default:
                    return null;
            }

        }

        public int hashCode()
        {
            return this.ToString().GetHashCode();
        }

        public MemoryStream deSerialize(byte[] data, AFClassLoader cl)
        {
            MemoryStream bin = new MemoryStream(data);
            DataInputStream din = new DataInputStream(bin);
            type = din.readInt();

            if (type != 0)
            {

                switch (type)
                {

                    case LONG:
                        ival = din.readLong();

                        break;

                    case BOOL:
                        bval = din.readbool();

                        break;

                    case DOUBLE:
                        dval = din.readDouble();

                        break;

                    case BYTEARRAY:

                        int l = din.readInt();
                        sval = new byte[l];
                        din.read(sval);

                        break;

                    default:

                }
            }

            return null;
        }

        public byte[] serialize()
        {
            MemoryStream bout = new MemoryStream();
            DataOutputStream dout = new DataOutputStream(bout);
            dout.writeInt(type);

            if (type != 0)
            {

                switch (type)
                {

                    case LONG:
                        dout.writeLong(ival);

                        break;

                    case BOOL:
                        dout.writebool(bval);

                        break;

                    case DOUBLE:
                        dout.writeDouble(dval);

                        break;

                    case BYTEARRAY:
                        dout.writeInt(sval.Length);
                        dout.write(sval);

                        break;

                    default:

                }
            }

            dout.flush();

            return bout.ToArray();
        }

    }
}
