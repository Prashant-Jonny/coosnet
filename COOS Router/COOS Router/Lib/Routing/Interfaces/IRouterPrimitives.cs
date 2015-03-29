namespace Org.Coos.Messaging.Routing
{
    public abstract class IRouterPrimitives {
// The name prefix indicating a UUID
    public static string UUID_PREFIX
    {
        get
        {
            return "UUID";
        }
    }

	// The name prefix indicating a Router UUID
    public static string ROUTER_UUID_PREFIX
    {
        get { return "UUID-R-"; }
    }

	// The pr coos local segment
    public static string LOCAL_SEGMENT
    {
        get { return "localcoos"; }
    }

	// The global namespace
    public static string DICO_SEGMENT
    {
        get { return "dico"; }
    }
    }
}