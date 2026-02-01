using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OmegaSudoku
{
    internal class Program
    {
        public static void MainLoop()
        {
            while (true)
            {
                string str = "";
                try
                {
                    Console.WriteLine("Enter formatted Sudoku to solve:");
                    str = Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("failed to read input! try again");
                }

                SudokuBoard board;
                try
                {
                    if (str.Length != 81)
                        throw new Exception("input board must be 81 digits long");
                    for (int i = 0; i < str.Length; i++)
                        if (!('0' <= str[i] && '9' >= str[i]))
                        {
                            throw new Exception("input board contains illegal characters! board must only contain digits between 0-9");
                        }
                    board = new SudokuBoard(str);
                    SudokuSolver solver = new SudokuSolver(board);
                    DateTime start = DateTime.Now;
                    SudokuBoard solved = solver.Solve();
                    TimeSpan solveTime = DateTime.Now - start;
                    Console.WriteLine(solved);
                    Console.WriteLine($"Solved in {solveTime.TotalMilliseconds}ms");
                    Console.WriteLine(solved.FormatString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void Main()
        {
             MainLoop();
            ///todo:
            ///seperate MainLoop into single-responsibillity functions
        }
    }
}
