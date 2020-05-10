using System;
using System.Drawing; 
using System.Collections.Generic;
using System.Linq;

namespace FiveSquared
{
    class Program
    {
        static void Main(string[] args)
        {
            Board brd = new Board();
            brd.CheckGridValidity();
            List<List<Slot>> Symmetrical = brd.SymmetricalPatterns();
            // foreach (List<Slot> pattern in Symmetrical) 
            // {
            //     brd.PrintPattern(pattern);
            // }
            
            brd.PopulateAllPatterns();
        }
    }
}
