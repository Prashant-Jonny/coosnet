#define NET
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
using System;
using System.IO;

namespace Org.Coos.Messaging
{
//package org.coos.messaging;

//import java.io.ByteArrayInputStream;
//import java.io.File;
//import java.io.IOException;
//import java.io.InputStream;
//import java.io.InputStreamReader;

//import org.coos.messaging.COContainer;

//import org.coos.util.macro.MacroSubstituteReader;


/**
 * Base class with default implementation of the COContainer interface.
 *
 * @author Robert Bj&aelig;rum, Tellu AS
 *
 */
public class BaseCOContainer : ICOContainer {
    public static readonly string COOS_CONFIG_PATH = "/org/coos/config";
    public static readonly string DEFAULT_COOS_CONFIG_DIR = "./coosConfig";
    protected string configDir = DEFAULT_COOS_CONFIG_DIR;

    public string getConfigDir() {
        return configDir;
    }

    public void setConfigDir(string configDir) {
        this.configDir = configDir;
    }

    public object getObject(string name) {
        return null;
    }

    /**
     * Return resource as Stream.
     * <p>
     * The resource name will be attempted resolved as follows. A <code>baseName</code> is formed following the same
     * rule as for Class.getResourceAsStream(), removing any "/" prefix.
     * <p>
     * 1. First, try <code>File("./", baseName)</code>
     * <p>
     * 2. Then, try <code>File(configDir, baseName</code>)
     * <p>
     * 3. Then, try getResourceAsStream(resourceName)
     * <p>
     * 4. Then, try getResourceAsStream(<code>COOS_CONFIG_PATH</code> and try the resulting path name.
     * <p>
     * 5. If failed, throw IOException
     *
     * @param resourceName complete path to resource or base name of resource
     * @throws IOException if resource get fails
     * @see Class
     */
#if JAVA
    public InputStream getResource(string resourceName) throws IOException {
#endif
#if NET
    public Stream getResource(string resourceName)
    {
    
    
    Stream ins = null;
        string baseName;

        if (resourceName.Equals(string.Empty) || resourceName == null) {
            return null;
        }

        if (resourceName.StartsWith("/")) {
            baseName = resourceName.Substring(1);
        } else {
            baseName = resourceName;
            resourceName = "/" + resourceName;
        }

        try {
            FileStream file = File.Open("."+baseName,FileMode.Open);
           
            //File file = new File(".", baseName);
            //is = file.toURI().toURL().openStream();
            ins = file;
            ins = substitute(ins);

            if (ins != null) {
                return ins;
            }
        } catch (Exception ignore) {
        }

        try {
            File file = new File(configDir, baseName);
            is = file.toURI().toURL().openStream();
            is = substitute(is);

            if (is != null) {
                return is;
            }
        } catch (Exception ignore) {
        }

        try {
            is = this.getClass().getResourceAsStream(resourceName);
            is = substitute(is);

            if (is != null) {
                return is;
            }
        } catch (Exception ignore) {
        }

        is = this.getClass().getResourceAsStream(COOS_CONFIG_PATH + resourceName);
        is = substitute(is);

        return is;
    }

    @SuppressWarnings("unchecked")
    public Class loadClass(string className) throws ClassNotFoundException {
        return Class.forName(className);
    }

#if JAVA
    protected InputStream substitute(InputStream is) throws IOException {

#endif
#if NET
    protected Stream substitute(Stream ins)
#endif
    {
        if (ins == null) {
            throw new IOException("Inputstream cannot be null");
        }

        InputStreamReader isr = new InputStreamReader(is);
        MacroSubstituteReader msr = new MacroSubstituteReader(isr);
        string substituted = msr.substituteMacros();
        is = new ByteArrayInputStream(substituted.getBytes());

        return is;
    }

    @Override public void start() throws Exception {
        // TODO Auto-generated method stub

    }

    @Override public void stop() throws Exception {
        // TODO Auto-generated method stub

    }

}}
