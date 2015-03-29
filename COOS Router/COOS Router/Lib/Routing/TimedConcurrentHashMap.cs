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
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System;

using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;

using java.util;

namespace Org.Coos.Messaging.Routing
{
//package org.coos.messaging.routing;

//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;

//import java.util.Timer;
//import java.util.TimerTask;
//import java.util.concurrent.ConcurrentHashMap;


public class TimedConcurrentHashMap<K,V> : ConcurrentDictionary<K,V> {

    /** Use serialVersionUID for interoperability. */
    private readonly static long serialVersionUID = -3568451421580405608L;
  
    // JAVA private Timer timer = new Timer("TimedConcurrentHashMap", true);
   // private Timer timer;

    private static  ConcurrentDictionary<K, CallbackTask> timers =  new ConcurrentDictionary<K, CallbackTask>();


    private static readonly ILog logger = LogFactory.getLog(typeof(TimedConcurrentHashMap<K,V>).Name, true);

    public TimedConcurrentHashMap() : base() {
      
    }

  
  

    ///<summary>This method cancels any current timer for the key</summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public  V put(K key, V value) {
       
        // JAVA CallbackTask tt = timers.remove(key);
        CallbackTask tt = cancelCallbackTask(key);
        V valueRetrived = base.AddOrUpdate(key,value,(K,V) => value);
           
        //return base.TryAdd(key, value);
        return valueRetrived;
    }


    
    ///<summary>This method cancels any current timers for the key</summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  bool remove(K key) {
        logger.trace("Removing :" + key);
        V value;

        CallbackTask tt = cancelCallbackTask(key);
            
        return base.TryRemove(key, out value);
    }

   

    ///<summary>This method puts a key, value pair into a hashtable and schedules a callback to be called after a timeout</summary>
    ///<remarks>Java data structure was concurrentHashMap, put-method overwrites previous value if key exsists and returns previous value or null if new key 
    ///THIS METHOD RETURNS the EXSISTING VALUE</remarks>
    ///
    ///<param name="callback">Callback to be called after a timeout</param>
    ///<param name="key">Key</param>
    ///<param name="timeout">Timeout</param>
    ///<param name="value">Value</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public  V put( K key, V value, long timeout,  IHashMapCallback callback) {
        logger.trace("putting timeout, Key, value: " + timeout + "," + key + ", value:" + value);

        // Remove previously defined callbacktask
        CallbackTask tt = cancelCallbackTask(key);

        try
        {
            tt = new CallbackTask(key, callback,this,timeout);
            

            // JAVA timer.schedule(tt, timeout);

            CallbackTask newCallbackTask = timers.AddOrUpdate(key, tt, (K, V) => tt);
#if JAVA
        } catch (IllegalStateException e) {
            logger.error("IllegalStateException ignored. key:" + key + ", value: " + value, e);
        }
#endif
        }
        catch (Exception e)
        {
            logger.error("Exception key:" + key + ", value: " + value, e);
        }

       return base.AddOrUpdate(key,value,(K,V) => value);
    }

    /// <summary>
    /// Refactored from Java code, was the same in 3 places
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private CallbackTask cancelCallbackTask(K key)
    {
        CallbackTask tt;
       
        bool result = timers.TryRemove(key, out tt);

        if (tt != null)
        {
            tt.cancel(); // .NET cannot access threadpool thread?
        }

        return tt;
    }

[MethodImpl(MethodImplOptions.Synchronized)]
    public  void stop() {
       // timer.Dispose();
        timers.Clear();

    }

    public void start() {
       // JAVA timer = new Timer("TimedConcurrentHashMap", true);
    }

    class CallbackTask : TimerTask {
        IHashMapCallback callback;
        K key;
        CallbackTask callbackTask;
        TimedConcurrentHashMap<K,V> timedConcurrentHashmap;
        public Timer timer;
        private long timeout;

        // Accessing outer class -> c# outer instance this, must be called as parameter in constructor of nested class
        // C# language specification - 10.3.8.4 this access
        // Accessed : 19 november 2010
        public CallbackTask(K key, IHashMapCallback callback, TimedConcurrentHashMap<K,V> timedConcurrentHashMap, long timeout) : base() {
           
            this.callback = callback;
            this.key = key;
            this.timedConcurrentHashmap = timedConcurrentHashMap;
            this.timeout = timeout;
            TimerCallback tcb = new TimerCallback(NETrun);

            timer = new Timer(tcb, null, timeout, 0);
           
        }

        public override bool cancel()
        {
            timer.Dispose();
            return true; // Optimistic dispose, assume success
           
        }
      
            public override void run() {

            if (callback != null && callback.remove(key, timedConcurrentHashmap)) {
                bool result = timers.TryRemove(key,out callbackTask);

                if (this.timedConcurrentHashmap.remove(key)) 
                    logger.debug("Removing Key:" + key);
                
            }

        }

        /// <summary>
        /// Callback delegate for .NET timer
        /// </summary>
        /// <param name="state"></param>
        public void NETrun(object state)
        {
            run();
        }

    }


}
}