using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Generator
    {
        private Random rand = new Random();

        public readonly static IEnumerable<IStrategy> SingletonStrategies = new List<IStrategy> {
                new SingleCandidateStrategy(),
                new OnePossibleValueInSquareStrategy(),
                new OnePossibleValueInColumnStrategy(),
                new OnePossibleValueInRowStrategy(),
            };

        public readonly static IEnumerable<IStrategy> ToughStrategies = SingletonStrategies.Union(new List<IStrategy> {
                new NakedCandidatesStrategy(2),
                new NakedCandidatesStrategy(3),
                new HiddenCandidatesStrategy(2),
                new HiddenCandidatesStrategy(3),
                new NakedCandidatesStrategy(4),
                new HiddenCandidatesStrategy(4),
                new PointingPairsStrategy(),
                new BoxLineReductionStrategy(),
                new XWingStrategy(),
            });


        public Board NewCompletedBoard()
        {

            var board = new Board();
            board.ResetAllCandidates();

            var result = Solve(board, new SolverOptions
            {
                MaxSolutions = 1,
                Strategies = SingletonStrategies,
                UseBruteForce = true,
            });

            return result.Puzzles.First();
        }


        public Board BuildPuzzle(Board board, GeneratorOptions generatorOptions)
        {
            var filledCells = GetRandomCellOrdering(board, generatorOptions);
            int filledCellCount = filledCells.Count();

            var options = new SolverOptions
            {
                MaxSolutions = 2,
                Strategies = generatorOptions.Strategies,
            };

            for (int cellNum = 0; cellNum < filledCellCount && board.CellsFilled > generatorOptions.MinimumFilledCells; cellNum++)
            {
                var cell = filledCells[cellNum];
                int oldValue = cell.Value;

                cell.Value = Cell.Empty;
                board.ResetAllCandidates();
                var cloneBoard = board.Clone();
                var result = Solve(cloneBoard, options);

                if (!(result.Status == BoardState.Solved && result.Puzzles.Count == 1))
                {
                    cell.Value = oldValue;
                }
            }

            return board;
        }

        private IList<Cell> GetRandomCellOrdering(Board board, GeneratorOptions options)
        {
            List<Cell> cells = new List<Cell>(board.AllCells());
            int n = cells.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Cell temp = cells[k];
                cells[k] = cells[n];
                cells[n] = temp;
            }

            return cells;
        }


        private SolverResult Solve(Board board, SolverOptions options)
        {

            var result = StrategySolver(board, options.Strategies);
            if (result.Status == BoardState.NoSolution)
            {
                return result;
            }

            if (board.CellsUnfilled == 0)
            {
                return new SolverResult
                {
                    Status = BoardState.Solved,
                    Puzzles = new List<Board>(new[] { board }),
                };
            }

            if (options.UseBruteForce)
            {
                return BruteForceSolver(board, options);
            }

            return SolverResult.NoSolution;
        }

        private SolverResult BruteForceSolver(Board board, SolverOptions options)
        {
            SolverResult result = null;

            // Find cells with least canidates
            List<Cell> loCells;
            if (CellsWithLeastCandidates(board, out loCells))
            {
                // Pick an a random cell from this list
                var cellIndex = rand.Next(loCells.Count);
                var randCell = loCells[cellIndex];

                // Try each of the candidates 
                foreach (var value in randCell.Candidates)
                {
                    var cloneBoard = board.Clone();
                    cloneBoard.SetCell(randCell.X, randCell.Y, value);

                    var temp = Solve(cloneBoard, options);

                    if (temp.Status == BoardState.Solved)
                    {
                        if (result != null && result.Puzzles.Any())
                        {
                            result.Puzzles.AddRange(temp.Puzzles);
                        }
                        else
                        {
                            result = temp;
                            result.NumberOfDecisionPoints++;
                        }

                        if (result.Puzzles.Count > options.MaxSolutions)
                        {
                            return result;
                        }
                    }
                }
            }

            return result;
        }


        private static bool CellsWithLeastCandidates(Board board, out List<Cell> cells)
        {
            bool result = true;

            int minCandidates = 10;
            var loCells = new List<Cell>();
            board.ForEachCell((x, y) =>
            {
                var cell = board[x, y];
                if (cell.Value == Cell.Empty)
                {
                    int count = cell.Candidates.Count;
                    if (count == 0)
                    {
                        result = false;

                    }
                    else if (count < minCandidates)
                    {
                        minCandidates = cell.Candidates.Count;
                        loCells.Clear();
                    }

                    if (count == minCandidates)
                    {
                        loCells.Add(cell);
                    }
                }
            });

            cells = result ? loCells : null;
            return result;
        }




        private SolverResult StrategySolver(Board board, IEnumerable<IStrategy> strategies)
        {
            try
            {
                int updates = 0;
                do
                {
                    board.UpdateChangedCandidates();

                    foreach (var strategy in strategies)
                    {
                        updates = strategy.Iterate(board);
                        if (updates != 0)
                        {
                            break;
                        }
                    }

                } while (updates != 0);

            }
            catch (InvalidOperationException)
            {
                return SolverResult.NoSolution;
            }

            return new SolverResult { Status = BoardState.Unknown };

        }


        public class SolverResult
        {
            public static readonly SolverResult NoSolution = new SolverResult { Status = BoardState.NoSolution };

            public BoardState Status { get; set; }

            public List<Board> Puzzles { get; set; }

            public int NumberOfDecisionPoints { get; set; }
        }

        public class SolverOptions
        {
            public int MaxSolutions { get; set; }

            public IEnumerable<IStrategy> Strategies { get; set; }

            public bool UseBruteForce { get; set; }
        }

        public class GeneratorOptions
        {
            public GeneratorOptions()
            {
                MinimumFilledCells = 17;
            }

            public IEnumerable<IStrategy> Strategies { get; set; }

            public int MinimumFilledCells { get; set; }
        }

        public enum BoardState
        {
            Unknown,
            Incomplete,
            NoSolution,
            Solved,
        }
    }
}
