using java.lang;
using Org.Coos.Module;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging.Impl
{
    public class TestTask : IRunnable
    {
        
        
        public void run()
        {
            for (int i = 0; i < 100000; i++) ;

        }

    }

}