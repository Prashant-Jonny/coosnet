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
//UPGRADE_TODO: The type 'org.apache.harmony.luni.internal_Renamed.nls.Messages' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//using Messages = org.apache.harmony.luni.internal_Renamed.nls.Messages;
namespace java.io
{
	
	/// <summary> The base class for all output streams. An output stream is a means of writing
	/// data to a target in a byte-wise manner. Most output streams expect the
	/// {@link #flush()} method to be called before closing the stream, to ensure all
	/// data is actually written through.
	/// <p>
	/// This abstract class does not provide a fully working implementation, so it
	/// needs to be subclassed, and at least the {@link #write(int)} method needs to
	/// be overridden. Overriding some of the non-abstract methods is also often
	/// advised, since it might result in higher efficiency.
	/// <p>
	/// Many specialized output streams for purposes like writing to a file already
	/// exist in this package.
	/// 
	/// </summary>
	/// <seealso cref="InputStream">
	/// </seealso>
	public abstract class OutputStream : Closeable, Flushable
	{
		
		/// <summary> Default constructor.</summary>
		public OutputStream():base()
		{
		}
		
		/// <summary> Closes this stream. Implementations of this method should free any
		/// resources used by the stream. This implementation does nothing.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs while closing this stream.
		/// </summary>
		public virtual void  Close()
		{
			/* empty */
		}
		
		/// <summary> Flushes this stream. Implementations of this method should ensure that
		/// any buffered data is written out. This implementation does nothing.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs while flushing this stream.
		/// </summary>
		public  virtual void Flush()
		{
			/* empty */
		}
		
		/// <summary> Writes the entire contents of the byte array {@code buffer} to this
		/// stream.
		/// 
		/// </summary>
		/// <param name="buffer">the buffer to be written.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs while writing to this stream.
		/// </summary>
		//UPGRADE_NOTE: The equivalent of method 'java.io.OutputStream.write' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public void  write(sbyte[] buffer)
		{
			Write(SupportClass.ToByteArray(buffer), 0, buffer.Length);
		}
		
		/// <summary> Writes {@code count} bytes from the byte array {@code buffer} starting at
		/// position {@code offset} to this stream.
		/// 
		/// </summary>
		/// <param name="buffer">the buffer to be written.
		/// </param>
		/// <param name="offset">
		/// the start position in {@code buffer} from where to get bytes.
		/// </param>
		/// <param name="count">
		/// the number of bytes from {@code buffer} to write to this
		/// stream.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs while writing to this stream.
		/// </summary>
		/// <throws>  IndexOutOfBoundsException </throws>
		/// <summary>             if {@code offset < 0} or {@code count < 0}, or if
		/// {@code offset + count} is bigger than the length of
		/// {@code buffer}.
		/// </summary>
		public  void  Write(System.Byte[] buffer, int offset, int count)
		{
			// avoid int overflow, check null buffer
			if (offset > buffer.Length || offset < 0 || count < 0 || count > buffer.Length - offset)
			{
				//throw new IndexOutOfBoundsException(Messages.getString("luni.13")); //$NON-NLS-1$
                throw new ArgumentOutOfRangeException();
            }
			for (int i = offset; i < offset + count; i++)
			{
				WriteByte((byte) buffer[i]);
			}
		}
		
		/// <summary> Writes a single byte to this stream. Only the least significant byte of
		/// the integer {@code oneByte} is written to the stream.
		/// 
		/// </summary>
		/// <param name="oneByte">the byte to be written.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs while writing to this stream.
		/// </summary>
		public abstract  void  WriteByte(int oneByte);
		//UPGRADE_TODO: The differences in the Expected value  of parameters for method 'WriteByte'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
		public virtual void  WriteByte(byte oneByte)
		{
			WriteByte((int) oneByte);
		}
		
		/// <summary> Returns true if this writer has encountered and suppressed an error. Used
		/// by PrintStreams as an alternative to checked exceptions.
		/// </summary>
		internal virtual bool checkError()
		{
			return false;
		}
	}
}