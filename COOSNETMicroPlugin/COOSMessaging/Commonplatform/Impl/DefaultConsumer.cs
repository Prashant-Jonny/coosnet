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

namespace Org.Coos.Messaging.Impl
{

    //package org.coos.messaging.impl;

    //import java.util.Vector;

    //import org.coos.messaging.*;


    /**
     * @author Knut Eilif Husa, Tellu AS
     */
    public abstract class DefaultConsumer : IConsumer
    {

        private IEndpoint endpoint;

        public DefaultConsumer(IEndpoint endpoint)
        {
            this.endpoint = endpoint;
        }

        public IEndpoint getEndpoint()
        {
            return endpoint;
        }

        public abstract void process(IExchange exchange);
        

    }
}
