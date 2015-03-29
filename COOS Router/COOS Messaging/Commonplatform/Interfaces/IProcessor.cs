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


namespace Org.Coos.Messaging {



/**
 * A Processor is used to implement a message translator and processing a message
 * 
 * @author Knut Eilif Husa, Tellu AS
 */
public interface IProcessor : IConfigurable {

	/**
	 * Returns the name of the processor
	 * 
	 * @return the name
	 */
	string getName();

	/**
	 * Sets the name of the Processor
	 * 
	 * @param name
	 */
	void setName(string name);

	/**
	 * Processes the message
	 * 
	 * @param msg
	 *            the message to be processed
	 */
    //void processMessage(IMessage msg) throws ProcessorException;
    void processMessage(IMessage msg);

	/**
	 * Sets flag that indicates that this processor is shared in this COOS
	 * 
	 * @param shared
	 *            true if shared
	 */
    
	void setShared(bool shared);

	/**
	 * Setter for the COOS Classloader
	 * 
	 * @param classLoader
	 *            the classloader
	 */
	 void setCoContainer(ICOContainer classLoader);

	/**
	 * Indicates that this processor is shared in this COOS
	 * 
	 * @return true if shared
	 */
	bool isShared();

	/**
	 * This method returns a copy of the processor. If shared is true the same instance will be returned.
	 * Else a new instance will be created.
	 * 
	 * @return the processor
	 */
	IProcessor copy();

}}
