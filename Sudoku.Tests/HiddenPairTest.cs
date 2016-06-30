using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sudoku.Tests
{
    [TestClass]
    public class HiddenPairTest
    {
        [TestMethod]
        public void HiddenPair_Success()
        {
            var row = new Cell[] {
                new Cell(0, 1) { Candidates = new HashSet<int>( new int [] { 4, 9 } ) },
                new Cell(1, 1) { Value = 1, Candidates = new HashSet<int>() },                   
                new Cell(2, 1) { Candidates = new HashSet<int>( new int [] { 2, 3, 4, 6, 9 } ) },
                new Cell(3, 1) { Candidates = new HashSet<int>( new int [] { 3, 9 } ) },
                new Cell(4, 1) { Candidates = new HashSet<int>( new int [] { 2, 4, 9 } ) },
                new Cell(5, 1) { Candidates = new HashSet<int>( new int [] { 2, 3, 4 } ) },
                new Cell(6, 1) { Candidates = new HashSet<int>( new int [] { 4, 6, 7, 8 } ) },
                new Cell(7, 1) { Value = 5, Candidates = new HashSet<int>() },                  
                new Cell(8, 1) { Candidates = new HashSet<int>( new int [] { 2, 7, 8 } ) },
            };


            var target = new HiddenCandidatesStrategy(2);
            var actual = target.HiddenCandidates(row, 2);

            var first = actual.First();

            CollectionAssert.AreEquivalent(new Cell[] { new Cell(6, 1), new Cell(8, 1) }, first.Cells.ToList());
            CollectionAssert.AreEquivalent(new int[] { 7, 8 }, first.Values.ToList());

        }

        [TestMethod]
        public void HiddenTriple_Success()
        {
            var row = new Cell[] {
                new Cell(0, 6) { Value = 5, Candidates = new HashSet<int>() },
                new Cell(1, 6) { Candidates = new HashSet<int>( new int [] { 1, 2, 4, 6 }) },                   
                new Cell(2, 6) { Candidates = new HashSet<int>( new int [] { 1, 4 } ) },
                new Cell(3, 6) { Candidates = new HashSet<int>( new int [] { 2, 6, 7 } ) },
                new Cell(4, 6) { Candidates = new HashSet<int>( new int [] { 2, 4, 6, 7, 9 } ) },
                new Cell(5, 6) { Candidates = new HashSet<int>( new int [] { 3, 9 } ) },
                new Cell(6, 6) { Candidates = new HashSet<int>( new int [] { 1, 3, 9 } ) },
                new Cell(7, 6) { Candidates = new HashSet<int>( new int [] { 1, 3, 8, 9 } ) },                  
                new Cell(8, 6) { Candidates = new HashSet<int>( new int [] { 3, 8 } ) },
            };


            var target = new HiddenCandidatesStrategy(3);
            var actual = target.HiddenCandidates(row, 3);

            var first = actual.First();

            CollectionAssert.AreEquivalent(new int[] { 2, 6, 7 }, first.Values.ToList());
            CollectionAssert.AreEquivalent(new Cell[] { new Cell(1, 6), new Cell(3, 6), new Cell(4, 6) }, first.Cells.ToList());

        }

        [TestMethod]
        public void HiddenQuad_Success()
        {
            var row = new Cell[] {
                new Cell(0, 3) { Candidates = new HashSet<int>( new int [] { 1,3,4,6 })},
                new Cell(1, 3) { Candidates = new HashSet<int>( new int [] { 1,4,8 }) },                   
                new Cell(2, 3) { Candidates = new HashSet<int>( new int [] { 1,6 } ) },
                new Cell(3, 3) { Candidates = new HashSet<int>( new int [] { 3,4,5,6,9 } ) },
                new Cell(4, 3) { Candidates = new HashSet<int>( new int [] { 2,3,4,5,6,7,9 } ) },
                new Cell(5, 3) { Candidates = new HashSet<int>( new int [] { 2,3,4,6,7,9 } ) },
                new Cell(6, 3) { Candidates = new HashSet<int>( new int [] { 1,3,4,8 } ) },
                new Cell(7, 3) { Candidates = new HashSet<int>( new int [] { 1,3,4 } ) },                  
                new Cell(8, 3) { Candidates = new HashSet<int>( new int [] { 2,5 } ) },
            };


            var target = new HiddenCandidatesStrategy(4);
            var actual = target.HiddenCandidates(row, 4);

            var first = actual.First();

            CollectionAssert.AreEquivalent(new int[] { 2, 5, 7, 9 }, first.Values.ToList());
            CollectionAssert.AreEquivalent(new Cell[] { new Cell(3, 3), new Cell(4, 3), new Cell(5, 3), new Cell(8, 3) }, first.Cells.ToList());

        }

    }
}
