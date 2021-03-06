using System;
using System.Drawing;
using System.Windows.Forms;

namespace areyesram
{
    public partial class MainForm : Form
    {
        private const int DishSize = 300;
        private static readonly bool[,] _cells = new bool[1 + DishSize + 1, 1 + DishSize + 1];
        private static readonly bool[,] _newCells = new bool[1 + DishSize + 1, 1 + DishSize + 1];
        private static readonly Random _random = new Random();
        private static Graphics _graphics;

        private readonly Color _back = Color.FromArgb(164, 161, 118);
        private readonly Color _fore = Color.FromArgb(210, 210, 186);

        private void MainForm_Load(object sender, EventArgs e)
        {
            _graphics = Graphics.FromHwnd(Handle);
            Randomize();
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
            NextGeneration();
        }

        private void NextGeneration()
        {
            for (var i = 1; i <= DishSize; i++)
            {
                for (var j = 1; j <= DishSize; j++)
                {
                    var neighbors =
                        (_cells[i - 1, j - 1] ? 1 : 0) +
                        (_cells[i, j - 1] ? 1 : 0) +
                        (_cells[i + 1, j - 1] ? 1 : 0) +
                        (_cells[i - 1, j] ? 1 : 0) +
                        (_cells[i + 1, j] ? 1 : 0) +
                        (_cells[i - 1, j + 1] ? 1 : 0) +
                        (_cells[i, j + 1] ? 1 : 0) +
                        (_cells[i + 1, j + 1] ? 1 : 0);
                    if (_cells[i, j])
                    {
                        // a cell with 2 or 3 neightbors lives on; else, it dies.
                        // (less than 2, it dies of loneliness; more than 3, it starves.)
                        _newCells[i, j] = neighbors == 2 || neighbors == 3;
                    }
                    else
                    {
                        // in an empty space with exactly 3 neighbors, a new cell is born
                        _newCells[i, j] = neighbors == 3;
                    }
                }
            }

            for (var i = 1; i <= DishSize; i++)
                for (var j = 1; j <= DishSize; j++)
                    _cells[i, j] = _newCells[i, j];
            DrawCells();
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            Randomize();
        }

        private void Randomize()
        {
            for (var i = 1; i <= DishSize; i++)
                for (var j = 1; j <= DishSize; j++)
                    _cells[i, j] = _random.Next() < int.MaxValue / 2;
            DrawCells();
        }

        private static readonly Bitmap _bmp = new Bitmap(DishSize, DishSize);

        private void DrawCells()
        {
            for (var x = 0; x < DishSize; x++)
            {
                for (var y = 0; y < DishSize; y++)
                {
                    if (InsideCircle(x, y, DishSize / 2))
                        _bmp.SetPixel(x, y, _cells[x + 1, y + 1] ? _fore : _back);
                }
            }
            _graphics.DrawImage(_bmp, 8, 40);
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _graphics.Dispose();
        }
    }
}
