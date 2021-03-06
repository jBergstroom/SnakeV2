﻿using SnakeV2;
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
            Console.WriteLine("Snakeserver now slithering...");
            Thread gameStartThread = new Thread(() => gamestartInput(myListener));
            gameStartThread.Start();
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
                //Game game = new Game();
                //Thread gameThread = new Thread(() => game.Start(clientList));
                //gameThread.Priority = ThreadPriority.AboveNormal;
                //gameThread.Start();
                //for (int i = 0; i < clientList.Count; i++)
                //{
                //    Thread inputThread = new Thread(() => ListenForInput(game, clientList[i], i));
                //    inputThread.Start();
                //}
                //while (GejmÅn)
                //{
                //    GameBroadcast();
                //    Thread.Sleep(2);
                //}
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

        private void gamestartInput(TcpListener listener)
        {
            while (!GejmÅn)
            {
                Console.WriteLine("Press 'S' to start the game.");
                ConsoleKeyInfo input = Console.ReadKey();
                if (input.Key == ConsoleKey.S)
                {
                    Console.WriteLine("Starting Game in..");
                    for (int i = 3; i > 0; i--)
                    {
                        Console.WriteLine(i+"..");
                        Thread.Sleep(1000);
                        Console.Beep();
                    }
                    StartGame();

                    GejmÅn = true;
                }
            }
        }
        private void StartGame()
        {
            Console.Clear();
            Game game = new Game();
            Thread gameThread = new Thread(() => game.Start(clientList));
            gameThread.Priority = ThreadPriority.AboveNormal;
            gameThread.Start();
            for (int i = 0; i < clientList.Count; i++)
            {
                Thread inputThread = new Thread(() => ListenForInput(game, clientList[i], i));
                inputThread.Start();
            }
            while (GejmÅn)
            {
                GameBroadcast();
                Thread.Sleep(2);
            }
        }

        public void ListenForInput(Game game, ClientHandler client, int index)
        {

            NetworkStream n = client.tcpclient.GetStream();
            BinaryReader listener = new BinaryReader(n);

            while (true) //game.snakelist[index].Alive
            {
                int input = listener.ReadInt32();
                if (input == 1)
                {
                    if (((int)game.snakelist[index].currentDirection - 1) < 0)
                        game.snakelist[index].currentDirection = (direction)3;
                    else
                        game.snakelist[index].currentDirection--;
                }
                else if (input == 0)
                {
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
                foreach (var player in tmpList)
                {
                    message += (player.name + " is connected.;");
                }
                foreach (var item in tmpList)
                {
                    if (tmpList.Count() == 1)
                    {
                        NetworkStream n = item.tcpclient.GetStream();
                        BinaryWriter w = new BinaryWriter(n);
                        w.Write(message + "You are currently alone, wait for more players to join the server.;");
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
                    //if (count >= 1)
                    //{
                    //    GejmÅn = true;
                    //}
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
