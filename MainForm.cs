using System;
using System.Drawing;
using System.Windows.Forms;

namespace areyesram
{
    public partial class MainForm : Form
    {
        private const int DishSize = 300;
        private static Graphics _graphics;
        private readonly Color _back = Color.FromArgb(164, 161, 118);
        private readonly Color _fore = Color.FromArgb(210, 210, 186);

        private void MainForm_Load(object sender, EventArgs e)
        {
            _graphics = Graphics.FromHwnd(Handle);
            Life.Size = DishSize;
            Life.Randomize();
            DrawCells();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Life.NewGeneration();
            DrawCells();
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            Life.Randomize();
            DrawCells();
        }

        private static readonly Bitmap Bmp = new Bitmap(DishSize, DishSize);

        private void DrawCells()
        {
            for (var x = 0; x < DishSize; x++)
            {
                for (var y = 0; y < DishSize; y++)
                {
                    if (InsideCircle(x, y, DishSize / 2))
                        Bmp.SetPixel(x, y, Life.Cells[x + 1, y + 1] ? _fore : _back);
                }
            }
            _graphics.DrawImage(Bmp, 8, 40);
        }

        /// <summary>
        /// Indicates wether a point is inside a circle of a certain radius.
        /// </summary>
        /// <param name="x">X coordinate of point.</param>
        /// <param name="y">Y coordinate of point</param>
        /// <param name="r">Radius.</param>
        /// <remarks>Assumes the circle has its origin in (r,r).</remarks>
        /// <returns>True if the point is inside the circle; False if not.</returns>
        private static bool InsideCircle(int x, int y, int r)
        {
            return (x - r) * (x - r) + (y - r) * (y - r) < r * r;
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            _graphics.Dispose();
        }
    }
}
