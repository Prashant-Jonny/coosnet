using System;
/*
*  Licensed to the Apache Software Foundation (ASF) under one or more
*  contributor license agreements.  See the NOTICE file distributed with
*  this work for additional information regarding copyright ownership.
*  The ASF licenses this file to You under the Apache License, Version 2.0
*  (the "License"); you may not use this file except in compliance with
*  the License.  You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing, software
*  distributed under the License is distributed on an "AS IS" BASIS,
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*  See the License for the specific language governing permissions and
*  limitations under the License.
*/

namespace java.io
{

    //package java.io;

    //import org.apache.harmony.luni.internal.nls.Messages;

    /// <summary> Wraps an existing {@link OutputStream} and writes typed data to it.
    /// Typically, this stream can be read in by DataInputStream. Types that can be
    /// written include byte, 16-bit short, 32-bit int, 32-bit float, 64-bit long,
    /// 64-bit double, byte strings, and {@link DataInput MUTF-8} encoded strings.
    /// 
    /// </summary>
    /// <seealso cref="DataInputStream">
    /// </seealso>
    public class DataOutputStream : DataOutput
    {

        /// <summary> The number of bytes written out so far.</summary>
        protected internal int written;
        internal sbyte[] buff;

        /// <summary> Constructs a new {@code DataOutputStream} on the {@code OutputStream}
        /// {@code out}. Note that data written by this stream is not in a human
        /// readable form but can be reconstructed by using a {@link DataInputStream}
        /// on the resulting output.
        /// 
        /// </summary>
        /// <param name="out">the target stream for writing.
        /// </param>
        public DataOutputStream(NetworkStream out_Renamed)
        {
            buff = new sbyte[8];
        }

        /// <summary> Flushes this stream to ensure all pending data is sent out to the target
        /// stream. This implementation then also flushes the target stream.
        /// 
        /// </summary>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs attempting to flush this stream.
        /// </summary>
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
#if DELAY_IMPLEMENTATION
    public virtual void  flush()
	{
		base.flush();
	}
#endif

        /// <summary> Returns the total number of bytes written to the target stream so far.
        /// 
        /// </summary>
        /// <returns> the number of bytes written to the target stream.
        /// </returns>
        public int size()
        {
            if (written < 0)
            {
                written = System.Int32.MaxValue;
            }
            return written;
        }

        /// <summary> Writes {@code count} bytes from the byte array {@code buffer} starting at
        /// {@code offset} to the target stream.
        /// 
        /// </summary>
        /// <param name="buffer">the buffer to write to the target stream.
        /// </param>
        /// <param name="offset">
        /// the index of the first byte in {@code buffer} to write.
        /// </param>
        /// <param name="count">
        /// the number of bytes from the {@code buffer} to write.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <throws>  NullPointerException </throws>
        /// <summary>             if {@code buffer} is {@code null}.
        /// </summary>
        /// <seealso cref="DataInputStream.readFully(byte[])">
        /// </seealso>
        /// <seealso cref="DataInputStream.readFully(byte[], int, int)">
        /// </seealso>
        public virtual void write(sbyte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new NullPointerException(Messages.getString("luni.11")); //$NON-NLS-1$
            }
            out_Renamed.write(buffer, offset, count);
            written += count;
        }

        /// <summary> Writes a byte to the target stream. Only the least significant byte of
        /// the integer {@code oneByte} is written.
        /// 
        /// </summary>
        /// <param name="oneByte">the byte to write to the target stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readByte()">
        /// </seealso>
        public virtual void write(int oneByte)
        {
            out_Renamed.write(oneByte);
            written++;
        }

        /// <summary> Writes a boolean to the target stream.
        /// 
        /// </summary>
        /// <param name="val">the boolean value to write to the target stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readBoolean()">
        /// </seealso>
        public void writeBoolean(bool val)
        {
            out_Renamed.write(val ? 1 : 0);
            written++;
        }

        /// <summary> Writes an 8-bit byte to the target stream. Only the least significant
        /// byte of the integer {@code val} is written.
        /// 
        /// </summary>
        /// <param name="val">the byte value to write to the target stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readByte()">
        /// </seealso>
        /// <seealso cref="DataInputStream.readUnsignedByte()">
        /// </seealso>
        public void writeByte(int val)
        {
            out_Renamed.write(val);
            written++;
        }

        /// <summary> Writes the low order bytes from a string to the target stream.
        /// 
        /// </summary>
        /// <param name="str">the string containing the bytes to write to the target stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readFully(byte[])">
        /// </seealso>
        /// <seealso cref="DataInputStream.readFully(byte[],int,int)">
        /// </seealso>
        public void writeBytes(System.String str)
        {
            if (str.Length == 0)
            {
                return;
            }
            sbyte[] bytes = new sbyte[str.Length];
            for (int index = 0; index < str.Length; index++)
            {
                bytes[index] = (sbyte)str[index];
            }
            out_Renamed.write(bytes);
            written += bytes.Length;
        }

        /// <summary> Writes a 16-bit character to the target stream. Only the two lower bytes
        /// of the integer {@code val} are written, with the higher one written
        /// first. This corresponds to the Unicode value of {@code val}.
        /// 
        /// </summary>
        /// <param name="val">the character to write to the target stream
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readChar()">
        /// </seealso>
        public void writeChar(int val)
        {
            buff[0] = (sbyte)(val >> 8);
            buff[1] = (sbyte)val;
            out_Renamed.write(buff, 0, 2);
            written += 2;
        }

        /// <summary> Writes the 16-bit characters contained in {@code str} to the target
        /// stream.
        /// 
        /// </summary>
        /// <param name="str">the string that contains the characters to write to this
        /// stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readChar()">
        /// </seealso>
        public void writeChars(System.String str)
        {
            sbyte[] newBytes = new sbyte[str.Length * 2];
            for (int index = 0; index < str.Length; index++)
            {
                int newIndex = index == 0 ? index : index * 2;
                newBytes[newIndex] = (sbyte)(str[index] >> 8);
                newBytes[newIndex + 1] = (sbyte)str[index];
            }
            out_Renamed.write(newBytes);
            written += newBytes.Length;
        }

        /// <summary> Writes a 64-bit double to the target stream. The resulting output is the
        /// eight bytes resulting from calling Double.doubleToLongBits().
        /// 
        /// </summary>
        /// <param name="val">the double to write to the target stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readDouble()">
        /// </seealso>
        public void writeDouble(double val)
        {
            //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
            writeLong(Double.doubleToLongBits(val));
        }

        /// <summary> Writes a 32-bit float to the target stream. The resulting output is the
        /// four bytes resulting from calling Float.floatToIntBits().
        /// 
        /// </summary>
        /// <param name="val">the float to write to the target stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readFloat()">
        /// </seealso>
        public void writeFloat(float val)
        {
            //UPGRADE_ISSUE: Method 'java.lang.Float.floatToIntBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangFloatfloatToIntBits_float'"
            writeInt(Float.floatToIntBits(val));
        }

        /// <summary> Writes a 32-bit int to the target stream. The resulting output is the
        /// four bytes, highest order first, of {@code val}.
        /// 
        /// </summary>
        /// <param name="val">the int to write to the target stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readInt()">
        /// </seealso>
        public void writeInt(int val)
        {
            buff[0] = (sbyte)(val >> 24);
            buff[1] = (sbyte)(val >> 16);
            buff[2] = (sbyte)(val >> 8);
            buff[3] = (sbyte)val;
            out_Renamed.write(buff, 0, 4);
            written += 4;
        }

        /// <summary> Writes a 64-bit long to the target stream. The resulting output is the
        /// eight bytes, highest order first, of {@code val}.
        /// 
        /// </summary>
        /// <param name="val">the long to write to the target stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readLong()">
        /// </seealso>
        public void writeLong(long val)
        {
            buff[0] = (sbyte)(val >> 56);
            buff[1] = (sbyte)(val >> 48);
            buff[2] = (sbyte)(val >> 40);
            buff[3] = (sbyte)(val >> 32);
            buff[4] = (sbyte)(val >> 24);
            buff[5] = (sbyte)(val >> 16);
            buff[6] = (sbyte)(val >> 8);
            buff[7] = (sbyte)val;
            out_Renamed.write(buff, 0, 8);
            written += 8;
        }

        internal virtual int writeLongToBuffer(long val, sbyte[] buffer, int offset)
        {
            buffer[offset++] = (sbyte)(val >> 56);
            buffer[offset++] = (sbyte)(val >> 48);
            buffer[offset++] = (sbyte)(val >> 40);
            buffer[offset++] = (sbyte)(val >> 32);
            buffer[offset++] = (sbyte)(val >> 24);
            buffer[offset++] = (sbyte)(val >> 16);
            buffer[offset++] = (sbyte)(val >> 8);
            buffer[offset++] = (sbyte)val;
            return offset;
        }

        /// <summary> Writes the specified 16-bit short to the target stream. Only the lower
        /// two bytes of the integer {@code val} are written, with the higher one
        /// written first.
        /// 
        /// </summary>
        /// <param name="val">the short to write to the target stream.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <seealso cref="DataInputStream.readShort()">
        /// </seealso>
        /// <seealso cref="DataInputStream.readUnsignedShort()">
        /// </seealso>
        public void writeShort(int val)
        {
            buff[0] = (sbyte)(val >> 8);
            buff[1] = (sbyte)val;
            out_Renamed.write(buff, 0, 2);
            written += 2;
        }

        internal virtual int writeShortToBuffer(int val, sbyte[] buffer, int offset)
        {
            buffer[offset++] = (sbyte)(val >> 8);
            buffer[offset++] = (sbyte)val;
            return offset;
        }

        /// <summary> Writes the specified encoded in {@link DataInput modified UTF-8} to this
        /// stream.
        /// 
        /// </summary>
        /// <param name="str">the string to write to the target stream encoded in
        /// {@link DataInput modified UTF-8}.
        /// </param>
        /// <throws>  IOException </throws>
        /// <summary>             if an error occurs while writing to the target stream.
        /// </summary>
        /// <throws>  UTFDataFormatException </throws>
        /// <summary>             if the encoded string is longer than 65535 bytes.
        /// </summary>
        /// <seealso cref="DataInputStream.readUTF()">
        /// </seealso>
        public void writeUTF(System.String str)
        {
            long utfCount = countUTFBytes(str);
            if (utfCount > 65535)
            {
                throw new UTFDataFormatException(Messages.getString("luni.AB")); //$NON-NLS-1$
            }
            sbyte[] buffer = new sbyte[(int)utfCount + 2];
            int offset = 0;
            offset = writeShortToBuffer((int)utfCount, buffer, offset);
            offset = writeUTFBytesToBuffer(str, (int)utfCount, buffer, offset);
            write(buffer, 0, offset);
        }

        internal virtual long countUTFBytes(System.String str)
        {
            int utfCount = 0, length = str.Length;
            for (int i = 0; i < length; i++)
            {
                int charValue = str[i];
                if (charValue > 0 && charValue <= 127)
                {
                    utfCount++;
                }
                else if (charValue <= 2047)
                {
                    utfCount += 2;
                }
                else
                {
                    utfCount += 3;
                }
            }
            return utfCount;
        }

        internal virtual int writeUTFBytesToBuffer(System.String str, long count, sbyte[] buffer, int offset)
        {
            int length = str.Length;
            for (int i = 0; i < length; i++)
            {
                int charValue = str[i];
                if (charValue > 0 && charValue <= 127)
                {
                    buffer[offset++] = (sbyte)charValue;
                }
                else if (charValue <= 2047)
                {
                    buffer[offset++] = (sbyte)(0xc0 | (0x1f & (charValue >> 6)));
                    buffer[offset++] = (sbyte)(0x80 | (0x3f & charValue));
                }
                else
                {
                    buffer[offset++] = (sbyte)(0xe0 | (0x0f & (charValue >> 12)));
                    buffer[offset++] = (sbyte)(0x80 | (0x3f & (charValue >> 6)));
                    buffer[offset++] = (sbyte)(0x80 | (0x3f & charValue));
                }
            }
            return offset;
        }
    }
}