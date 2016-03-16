using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SnakeServer
{
    public class ClientHandler
    {
        public TcpClient tcpclient;
        private Server myServer;
        public string name;
        public ClientHandler(TcpClient c, Server server)
        {
            tcpclient = c;
            this.myServer = server;
        }

        public void Run()
        {
            try
            {
                string message = "";
                bool nameOccupied = false;
                do
                {
                    NetworkStream n = tcpclient.GetStream();
                    message = new BinaryReader(n).ReadString();
                    nameOccupied = myServer.clientList.Select(x => x.name).Contains(message);
                    if (nameOccupied)
                    {
                        myServer.SingleBroadcast(this, "1;Name is occupadoo, gorom gorratt");
                    }
                    else
                    {
                        myServer.SingleBroadcast(this, $"0;Name is ok");
                        myServer.Broadcast(this, $"{message} has join the gaming worlds");
                    }
                } while (nameOccupied);

                while (!myServer.GejmÅn)
                {
                    myServer.Broadcast(this, "Waiting for other player");
                }
                while (myServer.GejmÅn && message != "ded")
                {
                    NetworkStream n = tcpclient.GetStream();
                    message = new BinaryReader(n).ReadString();
                    myServer.Broadcast(this, message);
                    Console.WriteLine(message);
                }

                myServer.DisconnectClient(this);
                tcpclient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                myServer.DisconnectClient(this);
                tcpclient.Close();
            }
        }
    }
}
