using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace VAPI
{
    public class Logger
    {
        public static StreamWriter LogFile;
        public static void Init()
        {
            LogFile = new StreamWriter("log.txt");
        }

        public static void Write(string Text)
        {
            LogFile.WriteLine(Text);
            #if DEBUG
            Console.WriteLine(Text);
            Debug.WriteLine(Text);
            #endif

        }
    }
}
