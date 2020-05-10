using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Drawing;
using System.IO; 

namespace FiveSquared
{
    public class Board
    {

        public char[,] Grid { get; set; } = new char[5, 5];       // 2 dimensional area to hold board at any given time
        public List<Slot> Pattern { get; set; }
        
        public List<PuzzlePiece> Pieces {get;set;}                // collection of slots representing pattern
        public List<Slot> KnownPattern { get; set; }              // collection of slots representing pattern 
        public List<List<Slot>> AllPatterns { get; set; }         // collection of all valid patterns 
        public int PatternCheckCount; 
        public List<Slot> PossibleTwoSlots;
        public List<Slot> PossibleThreeSlots;
        public List<List<Slot>> TwoCombinations;
        public List<List<Slot>> ThreeCombinations;

        public List<int[]> BinaryList; 

        // Default constructor
        public Board()
        {
            KnownPattern = InitializePattern();
            AllPatterns = InitializePatterns();
            PatternCheckCount = 0; 
            PossibleTwoSlots = new List<Slot>();
            PossibleThreeSlots = new List<Slot>(); 
            TwoCombinations = new List<List<Slot>>(); 
            ThreeCombinations = new List<List<Slot>>();
            BinaryList = new List<int[]>();
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    Grid[i, j] = '-';
                }
            }
            Pattern = InitializePattern();
            Pieces = InitializePieces();
        }


        // Constructor with length and symbols
        public Board(List<Slot> pattern)
        {
            KnownPattern = pattern;
        }

        public void Clear()
        {

        }

        public List<PuzzlePiece> InitializePieces()
        {
            List<PuzzlePiece> Pieces = new List<PuzzlePiece>();

            Pieces.Add(new PuzzlePiece(2, new List<char> { 'T', 'S' }));
            Pieces.Add(new PuzzlePiece(2, new List<char> { 'C', 'T' }));
            Pieces.Add(new PuzzlePiece(3, new List<char> { 'P', 'T', 'Q' }));
            Pieces.Add(new PuzzlePiece(3, new List<char> { 'S', 'P', 'Q' }));
            Pieces.Add(new PuzzlePiece(3, new List<char> { 'Q', 'S', 'P' }));
            Pieces.Add(new PuzzlePiece(3, new List<char> { 'C', 'P', 'T' }));
            Pieces.Add(new PuzzlePiece(3, new List<char> { 'T', 'C', 'S' }));
            Pieces.Add(new PuzzlePiece(3, new List<char> { 'C', 'P', 'Q' }));
            Pieces.Add(new PuzzlePiece(3, new List<char> { 'S', 'Q', 'C' }));

            return Pieces;
        }

        public List<Slot> InitializePattern()
        {
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
    
            return Pattern;

        }

        public void PrintMatrix<T>(T[,] Matrix) 
        {
            Console.WriteLine();
            for (int i = 0; i < Matrix.GetLength(0); i++) 
            {               
                for (int j = 0; j < Matrix.GetLength(1); j++) 
                {
                    Console.Write(Matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public void PrintPattern(List<Slot> pattern) 
        {   
            PrintMatrix(PatternToGrid(pattern)); 
        }
        
        public bool CheckGridValidity()
        {
            for (int i = 0; i < Grid.GetLength(0); i++) 
            {
                List<char> row = new List<char>();
                for (int j = 0; j < Grid.GetLength(1); j++) 
                {
                    char currSymbol = Grid[i, j];
                    if (row.Contains(currSymbol)) {
                        return false; 
                    } else if (currSymbol != '-') {
                        row.Add(currSymbol);
                    }
                }
            }

            for (int i = 0; i < Grid.GetLength(1); i++) 
            {
                List<char> col = new List<char>();
                for (int j = 0; j < Grid.GetLength(0); j++) 
                {
                    char currSymbol = Grid[j, i];
                    if (col.Contains(currSymbol)) {
                        return false; 
                    } else if (currSymbol != '-') {
                        col.Add(currSymbol);
                    }
                }
            }

            return true;
        }

        public bool CheckPatternValidity(List<Slot> pattern)
        {
            PatternCheckCount++; 
            List<Point> Points = new List<Point>(); 
            foreach (Slot s in pattern)
            {
                Point Start = s.StartPosition;
                Point End = s.EndPosition;
                if (Start.X == End.X) 
                {
                    for (int i = Start.Y; i <= End.Y; i++) 
                    {
                        Point AddPoint = new Point(Start.X, i);
                        if (Points.Contains(AddPoint)) return false; 
                        Points.Add(new Point(Start.X, i));                
                    }
                } else 
                {
                    for (int i = Start.X; i <= End.X; i++) 
                    {
                        Point AddPoint = new Point(i, Start.Y);
                        if (Points.Contains(AddPoint)) return false; 
                        Points.Add(new Point(i, Start.Y));
                    }
                } 
            }
            return true; 
        }

        public void InitializeSlotPossibilities() 
        {
            for (int i = 0; i < Grid.GetLength(0); i++) 
            {
                for (int j = 0; j < Grid.GetLength(1) - 1; j++) 
                {
                    PossibleTwoSlots.Add(new Slot(new Point(i, j), new Point(i, j + 1)));
                    if (!((j + 2) == Grid.GetLength(1)))
                    {
                        PossibleThreeSlots.Add(new Slot(new Point(i, j), new Point(i, j + 2)));
                    }
                }
            }

            for (int i = 0; i < Grid.GetLength(1); i++) 
            {
                for (int j = 0; j < Grid.GetLength(0) - 1; j++) 
                {   
                    PossibleTwoSlots.Add(new Slot(new Point(j, i), new Point(j + 1, i)));
                    if (!((j + 2) == Grid.GetLength(0)))
                    {
                        PossibleThreeSlots.Add(new Slot(new Point(j, i), new Point(j + 2, i)));
                    }
                }
            }
        } 

        public void GenerateSlotCombinations() 
        {
            Combinations Combo = new Combinations(); 
            TwoCombinations = Combo.FindCombinations(PossibleTwoSlots, 2);
            ThreeCombinations = Combo.FindCombinations(PossibleThreeSlots, 7);
        }

        public void GeneratePatterns()
        {
            StreamWriter PatternFile; 
            PatternFile = new StreamWriter("Pattern.txt", false);
            List<List<Slot>> FoundPatterns = new List<List<Slot>>(); 
            foreach (List<Slot> slots1 in TwoCombinations)
            {
                foreach (List<Slot> slots2 in ThreeCombinations)
                {
                    List<Slot> PotentialPattern = slots1.Concat(slots2).ToList();
                    if (CheckPatternValidity(PotentialPattern))
                    {
                        if(IsUniquePattern(PotentialPattern, FoundPatterns)) 
                        {
                            FoundPatterns.Add(PotentialPattern);
                            SavePatternFile(PatternFile, PotentialPattern);
                        }
                    }                        
                }
            }
            PatternFile.Close(); 
        }

        public bool IsUniquePattern(List<Slot> potentialPattern, List<List<Slot>> foundPatterns) {
            List<Slot> Rotated90 = RotateSlots90(potentialPattern);
            List<Slot> Rotated180 = RotateSlots90(Rotated90);
            List<Slot> Rotated270 = RotateSlots90(Rotated180);

            foreach (List<Slot> pattern in foundPatterns)
            {
                if (PatternEquals(potentialPattern, pattern) || PatternEquals(Rotated90, pattern) ||
                    PatternEquals(Rotated180, pattern) || PatternEquals(Rotated270, pattern))
                {
                    return false; 
                }
            }

            return true; 
        }

        public List<Slot> RotateSlots90(List<Slot> potentialPattern)
        {
            List<Slot> NewSlots = new List<Slot>();
            foreach(Slot s in potentialPattern)
            {
                Point Start = s.StartPosition;
                Point End = s.EndPosition; 
                Point NewStart = new Point((4 - Start.Y), Start.X);
                Point NewEnd = new Point((4 - End.Y), End.X);
                NewSlots.Add(new Slot(NewStart, NewEnd));
            }
            return NewSlots; 
        }

        public bool PatternEquals(List<Slot> pattern1, List<Slot> pattern2)
        {
            foreach (Slot s in pattern1)
            {
                bool Contains = false; 
                Point Start1 = s.StartPosition;
                Point End1 = s.EndPosition;
                foreach (Slot s2 in pattern2)
                {
                    Point Start2 = s2.StartPosition; 
                    Point End2 = s2.EndPosition; 
                    if ((Start1 == Start2 && End1 == End2) || (Start1 == End2 && End1 == Start2))
                    {
                        Contains = true; 
                    }
                }

                if (Contains == false)
                {
                    return false; 
                }
            }
            return true; 
        }

        public int[,] PatternToGrid(List<Slot> pattern) 
        {
            int[,] PatternGrid = new int[5, 5];
            foreach (Slot s in pattern)
            {
                Point Start = s.StartPosition;
                Point End = s.EndPosition;
                int Insert = pattern.IndexOf(s);
                if (Start.X == End.X) 
                {
                    if (Start.Y > End.Y) 
                    {
                        for (int i = End.Y; i <= Start.Y; i++) 
                        {
                            PatternGrid[Start.X, i] = Insert;                       
                        }
                    } else {
                        for (int i = Start.Y; i <= End.Y; i++) 
                        {
                            PatternGrid[Start.X, i] = Insert;                       
                        }
                    }
                } else 
                {
                    if (Start.X > End.X) {
                        for (int i = End.X; i <= Start.X; i++) 
                        {
                            PatternGrid[i, Start.Y] = Insert;
                        }
                    } else 
                    {
                        for (int i = Start.X; i <= End.X; i++) 
                        {
                            PatternGrid[i, Start.Y] = Insert;
                        }
                    }
                } 
            } 
            return PatternGrid; 
        }

        public void PlacePiece(Slot slot, PuzzlePiece piece)
        {
            if(piece.Symbols.ToArray().Length < 3)
            {
                Grid[(slot.StartPosition.X), (slot.StartPosition.Y)] = piece.Symbols[0];
                Grid[slot.EndPosition.X, slot.EndPosition.Y] = piece.Symbols[1];
            }else
            {
                Grid[(slot.StartPosition.X), (slot.StartPosition.Y)] = piece.Symbols[0];
                Grid[((slot.EndPosition.X+slot.StartPosition.X)/2), ((slot.EndPosition.Y+slot.StartPosition.Y)/2)] = piece.Symbols[1];
                Grid[slot.EndPosition.X, slot.EndPosition.Y] = piece.Symbols[2];                
            }
        }

        public void PopulatePattern(List<Slot> pattern, int patternCount)
        {
            Combinations Combo = new Combinations();
            List<PuzzlePiece> twoPieces = Pieces.GetRange(0,2);
            List<PuzzlePiece> threePieces = Pieces.GetRange(2,7);
            
            List<List<PuzzlePiece>> resultsThree = Combo.GeneratePermutations<PuzzlePiece>(threePieces);
            List<List<PuzzlePiece>> resultsTwo = Combo.GeneratePermutations<PuzzlePiece>(twoPieces);


            foreach(List<PuzzlePiece> twoPiece in resultsTwo)
            {
                List<List<PuzzlePiece>> twoMirrorPieces = generateMirrorPermuations(twoPiece);

                foreach (List<PuzzlePiece> mirrorPerm in twoMirrorPieces)
                {
                    for (int i = 0; i < mirrorPerm.Count; i++)
                    {
                        PlacePiece(pattern[i], mirrorPerm[i]);                      
                    }
                    foreach (List<PuzzlePiece> threePiece in resultsThree)
                    {
                        List<List<PuzzlePiece>> threeMirrorPieces = generateMirrorPermuations(threePiece);
                        foreach(List<PuzzlePiece> mirrorPermThree in threeMirrorPieces)
                        {
                            for (int j = 0; j < mirrorPermThree.Count; j++)
                            {
                                PlacePiece(pattern[j+2], mirrorPermThree[j]);
                            }

                            if(CheckGridValidity())
                            {
                                Console.WriteLine("-----------------------------------------------------------");
                                Console.WriteLine("Valid Solution for Pattern #" + patternCount);
                                PrintMatrix(Grid);
                                PrintPattern(pattern);
                            } 
                        }     
                    }    
                } 
            }
        }


        public List<List<Slot>> InitializePatterns()
        {
            StreamReader PatternFile = new StreamReader("Pattern.txt");
            String line; 
            List<List<Slot>> Patterns = new List<List<Slot>>(); 
            List<Slot> Additions = new List<Slot>(); 
            while((line = PatternFile.ReadLine()) != null)
            {
                if(line.Contains(","))
                {
                    line = line.Replace(",", "");
                    line = line.Replace(" ", "");
                    int StartX = Int32.Parse(line[0].ToString());
                    int StartY = Int32.Parse(line[1].ToString());
                    int EndX = Int32.Parse(line[2].ToString());
                    int EndY = Int32.Parse(line[3].ToString());
                    Additions.Add(new Slot(new Point(StartX, StartY), new Point(EndX, EndY)));
                } else {
                    Patterns.Add(Additions);
                    Additions = new List<Slot>();
                }
            }
            return Patterns; 
        } 

        public List<List<Slot>> SymmetricalPatterns()
        {
            List<List<Slot>> Symmetrical = new List<List<Slot>>();
            foreach (List<Slot> slots in AllPatterns)
            {
                List<Slot> Rotated90 = RotateSlots90(slots);
                List<Slot> Rotated180 = RotateSlots90(Rotated90);
                if (PatternEquals(Rotated180, slots))
                {
                    Symmetrical.Add(slots);
                }

            }
            return Symmetrical;
        }
        public void SavePatternFile(StreamWriter patternFile, List<Slot> pattern)
        {
            foreach (Slot s in pattern)
            {
                Point Start = s.StartPosition; 
                Point End = s.EndPosition; 
                patternFile.WriteLine(Start.X + ", " + Start.Y + ", " + End.X + ", " + End.Y);
            }
            patternFile.WriteLine();
        }

        public List<List<PuzzlePiece>> generateMirrorPermuations(List<PuzzlePiece> Permutation)
        {
            List<List<PuzzlePiece>> piecesMirrored = new List<List<PuzzlePiece>>();
            int PermLength = Permutation.Count;

            if(PermLength == 2)
            {
                for (uint bitmap = 0b0; bitmap <= 0b11; ++bitmap)
                {
                    List<PuzzlePiece> tempList = new List<PuzzlePiece>();
                    if ((bitmap & 0b01) == 0b01) tempList.Insert(0, (PuzzlePiece.MirrorPiece(Permutation[0]))); else tempList.Insert(0, Permutation[0]);
                    if ((bitmap & 0b10) == 0b10) tempList.Insert(1, (PuzzlePiece.MirrorPiece(Permutation[1]))); else tempList.Insert(1, Permutation[1]);

                    piecesMirrored.Add(tempList);
                }                
            }
            if(PermLength == 7)
            {
                for (uint bitmap = 0b0; bitmap <= 0b1111111; ++bitmap)
                {
                    List<PuzzlePiece> tempList = new List<PuzzlePiece>();
                    if ((bitmap & 0b0000001) == 0b0000001) tempList.Insert(0, (PuzzlePiece.MirrorPiece(Permutation[0]))); else tempList.Insert(0, Permutation[0]);
                    if ((bitmap & 0b0000010) == 0b0000010) tempList.Insert(1, (PuzzlePiece.MirrorPiece(Permutation[1]))); else tempList.Insert(1, Permutation[1]);
                    if ((bitmap & 0b0000100) == 0b0000100) tempList.Insert(2, (PuzzlePiece.MirrorPiece(Permutation[2]))); else tempList.Insert(2, Permutation[2]);
                    if ((bitmap & 0b0001000) == 0b0001000) tempList.Insert(3, (PuzzlePiece.MirrorPiece(Permutation[3]))); else tempList.Insert(3, Permutation[3]);
                    if ((bitmap & 0b0010000) == 0b0010000) tempList.Insert(4, (PuzzlePiece.MirrorPiece(Permutation[4]))); else tempList.Insert(4, Permutation[4]);
                    if ((bitmap & 0b0100000) == 0b0100000) tempList.Insert(5, (PuzzlePiece.MirrorPiece(Permutation[5]))); else tempList.Insert(5, Permutation[5]);
                    if ((bitmap & 0b1000000) == 0b1000000) tempList.Insert(6, (PuzzlePiece.MirrorPiece(Permutation[6]))); else tempList.Insert(6, Permutation[6]);

                    piecesMirrored.Add(tempList);
                }  
            }
            return piecesMirrored;
        }


        public void PopulateAllPatterns()
        {
            int patternCount = 0;
            foreach (List<Slot> pattern in AllPatterns)
            {
                PopulatePattern(pattern, patternCount);
                patternCount++;
                
            }
        }
    }
}
