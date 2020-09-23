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
        private readonly char[,] _board;

        public LineDetails(bool forColumn, int cellNumber, char[,] board)
        {
            _forColumn = forColumn;
            _cellNumber = cellNumber;
            _board = board;

            if (forColumn)
                RecalculateColumn();
            else
                RecalculateRow();

        }

        public IEnumerable<IEnumerable<int>> MatchPattern(string pattern)
        {
            var startOfMatch = AsText.IndexOf(pattern);
            while (startOfMatch != -1)
            {
                yield return Enumerable.Range(startOfMatch, pattern.Length);

                startOfMatch = AsText.IndexOf(pattern, startOfMatch + 1);
            }
        }

        internal void SetValue(int location, char value)
        {
            if (_forColumn)
            {
                _board[_cellNumber, location] = value;
                RecalculateColumn();
            }
            else
            {
                _board[location, _cellNumber] = value;
                RecalculateRow();
            }
        }

        private void ResetTotals()
        {
            NumberOf0s = 0;
            NumberOf1s = 0;
            NumberOfGaps = 0;
            AsText = string.Empty;
            Gaps.Clear();
        }

        private void RecalculateRow()
        {
            ResetTotals();

            for (int column = 0; column < 10; column++)
            {
                switch (_board[column, _cellNumber])
                {
                    case '0':
                        NumberOf0s++;
                        break;
                    case '1':
                        NumberOf1s++;
                        break;
                    default:
                        NumberOfGaps++;
                        Gaps.Add(column);
                        break;
                }
                AsText += _board[column, _cellNumber];
            }
        }

        private void RecalculateColumn()
        {
            ResetTotals();

            for (int row = 0; row < 10; row++)
            {
                switch (_board[_cellNumber, row])
                {
                    case '0':
                        NumberOf0s++;
                        break;
                    case '1':
                        NumberOf1s++;
                        break;
                    default:
                        NumberOfGaps++;
                        Gaps.Add(row);
                        break;
                }

                AsText += _board[_cellNumber, row];
            }
        }

        public static LineDetails ForColumn(char[,] board, int column) => new LineDetails(true, column, board);

        public static LineDetails ForRow(char[,] board, int row) => new LineDetails(false, row, board);
    }
}
