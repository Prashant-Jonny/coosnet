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
namespace Org.Coos.Messaging
{



    /**
     * The LinkManager, Both Endpoints and Routers are LinkManagers.
     *
     * @author Knut Eilif Husa, Tellu AS
     */
    public interface IConnectable
    {

        /**
         * Adds a Link to this LinkManager
         *
         * @param destinationUuid
         *            the unique identifier of the destination of this Link
         * @param link
         *            the link
         * @throws ConnectingException
         *             thrown if this LinkManager is not able to add the Link, i.e.
         *             there is a configuration mismatch
         */
        // JAVA void addLink(String destinationUuid, Link link) throws ConnectingException;

        void addLink(string destinationUuid, Link link);

        /**
         * The Default processor of this LinkManager,
         *
         * @return the default Processor
         */
        IProcessor getDefaultProcessor();

        /**
         * returns the link associated with the supplied destination Uuid
         *
         * @param destinationUuid
         *            the destination uuid
         * @return the link
         */
        Link getLink(string destinationUuid);

        /**
         * Removes the link associated with the supplied destinationUuid uuid
         *
         * @param destinationUuid
         *            the endpoint uuid
         * @return the link
         */

        void removeLink(string destinationUuid);

        /**
         * Removes Link by the unique identifier of this Link
         *
         * @param linkId
         */
        void removeLinkById(string linkId);


    }
}
