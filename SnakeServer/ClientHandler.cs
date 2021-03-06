﻿using System;
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
        public bool nameOccupied = true;
        private List<string> colorList = new List<string> { "Red", "Blue", "Green", "Yellow", "Cyan", "Magenta", "Gray" };
        private int colorCounter = 0;
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
                do
                {
                    NetworkStream n = tcpclient.GetStream();
                    message = new BinaryReader(n).ReadString();
                    message.Trim();
                    string color = colorList[colorCounter].ToUpper();
                    colorCounter++;
                    if (colorCounter > 7)
                        colorCounter = 0;

                    nameOccupied = myServer.clientList.Select(x => x.name).Contains(message);
                    string partName = message.Substring(0, 4);
                    if (message != "" && partName == "name")
                    {
                        if (nameOccupied)
                        {
                            myServer.SingleBroadcast(this, "1;Name is occupadoo, gorom gorratt");
                        }
                        else
                        {
                            string restname = message.Substring(4);
                            myServer.SingleBroadcast(this, $"0;Name {restname} is ok, your color is {color}");
                            name = restname;
                            myServer.Broadcast($"{message} has join the gaming worlds");
                        }
                    }
                } while (nameOccupied);
                Thread.Sleep(50);
                myServer.Broadcast("Waiting for other player;");
                while (!myServer.GejmÅn)
                {
                    //Console.WriteLine(name + " is waiting for game to start");
                    Thread.Sleep(500);
                    
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
                myServer.DisconnectClient(this);
                tcpclient.Close();
                Console.WriteLine(ex.Message + " clienthandler exception");
            }
            finally
            {
                tcpclient.Close();
            }
        }
    }
}
