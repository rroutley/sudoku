using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class SingleCandidateStrategy : IStrategy
    {

        public int Iterate(Board board)
        {
            int success = 0;

            board.ForEachCell((x, y) =>
            {
                var candidates = board.Cells[x, y].Candidates;
                if (candidates.Count == 1)
                {
                    board.SetCell(x, y, candidates.Single());
                    success++;
                }
            });

            return success;
        }
    }
}
