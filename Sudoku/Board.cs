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

        public Cell[,] Cells { get; private set; }

        public int CellsUnfilled { get; private set; }

        public int CellsFilled { get { return N * N - CellsUnfilled; } }

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

        public IEnumerable<Cell> AllCells()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    yield return Cells[i, j];
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

        public IEnumerable<Cell> CellsInSquare(int index)
        {
            int x = n * (index / n), y = n * (index % n);

            for (int i = n * (x / n), k = 0; k < n; k++, i++)
            {
                for (int j = n * (y / n), l = 0; l < n; l++, j++)
                {
                    yield return Cells[i, j];
                }
            }

        }

        public void UpdateCandidates(int x, int y)
        {
            var cell = Cells[x, y];
            var value = cell.Value;

            if (value == Cell.Empty)
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

        public static Board Load(IList<int> ints)
        {
            Board b = new Board();
            if (ints.Count != 81)
            {
                throw new ArgumentException();
            }

            int p = 0;
            b.ForEachCell((x, y) =>
            {
                b.Cells[x, y].Candidates = [];

                var value = ints[p];
                if (value >= 1 && value <= 9)
                {
                    b.Cells[x, y].Value = value;
                }
                else
                {
                    b.Cells[x, y].Value = Cell.Empty;
                    var candidates = new SortedSet<int>();
                    while (value > 0)
                    {
                        var candidate = value % 10;
                        if (candidate >= 1 && candidate <= 9)
                        {
                            candidates.Add(candidate);
                        }
                        value /= 10;
                    }
                    b.Cells[x, y].Candidates = [.. candidates];
                }
                p++;
            });

            return b;
        }

        public void Print(TextWriter writer)
        {
            if (writer == TextWriter.Null) return;

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

        public void PrintWithCandidates(TextWriter writer)
        {
            if (writer == TextWriter.Null) return;

            var width = (from c in AllCells()
                         orderby c.Candidates.Count descending
                         select c.Candidates.Count).First();
            if (width < 1) width = 1;

            var line = new string('-', 1 + width * 3 + 3);

            writer.WriteLine($"+{line}+{line}+{line}+");

            for (int i = 0; i < N; i++)
            {

                if (i % 3 == 0 && i > 0)
                {
                    writer.WriteLine($"|{line}+{line}+{line}|");
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
                        writer.Write(new string(' ', width - 1));

                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        foreach (var candidate in cell.Candidates)
                        {
                            writer.Write(candidate);
                        }
                        Console.ResetColor();
                        writer.Write(new string(' ', width - cell.Candidates.Count));
                    }

                    writer.Write(" ");


                }
                writer.WriteLine("|");

            }
            writer.WriteLine($"+{line}+{line}+{line}+");

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

                if (Cells[x, y].Value != Cell.Empty)
                {
                    CellsUnfilled--;

                    // Mark as new so UpdateCandates will be execute for this cell.
                    Cells[x, y].IsNew = true;
                }

            });
        }




        internal void SetCell(int x, int y, int value)
        {
            if (Cells[x, y].Value != Cell.Empty)
            {
                throw new InvalidOperationException("Cell already has a value");
            }

            ReplaceCell(x, y, value);
        }

        internal void ReplaceCell(int x, int y, int value)
        {
            if (!CanSetCell(x, y, value))
            {
                throw new InvalidOperationException("Cannot set cell to this value");
            }

            if (Cells[x, y].Value == Cell.Empty)
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
            Board clone = new Board();

            this.ForEachCell((x, y) =>
            {
                clone.Cells[x, y] = this.Cells[x, y].Clone();
            });

            clone.CellsUnfilled = this.CellsUnfilled;

            return clone;
        }

        public Cell this[int x, int y]
        {
            get
            {
                return this.Cells[x, y];
            }
        }

        public override string ToString()
        {
            return string.Join("", this.AllCells().Select(c => c.Value));
        }
    }
}

