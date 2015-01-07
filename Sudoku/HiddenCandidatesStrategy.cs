using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class HiddenCandidatesStrategy : IStrategy
    {
        public static int Count = 0;
        private int length;

        public HiddenCandidatesStrategy(int length)
        {
            this.length = length;
        }

        public int Iterate(Board board)
        {
            // find pairs 

            int success = 0;

            success += HiddenCandidates(board, this.length);

            if (success > 0) Count++;

            return success;
        }

        private int HiddenCandidates(Board board, int length)
        {
            int success = 0;
            success += HiddenCandidates(board.CellsInRow, length);
            success += HiddenCandidates(board.CellsInColumn, length);
            success += HiddenCandidates(board.CellsInSquare, length);
            return success;
        }

        public int HiddenCandidates(Func<int, IEnumerable<Cell>> cellsInUnit, int length)
        {
            int success = 0;

            for (int index = 0; index < Board.N; index++)
            {
                foreach (var p in HiddenCandidates(cellsInUnit(index), length))
                {
                    foreach (var c in p.Cells)
                    {

                        if (c.Candidates.Except(p.Values).Count() > 0)
                        {
                            c.Candidates.IntersectWith(p.Values);
                            success++;
                        }
                    }
                }
            }

            return success;
        }

        public IEnumerable<CellResult> HiddenCandidates(IEnumerable<Cell> cellsInUnit, int length)
        {
            if (length < 2 || length > 4) throw new ArgumentOutOfRangeException("length");

            var unitInfos = (from cell in cellsInUnit
                             from candidate in cell.Candidates
                             group cell by candidate into g
                             where g.Count() <= length
                             select new
                             {
                                 Value = g.Key,
                                 Cells = g.ToList(),
                             }).ToList();


            if (length == 2)
            {
                return from a in unitInfos
                       from b in unitInfos
                       where a.Value < b.Value
                       let union = a.Cells.Union(b.Cells)
                       where union.Count() == length
                       select new CellResult
                       {
                           Values = new[] { a.Value, b.Value },
                           Cells = union.ToList()
                       };


            }
            else if (length == 3)
            {
                return from a in unitInfos
                       from b in unitInfos
                       from c in unitInfos
                       where a.Value < b.Value && b.Value < c.Value
                       let union = a.Cells.Union(b.Cells).Union(c.Cells)
                       where union.Count() == length
                       select new CellResult
                       {
                           Values = new[] { a.Value, b.Value, c.Value },
                           Cells = union.ToList()
                       };
            }
            else
            {
                return from a in unitInfos
                       from b in unitInfos
                       from c in unitInfos
                       from d in unitInfos
                       where a.Value < b.Value && b.Value < c.Value && c.Value < d.Value
                       let union = a.Cells.Union(b.Cells).Union(c.Cells).Union(d.Cells)
                       where union.Count() == length
                       select new CellResult
                       {
                           Values = new[] { a.Value, b.Value, c.Value, d.Value },
                           Cells = union.ToList()
                       };
            }

        }


    }

}

