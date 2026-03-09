using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudoku;

[TestClass]
public class SolverTests
{
    [TestMethod]
    [DynamicData(nameof(Top95Data))]
    public void Top95(int number, string puzzle)
    {

        Board b = Board.Load(puzzle);
        var solver = new Solver();
        var success = solver.Execute(b);

        Assert.IsTrue(success, $"Solver failed to solve the puzzle {number}:{puzzle}");
    }

    internal static IEnumerable<(int Number, string Puzzle)> Top95Data()
    {
        var lines = File.ReadAllLines(@"..\\..\\..\\top95.txt");

        int number = 0;
        foreach (var puzzle in lines)
        {
            number++;
            yield return (number, puzzle);
        }
    }


    [TestMethod]
    [DynamicData(nameof(ProjectEuler96Data))]
    public void ProjectEuler_96(int number, string puzzle)
    {
        var solver = new Solver();
        var board = Board.Load(puzzle);
        var result = solver.Execute(board);

        Assert.IsTrue(result, $"Solver failed to solve the puzzle {number}");
    }

    internal static IEnumerable<(int Number, string Puzzle)> ProjectEuler96Data()
    {
        var lines = File.ReadAllLines(@"..\\..\\..\\p096_sudoku.txt");

        int line = 0;
        for (int number = 1; number <= 50; number++)
        {
            var gridName = lines[line++];
            if (gridName != $"Grid {number:00}") throw new InvalidOperationException();

            var puzzle = "";
            for (int i = 0; i < 9; i++, line++)
            {
                puzzle += lines[line];
            }

            yield return (number, puzzle);
        }
    }

 
}