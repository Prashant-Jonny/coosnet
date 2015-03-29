namespace Org.Coos.Messaging
{
 public class IEndpointPrimitives {

     #region Properties moved from IEndpoint, because C# does not allow properties with implemenetation in an interface

     public static string ACCESS_CONTROL_ADDRESS
     {

         get { return "coos://ac"; }
     }

     public static string LIFE_CYCLE_MANGER_ADDRESS
     { get { return "coos://localcoos.lcm/lcm@LifeCycleManager"; } }

     // Actorframe

     // type
     // of
     // endpoint
     public static string NOTIFICATION_BROKER_ADDRESS
     {
         get { return "coos://nb"; }

     }

     // properties that this plugin uses
     
     /// <summary>
     /// the time that the process exchange waits before it returns an error
     /// </summary>
     public static string PROP_EXCHANGE_TIMEOUT
     {
         get { return "coosEndpointTimeout"; }
     }


     /// <summary>
     /// Default timeout for message exchange
     /// </summary>
     public static int DEFAULT_TIMEOUT
     {
         get { return 10000; }
     }// 10 sec

     /// <summary>
     /// Allows for configuration of thread pool size
     /// </summary>
     public static string PROP_MAX_POOL_SIZE
     {
         get { return "coosEndpointMaxPoolSize"; }
     }
     /// <summary>
     /// Max. pool size for thread pool
     /// </summary>
     public static int DEFAULT_MAX_POOL_SIZE
     {
         get { return 2; }
     } //

     // login properties
     public static string PROP_LOGIN_REQUIRED
     {
         get { return "loginRequired"; }
     }
     public static string PROP_LOGIN_NAME
     {
         get { return "loginname"; }
     }
     public static string PROP_PASSWORD { get { return "password"; } }

     #region  LCM properties
     public static string PROP_LCM_REGISTRATION_ENABLED
     {
         get { return "lcmRegEnabled"; }
     }
     public static string PROP_LCM_REGISTRATION_REQUIRED
     {
         get { return "lcmRegRequired"; }
     }
     public static string PROP_LCM_POLLING_INTERVAL { get { return "lcmPollingInterval"; } }
     public static long DEFAULT_LCM_POLLING_INTERVAL { get { return 0; } } // Default off
     public static string PROP_LCM_HEARTBEAT_INTERVAL { get { return "lcmHeartbeatInterval"; } }
     #endregion

     #region LCM States
     public static string STATE_RUNNING { get { return "Running"; } }
     public static string STATE_STARTUP_FAILED { get { return "StartupFailed"; } }
     public static string STATE_STOPPING { get { return "Stopping"; } }
     public static string STATE_STARTING { get { return "Starting"; } }
     public static string STATE_INSTALLED { get { return "Installed"; } }
     public static string STATE_PAUSED { get { return "Paused"; } }
     public static string STATE_READY { get { return "Ready"; } }
     public static string STATE_UPDATING { get { return "Updating"; } }
     public static string STATE_UPGRADING { get { return "Upgrading"; } }
     public static string STATE_UNINNSTALLED { get { return "Uninstalled"; } }
     #endregion

     // Nameserver properties
     public static string PROP_REG_REQUIRED { get { return "registrationRequired"; } }

     // messages that the default endpoint handles

     // EPRMEssageConstants
     #endregion


}
}