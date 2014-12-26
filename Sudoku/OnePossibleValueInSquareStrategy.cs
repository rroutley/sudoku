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

            // for all squares
            for (int k = 0; k < Board.n; k++)
            {
                for (int l = 0; l < Board.n; l++)
                {
                    // Find Candidates that appear only once.
                    int[] frequency = new int[Board.N + 1];
                    Tuple<int, int>[] firstSeen = new Tuple<int, int>[Board.N + 1];

                    for (int i = 0; i < Board.n; i++)
                    {
                        for (int j = 0; j < Board.n; j++)
                        {
                            int x = Board.n * k + i, y = Board.n * l + j;
                            foreach (var candidate in board.candidates[x,y])
                            {
                                frequency[candidate]++;
                                if (frequency[candidate] == 1)
                                {
                                    firstSeen[candidate] = Tuple.Create(x, y);
                                }
                            }
                        }
                    }

                    success += SetSingletons(board, frequency, firstSeen);
                }
            }

            return success;
        }
    }
}
