using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Management;
using System.Diagnostics;

using Org.Coos.Messaging;

namespace COOS_Router
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Attemting to start COOS router and plugins in .NET container");

            showSystemInfo();

            NETContainer netContainer = new NETContainer();
            netContainer.start();


        }

        private static void showSystemInfo()
        {

            Debug.Write("Machine name : " + System.Environment.MachineName + " OS version: " + System.Environment.OSVersion);

            if (System.Environment.Is64BitOperatingSystem)
                Debug.WriteLine(" (64 bit operating system)");
            else
                Debug.WriteLine("\n");

            // From:  http://www.thereforesystems.com/get-processor-information-in-net-using-c/
            ManagementObjectSearcher mgmtObjects = new ManagementObjectSearcher("Select * from Win32_ComputerSystem");

            foreach (var item in mgmtObjects.Get())
            {
                //Console.WriteLine("Number Of Processors - " +  item["NumberOfProcessors"]);
                Debug.WriteLine(item["NumberOfProcessors"] +" processor(s) with "+item["NumberOfLogicalProcessors"] + " processor cores");
            }


            if (System.Environment.Is64BitProcess)
                Debug.WriteLine("Container is a 64 bit process");
            else
                Debug.WriteLine("Container is a 32 bit process");

            Debug.WriteLine("Current directory: " + System.Environment.CurrentDirectory);

        }
    }
}
