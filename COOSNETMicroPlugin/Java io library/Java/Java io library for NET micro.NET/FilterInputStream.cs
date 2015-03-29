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
	
	/// <summary> Wraps an existing {@link InputStream} and performs some transformation on
	/// the input data while it is being read. Transformations can be anything from a
	/// simple byte-wise filtering input data to an on-the-fly compression or
	/// decompression of the underlying stream. Input streams that wrap another input
	/// stream and provide some additional functionality on top of it usually inherit
	/// from this class.
	/// 
	/// </summary>
	/// <seealso cref="FilterOutputStream">
	/// </seealso>
	public class FilterInputStream:InputStream
	{
		
		/// <summary> The source input stream that is filtered.</summary>
		protected internal volatile System.IO.Stream in_Renamed;
		
		/// <summary> Constructs a new {@code FilterInputStream} with the specified input
		/// stream as source.
		/// 
		/// </summary>
		/// <param name="in">the non-null InputStream to filter reads on.
		/// </param>
		//UPGRADE_ISSUE: Constructor 'java.io.InputStream.InputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioInputStreamInputStream'"
		protected internal FilterInputStream(System.IO.Stream in_Renamed):base()
		{
			this.BaseStream = in_Renamed;
		}
		
		/// <summary> Returns the number of bytes that are available before this stream will
		/// block.
		/// 
		/// </summary>
		/// <returns> the number of bytes available before blocking.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs in this stream.
		/// </summary>
		public override int available()
		{
			long available;
			available = ((System.IO.Stream) BaseStream).Length - ((System.IO.Stream) BaseStream).Position;
			return (int) available;
		}
		
		/// <summary> Closes this stream. This implementation closes the filtered stream.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs while closing this stream.
		/// </summary>
		public override void  close()
		{
			((System.IO.Stream) BaseStream).Close();
		}
		
		/// <summary> Sets a mark position in this stream. The parameter {@code readlimit}
		/// indicates how many bytes can be read before the mark is invalidated.
		/// Sending {@code reset()} will reposition this stream back to the marked
		/// position, provided that {@code readlimit} has not been surpassed.
		/// <p>
		/// This implementation sets a mark in the filtered stream.
		/// 
		/// </summary>
		/// <param name="readlimit">the number of bytes that can be read from this stream before
		/// the mark is invalidated.
		/// </param>
		/// <seealso cref="markSupported()">
		/// </seealso>
		/// <seealso cref="reset()">
		/// </seealso>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'mark'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public override void  mark(int readlimit)
		{
			lock (this)
			{
				//UPGRADE_ISSUE: Method 'java.io.InputStream.mark' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioInputStreammark_int'"
				in_Renamed.mark(readlimit);
			}
		}
		
		/// <summary> Indicates whether this stream supports {@code mark()} and {@code reset()}.
		/// This implementation returns whether or not the filtered stream supports
		/// marking.
		/// 
		/// </summary>
		/// <returns> {@code true} if {@code mark()} and {@code reset()} are supported,
		/// {@code false} otherwise.
		/// </returns>
		/// <seealso cref="mark(int)">
		/// </seealso>
		/// <seealso cref="reset()">
		/// </seealso>
		/// <seealso cref="skip(long)">
		/// </seealso>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public override bool markSupported()
		{
			//UPGRADE_ISSUE: Method 'java.io.InputStream.markSupported' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioInputStreammarkSupported'"
			return in_Renamed.markSupported();
		}
		
		/// <summary> Reads a single byte from the filtered stream and returns it as an integer
		/// in the range from 0 to 255. Returns -1 if the end of this stream has been
		/// reached.
		/// 
		/// </summary>
		/// <returns> the byte read or -1 if the end of the filtered stream has been
		/// reached.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if the stream is closed or another IOException occurs.
		/// </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public   int Read()
		{
			return ((System.IO.Stream) BaseStream).ReadByte();
		}
		
		/// <summary> Reads bytes from this stream and stores them in the byte array
		/// {@code buffer}. Returns the number of bytes actually read or -1 if no
		/// bytes were read and the end of this stream was encountered. This
		/// implementation reads bytes from the filtered stream.
		/// 
		/// </summary>
		/// <param name="buffer">the byte array in which to store the read bytes.
		/// </param>
		/// <returns> the number of bytes actually read or -1 if the end of the
		/// filtered stream has been reached while reading.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if this stream is closed or another IOException occurs.
		/// </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public override int read(sbyte[] buffer)
		{
			return read(buffer, 0, buffer.Length);
		}
		
		/// <summary> Reads at most {@code count} bytes from this stream and stores them in the
		/// byte array {@code buffer} starting at {@code offset}. Returns the number
		/// of bytes actually read or -1 if no bytes have been read and the end of
		/// this stream has been reached. This implementation reads bytes from the
		/// filtered stream.
		/// 
		/// </summary>
		/// <param name="buffer">the byte array in which to store the bytes read.
		/// </param>
		/// <param name="offset">
		/// the initial position in {@code buffer} to store the bytes
		/// read from this stream.
		/// </param>
		/// <param name="count">
		/// the maximum number of bytes to store in {@code buffer}.
		/// </param>
		/// <returns> the number of bytes actually read or -1 if the end of the
		/// filtered stream has been reached while reading.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if this stream is closed or another I/O error occurs.
		/// </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public override int read(sbyte[] buffer, int offset, int count)
		{
			return SupportClass.ReadInput((System.IO.Stream) BaseStream, buffer, offset, count);
		}
		
		/// <summary> Resets this stream to the last marked location. This implementation
		/// resets the target stream.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if this stream is already closed, no mark has been set or the
		/// mark is no longer valid because more than {@code readlimit}
		/// bytes have been read since setting the mark.
		/// </summary>
		/// <seealso cref="mark(int)">
		/// </seealso>
		/// <seealso cref="markSupported()">
		/// </seealso>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'reset'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public override void  reset()
		{
			lock (this)
			{
				//UPGRADE_ISSUE: Method 'java.io.InputStream.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioInputStreamreset'"
				in_Renamed.reset();
			}
		}
		
		/// <summary> Skips {@code count} number of bytes in this stream. Subsequent
		/// {@code read()}'s will not return these bytes unless {@code reset()} is
		/// used. This implementation skips {@code count} number of bytes in the
		/// filtered stream.
		/// 
		/// </summary>
		/// <param name="count">the number of bytes to skip.
		/// </param>
		/// <returns> the number of bytes actually skipped.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if this stream is closed or another IOException occurs.
		/// </summary>
		/// <seealso cref="mark(int)">
		/// </seealso>
		/// <seealso cref="reset()">
		/// </seealso>
		public override long skip(long count)
		{
			System.IO.Stream temp_Stream;
			System.Int64 temp_Int64;
			temp_Stream = (System.IO.Stream) BaseStream;
			temp_Int64 = temp_Stream.Position;
			temp_Int64 = temp_Stream.Seek(count, System.IO.SeekOrigin.Current) - temp_Int64;
			return temp_Int64;
		}
	}
}