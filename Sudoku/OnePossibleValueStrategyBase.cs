using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public abstract class OnePossibleValueStrategyBase : IStrategy
    {
        protected int SetSingletons(Board board, IEnumerable<Cell> cells)
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

        public abstract int Iterate(Board board);
    }
}
