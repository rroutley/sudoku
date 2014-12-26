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

            string puzzle;

            int number = 0;
            while (true)
            {
                //Console.WriteLine("Enter Grid number");
                //var input = Console.ReadLine();
                //if (!int.TryParse(input, out number)){
                //    continue;
                //}
                number++;
                if (number < 1 || number > 50)
                {
                    break;
                }

                puzzle = LoadGrid(number);

                Board b = Board.Load(puzzle);

                b.Print(Console.Out);

                var solver = new Solver();
                solver.TraceOutput = Console.Out;
                var success = solver.Execute(b);
                if (!success)
                {
                    Console.ReadLine();
                }


            }
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
