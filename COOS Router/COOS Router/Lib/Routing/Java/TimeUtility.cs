using System;

namespace javaNETintegration
{

    //From: http://bytes.com/topic/c-sharp/answers/557734-java-system-currenttimemillis-equivalent
    public class Time
    {

        static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        public static long currentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Epoch).TotalMilliseconds;
        }
    }
}