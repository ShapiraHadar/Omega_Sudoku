using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku
{
    class SudokuBoard
    {
        private int squareSize; //3 by default
        private int boardSize; //9 by default
        private int[,] board; // when getting input, if taking characters, convert to ints.

        public int GetSquareSize() {  return squareSize; }
        public int GetBoardSize() { return boardSize; }
        public int[,] GetBoard() { return board; }
        public SudokuBoard() : this(3) { } // default ctor - 9*9 empty board
        public SudokuBoard(int squareSize) // single int ctor - empty (squareSize^2)*(squareSize^2) board
        {
            this.squareSize = squareSize;
            this.boardSize = (int)Math.Pow(squareSize, 2);
            this.board = new int[boardSize,boardSize];
            for (int i = 0; i < this.boardSize; i++)
                for (int j = 0; j < this.boardSize; j++)
                    this.board[i, j] = 0;
        }
        public SudokuBoard(SudokuBoard board) // copy ctor
        {
            this.squareSize=board.GetSquareSize();
            this.boardSize=board.GetBoardSize();
            this.board = new int[this.boardSize, this.boardSize];
            for (int i = 0; i < this.boardSize; i++)
                for (int j = 0; j < this.boardSize; j++)
                    this.board[i,j] = board.GetBoard()[i,j];
        }
        public SudokuBoard(int[,] board) // filled board ctor
        {
            double size=Math.Sqrt(board.GetLength(0));
            if (size != (int)size)
                throw new Exception("Board size must be a perfect square");
            if (board.GetLength(0) != board.GetLength(1))
                throw new Exception("Board must be a square");
            this.squareSize = (int)size;
            this.boardSize = board.GetLength(0);
            this.board = new int[this.boardSize, this.boardSize];
            for (int i = 0;i < this.boardSize;i++)
                for (int j = 0;j < this.boardSize;j++)
                    this.board[i,j] = board[i,j];
        }
        public SudokuBoard(string s) // string ctor
        {
            double boardSize=Math.Sqrt(s.Length);
            double squareSize=Math.Sqrt(boardSize);
            if (squareSize != (int)squareSize)
                throw new Exception("Board size must be a perfect square");
            this.boardSize = (int)boardSize;
            this.squareSize= (int)squareSize;
            this.board = new int[this.boardSize, this.boardSize];
            for (int i = 0; i < this.boardSize; i++)
                for (int j = 0; j < this.boardSize; j++)
                    this.board[i, j] = s[i * this.boardSize + j] - '0'; // only for 9*9 currently, easy to modify
        }
        public bool CheckComplete()
        {
            if(!this.CheckFilled())
                return false;
            for (int i = 0; i < this.boardSize; i++)
                if(!this.CheckCol(i) || !this.CheckRow(i))
                    return false;
            for(int i=0;i<this.squareSize;i++)
                for(int j=0;j<this.squareSize;j++)
                    if(!CheckSquare(i,j))
                        return false;
            return true;
        }
        public bool CheckFilled()
        {
            for (int i = 0; i < this.boardSize; i++)
                for (int j = 0; j < this.boardSize; j++)
                    if (this.board[i, j] == 0) return false;
            return true;
        }
        public bool CheckRow(int row)
        {
            int temp = 0;
            int[] nums=new int[this.boardSize];
            if(row<0  || row>=this.boardSize) return false;
            for(int i = 0; i < this.boardSize; i++)
            {
                if (this.board[row, i] != 0)
                {
                    for (int j = 0; j < temp; j++)
                    {
                        if(nums[j]==this.board[row,i])
                            return false;
                    }
                    nums[temp] = this.board[row, i];
                    temp++;
                }
            }
            return true;
        }
        public bool CheckCol(int col)
        {
            int temp = 0;
            int[] nums = new int[this.boardSize];
            if (col < 0 || col >= this.boardSize) return false;
            for (int i = 0; i < this.boardSize; i++)
            {
                if (this.board[i, col] != 0)
                {
                    for (int j = 0; j < temp; j++)
                    {
                        if (nums[j] == this.board[i,col])
                            return false;
                    }
                    nums[temp] = this.board[i,col];
                    temp++;
                }
            }
            return true;
        }
        public bool CheckSquare(int squareRow, int squareCol)
        {
            int temp = 0;
            int[] nums = new int[this.boardSize];
            if (squareRow < 0 || squareCol < 0 || squareRow >= this.squareSize || squareCol >= this.squareSize)
                return false;
            for(int i = this.squareSize * squareRow;i < this.squareSize * (squareRow + 1); i++)
                for (int j = this.squareSize * squareCol; j < this.squareSize * (squareCol + 1); j++)
                {
                    for (int k = 0; k < temp; k++)
                    {
                        if (nums[k] == this.board[i,j])
                            return false;
                    }
                    nums[temp] = this.board[i,j];
                    temp++;
                }
            return true;
        }
        public override string ToString()
        {
            string result = "";
            string seperator = "";
            for (int i = 0; i < squareSize; i++)
            {
                seperator += "+";
                for (int j = 0; j < squareSize * 2 + 1; j++) seperator += "-";
            }
            seperator += "+\n";

            for (int i = 0; i < squareSize; i++)
            {
                result += seperator;
                for (int j = 0; j < squareSize; j++)
                {
                    string line = "";
                    for (int k = 0; k < squareSize; k++)
                    {
                        line += "| ";
                        for (int t = 0; t < squareSize; t++)
                        {
                            line += board[i * squareSize + j, k * squareSize + t];
                            line += " ";
                        }
                    }
                    line += "|\n";
                    result += line;
                }
            }
            result += seperator;
            return result;
        }
    }
    internal class Program
    {
        static void Main()
        {
            int[,] solvedBoard = new int[9, 9]
            {
                { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
                { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
                { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
                { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
                { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
                { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
                { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
                { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
                { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
            };
            SudokuBoard board = new SudokuBoard(solvedBoard);
            Console.WriteLine(board);
            Console.WriteLine(board.CheckComplete()?"Solved":"Not Solved");
        }
    }
}
