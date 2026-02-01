using System;

public class SudokuBoard
{
    private int _squareSize; //3 by default
    private int _boardSize; //9 by default
    private int[,] _board; // when getting input, if taking characters, convert to ints.

    public int GetSquareSize() { return _squareSize; }
    public int GetBoardSize() { return _boardSize; }
    public int[,] GetBoard() { return _board; }
    public SudokuBoard() : this(3) { } // default ctor - 9*9 empty board
    public SudokuBoard(int squareSize) // single int ctor - empty (squareSize^2)*(squareSize^2) board
    {
        this._squareSize = squareSize;
        this._boardSize = (int)Math.Pow(squareSize, 2);
        this._board = new int[this._boardSize, this._boardSize];
        for (int i = 0; i < this._boardSize; i++)
            for (int j = 0; j < this._boardSize; j++)
                this._board[i, j] = 0;
    }
    public SudokuBoard(SudokuBoard board) // copy ctor
    {
        this._squareSize = board.GetSquareSize();
        this._boardSize = board.GetBoardSize();
        this._board = new int[this._boardSize, this._boardSize];
        for (int i = 0; i < this._boardSize; i++)
            for (int j = 0; j < this._boardSize; j++)
                this._board[i, j] = board.GetBoard()[i, j];
    }
    public SudokuBoard(int[,] board) // filled board ctor
    {
        double size = Math.Sqrt(board.GetLength(0));
        if (size != (int)size)
            throw new Exception($"Board size must be a perfect square {board.GetLength(0)}");
        if (board.GetLength(0) != board.GetLength(1))
            throw new Exception("Board must be a square");
        this._squareSize = (int)size;
        this._boardSize = board.GetLength(0);
        this._board = new int[this._boardSize, this._boardSize];
        for (int i = 0; i < this._boardSize; i++)
            for (int j = 0; j < this._boardSize; j++)
                this._board[i, j] = board[i, j];
    }
    public SudokuBoard(string s) // string ctor
    {
        double boardSize = Math.Sqrt(s.Length);
        double squareSize = Math.Sqrt(boardSize);
        if (squareSize != (int)squareSize)
            throw new Exception($"Board size must be a perfect square {s.Length}");
        this._boardSize = (int)boardSize;
        this._squareSize = (int)squareSize;
        this._board = new int[this._boardSize, this._boardSize];
        for (int i = 0; i < this._boardSize; i++)
            for (int j = 0; j < this._boardSize; j++)
            {
                if ('0' <= s[i * this._boardSize + j] && s[i * this._boardSize + j] <= '9')
                {
                    this._board[i, j] = s[i * this._boardSize + j] - '0'; // for digs between 0-9
                }
                else if ('A' <= s[i * this._boardSize + j] && s[i * this._boardSize + j] <= 'Z')
                {
                    this._board[i, j] = s[i * this._boardSize + j] - 'A' + 10; // letters represent numbers between 10-26
                }
            }
    }
    public bool CheckComplete()
    {
        if (!this.CheckFilled())
            return false;
        for (int i = 0; i < this._boardSize; i++)
            if (!this.CheckCol(i) || !this.CheckRow(i))
                return false;
        for (int i = 0; i < this._squareSize; i++)
            for (int j = 0; j < this._squareSize; j++)
                if (!CheckSquare(i, j))
                    return false;
        return true;
    } // function that checks if a board is completed correctly
    private bool CheckFilled()
    {
        for (int i = 0; i < this._boardSize; i++)
            for (int j = 0; j < this._boardSize; j++)
                if (this._board[i, j] == 0) return false;
        return true;
    }
    private bool CheckRow(int row)
    {
        int temp = 0;
        int[] nums = new int[this._boardSize];
        if (row < 0 || row >= this._boardSize) return false;
        for (int i = 0; i < this._boardSize; i++)
        {
            if (this._board[row, i] != 0)
            {
                for (int j = 0; j < temp; j++)
                {
                    if (nums[j] == this._board[row, i])
                        return false;
                }
                nums[temp] = this._board[row, i];
                temp++;
            }
        }
        return true;
    }
    private bool CheckCol(int col)
    {
        int temp = 0;
        int[] nums = new int[this._boardSize];
        if (col < 0 || col >= this._boardSize) return false;
        for (int i = 0; i < this._boardSize; i++)
        {
            if (this._board[i, col] != 0)
            {
                for (int j = 0; j < temp; j++)
                {
                    if (nums[j] == this._board[i, col])
                        return false;
                }
                nums[temp] = this._board[i, col];
                temp++;
            }
        }
        return true;
    }
    private bool CheckSquare(int squareRow, int squareCol)
    {
        int temp = 0;
        int[] nums = new int[this._boardSize];
        if (squareRow < 0 || squareCol < 0 || squareRow >= this._squareSize || squareCol >= this._squareSize)
            return false;
        for (int i = this._squareSize * squareRow; i < this._squareSize * (squareRow + 1); i++)
            for (int j = this._squareSize * squareCol; j < this._squareSize * (squareCol + 1); j++)
            {
                for (int k = 0; k < temp; k++)
                {
                    if (nums[k] == this._board[i, j])
                        return false;
                }
                nums[temp] = this._board[i, j];
                temp++;
            }
        return true;
    }
    public override string ToString()
    {
        string result = "";
        string seperator = "";
        for (int i = 0; i < this._squareSize; i++)
        {
            seperator += "+";
            for (int j = 0; j < this._squareSize * 2 + 1; j++) seperator += "-";
        }
        seperator += "+\n";

        for (int i = 0; i < this._squareSize; i++)
        {
            result += seperator;
            for (int j = 0; j < this._squareSize; j++)
            {
                string line = "";
                for (int k = 0; k < this._squareSize; k++)
                {
                    line += "| ";
                    for (int t = 0; t < this._squareSize; t++)
                    {
                        char cell = ' ';
                        int value = this._board[i * this._squareSize + j, k * this._squareSize + t];
                        if (0 <= value && value <= 9)
                            cell = (char)('0' + value);
                        else if (10 <= value && value <= 35)
                            cell = (char)('A' + value - 10);
                        line += cell;
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
    public string FormatString()
    {
        string str = "";
        foreach (int i in this._board)
            str += i;
        return str;
    }
}