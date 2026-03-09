using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudoku;

[TestClass]
public class YWingTests
{
    [TestMethod]
    public void TestYWing()
    {
        var puzzle = """
        +-------------------+-------------------+-------------------+
        | 4     8     7     | 3     12    12    | 56    9     56    |
        | 59    359   39    | 6     48    48    | 2     7     1     |
        | 1     2     6     | 57    9     57    | 3     8     4     |
        |-------------------+-------------------+-------------------|
        | 7     34    5     | 89    348   489   | 1     6     2     |
        | 69    13469 349   | 2     1346  57    | 8     34    57    |
        | 28    1346  28    | 57    1346  14    | 57    34    9     |
        |-------------------+-------------------+-------------------|
        | 58    45    1     | 48    7     6     | 9     2     3     |
        | 3     67    89    | 1     28    289   | 4     5     67    |
        | 269   4679  249   | 49    5     3     | 67    1     8     |
        +-------------------+-------------------+-------------------+
        """;

        var lines = puzzle.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var x = from l in lines
                where !l.Contains('-')
                from cell in l.Replace('|', ' ').Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                select int.Parse(cell);

        var board = Board.Load(x.ToArray());

        var solver = new YWingStrategy();
        var result = solver.Iterate(board);
        Assert.AreEqual(2, result); // We should be able to eliminate 2 candidates.
    }
}