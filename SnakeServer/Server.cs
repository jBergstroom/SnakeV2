using SnakeV2;
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
        public readonly object myLock = new object();
        public int count = 0;
        public void Run()
        {
            TcpListener myListener = new TcpListener(IPAddress.Any, 5000);
            Console.WriteLine("Snakeserver now listening...");
            try
            {
                myListener.Start();
                while (!GejmÅn)//Lobby
                {
                    try
                    {
                        TcpClient c = myListener.AcceptTcpClient();
                        ClientHandler newClient = new ClientHandler(c, this);
                        newClient.name = "Guest";
                        clientList.Add(newClient);
                        Thread clientThread = new Thread(newClient.Run);
                        clientThread.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "Server exception");
                    }
                }
                Game game = new Game();
                Thread gameThread = new Thread(() => game.Start(clientList));
                Thread inputThread = new Thread(() => ListenForInput(game, clientList.First()));
                gameThread.Start();
                inputThread.Start();
                while (GejmÅn)
                {
                    GameBroadcast();
                    Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " myListener exception");
            }
            finally
            {
                if (myListener != null)
                    myListener.Stop();
            }

        }

        public void ListenForInput(Game game, ClientHandler client)
        {
            while (true)
            {

                NetworkStream n = client.tcpclient.GetStream();
                BinaryReader listener = new BinaryReader(n);
                int input = listener.ReadInt32();
                if (input == 1)
                {
                    int index = clientList.IndexOf(client);

                    if (((int)game.snakelist[index].currentDirection - 1) < 0)
                        game.snakelist[index].currentDirection = (direction)3;
                    else
                        game.snakelist[index].currentDirection--;
                }
                else if (input == 0)
                {
                    int index = clientList.IndexOf(client);
                    if (((int)game.snakelist[index].currentDirection + 1) > 3)
                        game.snakelist[index].currentDirection = 0;
                    else
                        game.snakelist[index].currentDirection++;
                }
            }
        }

        public void DisconnectClient(ClientHandler client)
        {
            Console.WriteLine($"Client {client.name} has left the building...");
            Broadcast($"Client {client.name} chickend out...");
            clientList.RemoveAll(x => x.name == client.name);
            Console.Beep();
        }

        internal void Broadcast(string message)
        {
            lock (myLock)
            {
                List<ClientHandler> tmpList = new List<ClientHandler>();

                foreach (var item in clientList)
                {
                    //if (item.tcpclient.Connected && item.name.Trim() != "")
                    //{
                    //    tmpList.Add(item);
                    //}
                    if (item != null)
                    {
                        if (item.name.Trim() != "")
                        {
                            tmpList.Add(item);
                        }
                    }
                }
                clientList = tmpList;
                foreach (var players in tmpList)
                {
                    message += (players.name + " is connected;");
                }
                foreach (var item in tmpList)
                {
                    if (tmpList.Count() == 1)
                    {
                        NetworkStream n = item.tcpclient.GetStream();
                        BinaryWriter w = new BinaryWriter(n);
                        w.Write(message + "you are alone;");
                        w.Flush();
                    }
                    else
                    {
                        NetworkStream n = item.tcpclient.GetStream();
                        BinaryWriter w = new BinaryWriter(n);
                        w.Write(message);
                        w.Flush();
                    }
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
                    if (message.Split(';')[0] == "0")
                    {
                        count++;
                    }
                    if (count >= 1)
                    {
                        GejmÅn = true;
                    }
                    w.Write(message);
                    w.Flush();
                }
            }
        }
        internal void GameBroadcast()
        {
            lock (myLock)
            {
                List<ClientHandler> tmpList = new List<ClientHandler>();

                foreach (var item in clientList)
                {
                    if (item != null)
                    {
                        if (item.name.Trim() != "")
                        {
                            tmpList.Add(item);
                        }
                    }
                }
                clientList = tmpList;

                //foreach (var item in tmpList)
                //{
                //    if (tmpList.Count() == 1)
                //    {
                //        NetworkStream n = item.tcpclient.GetStream();
                //        BinaryWriter w = new BinaryWriter(n);
                //        w.Write(message + "you are alone;");
                //        w.Flush();
                //    }
                //    else
                //    {
                //        NetworkStream n = item.tcpclient.GetStream();
                //        BinaryWriter w = new BinaryWriter(n);
                //        w.Write(message);
                //        w.Flush();
                //    }
                //}
            }
        }

    }
}
