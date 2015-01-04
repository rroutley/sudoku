using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public static class CellExtensions
    {

        public static int RemoveCandidate(this IEnumerable<Cell> cells, int candidate, IEnumerable<Cell> exceptions)
        {

            int success = 0;
            foreach (var cell in cells)
            {
                if (cell.Value != Cell.Empty)
                {
                    continue;
                }

                if (exceptions.Contains(cell))
                {
                    continue;
                }

                if (cell.Candidates.Contains(candidate))
                {
                    cell.RemoveCandidate(candidate);
                    success++;
                }

            }

            return success;
        }


    }
}
