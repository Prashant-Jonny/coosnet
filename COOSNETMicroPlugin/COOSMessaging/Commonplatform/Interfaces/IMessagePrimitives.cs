namespace Org.Coos.Messaging
{

    public abstract class IMessagePrimitives
    {
        #region Header fields


        public static string TRACE_ROUTE
        {
            get { return "traceRoute"; }
        }


        public static string TRACE
        {
            get { return "trace"; }
        }

        public static string PRIORITY
        {
            get { return "priority"; }
        }

        public static string QOS_CLASS
        {
            get { return "QoS"; }
        }


        public static string MESSAGE_NAME
        {
            get { return "name"; }
        }


        public static string EXCHANGE_PREFIX
        {
            get { return "COOS"; }
        }


        public static string EXCHANGE_ID
        {
            get { return "xId"; }
        }



        public static string EXCHANGE_PATTERN
        {
            get { return "xpattern"; }
        }


        public static string DEFAULT_MESSAGE_NAME
        {
            get { return ""; }
        }

        public static string TIME_STAMP
        {
            get { return "ts"; }
        }


        public static string ROBUST_DELIVERY_TIME
        {
            get { return "gd"; }
        }

        public static string TRANSACTION_ID
        {
            get { return "tId"; }
        }

        public static string SENDER_ENDPOINT_NAME
        {
            get { return "senderEPName"; }
        }

        public static string RECEIVER_ENDPOINT_NAME
        {
            get { return "receiverEPName"; }
        }

        public static string DELIVERY_STATUS
        {
            get { return "deliveryStatus"; }
        }

        public static string DELIVERY_SUCCESS
        {
            get { return "deliverySuccess"; }
        }

        public static string DELIVERY_FAILED
        {
            get { return "deliveryFailed"; }
        }
        // message type header parameter

        public static string TYPE
        {
            get { return "type"; }
        }
        // message type values

        public static string TYPE_MSG
        {
            get { return "msg"; }
        }

        public static string TYPE_ERROR
        {
            get { return "error"; }
        }

        public static string TYPE_ANALYZE
        {
            get { return "analyze"; }
        }


        public static string TYPE_ROUTING_INFO
        {
            get { return "routingInfo"; }
        }


        public static string TYPE_ALIAS
        {
            get { return "alias"; }
        }


        public static string ERROR_REASON
        {
            get { return "errorReason"; }
        }

        public static string ERROR_CODE
        {
            get { return "errorCode"; }
        }


        public static string ERROR_NO_ROUTE
        {
            get { return "noRoute"; }
        }


        public static string ERROR_NO_ALIAS
        {
            get { return "noAlias"; }
        }

        public static string ERROR_TOO_MANY_HOPS
        {
            get { return "tooManyHops"; }
        }


        public static string ERROR_NO_RECEIVER
        {
            get { return "noReciver"; }
        }
        // message hops field

        public static string HOPS
        {
            get { return "hops"; }
        }
        // message segment field

        public static string SEGMENT
        {
            get { return "seg"; }
        }
        // Message content type header parameter

        public static string CONTENT_TYPE
        {
            get { return "contentType"; }
        }
        // Property content type

        public static string CONTENT_TYPE_PROPERTY
        {
            get { return "property"; }
        }
        // string content type

        public static string CONTENT_TYPE_STRING
        {
            get { return "string"; }
        }
        // Byte array content type

        public static string CONTENT_TYPE_BYTES
        {
            get { return "bytes"; }
        }

        // object content type

        public static string CONTENT_TYPE_OBJECT
        {
            get { return "object"; }
        }
        // body serialization method header parameter

        public static string SERIALIZATION_METHOD
        {
            get { return "ser"; }
        }
        // serialization method ActorFrame, not dependant on java SE but own
        // serialization must be implemented

        public static string SERIALIZATION_METHOD_AF
        {
            get { return "af"; }
        }
        // serialization method Java, dependant on Java SE

        public static string SERIALIZATION_METHOD_JAVA
        {
            get { return "java"; }
        }
        // serialization method default

        public static string SERIALIZATION_METHOD_DEFAULT
        {
            get { return "def"; }
        }
        //Robust delivery ack uri

        public static string ROBUST_DELIVERY_ACK_URI
        {
            get { return "rdAckUri"; }
        }

        #endregion
    }
}