using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MandelBrot
{
    public partial class Form1 : Form
    {
        Bitmap map;
        //const int WIDTH = 1750;
        //const int HEIGHT = 1000;
        const int WIDTH = 350;
        const int HEIGHT = 200;
        const double RATIO = (double) HEIGHT / WIDTH;
        const double xRATIO = 2.5 / 3.5;

        double currMini = 0, currMaxi = 0, currMinr = 0, currMaxr = 0;

        public Form1()
        {
            InitializeComponent();
            map = new Bitmap(WIDTH, HEIGHT);
            this.WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawMandel(1,-2.5,1,-1);
        }

        private void DrawMandel(double maxr, double minr, double maxi, double mini)
        {
            currMaxi = maxi;
            currMaxr = maxr;
            currMini = mini;
            currMinr = minr;

            progressBar1.Value = 0;
            int color = 0;
            double cx = 0, cy = 0;
            double zx = 0, zy = 0;
            double iteration = 0, max_iteration = (int)numericUpDown1.Value;

            double xjump = ((maxr - minr) / WIDTH);
            double yjump = ((maxi - mini) / HEIGHT);

            double tempzx = 0;

            for (int x = 0; x < WIDTH; x++)
            {
                cx = (xjump * x) - Math.Abs(minr);
                for (int y = 0; y < HEIGHT; y++)
                {

                    zx = 0;
                    zy = 0;
                    cy = (yjump * y) - Math.Abs(mini);
                    iteration = 0;

                    while (zx * zx + zy * zy <= 4 && iteration < max_iteration)
                    {
                        iteration++;
                        tempzx = zx;
                        zx = (zx * zx) - (zy * zy) + cx;
                        zy = (2 * tempzx * zy) + cy;
                    }

                    color = (iteration < max_iteration) ? Convert.ToInt32((iteration / max_iteration) * 254) : 255;
                    //color = (iteration < max_iteration) ? 0 : 255;
                    map.SetPixel(x, y, Color.FromArgb(color%128*2, color&32*7, color%16*14));
                    //map.SetPixel(x, y, Color.FromArgb(color, color, color));
                }

                progressBar1.Value = (int)Math.Ceiling(((double)x / WIDTH * 100));

                //if (x % 20 == 0)
                //    panel1.CreateGraphics().DrawImage(map, 0, 0);
            }

            panel1.CreateGraphics().DrawImage(map, 0, 0);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int ex = e.X;
            int ey = e.Y;
            double currentxjump = (currMaxr - currMinr) / WIDTH;
            double currentyjump = (currMaxi - currMini) / HEIGHT;

            int zoomx = WIDTH / 5;
            int zoomy = HEIGHT / 5;

            DrawMandel(((ex + zoomx) * currentxjump) - Math.Abs(currMinr), 
                ((ex - zoomx) * currentxjump) - Math.Abs(currMinr), 
                ((ey + zoomy) * currentyjump) - Math.Abs(currMini), 
                ((ey - zoomy) * currentyjump) - Math.Abs(currMini));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DrawMandel(1, -2.5, 1, -1);
        }

    }
}
