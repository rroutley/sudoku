using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class OnePossibleValueInSquareStrategy : OnePossibleValueStrategyBase
    {
        public override int Iterate(Board board)
        {
            int success = 0;

            // for all columns
            for (int i = 0; i < Board.N; i++)
            {
                //// Find Candidates that appear only once.
                success += SetSingletons(board, board.CellsInSquare(i));
            }

            return success;
        }
    }
}
