using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SnakeServer
{
    public class Server
    {
        List<ClientHandler> clientList = new List<ClientHandler>();
        public void Run()
        {
            TcpListener myListner = new TcpListener(IPAddress.Any, 5000);
            Console.WriteLine("Snakeserver now listning.");

            try
            {
                TcpClient c = myListner.AcceptTcpClient();
                ClientHandler newClient = new ClientHandler(c, this);
                clientList.Add(newClient);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }

    public class ClientHandler
    {
        public TcpClient tcpclient;
        private Server myServer;
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
                while (!message.Equals("quit"))
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
            }
        }
    }

}
