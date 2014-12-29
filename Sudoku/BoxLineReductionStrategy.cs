using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class BoxLineReductionStrategy : IStrategy
    {

        public int Iterate(Board board)
        {
            int success = 0;

            for (int index = 0; index < Board.N; index++)
            {
                var cellsInRow = board.CellsInRow(index);
                var candidatesInBox1 = from cell in cellsInRow
                                       where cell.Value == Cell.Empty
                                       from candidate in cell.Candidates
                                       group new { cell, candidate } by candidate into g
                                       where 1 < g.Count() && g.Count() <= 3
                                       let cc = g.Key
                                       let z = g.Select(c => c.cell.Z).Distinct()
                                       where z.Count() == 1
                                       select new
                                       {
                                           Candidate = cc,
                                           Z = z.ToList(),
                                           Cells = g.Select(c => c.cell).ToList()
                                       };

                var cellsInColumn = board.CellsInColumn(index);
                var candidatesInBox2 = from cell in cellsInColumn
                                       where cell.Value == Cell.Empty
                                       from candidate in cell.Candidates
                                       group new { cell, candidate } by candidate into g
                                       where 1 < g.Count() && g.Count() <= 3
                                       let cc = g.Key
                                       let z = g.Select(c => c.cell.Z).Distinct()
                                       where z.Count() == 1
                                       select new
                                       {
                                           Candidate = cc,
                                           Z = z.ToList(),
                                           Cells = g.Select(c => c.cell).ToList()
                                       };

                var candidatesInBox = candidatesInBox1.Union(candidatesInBox2);

                foreach (var p in candidatesInBox.Where(f => f.Z.Count() == 1))
                {
                    foreach (var cell in board.CellsInSquare(p.Z.Single()))
                    {
                        if (cell.Value != Cell.Empty) continue;

                        if (p.Cells.Contains(cell))
                        {
                            continue;
                        }

                        if (cell.Candidates.Contains(p.Candidate))
                        {
                            cell.RemoveCandidate(p.Candidate);
                            success++;
                        }
                    }
                }

            }

            return success;
        }
    }
}
