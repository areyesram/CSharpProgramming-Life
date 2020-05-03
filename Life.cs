using System;

namespace areyesram
{
    internal static class Life
    {
        internal static int Size
        {
            get { return _size; }
            set
            {
                _size = value;
                Cells = new bool[1 + Size + 1, 1 + Size + 1];
            }
        }

        internal static bool[,] Cells { get; private set; }

        private static readonly Random _random = new Random();
        private static int _size;

        internal static void NewGeneration()
        {
            var newCells = new bool[1 + Size + 1, 1 + Size + 1];
            for (var i = 1; i <= Size; i++)
            {
                for (var j = 1; j <= Size; j++)
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
                    if (Cells[i, j])
                    {
                        // a cell with 2 or 3 neightbors lives on; else, it dies.
                        // (less than 2, it dies of loneliness; more than 3, it starves.)
                        newCells[i, j] = neighbors == 2 || neighbors == 3;
                    }
                    else
                    {
                        // in an empty space with exactly 3 neighbors, a new cell is born
                        newCells[i, j] = neighbors == 3;
                    }
                }
            }

            for (var i = 1; i <= Size; i++)
                for (var j = 1; j <= Size; j++)
                    Cells[i, j] = newCells[i, j];
        }

        internal static void Randomize()
        {
            for (var i = 1; i <= Size; i++)
                for (var j = 1; j <= Size; j++)
                    Cells[i, j] = _random.Next() < int.MaxValue / 2;
        }
    }
}