using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FiveSquared
{
    public class Slot
    {

        public Point StartPosition {get;set;}
        public Point EndPosition {get;set;}
        public PuzzlePiece CurrentPiece {get;set;}

        // Default constructor
        public Slot()
        {
        
        }


        // Constructor with coordinates for start and end row and col
        public Slot(Point startPosition, Point endPosition)
        {
            StartPosition = startPosition;

            EndPosition = endPosition;
        }

    }
}
