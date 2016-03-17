using System;

namespace SnakeV2
{
    internal class Tile
    {
        public bool HasApple { get; set; }
        public ConsoleColor Color { get; set; }
        public bool HasBody { get; set; }
    }
}