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
using System.Net.Sockets;

//UPGRADE_TODO: The type 'org.apache.harmony.luni.internal_Renamed.nls.Messages' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//using Messages = org.apache.harmony.luni.internal_Renamed.nls.Messages;
using Util = org.apache.harmony.luni.util.Util;

namespace java.io
{
	
	/// <summary> Wraps an existing {@link InputStream} and reads typed data from it.
	/// Typically, this stream has been written by a DataOutputStream. Types that can
	/// be read include byte, 16-bit short, 32-bit int, 32-bit float, 64-bit long,
	/// 64-bit double, byte strings, and strings encoded in
	/// {@link DataInput modified UTF-8}.
	/// 
	/// </summary>
	/// <seealso cref="DataOutputStream">
	/// </seealso>
	//public class DataInputStream : FilterInputStream, DataInput
    public class DataInputStream 

{
		
		internal sbyte[] buff;
		
		/// <summary> Constructs a new DataInputStream on the InputStream {@code in}. All
		/// reads are then filtered through this stream. Note that data read by this
		/// stream is not in a human readable format and was most likely created by a
		/// DataOutputStream.
		/// 
		/// </summary>
		/// <param name="in">the source InputStream the filter reads from.
		/// </param>
		/// <seealso cref="DataOutputStream">
		/// </seealso>
		/// <seealso cref="RandomAccessFile">
		/// </seealso>
		public DataInputStream(NetworkStream in_Renamed):base(in_Renamed)
		{
			buff = new sbyte[8];
		}
		
		/// <summary> Reads bytes from this stream into the byte array {@code buffer}. Returns
		/// the number of bytes that have been read.
		/// 
		/// </summary>
		/// <param name="buffer">the buffer to read bytes into.
		/// </param>
		/// <returns> the number of bytes that have been read or -1 if the end of the
		/// stream has been reached.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.write(byte[])">
		/// </seealso>
		/// <seealso cref="DataOutput.write(byte[], int, int)">
		/// </seealso>
		public  override int read(sbyte[] buffer)
		{
			return SupportClass.ReadInput((System.IO.Stream) BaseStream, buffer, 0, buffer.Length);
		}
		
		/// <summary> Reads at most {@code length} bytes from this stream and stores them in
		/// the byte array {@code buffer} starting at {@code offset}. Returns the
		/// number of bytes that have been read or -1 if no bytes have been read and
		/// the end of the stream has been reached.
		/// 
		/// </summary>
		/// <param name="buffer">the byte array in which to store the bytes read.
		/// </param>
		/// <param name="offset">
		/// the initial position in {@code buffer} to store the bytes
		/// read from this stream.
		/// </param>
		/// <param name="length">
		/// the maximum number of bytes to store in {@code buffer}.
		/// </param>
		/// <returns> the number of bytes that have been read or -1 if the end of the
		/// stream has been reached.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.write(byte[])">
		/// </seealso>
		/// <seealso cref="DataOutput.write(byte[], int, int)">
		/// </seealso>
		public override int read(sbyte[] buffer, int offset, int length)
		{
			return SupportClass.ReadInput((System.IO.Stream) BaseStream, buffer, offset, length);
		}
		
		/// <summary> Reads a boolean from this stream.
		/// 
		/// </summary>
		/// <returns> the next boolean value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream is reached before one byte
		/// has been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeBoolean(boolean)">
		/// </seealso>
		public  bool ReadBoolean()
		{
			int temp = ((System.IO.Stream) BaseStream).ReadByte();
			if (temp < 0)
			{
				throw new System.IO.EndOfStreamException();
			}
			return temp != 0;
		}
		
		/// <summary> Reads an 8-bit byte value from this stream.
		/// 
		/// </summary>
		/// <returns> the next byte value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream is reached before one byte
		/// has been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeByte(int)">
		/// </seealso>
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.io.DataInputStream.readByte' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public  sbyte ReadSByte()
		{
			int temp = ((System.IO.Stream) BaseStream).ReadByte();
			if (temp < 0)
			{
				throw new System.IO.EndOfStreamException();
			}
			return (sbyte) temp;
		}
		
		/// <summary> Reads a 16-bit character value from this stream.
		/// 
		/// </summary>
		/// <returns> the next char value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream is reached before two bytes
		/// have been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeChar(int)">
		/// </seealso>
		private int readToBuff(int count)
		{
			int offset = 0;
			
			while (offset < count)
			{
				int bytesRead = SupportClass.ReadInput((System.IO.Stream) BaseStream, buff, offset, count - offset);
				if (bytesRead == - 1)
					return bytesRead;
				offset += bytesRead;
			}
			return offset;
		}
		
		public  char ReadChar()
		{
			if (readToBuff(2) < 0)
			{
				throw new System.IO.EndOfStreamException();
			}
			return (char) (((buff[0] & 0xff) << 8) | (buff[1] & 0xff));
		}
		
		/// <summary> Reads a 64-bit double value from this stream.
		/// 
		/// </summary>
		/// <returns> the next double value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream is reached before eight
		/// bytes have been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeDouble(double)">
		/// </seealso>
		public  double ReadDouble()
		{
			//UPGRADE_ISSUE: Method 'java.lang.Double.longBitsToDouble' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoublelongBitsToDouble_long'"
			return Double.longBitsToDouble(ReadInt64());
		}
		
		/// <summary> Reads a 32-bit float value from this stream.
		/// 
		/// </summary>
		/// <returns> the next float value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream is reached before four
		/// bytes have been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeFloat(float)">
		/// </seealso>
		public  float ReadSingle()
		{
			//UPGRADE_ISSUE: Method 'java.lang.Float.intBitsToFloat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangFloatintBitsToFloat_int'"
			return Float.intBitsToFloat(ReadInt32());
		}
		
		/// <summary> Reads bytes from this stream into the byte array {@code buffer}. This
		/// method will block until {@code buffer.length} number of bytes have been
		/// read.
		/// 
		/// </summary>
		/// <param name="buffer">to read bytes into.
		/// </param>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the source stream is reached before enough
		/// bytes have been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.write(byte[])">
		/// </seealso>
		/// <seealso cref="DataOutput.write(byte[], int, int)">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInputStream.readFully' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual void  readFully(sbyte[] buffer)
		{
			SupportClass.ReadInput(BaseStream, buffer, 0, buffer.Length);
		}
		
		/// <summary> Reads bytes from this stream and stores them in the byte array {@code
		/// buffer} starting at the position {@code offset}. This method blocks until
		/// {@code length} bytes have been read. If {@code length} is zero, then this
		/// method returns without reading any bytes.
		/// 
		/// </summary>
		/// <param name="buffer">the byte array into which the data is read.
		/// </param>
		/// <param name="offset">
		/// the offset in {@code buffer} from where to store the bytes
		/// read.
		/// </param>
		/// <param name="length">the maximum number of bytes to read.
		/// </param>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the source stream is reached before enough
		/// bytes have been read.
		/// </summary>
		/// <throws>  IndexOutOfBoundsException </throws>
		/// <summary>             if {@code offset < 0} or {@code length < 0}, or if {@code
		/// offset + length} is greater than the size of {@code buffer}.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <throws>  NullPointerException </throws>
		/// <summary>             if {@code buffer} or the source stream are null.
		/// </summary>
		/// <seealso cref="java.io.DataInput.readFully(byte[], int, int)">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInputStream.readFully' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual void  readFully(sbyte[] buffer, int offset, int length)
		{
			if (length < 0)
			{
				throw new System.IndexOutOfRangeException();
			}
			if (length == 0)
			{
				return ;
			}
			if ((System.IO.Stream) BaseStream == null)
			{
				throw new NullPointerException(Messages.getString("luni.AA")); //$NON-NLS-1$
			}
			if (buffer == null)
			{
				throw new NullPointerException(Messages.getString("luni.11")); //$NON-NLS-1$
			}
			if (offset < 0 || offset > buffer.Length - length)
			{
				throw new System.IndexOutOfRangeException();
			}
			while (length > 0)
			{
				int result = SupportClass.ReadInput((System.IO.Stream) BaseStream, buffer, offset, length);
				if (result < 0)
				{
					throw new System.IO.EndOfStreamException();
				}
				offset += result;
				length -= result;
			}
		}
		
		/// <summary> Reads a 32-bit integer value from this stream.
		/// 
		/// </summary>
		/// <returns> the next int value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream is reached before four
		/// bytes have been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeInt(int)">
		/// </seealso>
		public  int ReadInt32()
		{
			if (readToBuff(4) < 0)
			{
				throw new System.IO.EndOfStreamException();
			}
			return ((buff[0] & 0xff) << 24) | ((buff[1] & 0xff) << 16) | ((buff[2] & 0xff) << 8) | (buff[3] & 0xff);
		}
		
		/// <summary> Returns a string that contains the next line of text available from the
		/// source stream. A line is represented by zero or more characters followed
		/// by {@code '\n'}, {@code '\r'}, {@code "\r\n"} or the end of the stream.
		/// The string does not include the newline sequence.
		/// 
		/// </summary>
		/// <returns> the contents of the line or {@code null} if no characters were
		/// read before the end of the source stream has been reached.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <deprecated> Use {@link BufferedReader}
		/// </deprecated>
#if ENABLECODE
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Deprecated
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInputStream.readLine' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual System.String readLine()
		{
			StringBuilder line = new StringBuilder(80); // Typical line length
			bool foundTerminator = false;
			while (true)
			{
				int nextByte = ((System.IO.Stream) BaseStream).ReadByte();
				switch (nextByte)
				{
					
					case - 1: 
						if (line.length() == 0 && !foundTerminator)
						{
							return null;
						}
						return line.toString();
					
					case (sbyte) '\r': 
						if (foundTerminator)
						{
							((SupportClass.BackInputStream) BaseStream).UnRead(nextByte);
							return line.toString();
						}
						foundTerminator = true;
						/* Have to be able to peek ahead one byte */
						if (!(((System.IO.Stream) BaseStream).GetType() == typeof(SupportClass.BackInputStream)))
						{
							BaseStream = new SupportClass.BackInputStream((System.IO.Stream) BaseStream);
						}
						break;
					
					case (sbyte) '\n': 
						return line.toString();
					
					default: 
						if (foundTerminator)
						{
							((SupportClass.BackInputStream) BaseStream).UnRead(nextByte);
							return line.toString();
						}
						line.append((char) nextByte);
						break;
					
				}
			}
		}
#endif
		/// <summary> Reads a 64-bit long value from this stream.
		/// 
		/// </summary>
		/// <returns> the next long value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream is reached before eight
		/// bytes have been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeLong(long)">
		/// </seealso>
		public  long ReadInt64()
		{
			if (readToBuff(8) < 0)
			{
				throw new System.IO.EndOfStreamException();
			}
			int i1 = ((buff[0] & 0xff) << 24) | ((buff[1] & 0xff) << 16) | ((buff[2] & 0xff) << 8) | (buff[3] & 0xff);
			int i2 = ((buff[4] & 0xff) << 24) | ((buff[5] & 0xff) << 16) | ((buff[6] & 0xff) << 8) | (buff[7] & 0xff);
			
			return ((i1 & unchecked((int) 0xffffffffL)) << 32) | (i2 & unchecked((int) 0xffffffffL));
		}
		
		/// <summary> Reads a 16-bit short value from this stream.
		/// 
		/// </summary>
		/// <returns> the next short value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream is reached before two bytes
		/// have been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeShort(int)">
		/// </seealso>
		public  short ReadInt16()
		{
			if (readToBuff(2) < 0)
			{
				throw new System.IO.EndOfStreamException();
			}
			return (short) (((buff[0] & 0xff) << 8) | (buff[1] & 0xff));
		}
		
		/// <summary> Reads an unsigned 8-bit byte value from this stream and returns it as an
		/// int.
		/// 
		/// </summary>
		/// <returns> the next unsigned byte value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream has been reached before one
		/// byte has been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeByte(int)">
		/// </seealso>
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.io.DataInputStream.readUnsignedByte' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public override byte ReadByte()
		{
			int temp = ((System.IO.Stream) BaseStream).ReadByte();
			if (temp < 0)
			{
				throw new System.IO.EndOfStreamException();
			}
            
			return temp;
		}
		
		/// <summary> Reads a 16-bit unsigned short value from this stream and returns it as an
		/// int.
		/// 
		/// </summary>
		/// <returns> the next unsigned short value from the source stream.
		/// </returns>
		/// <throws>  EOFException </throws>
		/// <summary>             if the end of the filtered stream is reached before two bytes
		/// have been read.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeShort(int)">
		/// </seealso>
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.io.DataInputStream.readUnsignedShort' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public  System.UInt16 ReadUInt16()
		{
			if (readToBuff(2) < 0)
			{
				throw new System.IO.EndOfStreamException();
			}
			return (char) (((buff[0] & 0xff) << 8) | (buff[1] & 0xff));
		}
		
		/// <summary> Reads an string encoded in {@link DataInput modified UTF-8} from this
		/// stream.
		/// 
		/// </summary>
		/// <returns> the next {@link DataInput MUTF-8} encoded string read from the
		/// source stream.
		/// </returns>
		/// <throws>  EOFException if the end of the input is reached before the read </throws>
		/// <summary>         request can be satisfied.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutput.writeUTF(java.lang.String)">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInputStream.readUTF' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual System.String readUTF()
		{
			return decodeUTF(ReadUInt16());
		}
		
		
		internal virtual System.String decodeUTF(int utfSize)
		{
			return decodeUTF(utfSize, this);
		}
		
		private static System.String decodeUTF(int utfSize, DataInput in_Renamed)
		{
			sbyte[] buf = new sbyte[utfSize];
			char[] out_Renamed = new char[utfSize];
			in_Renamed.readFully(buf, 0, utfSize);
			
			return Util.convertUTF8WithBuf(buf, out_Renamed, 0, utfSize);
		}
		
		/// <summary> Reads a string encoded in {@link DataInput modified UTF-8} from the
		/// {@code DataInput} stream {@code in}.
		/// 
		/// </summary>
		/// <param name="in">the input stream to read from.
		/// </param>
		/// <returns> the next {@link DataInput MUTF-8} encoded string from the source
		/// stream.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs while reading from this stream.
		/// </summary>
		/// <seealso cref="DataOutputStream.writeUTF(java.lang.String)">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInputStream.readUTF' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_TODO: Interface 'java.io.DataInput' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInput'"
		public static System.String readUTF(NetworkStream in_Renamed)
		{
			return decodeUTF((System.Int32) in_Renamed.ReadUInt16(), in_Renamed);
		}
		
		/// <summary> Skips {@code count} number of bytes in this stream. Subsequent {@code
		/// read()}s will not return these bytes unless {@code reset()} is used.
		/// 
		/// This method will not throw an {@link EOFException} if the end of the
		/// input is reached before {@code count} bytes where skipped.
		/// 
		/// </summary>
		/// <param name="count">the number of bytes to skip.
		/// </param>
		/// <returns> the number of bytes actually skipped.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if a problem occurs during skipping.
		/// </summary>
		/// <seealso cref="mark(int)">
		/// </seealso>
		/// <seealso cref="reset()">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.DataInputStream.skipBytes' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual int skipBytes(int count)
		{
			int skipped = 0;
			long skip;
			System.IO.Stream temp_Stream;
			System.Int64 temp_Int64;
			temp_Stream = (System.IO.Stream) BaseStream;
			temp_Int64 = temp_Stream.Position;
			temp_Int64 = temp_Stream.Seek(count - skipped, System.IO.SeekOrigin.Current) - temp_Int64;
			while (skipped < count && (skip = temp_Int64) != 0)
			{
				skipped = (int) (skipped + skip);
				temp_Stream = (System.IO.Stream) BaseStream;
				temp_Int64 = temp_Stream.Position;
				temp_Int64 = temp_Stream.Seek(count - skipped, System.IO.SeekOrigin.Current) - temp_Int64;
			}
			if (skipped < 0)
			{
				throw new System.IO.EndOfStreamException();
			}
			return skipped;
		}
	}
}