using SnakeServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeV2
{
    public class Game
    {
        private static int BoardX = 30;
        private static int BoardY = 30;
        private Tile[,] board;
        public List<Snake> snakelist = new List<Snake>();
        public bool gameOver = false;
        public int colorCount = 0;
        public Random rng;
        List<ConsoleColor> colorList = new List<ConsoleColor> { ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Gray };

        public void Start(List<ClientHandler> list)
        {
            rng = new Random();
            Console.OutputEncoding = Encoding.UTF8;
            CreateBoard();
            foreach (var item in list)
            {
                Snakearator();
            }

            Thread boardThread = new Thread(RunGame);
            //Thread directionThread = new Thread(DirectionInput);
            //directionThread.Start();
            boardThread.Start();


            //GameOver();
        }

        //private void DirectionInput()
        //{
        //    do
        //    {
        //        ConsoleKeyInfo input = Console.ReadKey();
        //        if (input != null)
        //        {
        //            switch (input.Key)
        //            {
        //                case ConsoleKey.LeftArrow:
        //                    //int leftTemp = ((int)ourSnake.currentDirection - 1) % 4;
        //                    //ourSnake.currentDirection = (direction)leftTemp;
        //                    ourSnake.currentDirection--;
        //                    if ((int)ourSnake.currentDirection < 0)
        //                        ourSnake.currentDirection = (direction)3;
        //                    break;
        //                case ConsoleKey.RightArrow:
        //                    //int rightTemp = ((int)ourSnake.currentDirection + 1) % 4;
        //                    //ourSnake.currentDirection = (direction)rightTemp;
        //                    ourSnake.currentDirection++;
        //                    if ((int)ourSnake.currentDirection > 3)
        //                        ourSnake.currentDirection = 0;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        else
        //            Thread.Sleep(10);
        //    } while (true);
        //}

        private void RunGame()
        {
            while (gameOver == false)
            {
                //Console.Clear();

                DisplayBoard();
                Movement();
                Thread.Sleep(50);
            }
        }

        private void DisplayBoard()
        {
            Console.SetCursorPosition(0, 0);
            string worldString = "";
            for (int y = 0; y < BoardY; y++)
            {
                for (int x = 0; x < BoardX; x++)
                {
                    Tile tile = board[x, y];
                    bool snakehead = false;
                    for (int i = 0; i < snakelist.Count; i++)
                    {
                        if (x == snakelist[i].HeadX && y == snakelist[i].HeadY)
                        {
                            worldString += " S ";
                            snakehead = true;
                        }
                    }
                    if (snakehead)
                    {
                        Console.Write(" S ");
                    }
                    else if (tile.HasBody)
                    {
                        Console.ForegroundColor = tile.Color;
                        Console.Write(" 0 ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(" . ");
                    }
                }

                Console.WriteLine();
            }
        }

        private void Movement()
        {
            foreach (var ourSnake in snakelist)
            {
                board[ourSnake.HeadX, ourSnake.HeadY].HasBody = true;
                board[ourSnake.HeadX, ourSnake.HeadY].Color = ourSnake.SnakeColor;
                if (ourSnake.Alive)
                {
                    switch (ourSnake.currentDirection)
                    {
                        case direction.up:
                            if (ourSnake.HeadY - 1 >= 0)
                                ourSnake.HeadY--;
                            //else
                            //    ourSnake.Alive = false;//GameOver();
                            break;
                        case direction.right:
                            if (ourSnake.HeadX + 1 < BoardX)
                                ourSnake.HeadX++;
                            //else
                            //    ourSnake.Alive = false;//GameOver();
                            break;
                        case direction.down:
                            if (ourSnake.HeadY + 1 < BoardY)
                                ourSnake.HeadY++;
                            //else
                            //    ourSnake.Alive = false;//GameOver();
                            break;
                        case direction.left:
                            if (ourSnake.HeadX - 1 >= 0)
                                ourSnake.HeadX--;
                            //else
                            //    ourSnake.Alive = false;//GameOver();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void CreateBoard()
        {
            board = new Tile[BoardX, BoardY];
            for (int y = 0; y < BoardY; y++)
            {
                for (int x = 0; x < BoardX; x++)
                {
                    Tile tile = new Tile();
                    board[x, y] = tile;
                }
            }
            //board[4, 6].HasBody = true;
            //board[4, 8].HasApple = true;
        }

        private void Snakearator()
        {
            Snake ourSnake = new Snake(rng.Next(5, BoardX-5), rng.Next(5, BoardY-5));
            snakelist.Add(ourSnake);
            ourSnake.SnakeColor = colorList[colorCount];
            colorCount++;
            ourSnake.currentDirection = (direction)rng.Next(0, 4);
        }

        private void GameOver()
        {
            gameOver = true;
            Console.Clear();
            Console.WriteLine("Fyyyyyy du dog");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}