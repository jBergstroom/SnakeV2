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
        public List<ClientHandler> clientList2 = new List<ClientHandler>();
        public readonly object myLock = new object();
        public void Run()
        {
            TcpListener myListner = new TcpListener(IPAddress.Any, 5000);
            Console.WriteLine("Snakeserver now listening...");
            try
            {
                myListner.Start();
                while (true)
                {

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " myListener exception");
            }
            finally
            {
                if (myListner != null)
                    myListner.Stop();
            }

        }
        public void DisconnectClient(ClientHandler client)
        {
            clientList.Remove(client);
            Console.WriteLine($"Client {client.name} has left the building...");
            Broadcast($"Client {client.name} chickend out...");
            Console.Beep();
        }

        internal void Broadcast(string message)
        {
            List<ClientHandler> tmpList = new List<ClientHandler>();
            lock (myLock)
            {

                foreach (var item in clientList)
                {
                    //if (item.tcpclient.Connected && item.name.Trim() != "")
                    //{
                    //    tmpList.Add(item);
                    //}
                    if (item.name.Trim() != "")
                    {
                        tmpList.Add(item);
                    }
                }
                clientList = tmpList;
                foreach (var item in tmpList)
                {
                    foreach (var players in tmpList)
                    {
                        message += (players.name + " is connected;");
                    }

                    NetworkStream n = item.tcpclient.GetStream();
                    BinaryWriter w = new BinaryWriter(n);
                    w.Write(message);
                    w.Flush();
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
