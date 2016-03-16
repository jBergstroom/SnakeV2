using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeV2
{
    class Snake
    {
        public int HeadX { get; set; }
        public int HeadY { get; set; }
        public List<Point> Body { get; set; }
        public int Direction { get; set; } // 1-4 Clockwise. Ex 1 = upp, 2 = Höger
    }
}
