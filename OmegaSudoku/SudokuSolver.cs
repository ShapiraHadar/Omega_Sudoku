using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku
{
    public class SudokuSolver
    {
        private SudokuBoard _board;

        public void SetBoard(SudokuBoard board)
        {
            this._board = board;
        }
        public SudokuBoard GetBoard()
        {
            return this._board;
        }
        public SudokuSolver(SudokuBoard board)
        {
            this._board = board;
        }
        
        public SudokuBoard Solve()
        {
            
        }

    }
}
