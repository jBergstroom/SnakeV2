using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeV2
{
    internal class Game
    {
        static int BoardX = 10;
        static int BoardY = 10;
        Tile[,] board;

        public void Start()
        {
            CreateBoard();
            Snakearator();

            do
            {
                Console.Clear();

                DisplayBoard();
                Movement();
            } while (true);

            GameOver();
        }

        private void DisplayBoard()
        {
            throw new NotImplementedException();
        }

        private void Movement()
        {
            throw new NotImplementedException();
        }

        private void CreateBoard()
        {
            board = new Tile[BoardX, BoardY];


        }
        private void Snakearator()
        {
            throw new NotImplementedException();
        }

        private void GameOver()
        {
            throw new NotImplementedException();
        }
    }
}