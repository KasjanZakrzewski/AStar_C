using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp.Views.Desktop;
using SkiaSharp;


namespace AStar_G
{
    internal class Cell
    {
        public int x;
        public int y;
        public SKColor color;
        public List<Cell> neighbours;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.color = SKColors.Black;
            this.neighbours = new List<Cell>();
        }

        public void add_neighbour(Cell neighbour)
        {
            this.neighbours.Add(neighbour);
        }
    }
}
