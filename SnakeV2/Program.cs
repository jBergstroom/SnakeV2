using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeV2
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            Client client = new Client();

            Thread clientThread = new Thread(client.Start);
            clientThread.Start();
            clientThread.Join();
            //client.Start();
            //game.Start();
        }
    }
}
