using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class OnePossibleValueStrategy : IStrategy
    {
        public int Iterate(Board board)
        {
            int success = 0;

            // for all columns
            for (int i = 0; i < Board.N; i++)
            {
                //// Find Candidates that appear only once.
                success += SetSingletons(board, board.CellsInSquare(i));
                success += SetSingletons(board, board.CellsInRow(i));
                success += SetSingletons(board, board.CellsInColumn(i));
            }

            return success;
        }

        private int SetSingletons(Board board, IEnumerable<Cell> cells)
        {
            int success = 0;
            var singletons = from cell in cells
                             where cell.Value == Cell.Empty
                             from candidate in cell.Candidates
                             group cell by candidate into g
                             where g.Count() == 1
                             select new
                             {
                                 Value = g.Key,
                                 X = g.Single().X,
                                 Y = g.Single().Y,
                             };

            foreach (var singleton in singletons)
            {
                board.SetCell(singleton.X, singleton.Y, singleton.Value);
                success++;
            }

            return success;
        }

    }
}

