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
        Stack<Board> boardStack = new Stack<Board>();

        public Solver()
        {
            TraceOutput = TextWriter.Null;

            strategies = new List<IStrategy> {
                new SingleCandidateStrategy(),
                new OnePossibleValueInSquareStrategy(),
                new OnePossibleValueInColumnStrategy(),
                new OnePossibleValueInRowStrategy(),
                new NakedCandidatesStrategy(),
                new HiddenCandidatesStrategy(),
                new PointingPairsStrategy(),
                new BoxLineReductionStrategy(),
                new XWingStrategy(),
                //new BruteForceStrategy(this),
            };
        }

        public bool Execute(Board board)
        {

            board.ResetAllCandidates();

            int updates;
            do
            {
                board.UpdateChangedCandidates();

                updates = 0;
                try
                {
                    foreach (var strategy in strategies)
                    {
                        updates = strategy.Iterate(board);

                        if (updates > 0)
                        {
                            TraceOutput.WriteLine(strategy.GetType().Name);
                            break;
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    if (boardStack.Count > 0)
                    {
                        TraceOutput.WriteLine("Backtrack");
                        board = PopState();
                        updates = 1;
                    }
                }

                board.Print(TraceOutput);

                if (board.CellsUnfilled == 0)
                {
                    TraceOutput.WriteLine("Complete");
                    break;
                }

            } while (updates > 0);


            return board.CellsUnfilled == 0;
        }


        public void PushState(Board board)
        {
            boardStack.Push(board.Clone());
        }

        public Board PopState()
        {
            return boardStack.Pop();
        }

    }
}
