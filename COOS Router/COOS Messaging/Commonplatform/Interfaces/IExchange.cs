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
namespace Org.Coos.Messaging
{

    ///<summary>
    ///The base message exchange interface providing access to the request, response and fault <seealso cref="IMessage"/> instances.
    /// </summary>
    /// <author>Knut Eilif Husa, Tellu AS</author>
public interface IExchange {

    /**
     * Set the {@link ExchangePattern} (MEP) of this exchange.
     *
     * @param pattern
     *            the message exchange pattern of this exchange
     */
    void setPattern(ExchangePattern pattern);

    /**
     * Returns the {@link ExchangePattern} (MEP) of this exchange.
     *
     * @return the message exchange pattern of this exchange
     */
    ExchangePattern getPattern();

    /**
     * Returns the outbound request message
     *
     * @return the message
     */
    IMessage getOutBoundMessage();

    /**
     * Returns the inBound response message
     *
     * @return the response
     */
    IMessage getInBoundMessage();

    /**
     * Sets the outbound message instance
     *
     * @param in
     *            the inbound message
     */
    void setOutBoundMessage(IMessage outboundMsg);

    /**
     * Sets the inbound message
     *
     * @param out
     *            the outbound message
     */
    void setInBoundMessage(IMessage inboundMsg);

    /**
     * Returns the exchange id
     *
     * @return the unique id of the exchange
     */
    string getExchangeId();

    /**
     * Set the exchange id
     *
     * @param exchangeId
     *            the exchange id
     */
    void setExchangeId(string exchangeId);

    /**
     * Gets the fault message
     *
     * @return the fault message
     */
    IMessage getFaultMessage();

    /**
     * Sets the fault message
     *
     * @param fault
     *            the fault message
     */
    void setFaultMessage(IMessage fault);

    /**
     * Gets the exception
     *
     * @return the Exception
     */
     // JAVA Throwable getException();
    Exception getException();
    /**
     * Sets the exception
     *
     * @param exception
     *            the exception
     */
    //JAVA void setException(Throwable exception);

    void setException(Exception exception);

    void setProcessed(bool isProcessed);

    bool isProcessed();

}}
