using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class YWingStrategy : IStrategy
    {
        public int Iterate(Board board)
        {
            int success = 0;

            var twoCandidateCells = from cell in board.AllCells()                           // for each cell
                                    where cell.Candidates.Count == 2                               // where there are exactly two candidates
                                    select cell;

            // To start, we need to find a cell with exactly two candidates. We'll call this cell a pivot.
            // Then, we'll look for two more cells with 2 candidates as well. These cells (called pincers)
            // should be in the same row, column or block as the pivot. One of the two numbers in each 
            // pincer should be the same as in the pivot. The other number is the same for both pincers.   
            foreach (var cell in twoCandidateCells)
            {
                var pivotCandidates = cell.Candidates.ToArray();

                var pincerCells = (from c in twoCandidateCells
                                   where c != cell
                                   where c.X == cell.X || c.Y == cell.Y || c.Z == cell.Z
                                   where c.Candidates.Contains(pivotCandidates[0]) ^ c.Candidates.Contains(pivotCandidates[1])
                                   select c).ToArray();

                // for each pair of pincers, we need to check if they share a candidate that is not in the pivot. If they do, we can eliminate that candidate from any cell that sees both pincers.

                for (int i = 0; i < pincerCells.Length; i++)
                {
                    for (int j = i + 1; j < pincerCells.Length; j++)
                    {
                        var pincer1 = pincerCells[i];
                        var pincer2 = pincerCells[j];

                        var pincer1Candidates = pincer1.Candidates.ToArray();
                        var pincer2Candidates = pincer2.Candidates.ToArray();

                        var candidateCount = cell.Candidates.Union(pincer1Candidates).Union(pincer2Candidates).Distinct().Count(); ;
                        if (candidateCount != 3) continue;

                        // Now let's look where the both pincers intersect. If that cell contains a candiate that is shared by both pincers, we can eliminate it.
                        var sharedCandidates = pincer1Candidates.Intersect(pincer2Candidates);
                        if (sharedCandidates.Count() != 1) continue;

                        var sharedCandidate = sharedCandidates.First();

                        var affectedCells = from c in board.AllCells()
                                            where c != cell
                                            where (c.X == pincer1.X || c.Y == pincer1.Y || c.Z == pincer1.Z) && c != pincer1
                                            where (c.X == pincer2.X || c.Y == pincer2.Y || c.Z == pincer2.Z) && c != pincer2
                                            where c.Candidates.Contains(sharedCandidate)
                                            select c;

                        if (!affectedCells.Any()) continue;

                        foreach (var affectedCell in affectedCells)
                        {
                            Console.WriteLine($"YWing: Removing {sharedCandidate} from cell ({affectedCell.X}, {affectedCell.Y}, {affectedCell.Z}) because of pivot ({cell.X}, {cell.Y}, {cell.Z}) and pincers ({pincer1.X}, {pincer1.Y}, {pincer1.Z}), ({pincer2.X}, {pincer2.Y}, {pincer2.Z})");
                            affectedCell.Candidates.Remove(sharedCandidate);
                            success++;
                        }
                        return success;
                    }
                }

            }
            return success;
        }

    }
}
