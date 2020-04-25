using System;
using System.Drawing;
using System.Windows.Forms;

namespace areyesram
{
    public partial class MainForm : Form
    {
        private const int DishSize = 300;
        private static readonly bool[,] Cells = new bool[1 + DishSize + 1, 1 + DishSize + 1];
        private static readonly bool[,] NewCells = new bool[1 + DishSize + 1, 1 + DishSize + 1];
        private static Random _random = new Random();
        private static Graphics _graphics;

        private readonly Color _back = Color.FromArgb(164, 161, 118);
        private readonly Color _fore = Color.FromArgb(210, 210, 186);

        private void Form_Load(object sender, EventArgs e)
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
                        (Cells[i - 1, j - 1] ? 1 : 0) +
                        (Cells[i, j - 1] ? 1 : 0) +
                        (Cells[i + 1, j - 1] ? 1 : 0) +
                        (Cells[i - 1, j] ? 1 : 0) +
                        (Cells[i + 1, j] ? 1 : 0) +
                        (Cells[i - 1, j + 1] ? 1 : 0) +
                        (Cells[i, j + 1] ? 1 : 0) +
                        (Cells[i + 1, j + 1] ? 1 : 0);
                    NewCells[i, j] = Cells[i, j] ? neighbors == 2 || neighbors == 3 : neighbors == 3;
                }
            }

            for (var i = 1; i <= DishSize; i++)
                for (var j = 1; j <= DishSize; j++)
                    Cells[i, j] = NewCells[i, j];
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
                    Cells[i, j] = _random.Next() < int.MaxValue / 2;
            DrawCells();
        }

        private static readonly Bitmap Bmp = new Bitmap(DishSize, DishSize);

        private void DrawCells()
        {
            for (var x = 0; x < DishSize; x++)
                for (var y = 0; y < DishSize; y++)
                    if ((x - DishSize / 2) * (x - DishSize / 2) + (y - DishSize / 2) * (y - DishSize / 2) <
                        (DishSize / 2) * (DishSize / 2))
                        Bmp.SetPixel(x, y, Cells[x + 1, y + 1] ? _fore : _back);
            _graphics.DrawImage(Bmp, 8, 40);
        }

        private void Form2_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            _graphics.Dispose();
        }
    }
}
