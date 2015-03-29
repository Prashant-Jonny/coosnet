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

using System.IO;

namespace Org.Coos.Util.Serialize
{
    //package org.coos.util.serialize;

    //import java.io.ByteArrayInputStream;
    //import java.io.IOException;

    /**
     * User: Arild Herstad, Telenor FoU
     * 
     * 
     */
    public interface AFSerializer
    {
        /**
         * This function must implement the serialization of the object.
         * 
         * @return a byte array with the objects data
         * @throws IOException
         */
        byte[] serialize(); /*throws IOException; */

        /**
         * Use this function for resurrection of the object
         * 
         * @param data
         *            The serialized data containing the object data
         * @throws IOException
         */
        MemoryStream deSerialize(byte[] data, AFClassLoader cl); /* throws IOException */
    }
}
