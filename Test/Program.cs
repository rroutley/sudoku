using Sudoku;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var gen = new Generator();
            Board newPuzzle;
            //do
            //{
                var completeBoard = gen.NewCompletedBoard();

                //completeBoard.Print(Console.Out);

                newPuzzle = gen.BuildPuzzle(completeBoard, new Generator.GeneratorOptions { Strategies = new[] { new SingleCandidateStrategy() } });
                //newPuzzle.Print(Console.Out);

            //} while (newPuzzle.CellsFilled > 20);

            Console.WriteLine(newPuzzle);

            var solver = new Solver();
            solver.TraceOutput = Console.Out;
            var t = Board.Load(newPuzzle.ToString());
            var success = solver.Execute(t);
            
            Console.WriteLine(newPuzzle);

          //  string puzzle;

            //var x = File.ReadLines(@"..\..\top95.txt");

            //int number = 0;
            ////foreach(var puzzle in x)
            //while(true)
            //{
            //    number++;
            //    //Console.WriteLine("Enter Grid number");
            //    //var input = Console.ReadLine();
            //    //if (!int.TryParse(input, out number))
            //    //{
            //    //    continue;
            //    //}


            //    if (number < 1 || number > 50)
            //    {
            //        break;
            //    }

            //    Console.WriteLine("Grid {0:00}", number);
            //    var puzzle = LoadGrid(number);

            //    Board b = Board.Load(puzzle);

            //    var solver = new Solver();
            //   // solver.TraceOutput = Console.Out;
            //    b.Print(solver.TraceOutput);

            //    var success = solver.Execute(b);
            //    if (!success)
            //    {
            //        b.Print(Console.Out);
            //    }


            //}

            //Console.WriteLine("Naked Pair used {0} times", NakedCandidatesStrategy.Count);
            //if (BruteForceStrategy.Count > 0)
            //{
            //    Console.WriteLine("Brute Force used {0} times", BruteForceStrategy.Count);
            //}
            Console.ReadLine();
        }

        private static string LoadGrid(int number)
        {
            string puzzle;
            var grid = string.Format("Grid {0:00}", number);

            int line = 0;
            var lines = File.ReadAllLines(@"..\..\p096_sudoku.txt");
            for (; lines[line] != grid; line++) ;

            line++;

            puzzle = "";
            for (int i = 0; i < 9; i++, line++)
            {
                puzzle += lines[line];
            }
            return puzzle;
        }
    }
}
