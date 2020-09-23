using System.Collections.Generic;
using System.Linq;

namespace Binary
{
    public class LineDetails
    {
        public int NumberOf0s { get; private set; }
        public int NumberOf1s { get; private set; }
        public int NumberOfGaps { get; private set; }
        public List<int> Gaps { get; private set; } = new List<int>();
        public string AsText { get; private set; } = string.Empty;

        private readonly bool _forColumn;
        private readonly int _cellNumber;
        private readonly char[,] board;

        public LineDetails(bool forColumn, int cellNumber, char[,] board)
        {
            _forColumn = forColumn;
            _cellNumber = cellNumber;
            this.board = board;
        }

        public LineDetails Refresh() => _forColumn ? ForColumn(board, _cellNumber) : ForRow(board, _cellNumber);

        public IEnumerable<IEnumerable<int>> MatchPattern(string pattern)
        {
            var startOfMatch = AsText.IndexOf(pattern);
            while (startOfMatch != -1)
            {
                yield return Enumerable.Range(startOfMatch, pattern.Length);

                startOfMatch = AsText.IndexOf(pattern, startOfMatch + 1);
            }
        }

        public static LineDetails ForColumn(char[,] board, int column)
        {
            var result = new LineDetails(true, column, board);

            for (int row = 0; row < 10; row++)
            {
                if (board[column, row] == '0') result.NumberOf0s++;
                if (board[column, row] == '1') result.NumberOf1s++;
                if (board[column, row] == ' ')
                {
                    result.NumberOfGaps++;
                    result.Gaps.Add(row);
                }
                result.AsText += board[column, row];
            }

            return result;
        }

        public static LineDetails ForRow(char[,] board, int row)
        {
            var result = new LineDetails(false, row, board);

            for (int column = 0; column < 10; column++)
            {
                if (board[column, row] == '0') result.NumberOf0s++;
                if (board[column, row] == '1') result.NumberOf1s++;
                if (board[column, row] == ' ')
                {
                    result.NumberOfGaps++;
                    result.Gaps.Add(column);
                }
                result.AsText += board[column, row];
            }

            return result;
        }
    }
}
