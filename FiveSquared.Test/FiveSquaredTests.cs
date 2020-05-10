using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing; 

namespace FiveSquared.Test
{
    [TestClass]
    public class FiveSquaredTests
    {
        [TestMethod]
        public void TestPiece()
        {
            PuzzlePiece Piece1 = new PuzzlePiece(3, new List<char> { 'S', 'C', 'T' });
            Console.WriteLine($"Piece length is {Piece1.Length}");
            Assert.IsTrue(Piece1.Length == 3);            
        }

        [TestMethod]
        public void TestPattern()
        {
            Board brd = new Board();
            List<Slot> Pattern = brd.InitializePattern();
            Assert.IsTrue(Pattern.Count == 9);                      
        }

        public void SetGridRow(Board brd, int startCol, char letter)
        {
            int row = 0;
            int col = startCol;
            for (int i = startCol; i < brd.Grid.GetLength(0) + startCol; ++i)
            {
                if (i == 0) col = 0; else col = i % 5;
                brd.Grid[col, row] = letter;
                ++row;
            }
        }

        [TestMethod]
        public void TestBoardValidity()
        {
            Board brd = new Board();

            SetGridRow(brd, 0, 'Q');            
            SetGridRow(brd, 1, 'S');            
            SetGridRow(brd, 2, 'C');         
            SetGridRow(brd, 3, 'T');         
            SetGridRow(brd, 4, 'P');     

            brd.PrintMatrix(brd.Grid);
            Assert.IsTrue(brd.CheckGridValidity());    

            SetGridRow(brd, 0, 'P');
            brd.PrintMatrix(brd.Grid);
            Assert.IsFalse(brd.CheckGridValidity()); 
        }

        [TestMethod]
        public void TestPatternValidity()
        {
            Board brd = new Board();
            List<Slot> Pattern = new List<Slot>();

            Pattern.Add(new Slot(new Point(0, 3), new Point(0, 4)));
            Pattern.Add(new Slot(new Point(4, 0), new Point(4, 1)));
            Pattern.Add(new Slot(new Point(0, 0), new Point(0, 2)));
            Pattern.Add(new Slot(new Point(1, 0), new Point(3, 0)));
            Pattern.Add(new Slot(new Point(1, 1), new Point(1, 3)));
            Pattern.Add(new Slot(new Point(2, 1), new Point(2, 3)));
            Pattern.Add(new Slot(new Point(3, 1), new Point(3, 3)));
            Pattern.Add(new Slot(new Point(1, 4), new Point(3, 4)));
            Pattern.Add(new Slot(new Point(4, 2), new Point(4, 4)));

            Assert.IsTrue(brd.CheckPatternValidity(Pattern));

            Pattern[0] = new Slot(new Point(1,0), new Point(4, 4));
            Assert.IsFalse(brd.CheckPatternValidity(Pattern));

        }

        [TestMethod]
        public void TestCombinations()
        {
            List<int> Nums = new List<int>() {1, 2, 3, 4}; 
            int k = 3; 
            Combinations Combo = new Combinations();
            List<List<int>> NumCombos = Combo.FindCombinations(Nums, k);
            
            foreach (List<int> list in NumCombos)
            {
                Console.WriteLine();
                for (int i = 0; i < list.Count; i++)
                {
                    Console.Write(list[i] + " ");
                }
            }

            Assert.IsTrue(NumCombos[0].Contains(1));
        }

        [TestMethod]
        public void TestPatternEquals()
        {
            Board brd = new Board();

            List<Slot> Pattern1 = new List<Slot>(); 
            Pattern1.Add(new Slot(new Point(0, 0), new Point(0, 2)));
            Pattern1.Add(new Slot(new Point(1, 0), new Point(1, 2)));
            Pattern1.Add(new Slot(new Point(2, 0), new Point(2, 2)));
            Pattern1.Add(new Slot(new Point(3, 0), new Point(3, 1)));
            Pattern1.Add(new Slot(new Point(4, 0), new Point(4, 1)));
            Pattern1.Add(new Slot(new Point(0, 3), new Point(2, 3)));
            Pattern1.Add(new Slot(new Point(0, 4), new Point(2, 4)));
            Pattern1.Add(new Slot(new Point(3, 2), new Point(3, 4)));
            Pattern1.Add(new Slot(new Point(4, 2), new Point(4, 4)));

            List<Slot> Pattern2 = new List<Slot>(); 
            Pattern2.Add(new Slot(new Point(4, 0), new Point(2, 0)));
            Pattern2.Add(new Slot(new Point(4, 1), new Point(2, 1)));
            Pattern2.Add(new Slot(new Point(4, 2), new Point(2, 2)));
            Pattern2.Add(new Slot(new Point(4, 3), new Point(3, 3)));
            Pattern2.Add(new Slot(new Point(4, 4), new Point(3, 4)));
            Pattern2.Add(new Slot(new Point(1, 0), new Point(1, 2)));
            Pattern2.Add(new Slot(new Point(0, 0), new Point(0, 2)));
            Pattern2.Add(new Slot(new Point(2, 3), new Point(0, 3)));
            Pattern2.Add(new Slot(new Point(2, 4), new Point(0, 4)));

            Assert.IsFalse(brd.PatternEquals(Pattern1, Pattern2));

            Pattern1 = brd.RotateSlots90(Pattern1);
            Assert.IsTrue(brd.PatternEquals(Pattern1, Pattern2));
        }
    }
}
