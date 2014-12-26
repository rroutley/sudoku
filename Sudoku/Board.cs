using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Board
    {
        public const int n = 3;
        public const int N = n * n;
        public const int Empty = 0;

        internal int[,] cell = new int[N, N];
        internal bool[,] isNew = new bool[N, N];
        internal HashSet<int>[,] candidates = new HashSet<int>[N, N];

        internal int CellsUnfilled = N * N;


        public void ForEachCell(Action<int, int> action)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    action(i, j);
                }
            }
        }

        public void ComputeCandidate(int x, int y)
        {
            HashSet<int> candidates;
            if (cell[x, y] != Empty)
            {
                candidates = new HashSet<int>();
            }
            else
            {

                candidates = new HashSet<int>(Enumerable.Range(1, 9));

                // Remove any numbers found in the row 
                for (int i = 0; i < N; i++)
                {
                    if (cell[i, y] != Empty)
                    {
                        candidates.Remove(cell[i, y]);
                    }
                }

                // Remove any numbers found in the column
                for (int j = 0; j < N; j++)
                {
                    if (cell[x, j] != Empty)
                    {
                        candidates.Remove(cell[x, j]);
                    }
                }

                // Remove any numbers found in the square
                for (int i = n * (x / n), k = 0; k < n; k++, i++)
                {
                    for (int j = n * (y / n), l = 0; l < n; l++, j++)
                    {
                        if (cell[i, j] != Empty)
                        {
                            candidates.Remove(cell[i, j]);
                        }
                    }
                }
            }

            this.candidates[x, y] = candidates;
        }

        public static Board Load(string text)
        {
            Board b = new Board();
            if (text.Length != 81)
            {
                throw new ArgumentException();
            }

            int p = 0;
            text = text.Replace('.', '0');
            b.ForEachCell((x, y) =>
            {
                b.cell[x, y] = int.Parse(text.Substring(p, 1));
                p++;
            });

            return b;
        }

        public void Print(TextWriter writer)
        {



            writer.WriteLine("+-----------------------+");

            for (int i = 0; i < N; i++)
            {

                if (i % 3 == 0 && i > 0)
                {
                    writer.WriteLine("|-------+-------+-------|");
                }

                for (int j = 0; j < N; j++)
                {
                    if (j % n == 0)
                    {
                        writer.Write("| ");
                    }

                    if (cell[i, j] != 0)
                    {
                        if (isNew[i, j])
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        writer.Write(cell[i, j]);

                        Console.ResetColor();
                    }
                    else
                    {
                        writer.Write(" ");
                    }

                    writer.Write(" ");


                }
                writer.WriteLine("|");

            }
            writer.WriteLine("+-----------------------+");

        }


        public void ComputeAllCandiddates()
        {
            CellsUnfilled = N * N;
            this.ForEachCell((x, y) => {

                if (cell[x, y] != Empty)
                {
                    CellsUnfilled--;
                }

                ComputeCandidate(x, y);
                isNew[x, y] = false;
            });
        }




        internal void SetCell(int x, int y, int value)
        {
            if (this.cell[x, y] != Empty && cell[x, y] != value)
            {
                throw new InvalidOperationException();
            }

            this.cell[x, y] = value;
            this.isNew[x, y] = true;
            this.CellsUnfilled--;
        }
    }
}

