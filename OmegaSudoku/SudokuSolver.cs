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
        private ulong[] colsBitmap; //max 64*64
        private ulong[] rowsBitmap; //max 64*64
        private ulong[] sqrBitmap; //max 64*64
        private ulong FULL_MASK;


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
            this.colsBitmap = new ulong[board.GetBoardSize()];
            this.rowsBitmap = new ulong[board.GetBoardSize()];
            this.sqrBitmap = new ulong[board.GetBoardSize()];
        }

        public SudokuBoard Solve()
        {
            InitiateBitmaps();
            FULL_MASK = (1UL << _board.GetBoardSize()) - 1;

            if (!SolveRecursive())
                throw new Exception("No solution");

            return _board;
        }
        private bool SolveRecursive()
        {
            if (!FindBestCell(out int row, out int col, out ulong mask))
                return false; 

            if (row == -1)
                return true; 

            int boxSize = _board.GetSquareSize();
            int sqrIndex = (row / boxSize) * boxSize + (col / boxSize);

            while (mask != 0)
            {
                ulong bit = mask & (~mask + 1);
                mask -= bit;

                int value = BitToValue(bit);

                _board.GetBoard()[row, col] = value;
                rowsBitmap[row] |= bit;
                colsBitmap[col] |= bit;
                sqrBitmap[sqrIndex] |= bit;

                if (SolveRecursive())
                    return true;

                _board.GetBoard()[row, col] = 0;
                rowsBitmap[row] &= ~bit;
                colsBitmap[col] &= ~bit;
                sqrBitmap[sqrIndex] &= ~bit;
            }

            return false;
        }

        private int BitToValue(ulong bit)
        {
            int value = 1;
            while (bit > 1)
            {
                bit >>= 1;
                value++;
            }
            return value;
        }


        private ulong GetCandidatesMask(int row, int col) // returns the mask of the candidates for [row,col]
        {
            int boxSize = _board.GetSquareSize();
            int sqrIndex = (row / boxSize) * boxSize + (col / boxSize);
            return FULL_MASK & ~(rowsBitmap[row] | colsBitmap[col] | sqrBitmap[sqrIndex]);
        }
        private bool FindBestCell(out int row, out int col, out ulong bestMask)
        // returns false if there is a conflict(an empty cell with no candidates)
        // if the board is not full or conflicted, row, col and mask will represent the cell with minimum candidates.
        // if the board is full => row=col=-1
        {
            int n = this._board.GetBoardSize();
            row = -1;
            col = -1;
            bestMask = 0;
            int bestCount = int.MaxValue;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (_board.GetBoard()[i, j] != 0)
                        continue;
                    ulong mask = GetCandidatesMask(i, j);
                    int count = PopCount(mask); // how many candidates
                    if (count == 0)
                        return false; // conflict
                    if(count<bestCount)
                    {
                        bestMask = mask;
                        row = i;
                        col=j;
                        bestCount = count;
                        if (count == 1)
                            return true; // no need to continue scan - immediate find
                    }
                }
            }
            return true; // check for full board in calling method (row==-1)
        }
        private int PopCount(ulong x) // takes a bitmask x and returns how many bits are on
        {
            int count = 0;
            while (x != 0)
            {
                x &= x - 1;
                count++;
            }
            return count;
        }
        private void InitiateBitmaps() //initiating bitmask before scans and backtracking
        {
            int n = _board.GetBoardSize();
            int boxSize = _board.GetSquareSize();
            int[,] board = _board.GetBoard();

            for (int i = 0; i < n; i++)
            {
                rowsBitmap[i] = 0;
                colsBitmap[i] = 0;
                sqrBitmap[i] = 0;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int val = board[i, j];
                    if (val == 0)
                        continue;

                    if (val < 1 || val > n)
                        throw new Exception("Illegal value on board");

                    int bitIndex = val - 1;
                    ulong bit = 1UL << bitIndex;

                    int sqrIndex = (i / boxSize) * boxSize + (j / boxSize);

                    if ((rowsBitmap[i] & bit) != 0)
                        throw new Exception("Row conflict");

                    if ((colsBitmap[j] & bit) != 0)
                        throw new Exception("Column conflict");

                    if ((sqrBitmap[sqrIndex] & bit) != 0)
                        throw new Exception("Square conflict");

                    rowsBitmap[i] |= bit;
                    colsBitmap[j] |= bit;
                    sqrBitmap[sqrIndex] |= bit;
                }
            }
        }



    }
}
