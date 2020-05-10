using System;
using System.Collections.Generic;
using System.Text;

namespace FiveSquared
{
    public class PuzzlePiece
    {

        public int Length { get; set; }             // always 2 or 3
        public List<char> Symbols { get; set; }     // characters represent symbols

        public bool Placed {get;set;}
        // Default constructor
        public PuzzlePiece()
        {

        }


        // Constructor with length and symbols
        public PuzzlePiece(int length, List<char> symbols)
        {
            Length = length;
            Symbols = symbols;
            Placed = false;
        }

        public static PuzzlePiece MirrorPiece(PuzzlePiece piece)
        {
            PuzzlePiece ReversePiece = new PuzzlePiece();
            ReversePiece.Length = piece.Length;
            ReversePiece.Placed = piece.Placed;
            ReversePiece.Symbols = new List<char>();
            foreach (char ch in piece.Symbols) ReversePiece.Symbols.Add(ch);
            ReversePiece.Symbols.Reverse();
            return ReversePiece;
        }


       
    }
}
