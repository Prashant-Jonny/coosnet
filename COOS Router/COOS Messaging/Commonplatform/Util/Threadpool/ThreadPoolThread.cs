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
using java.lang;
using System.Threading;
using System;

#if MICROFRAMEWORK
using Microsoft.SPOT;
#endif

namespace Org.Coos.Messaging.Util
{
    //package org.coos.messaging.util;

    /**
     * Thread pool thread to use in environments
     * that do not support java.util.concurrent.
     * 
     * @author Knut Eilif Husa, Tellu AS
     */
    public class ThreadPoolThread : IRunnable
    {
        private Thread thread;
        private ThreadPool tp;
        private bool _active = false, running = true;
      
        

        public ThreadPoolThread(ThreadPool tp)
        {
            this.tp = tp;
           
         
            // JAVA thread = new Thread(this, tp.name + "-" + this);
            // No Name property for thread in .NET micro framework

            thread = new Thread(() => run());
            
            
            thread.Start();
        }

        public void run()
        {
           

            while (running)
            {
               
                    try
                    {

                        // Get the next task from the parent ThreadPool
                        IRunnable r = tp.getRunnable();
                        // Run the task!
                        _active = true;
#if MICROFRAMEWORK
                        Debug.Print("Running task: " + r.GetType().Name.ToString());
#endif
                        r.run();
                        _active = false;

                    }
                    //catch (InterruptedException e)
                    //{
                    //    return;
                    //}
                    catch (ThreadAbortException e)
                    {
                        //Debug.Print("Thread pool thread aborting");
                        // return;
                        running = false;
                    }

                    catch (Exception e)
                    {
                        LogFactory.getLog(typeof(ThreadPoolThread).FullName).error("Exception in ThreadPoolThread", e);
                    }
                }
            
        }

        public bool active()
        {
            return _active;
        }

        public void stop()
        {
            // Running may be changed from another thread that calls stop, f.ex. during endpoint shutdown
                running = false;
            
            //thread.interrupt();
           // thread.Abort();
            
        }
    }
}
