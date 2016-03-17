using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeV2
{
    internal class Game
    {
        private static int BoardX = 20;
        private static int BoardY = 10;
        private Tile[,] board;
        Snake ourSnake;

        public bool gameOver = false;

        public void Start()
        {
            Console.OutputEncoding = Encoding.UTF8;
            CreateBoard();
            Snakearator();

            Thread boardThread = new Thread(RunGame);
            Thread directionThread = new Thread(DirectionInput);
            directionThread.Start();
            boardThread.Start();


            //GameOver();
        }

        private void DirectionInput()
        {
            do
            {
                ConsoleKeyInfo input = Console.ReadKey();
                if (input != null)
                {
                    switch (input.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            //int leftTemp = ((int)ourSnake.currentDirection - 1) % 4;
                            //ourSnake.currentDirection = (direction)leftTemp;
                            ourSnake.currentDirection--;
                            if ((int)ourSnake.currentDirection < 0)
                                ourSnake.currentDirection = (direction)3;
                            break;
                        case ConsoleKey.RightArrow:
                            //int rightTemp = ((int)ourSnake.currentDirection + 1) % 4;
                            //ourSnake.currentDirection = (direction)rightTemp;
                            ourSnake.currentDirection++;
                            if ((int)ourSnake.currentDirection > 3)
                                ourSnake.currentDirection = 0;
                            break;
                        default:
                            break;
                    }
                }
                else
                    Thread.Sleep(10);
            } while (true);
        }

        private void RunGame()
        {
            while (gameOver == false)
            {
                Console.Clear();

                DisplayBoard();
                Movement();
                Thread.Sleep(200);
            } 
        }

        private void DisplayBoard()
        {
            for (int y = 0; y < BoardY; y++)
            {
                for (int x = 0; x < BoardX; x++)
                {
                    Tile tile = board[x, y];
                    if (x == ourSnake.HeadX && y == ourSnake.HeadY)
                    {
                        Console.Write('S');
                    }
                    else if (tile.HasBody)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("0");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (tile.HasApple)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("a");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        private void Movement()
        {
            board[ourSnake.HeadX, ourSnake.HeadY].HasBody = true;

            switch (ourSnake.currentDirection)
            {
                case direction.up:
                    if (ourSnake.HeadY - 1 >= 0)
                        ourSnake.HeadY--;
                    else
                        GameOver();
                    break;
                case direction.right:
                    if (ourSnake.HeadX + 1 < BoardX)
                        ourSnake.HeadX++;
                    else
                        GameOver();
                    break;
                case direction.down:
                    if (ourSnake.HeadY + 1 < BoardY)
                        ourSnake.HeadY++;
                    else
                        GameOver();
                    break;
                case direction.left:
                    if (ourSnake.HeadY - 1 >= 0)
                        ourSnake.HeadX--;
                    else
                        GameOver();
                    break;
                default:
                    break;
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
            board[4, 8].HasApple = true;
        }

        private void Snakearator()
        {
            Random rng = new Random();
            ourSnake = new Snake(rng.Next(1, BoardX), rng.Next(1, BoardY));

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