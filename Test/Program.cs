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

            while (true)
            {
                Console.WriteLine("Enter Grid number");
                var input = Console.ReadLine();
                int number;
                if (!int.TryParse(input, out number)){
                    continue;
                }
                if (number < 1 || number > 50)
                {
                    break;
                }

                puzzle = LoadGrid(number);

                Board b = Board.Load(puzzle);

                b.Print(Console.Out);

                var solver = new Solver();
                solver.TraceOutput  =Console.Out;
                solver.Execute(b);


             
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
