using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar_G
{
    internal class Empty: Cell
    {
        public Empty came;
        public double f = int.MaxValue;
        public double g = int.MaxValue;
        public double h = 0;

        public Empty(int x, int y) : base(x, y)
        {
            this.color = SKColors.LightGray;
        }
    }
}
