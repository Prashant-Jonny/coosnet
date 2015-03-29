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
	
	/// <summary> Defines an interface for classes that are able to write typed data to some
	/// target. Typically, this data can be read in by a class which implements
	/// DataInput. Types that can be written include byte, 16-bit short, 32-bit int,
	/// 32-bit float, 64-bit long, 64-bit double, byte strings, and {@link DataInput
	/// MUTF-8} encoded strings.
	/// 
	/// </summary>
	/// <seealso cref="DataOutputStream">
	/// </seealso>
	/// <seealso cref="RandomAccessFile">
	/// </seealso>
	public interface DataOutput
	{
		
		/// <summary> Writes the entire contents of the byte array {@code buffer} to this
		/// stream.
		/// 
		/// </summary>
		/// <param name="buffer">the buffer to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readFully(byte[])">
		/// </seealso>
		/// <seealso cref="DataInput.readFully(byte[], int, int)">
		/// </seealso>
		void  Write(System.Byte[] buffer);
		
		/// <summary> Writes {@code count} bytes from the byte array {@code buffer} starting at
		/// offset {@code index}.
		/// 
		/// </summary>
		/// <param name="buffer">the buffer to write.
		/// </param>
		/// <param name="offset">
		/// the index of the first byte in {@code buffer} to write.
		/// </param>
		/// <param name="count">
		/// the number of bytes from the {@code buffer} to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readFully(byte[])">
		/// </seealso>
		/// <seealso cref="DataInput.readFully(byte[], int, int)">
		/// </seealso>
		void  Write(System.Byte[] buffer, int offset, int count);
		
		/// <summary> Writes the specified 8-bit byte.
		/// 
		/// </summary>
		/// <param name="oneByte">the byte to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readByte()">
		/// </seealso>
		//UPGRADE_TODO: More than one of the Java class members are converted to this same member in .NET. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1231'"
		void  Write(byte oneByte);
		
		/// <summary> Writes the specified boolean.
		/// 
		/// </summary>
		/// <param name="val">the boolean value to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readBoolean()">
		/// </seealso>
		void  Write(bool val);
		
		/// <summary> Writes the specified 8-bit byte.
		/// 
		/// </summary>
		/// <param name="val">the byte value to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readByte()">
		/// </seealso>
		/// <seealso cref="DataInput.readUnsignedByte()">
		/// </seealso>
		void  Write(System.SByte val);
		
		/// <summary> Writes the low order 8-bit bytes from the specified string.
		/// 
		/// </summary>
		/// <param name="str">the string containing the bytes to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readFully(byte[])">
		/// </seealso>
		/// <seealso cref="DataInput.readFully(byte[],int,int)">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataOutput.writeBytes' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		void  writeBytes(System.String str);
		
		/// <summary> Writes the specified 16-bit character. Only the two least significant
		/// bytes of the integer {@code oneByte} are written, with the higher one
		/// written first. This represents the Unicode value of the char.
		/// 
		/// </summary>
		/// <param name="val">the character to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readChar()">
		/// </seealso>
		void  Write(System.Char val);
		
		/// <summary> Writes the 16-bit characters contained in {@code str}.
		/// 
		/// </summary>
		/// <param name="str">the string that contains the characters to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readChar()">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataOutput.writeChars' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		void  writeChars(System.String str);
		
		/// <summary> Writes the specified 64-bit double. The resulting output is the eight
		/// bytes returned by {@link Double#doubleToLongBits(double)}.
		/// 
		/// </summary>
		/// <param name="val">the double to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readDouble()">
		/// </seealso>
		void  Write(double val);
		
		/// <summary> Writes the specified 32-bit float. The resulting output is the four bytes
		/// returned by {@link Float#floatToIntBits(float)}.
		/// 
		/// </summary>
		/// <param name="val">the float to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readFloat()">
		/// </seealso>
		void  Write(float val);
		
		/// <summary> Writes the specified 32-bit int. The resulting output is the four bytes,
		/// highest order first, of {@code val}.
		/// 
		/// </summary>
		/// <param name="val">the int to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readInt()">
		/// </seealso>
		//UPGRADE_TODO: More than one of the Java class members are converted to this same member in .NET. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1231'"
		void  Write(int val);
		
		/// <summary> Writes the specified 64-bit long. The resulting output is the eight
		/// bytes, highest order first, of {@code val}.
		/// 
		/// </summary>
		/// <param name="val">the long to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readLong()">
		/// </seealso>
		void  Write(long val);
		
		/// <summary> Writes the specified 16-bit short. Only the lower two bytes of {@code
		/// val} are written with the higher one written first.
		/// 
		/// </summary>
		/// <param name="val">the short to write.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readShort()">
		/// </seealso>
		/// <seealso cref="DataInput.readUnsignedShort()">
		/// </seealso>
		void  Write(short val);
		
		/// <summary> Writes the specified string encoded in {@link DataInput modified UTF-8}.
		/// 
		/// </summary>
		/// <param name="str">
		/// the string to write encoded in {@link DataInput modified UTF-8}.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing.
		/// </summary>
		/// <seealso cref="DataInput.readUTF()">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataOutput.writeUTF' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		void  writeUTF(System.String str);
	}
}