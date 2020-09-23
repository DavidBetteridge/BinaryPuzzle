using System;
using System.Linq;

namespace Binary
{
    class Program
    {
        static void Main()
        {
            var board = new char[10, 10];
            SetupInitialBoard(board);

            while (true)
            {
                for (int column = 0; column < 10; column++)
                {
                    var lineDetails = LineDetails.ForColumn(board, column);

                    ApplyPatterns(lineDetails);
                }

                for (int row = 0; row < 10; row++)
                {
                    var lineDetails = LineDetails.ForRow(board, row);

                    ApplyPatterns(lineDetails);
                }

                Display(board);
                Console.ReadKey(true);
            }
        }

        private static void ApplyPatterns(LineDetails lineDetails)
        {
            var m = lineDetails.MatchPattern("11 ");
            foreach (var match in m)
            {
                lineDetails.SetValue(match.Last(), '0');
            }

            m = lineDetails.MatchPattern(" 11");
            foreach (var match in m)
            {
                lineDetails.SetValue(match.First(), '0');
            }

            m = lineDetails.MatchPattern("00 ");
            foreach (var match in m)
            {
                lineDetails.SetValue(match.Last(), '1');
            }

            m = lineDetails.MatchPattern(" 00");
            foreach (var match in m)
            {
                lineDetails.SetValue(match.First(), '1');
            }

            m = lineDetails.MatchPattern("0 0");
            foreach (var match in m)
            {
                lineDetails.SetValue(match.ElementAt(1), '1');
            }

            m = lineDetails.MatchPattern("1 1");
            foreach (var match in m)
            {
                lineDetails.SetValue(match.ElementAt(1), '0');
            }

            if (lineDetails.NumberOfGaps != 0 && lineDetails.NumberOf0s == 5)
            {
                foreach (var gap in lineDetails.Gaps.ToArray())
                {
                    lineDetails.SetValue(gap, '1');
                }
            }

            if (lineDetails.NumberOfGaps != 0 && lineDetails.NumberOf1s == 5)
            {
                foreach (var gap in lineDetails.Gaps.ToArray())
                {
                    lineDetails.SetValue(gap, '0');
                }
            }

            if (lineDetails.NumberOfGaps == 3 && lineDetails.NumberOf0s == 4)
            {
                var matchPattern = lineDetails.MatchPattern("  1");
                if (matchPattern.Count() == 1)
                {
                    var otherGap = lineDetails.Gaps.Except(matchPattern.Single()).Single();
                    lineDetails.SetValue(otherGap, '1');
                }
                else
                {
                    matchPattern = lineDetails.MatchPattern("1  ");
                    if (matchPattern.Count() == 1)
                    {
                        var otherGap = lineDetails.Gaps.Except(matchPattern.Single()).Single();
                        lineDetails.SetValue(otherGap, '1');
                    }
                }
            }

            if (lineDetails.NumberOfGaps == 3 && lineDetails.NumberOf1s == 4)
            {
                var matchPattern = lineDetails.MatchPattern("  0");
                if (matchPattern.Count() == 1)
                {
                    var otherGap = lineDetails.Gaps.Except(matchPattern.Single()).Single();
                    lineDetails.SetValue(otherGap, '0');
                }
                else
                {
                    matchPattern = lineDetails.MatchPattern("0  ");
                    if (matchPattern.Count() == 1)
                    {
                        var otherGap = lineDetails.Gaps.Except(matchPattern.Single()).Single();
                        lineDetails.SetValue(otherGap, '0');
                    }
                }
            }
        }

        private static void Display(char[,] board)
        {
            for (int row = 0; row < 10; row++)
            {
                for (int column = 0; column < 10; column++)
                {
                    Console.Write(board[column, row]);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static void SetupInitialBoard(char[,] board)
        {
            for (int row = 0; row < 10; row++)
            {
                for (int column = 0; column < 10; column++)
                {
                    board[column, row] = ' ';
                }
            }
            board[0, 0] = '0';
            board[2, 0] = '0';
            board[6, 1] = '0';
            board[8, 1] = '1';
            board[4, 2] = '1';
            board[5, 2] = '1';
            board[1, 3] = '1';
            board[8, 3] = '0';
            board[3, 4] = '0';
            board[6, 4] = '1';
            board[7, 4] = '1';
            board[3, 5] = '0';
            board[4, 5] = '0';
            board[9, 5] = '0';
            board[0, 6] = '1';
            board[5, 6] = '1';
            board[1, 7] = '0';
            board[1, 8] = '1';
            board[5, 8] = '1';
            board[7, 8] = '1';
            board[5, 9] = '1';
            board[8, 9] = '0';
        }
    }
}
