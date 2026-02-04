using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OmegaSudoku
{
    internal class Testing
    {
        public static bool TestBoardBuilding()
        {
            foreach (string line in File.ReadLines("..\\..\\sudoku-3m.csv"))
            {
                string row = line.Replace(".", "0");
                SudokuBoard board = new SudokuBoard(row);
                if (board.FormatString() != row)
                {
                    Console.WriteLine($"TestBoardBuilding failed:\n\tinput: {row}\n\toutput: {board.FormatString()}\n\texpected:{row}");
                    return false;
                }
            }
            return true;
        }
        public static bool TestSolving(int n) // 0<n<1000000
        {
            foreach (string line in File.ReadLines("..\\..\\sudoku-3m.csv"))
            {
                string row = line.Replace(".", "0");
                SudokuBoard board = new SudokuBoard(row);
                SudokuSolver solver = new SudokuSolver(board);
                SudokuBoard solved = solver.Solve();
                if (!solved.CheckComplete())
                {
                    Console.WriteLine($"TestSolving failed:\n\tinput: {row}\n\toutput: {solved}\n\t");
                    return false;
                }
            }
            return true;
        }
        public static bool TestSolvingTime(int n) // 0<n<1000000
        {

            foreach (string line in File.ReadLines("..\\..\\sudoku-3m.csv"))
            {
                string row = line.Replace(".", "0");
                SudokuBoard board = new SudokuBoard(row);
                SudokuSolver solver = new SudokuSolver(board);
                DateTime time = DateTime.Now;
                SudokuBoard solved = solver.Solve();
                TimeSpan solveTime= DateTime.Now - time;
                if (solveTime > TimeSpan.FromMilliseconds(1000))
                {
                    Console.WriteLine($"TestSolvingTime failed:\n\tinput: {row}\n\tsolving took: {solveTime.TotalMilliseconds}");
                    return false;
                }
            }
            return true;
        }
    }
}
