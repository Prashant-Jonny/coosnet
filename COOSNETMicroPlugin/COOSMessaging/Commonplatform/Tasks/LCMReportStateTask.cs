using java.lang;
using Org.Coos.Module;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging.Impl
{
    public class LCMReportStateTask : IRunnable
    {
        private LCMEndpointReport lcm;
        private ILog log;

        public LCMReportStateTask(LCMEndpointReport lcm, ILog log)
        {
            this.lcm = lcm;
            this.log = log;
        }

        public void run()
        {
               try {
                   lcm.reportState();
               }
               catch (EndpointException e)
               {
                   log.warn(
                       "Exception caught by processMessage(). Message " +
                       EdgeLCMMessageFactory.EDGE_REQUEST_STATE, e);
               }
        }

    }

}