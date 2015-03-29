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
	
	/// <summary> The base class for all input streams. An input stream is a means of reading
	/// data from a source in a byte-wise manner.
	/// <p>
	/// Some input streams also support marking a position in the input stream and
	/// returning to this position later. This abstract class does not provide a
	/// fully working implementation, so it needs to be subclassed, and at least the
	/// {@link #read()} method needs to be overridden. Overriding some of the
	/// non-abstract methods is also often advised, since it might result in higher
	/// efficiency.
	/// <p>
	/// Many specialized input streams for purposes like reading from a file already
	/// exist in this package.
	/// 
	/// </summary>
	/// <seealso cref="OutputStream">
	/// </seealso>
	public abstract class InputStream:System.Object, Closeable
	{
		
		private static sbyte[] skipBuf;
		
		/// <summary> This constructor does nothing. It is provided for signature
		/// compatibility.
		/// </summary>
		public InputStream()
		{
			/* empty */
		}
		
		/// <summary> Returns the number of bytes that are available before this stream will
		/// block. This implementation always returns 0. Subclasses should override
		/// and indicate the correct number of bytes available.
		/// 
		/// </summary>
		/// <returns> the number of bytes available before blocking.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs in this stream.
		/// </summary>
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.available' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual int available()
		{
			return 0;
		}
		
		/// <summary> Closes this stream. Concrete implementations of this class should free
		/// any resources during close. This implementation does nothing.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if an error occurs while closing this stream.
		/// </summary>
		public virtual void  close()
		{
			/* empty */
		}
		
		/// <summary> Sets a mark position in this InputStream. The parameter {@code readlimit}
		/// indicates how many bytes can be read before the mark is invalidated.
		/// Sending {@code reset()} will reposition the stream back to the marked
		/// position provided {@code readLimit} has not been surpassed.
		/// <p>
		/// This default implementation does nothing and concrete subclasses must
		/// provide their own implementation.
		/// 
		/// </summary>
		/// <param name="readlimit">the number of bytes that can be read from this stream before
		/// the mark is invalidated.
		/// </param>
		/// <seealso cref="markSupported()">
		/// </seealso>
		/// <seealso cref="reset()">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.mark' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual void  mark(int readlimit)
		{
			/* empty */
		}
		
		/// <summary> Indicates whether this stream supports the {@code mark()} and
		/// {@code reset()} methods. The default implementation returns {@code false}.
		/// 
		/// </summary>
		/// <returns> always {@code false}.
		/// </returns>
		/// <seealso cref="mark(int)">
		/// </seealso>
		/// <seealso cref="reset()">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.markSupported' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual bool markSupported()
		{
			return false;
		}
		
		/// <summary> Reads a single byte from this stream and returns it as an integer in the
		/// range from 0 to 255. Returns -1 if the end of the stream has been
		/// reached. Blocks until one byte has been read, the end of the source
		/// stream is detected or an exception is thrown.
		/// 
		/// </summary>
		/// <returns> the byte read or -1 if the end of stream has been reached.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if the stream is closed or another IOException occurs.
		/// </summary>
		abstract public override int ReadByte();
		
		/// <summary> Reads bytes from this stream and stores them in the byte array {@code b}.
		/// 
		/// </summary>
		/// <param name="b">the byte array in which to store the bytes read.
		/// </param>
		/// <returns> the number of bytes actually read or -1 if the end of the stream
		/// has been reached.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if this stream is closed or another IOException occurs.
		/// </summary>
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.read' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual int read(sbyte[] b)
		{
			return SupportClass.ReadInput(this, b, 0, b.Length);
		}
		
		/// <summary> Reads at most {@code length} bytes from this stream and stores them in
		/// the byte array {@code b} starting at {@code offset}.
		/// 
		/// </summary>
		/// <param name="b">the byte array in which to store the bytes read.
		/// </param>
		/// <param name="offset">
		/// the initial position in {@code buffer} to store the bytes read
		/// from this stream.
		/// </param>
		/// <param name="length">
		/// the maximum number of bytes to store in {@code b}.
		/// </param>
		/// <returns> the number of bytes actually read or -1 if the end of the stream
		/// has been reached.
		/// </returns>
		/// <throws>  IndexOutOfBoundsException </throws>
		/// <summary>             if {@code offset < 0} or {@code length < 0}, or if
		/// {@code offset + length} is greater than the length of
		/// {@code b}.
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if the stream is closed or another IOException occurs.
		/// </summary>
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.read' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public  virtual int read(sbyte[] b, int offset, int length)
		{
			// Force null check for b first!
			if (offset > b.Length || offset < 0)
			{
				// luni.12=Offset out of bounds \: {0}
				throw new ArrayIndexOutOfBoundsException(Messages.getString("luni.12", offset)); //$NON-NLS-1$
			}
			if (length < 0 || length > b.Length - offset)
			{
				// luni.18=Length out of bounds \: {0}
				throw new ArrayIndexOutOfBoundsException(Messages.getString("luni.18", length)); //$NON-NLS-1$
			}
			for (int i = 0; i < length; i++)
			{
				int c;
				try
				{
					if ((c = ReadByte()) == - 1)
					{
						return i == 0?- 1:i;
					}
				}
				catch (System.IO.IOException e)
				{
					if (i != 0)
					{
						return i;
					}
					throw e;
				}
				b[offset + i] = (sbyte) c;
			}
			return length;
		}
		
		/// <summary> Resets this stream to the last marked location. Throws an
		/// {@code IOException} if the number of bytes read since the mark has been
		/// set is greater than the limit provided to {@code mark}, or if no mark
		/// has been set.
		/// <p>
		/// This implementation always throws an {@code IOException} and concrete
		/// subclasses should provide the proper implementation.
		/// 
		/// </summary>
		/// <throws>  IOException </throws>
		/// <summary>             if this stream is closed or another IOException occurs.
		/// </summary>
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.reset' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'reset'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  reset()
		{
			lock (this)
			{
				throw new System.IO.IOException();
			}
		}
		
		/// <summary> Skips at most {@code n} bytes in this stream. It does nothing and returns
		/// 0 if {@code n} is negative. Less than {@code n} characters are skipped if
		/// the end of this stream is reached before the operation completes.
		/// <p>
		/// This default implementation reads {@code n} bytes into a temporary
		/// buffer. Concrete subclasses should provide their own implementation.
		/// 
		/// </summary>
		/// <param name="n">the number of bytes to skip.
		/// </param>
		/// <returns> the number of bytes actually skipped.
		/// </returns>
		/// <throws>  IOException </throws>
		/// <summary>             if this stream is closed or another IOException occurs.
		/// </summary>
		//UPGRADE_NOTE: The equivalent of method 'java.io.InputStream.skip' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public  virtual long skip(long n)
		{
			if (n <= 0)
			{
				return 0;
			}
			long skipped = 0;
			int toRead = n < 4096?(int) n:4096;
			// We are unsynchronized, so take a local copy of the skipBuf at some
			// point in time.
			sbyte[] localBuf = skipBuf;
			if (localBuf == null || localBuf.Length < toRead)
			{
				// May be lazily written back to the static. No matter if it
				// overwrites somebody else's store.
				skipBuf = localBuf = new sbyte[toRead];
			}
			while (skipped < n)
			{
				int read = SupportClass.ReadInput(this, localBuf, 0, toRead);
				if (read == - 1)
				{
					return skipped;
				}
				skipped += read;
				if (read < toRead)
				{
					return skipped;
				}
				if (n - skipped < toRead)
				{
					toRead = (int) (n - skipped);
				}
			}
			return skipped;
		}
	}
}