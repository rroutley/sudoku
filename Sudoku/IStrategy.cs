using System;
namespace Sudoku
{
    public interface IStrategy
    {
        int Iterate(Board board);
    }
}
