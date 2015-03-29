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

using System.Collections.Generic;

namespace Org.Coos.Messaging {

//import java.util.Hashtable;
//import java.util.Vector;


/**
 * The Endpoint that can receive and send messages to the bus and exchanges towards the consumer/producer.
 *
 * @author Knut Eilif Husa, Tellu AS
 */
public interface IEndpoint : IProcessor, IService, IConnectable {

    // framework plugins with fixed uuids that the Default endpoint communicates
    // with

    string getEndpointState();

    void setEndpointState(string endpointState);
   
   

    void addAlias(string alias);

     void removeAlias(string alias);

    List<string> getAliases();

    /**
     * Setter method for the uuid
     *
     * @param uuid
     *            the uuid
     */
     void setEndpointUuid(string uuid);

    /**
     * Getter for the endpoint uuid
     *
     * @return the uuid
     */
     string getEndpointUuid();


    /**
     * Creates a new consumer that consumes exchanges from the endpoint. An
     * endpoint will only have one consumer associated
     *
     * @return a newly created consumer
     */
    IConsumer createConsumer();

    /**
     * Creates a new producer which is used send messages into the message bus
     *
     * @return a newly created producer
     */
    IProducer createProducer();

    /**
     * Create a new exchange for communicating with this endpoint
     */
    IExchange createExchange();

    /**
     * Create a new exchange for communicating with this endpoint with the
     * specified {@link ExchangePattern} such as whether its going to be an
     * {@link ExchangePattern#OutOnly} or {@link ExchangePattern#OutIn} exchange
     *
     * @param pattern
     *            the message exchange pattern for the exchange
     */
    IExchange createExchange(ExchangePattern pattern);

    /**
     * Returns the string representation of the endpoint URI
     *
     * @return the uri iString representation
     */
    string getEndpointUri();

    /**
     * Setter for the endpoint Uri
     *
     * @param endpointUri
     *            the endpoint Uri
     */
    void setEndpointUri(string endpointUri);

    /**
     * Processing the outwards exchange
     *
     * @param exchange
     *            the exchange to be processed
     * @return the processed exchange
     */
   IExchange processExchange(IExchange exchange);

    /**
     * Processing the exchange with asynchronous notification when the exchange
     * is ready
     *
     * @param exchange
     *            the exchange to be processed
     * @param callback
     *            the callback
     */
    void processExchange(IExchange exchange, IAsyncCallback callback);

    /**
     * This method initalizes the endpoint
     */
   void initializeEndpoint();

    /**
     * This method shuts down the endpoint
     */
  void shutDownEndpoint();

    
    /**
     * This method allows you to subscribe for notifications using a
     * {@link SubscriptionFilter}
     *
     * @param filter
     *            the subscription filter
     */
     bool subscribe(ISubscriptionFilter filter);

    /**
     * Unsubscribes a specific subscription
     *
     * @param filter
     *            the subscription to be cancelled
     */
     void unsubscribe(ISubscriptionFilter filter);

    /**
     * Unsubscribes all current subscritpions
     */
    void unsubscribe();

    /**
     * Publish notifications using {@link Notification}.
     *
     * @param notification
     *            the notification
     */
     void publish(INotification notification);

    // C# hide inherited property ; Hashtable getProperties();

     void setPlugin(Plugin plugin);

     Plugin getPlugin();

     void setTimeout(long timeout);

     void setMaxPoolSize(int maxPoolSize);

}
}
