using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class XWingStrategy : IStrategy
    {
        public int Iterate(Board board)
        {
            int success = 0;

            for (int c = 0; c < Board.N; c++)
            {

                success += XWingByRows(board, c);
                success += XWingByColumns(board, c);


            }


            return success;
        }

        private static int XWingByRows(Board board, int c)
        {
            int success = 0;

            var xwingColumns = from row in Enumerable.Range(0, 9)                           // for each row
                               from cell in board.CellsInRow(row)                           // for each cell in row
                               from candidate in cell.Candidates                            // for each candidate in cell
                               where candidate == c                                         // where candidate is the value we're looking for
                               group cell by row into rowGrouping                           // group cell by row
                               where rowGrouping.Count() == 2                               // find rows where candidate appears exactly twice 
                               group rowGrouping by rowGrouping.Sum(a => 1 << a.Y) into columnArrangementGrouping   // form a signature for the column arrangement using bitwise method
                               where columnArrangementGrouping.Count() == 2                 // find any arrangements that appear twice
                               from rowGrouping in columnArrangementGrouping                // UnGroup
                               from cell in rowGrouping                                     // UnGroup 
                               group cell by cell.Y into hh                                 // Group by Column
                               select new
                               {
                                   Index = hh.Key,
                                   Cells = hh.ToList()
                               };


            foreach (var column in xwingColumns)
            {
                success += board.CellsInColumn(column.Index).RemoveCandidate(c, column.Cells);
            }

            return success;
        }





        private static int XWingByColumns(Board board, int c)
        {
            int success = 0;

            var xwingRows = from column in Enumerable.Range(0, 9)                           // for each column
                               from cell in board.CellsInColumn(column)                           // for each cell in column
                               from candidate in cell.Candidates                            // for each candidate in cell
                               where candidate == c                                         // where candidate is the value we're looking for
                               group cell by column into columnGrouping                           // group cell by column
                               where columnGrouping.Count() == 2                               // find columns where candidate appears exactly twice 
                               group columnGrouping by columnGrouping.Sum(a => 1 << a.X) into rowArrangementGrouping   // form a signature for the column arrangement using bitwise method
                               where rowArrangementGrouping.Count() == 2                 // find any arrangements that appear twice
                               from columnGrouping in rowArrangementGrouping                // UnGroup
                               from cell in columnGrouping                                     // UnGroup 
                               group cell by cell.X into hh                                 // Group by Column
                               select new
                               {
                                   Index = hh.Key,
                                   Cells = hh.ToList()
                               };


            foreach (var row in xwingRows)
            {
                success += board.CellsInRow(row.Index).RemoveCandidate(c, row.Cells);
            }

            return success;
        }
    }
}
