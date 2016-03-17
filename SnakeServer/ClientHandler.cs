using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
                bool nameOccupied = true;
                do
                {
                    NetworkStream n = tcpclient.GetStream();
                    message = new BinaryReader(n).ReadString();
                    message.Trim();
                    nameOccupied = myServer.clientList.Select(x => x.name).Contains(message);
                    if (message != "" && message.Substring(0, 4) == "name")
                    {
                        if (nameOccupied)
                        {
                            myServer.SingleBroadcast(this, "1;Name is occupadoo, gorom gorratt");
                        }
                        else
                        {
                            myServer.SingleBroadcast(this, $"0;Name {message} is ok");
                            name = message;
                            //myServer.Broadcast($"{message} has join the gaming worlds");
                        }
                    }
                } while (nameOccupied);

                while (!myServer.GejmÅn)
                {
                    myServer.Broadcast("Waiting for other player;");
                    Thread.Sleep(5);
                }
                while (myServer.GejmÅn && message != "ded")
                {
                    NetworkStream n = tcpclient.GetStream();
                    message = new BinaryReader(n).ReadString();
                    myServer.Broadcast(message);
                    Console.WriteLine(message);
                }

                //myServer.DisconnectClient(this);
                //tcpclient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " clienthandler exception");
            }
            finally
            {
                myServer.DisconnectClient(this);
                tcpclient.Close();
            }
        }
    }
}
