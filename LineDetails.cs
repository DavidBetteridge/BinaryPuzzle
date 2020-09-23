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
            var result = new LineDetails();

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
    }
}
