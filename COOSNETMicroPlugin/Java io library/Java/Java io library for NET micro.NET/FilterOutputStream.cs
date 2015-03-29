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
//UPGRADE_TODO: The type 'org.apache.harmony.luni.util.SneakyThrow' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//using SneakyThrow = org.apache.harmony.luni.util.SneakyThrow;
namespace java.io
{
	
	/// <summary> Wraps an existing {@link OutputStream} and performs some transformation on
	/// the output data while it is being written. Transformations can be anything
	/// from a simple byte-wise filtering output data to an on-the-fly compression or
	/// decompression of the underlying stream. Output streams that wrap another
	/// output stream and provide some additional functionality on top of it usually
	/// inherit from this class.
	/// 
	/// </summary>
	/// <seealso cref="FilterOutputStream">
	/// </seealso>
	public class FilterOutputStream: OutputStream
	{
		
		/// <summary> The target output stream for this filter stream.</summary>
		protected internal System.IO.Stream out_Renamed;
		
		/// <summary> Constructs a new {@code FilterOutputStream} with {@code out} as its
		/// target stream.
		/// 
		/// </summary>
		/// <param name="out">the target stream that this stream writes to.
		/// </param>
		public FilterOutputStream(System.IO.Stream out_Renamed)
		{
			this.BaseStream = out_Renamed;
		}
		
		/// <summary> Closes this stream. This implementation closes the target stream.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs attempting to close this stream.
		/// </summary>
	
		public override void  Close()
		{
			//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			System.Exception thrown = null;
			try
			{
				Flush();
			}
			//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				thrown = e;
			}
			
			try
			{
				BaseStream.Close();
			}
			//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				if (thrown == null)
				{
					thrown = e;
				}
			}
			
			if (thrown != null)
			{
				SneakyThrow.sneakyThrow(thrown);
			}
		}
		
		/// <summary> Ensures that all pending data is sent out to the target stream. This
		/// implementation flushes the target stream.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs attempting to flush this stream.
		/// </summary>
		public override void  Flush()
		{
			BaseStream.Flush();
		}
		
		/// <summary> Writes the entire contents of the byte array {@code buffer} to this
		/// stream. This implementation writes the {@code buffer} to the target
		/// stream.
		/// 
		/// </summary>
		/// <param name="buffer">the buffer to be written.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing to this stream.
		/// </summary>
		public  void  Write(System.Byte[] buffer)
		{
			Write(SupportClass.ToByteArray(buffer), 0, buffer.Length);
		}
		
		/// <summary> Writes {@code count} bytes from the byte array {@code buffer} starting at
		/// {@code offset} to the target stream.
		/// 
		/// </summary>
		/// <param name="buffer">the buffer to write.
		/// </param>
		/// <param name="offset">
		/// the index of the first byte in {@code buffer} to write.
		/// </param>
		/// <param name="length">
		/// the number of bytes in {@code buffer} to write.
		/// </param>
		/// <throws>  IndexOutOfBoundsException </throws>
		/// <summary>             if {@code offset < 0} or {@code count < 0}, or if
		/// {@code offset + count} is bigger than the length of
		/// {@code buffer}.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing to this stream.
		/// </summary>
		public  override void  Write(System.Byte[] buffer, int offset, int length)
		{
			// Force null buffer check first!
			if (offset > buffer.Length || offset < 0)
			{
				// luni.12=Offset out of bounds \: {0}
				throw new ArrayIndexOutOfBoundsException(Messages.getString("luni.12", offset)); //$NON-NLS-1$
			}
			if (length < 0 || length > buffer.Length - offset)
			{
				// luni.18=Length out of bounds \: {0}
				throw new ArrayIndexOutOfBoundsException(Messages.getString("luni.18", length)); //$NON-NLS-1$
			}
			for (int i = 0; i < length; i++)
			{
				// Call write() instead of out.write() since subclasses could
				// override the write() method.
				write(buffer[offset + i]);
			}
		}
		
		/// <summary> Writes one byte to the target stream. Only the low order byte of the
		/// integer {@code oneByte} is written.
		/// 
		/// </summary>
		/// <param name="oneByte">the byte to be written.
		/// </param>
		/// <throws>  IOException </throws>
		/// <summary>             if an I/O error occurs while writing to this stream.
		/// </summary>
		public override void  write(int oneByte)
		{
			BaseStream.WriteByte((System.Byte) oneByte);
		}
	}
}