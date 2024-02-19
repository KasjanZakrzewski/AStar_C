using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace AStar_G
{
    public partial class Form1 : Form
    {
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        private SKControl skiaCanvas;

        static int wall_procent = 45;
        Empty start;
        Empty finish;
        List<Empty> openSet;
        private bool done = false;

        Random randy;

        static int width = 1200;
        static int height = 600;
        static int w = 60;
        static int h = 30;
        private float squareWidth = width / w; // Szerokoœæ kwadratu
        private float squareHeight = height / h; // Wysokoœæ kwadratu

        private float E = 2;


        private Cell[,] world = new Cell[w, h];
        public Form1()
        {
            Cell temp;
            openSet = new List<Empty>();
            randy = new Random();

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    world[i, j] = Generate(i, j);
                    //world[i, j] = new Cell(i, j);
                }
            }

            neighbours(h, w, world);

            temp = world[randy.Next(w), randy.Next(h)];
            while (temp is Wall)
            {
                temp = world[randy.Next(w), randy.Next(h)];
            }
            start = (Empty)temp;
            start.g = 0;
            

            temp = world[randy.Next(w), randy.Next(h)];
            while (temp is Wall)
            {
                temp = world[randy.Next(w), randy.Next(h)];
            }
            finish = (Empty)temp;
            start.f = dist(start, finish);

            openSet.Add(start);

            start.color = SKColors.LimeGreen;
            finish.color = SKColors.LimeGreen;

            InitializeComponent();
            this.Size = new Size(width + 17, height + 40);

            InitializeSkiaCanvas();
            StartAnimation();
        }

        private Cell Generate(int x, int y)
        {
            Random randy = new Random();

            if (randy.Next(100) < wall_procent) {
                return new Wall(x, y);
            }
            else
            {
                return new Empty(x, y);
            }
        }

        void neighbours(int h, int w, Cell[,] world)
        {
            int pomI;
            int pomJ;
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    pomI = i - 1;
                    pomJ = j - 1;
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            if (pomI >= 0 && pomI < w && pomJ >= 0 && pomJ < h)
                            {
                                world[i, j].add_neighbour(world[pomI, pomJ]);
                            }
                            pomI += 1;
                        }
                        pomI = i - 1;
                        pomJ += 1;
                    }
                }
            }
        }

        private void StartAnimation()
        {
            // Uruchom timer do odœwie¿ania ekranu
            myTimer.Interval = 16; // Oko³o 60 klatek na sekundê
            myTimer.Tick += Timer_Tick;
            myTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!done)
            {
                if (openSet.Count() > 0)
                {
                    Empty current = openSet.OrderBy(o => o.f).ThenBy(o => o.h).First();
                    if (current == finish)
                    {
                        done = true;

                        start.color = SKColors.LimeGreen;
                        finish.color = SKColors.LimeGreen;
                        current = finish.came;

                        while (current != start)
                        {
                            current.color = SKColors.Yellow;
                            current = current.came;
                        }
                        skiaCanvas.Invalidate();
                    }
                    else
                    {
                        openSet.Remove(current);
                        current.color = SKColors.DarkBlue;

                        Empty temp_cell;
                        double temp;
                        foreach (Cell n in current.neighbours)
                        {
                            if (n is Wall)
                            {
                                continue;
                            }

                            temp_cell = (Empty)n;
                            temp = current.g + dist(current, temp_cell);
                            if (temp < temp_cell.g)
                            {
                                temp_cell.came = (Empty)current;
                                temp_cell.g = temp;
                                temp_cell.h = dist(temp_cell, finish);
                                temp_cell.f = temp + E*temp_cell.h;

                                if (!openSet.Contains(temp_cell))
                                {
                                    temp_cell.color = SKColors.Aquamarine;
                                    openSet.Add(temp_cell);
                                }
                            }
                        }
                        skiaCanvas.Invalidate();
                        current.color = SKColors.BlueViolet;
                    }
                    
                }
            }
            


            /*int i = randy.Next(w);
            int j = randy.Next(h);

            world[i, j].color = SKColors.Red;*/

            // Odœwie¿ kontrolkê SKControl, aby narysowaæ kwadrat na nowej pozycji

        }

        double dist(Cell cell, Cell finish)
        {
            int x = Math.Abs(cell.x - finish.x);
            int y = Math.Abs(cell.y - finish.y);

            double h = x * x + y * y;
            h = Math.Sqrt(h);

            return h;
        }

        private void InitializeSkiaCanvas()
        {
            skiaCanvas = new SKControl();
            skiaCanvas.PaintSurface += SkiaCanvas_PaintSurface;

            skiaCanvas.Dock = DockStyle.Fill;
            Controls.Add(skiaCanvas);

        }

        private void SkiaCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            // Wyczyœæ p³ótno
            canvas.Clear(SKColors.Black);

            // Ustaw kolor i styl pêdzla
            SKPaint paint = new SKPaint
            {
                Color = SKColors.Blue,
                IsAntialias = true
            };

            SKRect rect;
            float x, y;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    x = world[i, j].x * squareWidth + 1;
                    y = world[i, j].y * squareHeight + 1;
                    paint.Color = world[i, j].color;
                    rect = new SKRect(x, y, x + squareWidth - 1, y + squareHeight - 1);
                    canvas.DrawRect(rect, paint);
                }
            }
        }
    }
}