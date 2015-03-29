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

using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;

using Microsoft.SPOT;

namespace Org.Coos.Messaging.Util
{
    //package org.coos.messaging.util;

    //import java.util.Vector;


    /**
     * @author Knut Eilif Husa, Tellu AS An object queue
     */

    public class Queue
    {
        private ArrayList vec = new ArrayList();

        AutoResetEvent newRunnableSignal = new AutoResetEvent(false);

       

       
        // Try using lock on vec-write operation instead of synchronising queue instance

        //[MethodImpl(MethodImplOptions.Synchronized)]
        // .NET Micro framework; adding synchronized here results in hang
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public void put(object o)
        {
           
               // Debug.Print("New task to be run " + o.GetType().Name);
                // Add the element
            lock (vec.SyncRoot)
            {
                vec.Add(o);
            }
                // There might be threads waiting for the new object --
                // give them a chance to get it
                //notifyAll();
            //Info : http://msdn.microsoft.com/en-us/library/system.threading.autoresetevent.aspx
            // .Set - Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
                newRunnableSignal.Set();
            
        }

       // [MethodImpl(MethodImplOptions.Synchronized)]
        public object get() /*throws InterruptedException */ {

          
                while (true)
                {

                    if (vec.Count > 0)
                    {
                        // Debug.Print("Queue size for task to be runned: " + vec.Count);
                        // There's an available object!
                        object o = vec[0];

                        // Remove it from our internal list, so someone else
                        // doesn't get it.
                        lock (vec.SyncRoot)
                        {
                            vec.Remove(o);
                        }
                        // Return the object
                      

                        return o;
                    }
                    else
                    {

                        // There aren't any objects available. Do a wait(),
                        // and when we wake up, check again to see if there
                        // are any.
                        // JAVA wait();
                       
                            newRunnableSignal.WaitOne();
                       



                    }
                }
            }
        }
    }

