using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    [Serializable]
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


        private void UpdateCandidates(int x, int y)
        {
            var value = cell[x, y];
            if (value == Empty)
            {
                return;
            }

            candidates[x, y].Clear();

            for (int i = 0; i < N; i++)
            {
                candidates[i, y].Remove(value);
            }

            for (int j = 0; j < N; j++)
            {
                candidates[x, j].Remove(value);
            }

            // Remove this value from candidates found in the square
            for (int i = n * (x / n), k = 0; k < n; k++, i++)
            {
                for (int j = n * (y / n), l = 0; l < n; l++, j++)
                {
                    candidates[i, j].Remove(value);
                }
            }

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

        public void UpdateChangedCandidates()
        {
            this.ForEachCell((x, y) =>
            {
                // Update the candidates that will be affected by placing this new cell.
                if (isNew[x, y])
                {
                    UpdateCandidates(x, y);

                    isNew[x, y] = false;
                }

            });
        }

       


        public void ResetAllCandidates()
        {
            CellsUnfilled = N * N;
            this.ForEachCell((x, y) => {

                candidates[x, y] = new HashSet<int>(Enumerable.Range(1, 9));

                if (cell[x, y] != Empty)
                {
                    CellsUnfilled--;

                    // Mark as new so UpdateCandates will be execute for this cell.
                    isNew[x, y] = true;
                }
                
            });
        }




        internal void SetCell(int x, int y, int value)
        {
            if (this.cell[x, y] != Empty)
            {
                throw new InvalidOperationException();
            }

            ReplaceCell(x, y, value);
        }

        internal void ReplaceCell(int x, int y, int value)
        {
            if (!CanSetCell(x, y, value))
            {
                throw new InvalidOperationException();
            }

            if (cell[x, y] == Empty)
            {
                this.CellsUnfilled--;
            }


            this.cell[x, y] = value;
            this.isNew[x, y] = true;
        }



        private bool CanSetCell(int x, int y, int value)
        {
            for (int i = 0; i < N; i++)
            {
                if (i != x && cell[i, y] == value)
                {
                    return false;
                }
            }

            for (int j = 0; j < N; j++)
            {
                if (j != y && cell[x, j] == value)
                {
                    return false;
                }
            }

            // Remove this value from candidates found in the square
            for (int i = n * (x / n), k = 0; k < n; k++, i++)
            {
                for (int j = n * (y / n), l = 0; l < n; l++, j++)
                {
                    if (i != x && j != y && cell[i, j] == value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        internal Board Clone()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;

                return (Board)formatter.Deserialize(ms);
            }
        }
    }
}

