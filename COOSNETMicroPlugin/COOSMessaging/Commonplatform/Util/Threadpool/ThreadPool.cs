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
using java.lang;

namespace Org.Coos.Messaging.Util
{
//package org.coos.messaging.util;

//import java.util.ArrayList;


/**
 * @author Knut Eilif Husa, Tellu AS Threadpool to use in environments that do
 *         not support java.util.concurrent
 */
public class ThreadPool : IExecutorService {
    private Coos.Messaging.Util.Queue queue = new Coos.Messaging.Util.Queue();
    private int numThreads;
    public string name;
    ArrayList threadPoolThreads = new ArrayList();

    public ThreadPool(string name, int numThreads) {
        this.numThreads = numThreads;
        this.name = name;

        // Create N threads
        for (int i = 0; i < numThreads; ++i)
            threadPoolThreads.Add(new ThreadPoolThread(this));
    }

    public void execute(IRunnable runnable) {
        queue.put(runnable);
    }


    public IRunnable getRunnable() /*throws InterruptedException */ {
        return (IRunnable) queue.get();
    }

    public void setMaxPoolSize(int maxPoolSize) {
        numThreads = maxPoolSize;

        // Add and start threads up to max pool size
        while (threadPoolThreads.Count < maxPoolSize) {
            threadPoolThreads.Add(new ThreadPoolThread(this));
        }

        // Remove and stop threads that are exceeding new max pool size
        while (threadPoolThreads.Count > maxPoolSize) {
            ThreadPoolThread tpt = (ThreadPoolThread)threadPoolThreads[0];
            threadPoolThreads.Remove(tpt);
            tpt.stop();
        }
    }

    
    public void stop() {

        foreach(ThreadPoolThread tpt in threadPoolThreads)
            tpt.stop();

    }
}}
