using java.lang;

namespace java.util
{
    public abstract class TimerTask : IRunnable
    {
        public abstract bool cancel();
        public abstract void run();
    }
}