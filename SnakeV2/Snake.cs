using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeV2
{
    public enum direction { up, right, down, left }
    class Snake 
    {
        
        public direction currentDirection { get; set; }

        public int HeadX { get; set; }
        public int HeadY { get; set; }
        public List<Point> Body { get; set; }
        public Snake(int x, int y)
        {
            HeadX = x;
            HeadY = y;
        }
    }
}
