using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class NakedCandidatesStrategy : IStrategy
    {
        public static int Count = 0;

        public int Iterate(Board board)
        {
            // find pairs 

            int success = 0;

            success += NakedCandidates(board, 2);
            success += NakedCandidates(board, 3);
            success += NakedCandidates(board, 4);

            if (success > 0) Count++;

            return success;
        }

        private int NakedCandidates(Board board, int length)
        {
            int success = 0;
            success += NakedCandidates(board.CellsInRow, length);
            success += NakedCandidates(board.CellsInColumn, length);
            success += NakedCandidates(board.CellsInSquare, length);
            return success;
        }

        private int NakedCandidates(Func<int, IEnumerable<Cell>> cellsInUnit, int length)
        {
            int success = 0;

            for (int index = 0; index < Board.N; index++)
            {
                foreach (var p in NakedCandidates(cellsInUnit(index), length))
                {
                    // Found Naked Pair
                    // Remove pair values from other candidates in unit
                    foreach (var cell in cellsInUnit(index))
                    {
                        if (cell.Value != Cell.Empty) continue;

                        if (p.Cells.Contains(cell))
                        {
                            continue;
                        }

                        foreach (var candidate in p.Values)
                        {
                            var cellCandidates = cell.Candidates;
                            if (cellCandidates.Contains(candidate))
                            {
                                cell.RemoveCandidate(candidate);

                                success++;
                            }
                        }

                    }
                }

            }

            return success;
        }


        public IEnumerable<CellResult> NakedCandidates(IEnumerable<Cell> cellsInUnit, int length)
        {

            if (length < 2 || length > 4) throw new ArgumentOutOfRangeException("length");

            var unitInfos = (from cell in cellsInUnit
                             where cell.Value == Cell.Empty
                             where cell.Candidates.Count <= length
                             select new
                             {
                                 Index = cell.X * 9 + cell.Y, // Could make Cell IComparable?
                                 Candidates = cell.Candidates,
                                 Cell = cell,
                             }).ToList();

            if (length == 2)
            {

                return from a in unitInfos
                       from b in unitInfos
                       where a.Index < b.Index
                       let union = a.Candidates.Union(b.Candidates)
                       where union.Count() == length
                       select new CellResult
                       {
                           Values = union.ToList(),
                           Cells = new[] { a.Cell, b.Cell }
                       };

            }
            else if (length == 3)
            {

                return from a in unitInfos
                       from b in unitInfos
                       from c in unitInfos
                       where a.Index < b.Index && b.Index < c.Index
                       let union = a.Candidates.Union(b.Candidates).Union(c.Candidates)
                       where union.Count() == length
                       select new CellResult
                       {
                           Values = union.ToList(),
                           Cells = new[] { a.Cell, b.Cell, c.Cell }
                       };

            }
            else
            {
                return from a in unitInfos
                       from b in unitInfos
                       from c in unitInfos
                       from d in unitInfos
                       where a.Index < b.Index && b.Index < c.Index && c.Index < d.Index
                       let union = a.Candidates.Union(b.Candidates).Union(c.Candidates).Union(d.Candidates)
                       where union.Count() == length
                       select new CellResult
                       {
                           Values = union.ToList(),
                           Cells = new[] { a.Cell, b.Cell, c.Cell, d.Cell }
                       };
            }

        }
    }
}

