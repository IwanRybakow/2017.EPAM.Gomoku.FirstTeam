using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Square
    {
        public Point Location { get; set; }
        public int Value { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public Square (Point location, int value, int coordX, int coordY)
        {
            Location = location;
            Value = value;
            CoordX = coordX;
            CoordY = coordY;
        }
    }
}
