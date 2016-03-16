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

        public void Start()
        {
            Console.OutputEncoding = Encoding.UTF8;
            CreateBoard();
            Snakearator();

            do
            {
                Console.Clear();

                DisplayBoard();
                Movement();
                Thread.Sleep(400);
            } while (true);

            GameOver();
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
                        Console.Write("0");
                    }
                    else if (tile.HasApple)
                    {
                        Console.Write("a");
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
                    ourSnake.HeadY--;
                    break;
                case direction.right:
                    ourSnake.HeadX++;
                    break;
                case direction.down:
                    ourSnake.HeadY++;
                    break;
                case direction.left:
                    ourSnake.HeadX--;
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
            board[4, 6].HasBody = true;
            board[4, 8].HasApple = true;
        }

        private void Snakearator()
        {
            Random rng = new Random();
            ourSnake = new Snake(rng.Next(1,BoardX), rng.Next(1,BoardY));

            ourSnake.currentDirection = (direction)rng.Next(0, 4);
        }

        private void GameOver()
        {
            
        }
    }
}