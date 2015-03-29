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

    ///<summary>A helperclass for communicating with the bus</summary>
    ///<author>Knut Eilif Husa, Tellu AS</author>
    public class InteractionHelper
    {

        IEndpoint endpoint;

        public InteractionHelper(IEndpoint endpoint)
        {
            this.endpoint = endpoint;
        }

        ///<summary>
        ///Request an exchange from the supplied endpointUri
        ///</summary>
        ///<param name="endpointUri">the endpoint uri</param>
        ///<param name="exchange">the exchange to be processed</param>
        ///<returns>the processed exchange</returns>
        public IExchange request(string endpointUri, IExchange exchange)
        {
            exchange.setPattern(new ExchangePattern(ExchangePattern.OutIn));

            IMessage msg = exchange.getOutBoundMessage();
            msg.setReceiverEndpointUri(endpointUri);

            return endpoint.processExchange(exchange);
        }

        ///<summary>
        /// Sends a reply to the exchange
        /// <param name="exchange">the exchange to be processed</param>
        /// <returns>the processed exchange</returns>
        ///</summary>
        public IExchange reply(IExchange exchange)
        {
            IMessage inMsg = exchange.getInBoundMessage();
            IMessage outMsg = exchange.getOutBoundMessage();
            outMsg.setReceiverEndpointUri(inMsg.getSenderEndpointUri());
            outMsg.setSenderEndpointUri(inMsg.getReceiverEndpointUri());

            return endpoint.processExchange(exchange);
        }

        /// <summary>
        /// Sends an exchange to the endpoint using the supplied endpoint uri. 
        /// Note that the exchangePattern of the exchange is preserved through this method. 
        /// Hence it is possible to send exchanges of all exchange patterns using this method.
        /// </summary>
        /// <param name="endpointUri">the endpoint uri</param>
        /// <param name="exchange">the exchange to be processed</param>
        public IExchange send(string endpointUri, IExchange exchange)
        {
            IMessage msg = exchange.getOutBoundMessage();
            msg.setReceiverEndpointUri(endpointUri);
          
            return endpoint.processExchange(exchange);
        }

        ///<summary>Sends an exchange to the endpoint using the supplied endpoint uri using a send and forget
        ///exchange strategy meaning that the receiver must not acknowledge the reception of the message.
        ///</summary>
        ///<param name="endpointUri">the endpoint uri</param>
        ///<param name="exchange">the exchange to be processed</param>
        ///<returns>the processed exchange</returns>
        public IExchange sendAndForget(string endpointUri, IExchange exchange)
        {
            exchange.setPattern(new ExchangePattern(ExchangePattern.OutOnly));

            IMessage msg = exchange.getOutBoundMessage();
            msg.setReceiverEndpointUri(endpointUri);

            return endpoint.processExchange(exchange);
        }

        ///<summary>
        ///Sends an exchange to the endpoint using the supplied endpoint uri using an robust exchange scheme 
        ///meaning that the receiver must acknowledge the reception of the message.
        ///</summary>
        ///<param name="endpointUri">the endpoint uri</param>
        ///<param name="exchange">the exchange to be processed</param>
        ///<returns>the processed exchange</returns>
        public IExchange sendRobust(string endpointUri, IExchange exchange)
        {
            exchange.setPattern(new ExchangePattern(ExchangePattern.RobustOutOnly));

            IMessage msg = exchange.getOutBoundMessage();
            msg.setReceiverEndpointUri(endpointUri);

            return endpoint.processExchange(exchange);
        }

        ///<summary>
        ///Sends an exchange to the endpoint using the supplied endpoint uri and asyncCallback. 
        ///Note that the exchangePattern of the exchange is preserved through this method.
        ///Hence it is possible to send exchanges of all exchange patterns using this method.
        ///<param name="endpointUri">the endpoint uri</param>
        ///<param name="exchange">the exchange to be processed</param>
        ///<param name="callback">the callback notifying when the exchange is ready</param>
        ///</summary>
        public void send(string endpointUri, IExchange exchange, IAsyncCallback callback)
        {
            IMessage msg = exchange.getOutBoundMessage();
            msg.setReceiverEndpointUri(endpointUri);
            endpoint.processExchange(exchange, callback);
        }
    }
}