using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Tests
{
    [TestClass]
    public class NakedPairTests
    {
        [TestMethod]
        public void NakedPairTest()
        {

            var row = new Cell[] {
                new Cell(1, 0) { Value = 5, Candidates = new HashSet<int>()},
                new Cell(1, 1) { Candidates = new HashSet<int>( new int [] {4,8 }) },                   
                new Cell(1, 2) { Candidates = new HashSet<int>( new int [] {4,8 }) },
                new Cell(1, 3) { Candidates = new HashSet<int>( new int [] { 4,7} ) },
                new Cell(1, 4) { Value=9, Candidates = new HashSet<int>() },
                new Cell(1, 5) { Value=2, Candidates = new HashSet<int>() },
                new Cell(1, 6) { Value=1, Candidates = new HashSet<int>() },
                new Cell(1, 7) { Candidates = new HashSet<int>(new int [] { 3,6,7 }) },                  
                new Cell(1, 8) { Candidates = new HashSet<int>(new int [] { 3,6,8 }) },
            };


            var target = new NakedCandidatesStrategy();
            var actual = target.NakedCandidates(row, 2);

            var first = actual.First();

            CollectionAssert.AreEquivalent(new int[] { 4, 8 }, first.Values.ToList());
            CollectionAssert.AreEquivalent(new Cell[] { new Cell(1, 1), new Cell(1, 2)}, first.Cells.ToList());
        }


        [TestMethod]
        public void NakedTripleTest() {

         var row = new Cell[] {
                new Cell(0, 2) { Candidates = new HashSet<int>( new int [] { 5,6,9 })},
                new Cell(1, 2) { Candidates = new HashSet<int>( new int [] { 2,3,5,6,8 }) },                   
                new Cell(2, 2) { Value=1, Candidates = new HashSet<int>( ) },
                new Cell(3, 2) { Candidates = new HashSet<int>( new int [] { 2,3,9} ) },
                new Cell(4, 2) { Candidates = new HashSet<int>( new int [] { 3,9 } ) },
                new Cell(5, 2) { Candidates = new HashSet<int>( new int [] { 2,3 } ) },
                new Cell(6, 2) { Candidates = new HashSet<int>( new int [] { 5,8} ) },
                new Cell(7, 2) { Value=7, Candidates = new HashSet<int>() },                  
                new Cell(8, 2) { Value=4,Candidates = new HashSet<int>() },
            };


            var target = new NakedCandidatesStrategy();
            var actual = target.NakedCandidates(row, 3);

            var first = actual.First();

            CollectionAssert.AreEquivalent(new int[] { 2, 3, 9 }, first.Values.ToList());
            CollectionAssert.AreEquivalent(new Cell[] { new Cell(3, 2), new Cell(4, 2), new Cell(5, 2)}, first.Cells.ToList());
        }
    }
}
