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
using System;
namespace java.io
{
	
	/// <summary> Defines an interface for classes that are able to read typed data from some
	/// source. Typically, this data has been written by a class which implements
	/// {@link DataOutput}. Types that can be read include byte, 16-bit short, 32-bit
	/// int, 32-bit float, 64-bit long, 64-bit double, byte strings, and MUTF-8
	/// strings.
	/// 
	/// <h3>MUTF-8 (Modified UTF-8) Encoding</h3>
	/// <p>
	/// When encoding strings as UTF, implementations of {@code DataInput} and
	/// {@code DataOutput} use a slightly modified form of UTF-8, hereafter referred
	/// to as MUTF-8. This form is identical to standard UTF-8, except:
	/// <ul>
	/// <li>Only the one-, two-, and three-byte encodings are used.</li>
	/// <li>Code points in the range <code>U+10000</code> &hellip;
	/// <code>U+10ffff</code> are encoded as a surrogate pair, each of which is
	/// represented as a three-byte encoded value.</li>
	/// <li>The code point <code>U+0000</code> is encoded in two-byte form.</li>
	/// </ul>
	/// <p>
	/// Please refer to <a href="http://unicode.org">The Unicode Standard</a> for
	/// further information about character encoding. MUTF-8 is actually closer to
	/// the (relatively less well-known) encoding <a
	/// href="http://www.unicode.org/reports/tr26/">CESU-8</a> than to UTF-8 per se.
	/// 
	/// </summary>
	/// <seealso cref="DataInputStream">
	/// </seealso>
	/// <seealso cref="RandomAccessFile">
	/// </seealso>
	public interface DataInput
	{
		/// <summary> Reads a boolean.
		/// 
		/// </summary>
		/// <returns> the next boolean value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeBoolean(boolean)">
		/// </seealso>
		bool ReadBoolean();
		
		/// <summary> Reads an 8-bit byte value.
		/// 
		/// </summary>
		/// <returns> the next byte value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeByte(int)">
		/// </seealso>
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.io.DataInput.readByte' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		sbyte ReadSByte();
		
		/// <summary> Reads a 16-bit character value.
		/// 
		/// </summary>
		/// <returns> the next char value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeChar(int)">
		/// </seealso>
		char ReadChar();
		
		/// <summary> Reads a 64-bit double value.
		/// 
		/// </summary>
		/// <returns> the next double value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeDouble(double)">
		/// </seealso>
		double ReadDouble();
		
		/// <summary> Reads a 32-bit float value.
		/// 
		/// </summary>
		/// <returns> the next float value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeFloat(float)">
		/// </seealso>
		float ReadSingle();
		
		/// <summary> Reads bytes into the byte array {@code buffer}. This method will block
		/// until {@code buffer.length} number of bytes have been read.
		/// 
		/// </summary>
		/// <param name="buffer">the buffer to read bytes into.
		/// </param>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.write(byte[])">
		/// </seealso>
		/// <seealso cref="DataOutput.write(byte[], int, int)">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInput.readFully' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		void  readFully(sbyte[] buffer);
		
		/// <summary> Reads bytes and stores them in the byte array {@code buffer} starting at
		/// offset {@code offset}. This method blocks until {@code count} number of
		/// bytes have been read.
		/// 
		/// </summary>
		/// <param name="buffer">the byte array in which to store the bytes read.
		/// </param>
		/// <param name="offset">
		/// the initial position in {@code buffer} to store the bytes
		/// read.
		/// </param>
		/// <param name="count">
		/// the maximum number of bytes to store in {@code buffer}.
		/// </param>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.write(byte[])">
		/// </seealso>
		/// <seealso cref="DataOutput.write(byte[], int, int)">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInput.readFully' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		void  readFully(sbyte[] buffer, int offset, int count);
		
		/// <summary> Reads a 32-bit integer value.
		/// 
		/// </summary>
		/// <returns> the next int value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeInt(int)">
		/// </seealso>
		int ReadInt32();
		
		/// <summary> Returns a string containing the next line of text available from this
		/// stream. A line is made of zero or more characters followed by {@code
		/// '\n'}, {@code '\r'}, {@code "\r\n"} or the end of the stream. The string
		/// does not include the newline sequence.
		/// 
		/// </summary>
		/// <returns> the contents of the line or null if no characters have been read
		/// before the end of the stream.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		System.String ReadString();
		
		/// <summary> Reads a 64-bit long value.
		/// 
		/// </summary>
		/// <returns> the next long value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeLong(long)">
		/// </seealso>
		long ReadInt64();
		
		/// <summary> Reads a 16-bit short value.
		/// 
		/// </summary>
		/// <returns> the next short value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeShort(int)">
		/// </seealso>
		short ReadInt16();
		
		/// <summary> Reads an unsigned 8-bit byte value and returns it as an int.
		/// 
		/// </summary>
		/// <returns> the next unsigned byte value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeByte(int)">
		/// </seealso>
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.io.DataInput.readUnsignedByte' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		byte ReadByte();
		
		/// <summary> Reads a 16-bit unsigned short value and returns it as an int.
		/// 
		/// </summary>
		/// <returns> the next unsigned short value.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeShort(int)">
		/// </seealso>
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.io.DataInput.readUnsignedShort' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		System.UInt16 ReadUInt16();
		
		/// <summary> Reads a string encoded with {@link DataInput modified UTF-8}.
		/// 
		/// </summary>
		/// <returns> the next string encoded with {@link DataInput modified UTF-8}.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while reading.
		/// </summary>
		/// <seealso cref="DataOutput.writeUTF(java.lang.String)">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInput.readUTF' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		System.String readUTF();
		
		/// <summary> Skips {@code count} number of bytes. This method will not throw an
		/// {@link EOFException} if the end of the input is reached before
		/// {@code count} bytes where skipped.
		/// 
		/// </summary>
		/// <param name="count">the number of bytes to skip.
		/// </param>
		/// <returns> the number of bytes actually skipped.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs during skipping.
		/// </summary>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInput.skipBytes' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		int skipBytes(int count);
	}
}