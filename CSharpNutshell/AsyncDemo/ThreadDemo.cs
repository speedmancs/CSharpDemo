using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    class ThreadDemo
    {
        static public void Run()
        {
            Thread t = new Thread(WriteY);          // Kick off a new thread
            t.Start();                               // running WriteY()
            // t.Join();
            // Simultaneously, do something on the main thread.
            for (int i = 0; i < 1000; i++) Console.Write("x");
            Console.WriteLine("Main thread end");
        }

        static void WriteY()
        {
            for (int i = 0; i < 1000; i++) Console.Write("y");
            Thread.Sleep(5000);
            Console.WriteLine("Work thread end");
        }
    }
}
