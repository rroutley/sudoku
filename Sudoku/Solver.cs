using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Solver
    {
        IEnumerable<IStrategy> strategies;
        public TextWriter TraceOutput { get; set; }

        public Solver()
        {

            
            strategies = new List<IStrategy>{
                new SingleCandidateStrategy(),
                new OnePossibleValueInSquareStrategy(),
                new OnePossibleValueInColumnStrategy(),
                new OnePossibleValueInRowStrategy(),
            };
        }

        public void Execute(Board board)
        {

            int updates;
            do
            {
                board.ComputeAllCandiddates();

                updates = 0;
                foreach (var strategy in strategies)
                {
                    updates = strategy.Iterate(board);
                    if (updates > 0)
                    {
                        TraceOutput.WriteLine(strategy.GetType().Name);
                        break;
                    }
                }

                board.Print(TraceOutput);

                if (board.CellsUnfilled == 0)
                {
                    TraceOutput.WriteLine("Complete");
                    break;
                }

            } while (updates > 0);

        }

    }
}
