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
	
	/// <summary> Thrown when a program encounters the end of a file or stream during an input
	/// operation.
	/// </summary>
	[Serializable]
	public class EOFException:System.IO.IOException
	{
		
		private const long serialVersionUID = 6433858223774886977L;
		
		/// <summary> Constructs a new {@code EOFException} with its stack trace filled in.</summary>
		public EOFException():base()
		{
		}
		
		/// <summary> Constructs a new {@code EOFException} with its stack trace and detail
		/// message filled in.
		/// 
		/// </summary>
		/// <param name="detailMessage">the detail message for this exception.
		/// </param>
		public EOFException(System.String detailMessage):base(detailMessage)
		{
		}
	}
}