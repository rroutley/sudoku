using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public abstract class OnePossibleValueStrategyBase : IStrategy
    {
        protected int SetSingletons(Board board, int[] frequency, Tuple<int, int>[] firstSeen)
        {
            int success = 0;
            for (int f = 1; f <= Board.N; f++)
            {
                if (frequency[f] == 1)
                {
                    board.SetCell(firstSeen[f].Item1, firstSeen[f].Item2, f);
                    success++;
                }
            }
            return success;
        }

        public abstract int Iterate(Board board);
    }
}
