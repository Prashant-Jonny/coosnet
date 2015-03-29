namespace Org.Coos.Messaging
{
    /// <summary>
    /// Refactored out of ISubscription-filter interface, because C# interface does not allow property implementation
    /// </summary>
    public class ISubscriptionFilterPrimitives
    {
        public static string SUBSCRIBE { get { return "sub"; } }
        public static string SUBSCRIBE_OK { get { return "subOk"; } }
        public static string UNSUBSCRIBE { get { return "unsub"; } }
        public static string TYPE_FILTER { get { return "filter"; } }
        public static string UNSUBSCRIBE_ALL { get { return "unsub_all"; } }
    }
}