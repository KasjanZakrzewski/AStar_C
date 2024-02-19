using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar_G
{
    internal class Wall : Cell
    {
        public Wall(int x, int y) : base(x, y)
        {
            this.color = SKColors.MediumVioletRed;
        }
    }
}
