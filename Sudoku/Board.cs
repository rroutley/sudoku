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

        public Cell[,] Cells { get; private set; }

        public int CellsUnfilled { get; private set; }

        public Board()
        {
            Cells = new Cell[N, N];

            ForEachCell((x, y) =>
            {
                Cells[x, y] = new Cell(x, y);
            });

            CellsUnfilled = N * N;
        }

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

        public IEnumerable<Cell> CellsInColumn(int index)
        {
            for (int i = 0; i < N; i++)
            {
                yield return Cells[i, index];
            }
        }

        public IEnumerable<Cell> CellsInRow(int index)
        {
            for (int j = 0; j < N; j++)
            {
                yield return Cells[index, j];
            }
        }

        private void UpdateCandidates(int x, int y)
        {
            var cell = Cells[x, y];
            var value = cell.Value;

            if (value == Empty)
            {
                return;
            }

            cell.Candidates.Clear();

            for (int i = 0; i < N; i++)
            {
                Cells[i, y].Candidates.Remove(value);
            }

            for (int j = 0; j < N; j++)
            {
                Cells[x, j].Candidates.Remove(value);
            }

            // Remove this value from candidates found in the square
            for (int i = n * (x / n), k = 0; k < n; k++, i++)
            {
                for (int j = n * (y / n), l = 0; l < n; l++, j++)
                {
                    Cells[i, j].Candidates.Remove(value);
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
                b.Cells[x, y].Value = int.Parse(text.Substring(p, 1));
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

                    var cell = Cells[i, j];
                    if (cell.Value != 0)
                    {
                        if (cell.IsNew)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        writer.Write(cell.Value);

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
                if (Cells[x, y].IsNew)
                {
                    UpdateCandidates(x, y);

                    Cells[x, y].IsNew = false;
                }

            });
        }




        public void ResetAllCandidates()
        {
            CellsUnfilled = N * N;
            this.ForEachCell((x, y) =>
            {

                Cells[x, y].Candidates = new HashSet<int>(Enumerable.Range(1, 9));

                if (Cells[x, y].Value != Empty)
                {
                    CellsUnfilled--;

                    // Mark as new so UpdateCandates will be execute for this cell.
                    Cells[x, y].IsNew = true;
                }

            });
        }




        internal void SetCell(int x, int y, int value)
        {
            if (Cells[x, y].Value != Empty)
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

            if (Cells[x, y].Value == Empty)
            {
                this.CellsUnfilled--;
            }


            this.Cells[x, y].Value = value;
            this.Cells[x, y].IsNew = true;
        }



        private bool CanSetCell(int x, int y, int value)
        {
            for (int i = 0; i < N; i++)
            {
                if (i != x && Cells[i, y].Value == value)
                {
                    return false;
                }
            }

            for (int j = 0; j < N; j++)
            {
                if (j != y && Cells[x, j].Value == value)
                {
                    return false;
                }
            }

            // Remove this value from candidates found in the square
            for (int i = n * (x / n), k = 0; k < n; k++, i++)
            {
                for (int j = n * (y / n), l = 0; l < n; l++, j++)
                {
                    if (i != x && j != y && Cells[i, j].Value == value)
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


        public Cell this[int x, int y]
        {
            get
            {
                return this.Cells[x, y];
            }
        }
    }
}

