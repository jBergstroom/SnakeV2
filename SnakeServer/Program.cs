using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server ourServer = new Server();
            Thread serverThread = new Thread(ourServer.Run);
            serverThread.Start();
            serverThread.Join();
        }
    }
}
