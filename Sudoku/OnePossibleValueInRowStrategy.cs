using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class OnePossibleValueInRowStrategy : OnePossibleValueStrategyBase
    {
        public override int Iterate(Board board)
        {
            int success = 0;


            // for all rows
            for (int j = 0; j < Board.N; j++)
            {
                // Find Candidates that appear only once.
                int[] frequency = new int[Board.N + 1];
                Tuple<int, int>[] firstSeen = new Tuple<int, int>[Board.N + 1];
                for (int i = 0; i < Board.N; i++)
                {
                    foreach (var candidate in board.Cells[i, j].Candidates)
                    {
                        frequency[candidate]++;
                        if (frequency[candidate] == 1)
                        {
                            firstSeen[candidate] = Tuple.Create(i, j);
                        }
                    }
                }

                success += SetSingletons(board, frequency, firstSeen);
            }

              
            

            return success;
        }

    }
}
