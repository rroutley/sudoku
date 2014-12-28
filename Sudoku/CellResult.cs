using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class CellResult
    {
        public IEnumerable<int> Values { get; set; }
        public IEnumerable<Cell> Cells { get; set; }
    }
}
