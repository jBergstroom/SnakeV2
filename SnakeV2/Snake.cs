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
        public List<Coordinate> Body { get; set; }

    }
}
