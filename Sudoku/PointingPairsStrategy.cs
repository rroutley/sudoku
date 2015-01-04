using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public class PointingPairsStrategy : IStrategy
    {
        public int Iterate(Board board)
        {
            int success = 0;

            for (int index = 0; index < Board.N; index++)
            {
                var cells = board.CellsInSquare(index);

                var foo = from cell in cells
                          where cell.Value == Cell.Empty
                          from candidate in cell.Candidates
                          group new { cell, candidate } by candidate into g
                          where 1 < g.Count() && g.Count() <= 3
                          let cc = g.Key
                          let x = g.Select(c => c.cell.X).Distinct()
                          let y = g.Select(c => c.cell.Y).Distinct()
                          where x.Count() == 1 || y.Count() == 1
                          select new
                          {
                              Candidate = cc,
                              X = x.ToList(),
                              Y = y.ToList(),
                              Cells = g.Select(c => c.cell).ToList()
                          };




                foreach (var p in foo.Where(f => f.X.Count() == 1))
                {
                    success += board.CellsInRow(p.X.Single()).RemoveCandidate(p.Candidate, p.Cells);
                }

                foreach (var p in foo.Where(f => f.Y.Count() == 1))
                {
                    success += board.CellsInColumn(p.Y.Single()).RemoveCandidate(p.Candidate, p.Cells);
                }

            }

            return success;
        }
    }
}
