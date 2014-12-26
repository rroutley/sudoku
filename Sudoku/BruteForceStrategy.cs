using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class BruteForceStrategy : IStrategy
    {
        private Solver solver;

        public BruteForceStrategy(Solver solver)
        {
            this.solver = solver;
        }

        public int Iterate(Board board)
        {
            // find the cell with the least number of candidates
            int minX = -1, minY = -1, minCount = 10;
            board.ForEachCell((x, y) =>
            {
                if (board.cell[x, y] == Board.Empty)
                {
                    var c = board.candidates[x, y].Count;
                    if (c == 0)
                    {
                        throw new InvalidOperationException();
                    }


                    if (c < minCount)
                    {
                        minX = x;
                        minY = y;
                        minCount = c;
                    }
                }
            });


            // Save the state of thew board foreach candidiate
            foreach (var value in board.candidates[minX, minY])
            {
                board.ReplaceCell(minX, minY, value);
                solver.PushState(board);
            }

            // throw away the top of stack as its the same as the current state.
            solver.PopState();

            // Continue iterating with all strategies until complete or inconsistant

            return 1;
        }
    }
}
