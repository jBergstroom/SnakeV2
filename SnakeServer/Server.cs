using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeServer
{
    public class Server
    {
        public bool GejmÅn = false;
        public List<ClientHandler> clientList = new List<ClientHandler>();
        public void Run()
        {
            TcpListener myListner = new TcpListener(IPAddress.Any, 5000);
            Console.WriteLine("Snakeserver now listning.");
            myListner.Start();
            try
            {
                TcpClient c = myListner.AcceptTcpClient();
                ClientHandler newClient = new ClientHandler(c, this);
                clientList.Add(newClient);
                Thread clientThread = new Thread(newClient.Run);
                clientThread.Start();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + "Server exception");
            }
        }
        public void DisconnectClient(ClientHandler client)
        {
            clientList.Remove(client);
        }

        internal void Broadcast(ClientHandler clientHandler, string message)
        {
            List<ClientHandler> tmpList = new List<ClientHandler>();
            foreach (var item in clientList)
            {
                if (item.tcpclient.Connected)
                {
                    tmpList.Add(item);
                }
            }
            foreach (var item in tmpList)
            {
                NetworkStream n = item.tcpclient.GetStream();
                BinaryWriter w = new BinaryWriter(n);
                w.Write(message);
                w.Flush();

                if (tmpList.Count() == 1)
                {
                    NetworkStream not = item.tcpclient.GetStream();
                    BinaryWriter what = new BinaryWriter(not);
                    what.Write("Du är ju sååååååååååå ensam");
                    what.Flush();
                }
            }
        }
        internal void SingleBroadcast(ClientHandler clientHandler, string message)
        {
            List<ClientHandler> tmpList = new List<ClientHandler>();
            foreach (var item in clientList)
            {
                if (item.tcpclient.Connected)
                {
                    tmpList.Add(item);
                }
            }
            foreach (var item in tmpList)
            {
                if (item == clientHandler)
                {
                    NetworkStream n = item.tcpclient.GetStream();
                    BinaryWriter w = new BinaryWriter(n);
                    Console.WriteLine($"{message}");
                    w.Write(message);
                    w.Flush();
                }

                //if (tmpList.Count() == 1)
                //{
                //    NetworkStream not = item.tcpclient.GetStream();
                //    BinaryWriter what = new BinaryWriter(not);
                //    what.Write("Du är ju sååååååååååå ensam");
                //    what.Flush();
                //}
            }
        }
    }
}
