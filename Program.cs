﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Binary
{
    class Match
    {
        public List<Location> Locations { get; set; }
    }

    class Location
    {
        public int Column { get; set; }
        public int Row { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var board = new char[10, 10];
            SetupInitialBoard(board);

            while (true)
            {
                var matches = FindPatterns(board, "11 ");
                foreach (var match in matches)
                {
                    var location = match.Locations.Last();
                    board[location.Column, location.Row] = '0';
                }

                matches = FindPatterns(board, " 11");
                foreach (var match in matches)
                {
                    var location = match.Locations.First();
                    board[location.Column, location.Row] = '0';
                }

                matches = FindPatterns(board, "00 ");
                foreach (var match in matches)
                {
                    var location = match.Locations.Last();
                    board[location.Column, location.Row] = '1';
                }

                matches = FindPatterns(board, " 00");
                foreach (var match in matches)
                {
                    var location = match.Locations.First();
                    board[location.Column, location.Row] = '1';
                }

                matches = FindPatterns(board, "0 0");
                foreach (var match in matches)
                {
                    var location = match.Locations[1];
                    board[location.Column, location.Row] = '1';
                }

                matches = FindPatterns(board, "1 1");
                foreach (var match in matches)
                {
                    var location = match.Locations[1];
                    board[location.Column, location.Row] = '0';
                }

                for (int column = 0; column < 10; column++)
                {
                    var columnDetails = LineDetails.ForColumn(board, column);

                    if (columnDetails.NumberOfGaps == 3 && columnDetails.NumberOf0s == 4)
                    {

                        var matchPattern = columnDetails.MatchPattern("  1");
                        if (matchPattern.Count() == 1)
                        {
                            var otherGap = columnDetails.Gaps.Except(matchPattern.Single()).Single();
                            board[column, otherGap] = '1';
                        }

                        else
                        {
                            matchPattern = columnDetails.MatchPattern("1  ");
                            if (matchPattern.Count() == 1)
                            {
                                // 1 goes in the final gap
                                var otherGap = columnDetails.Gaps.Except(matchPattern.Single()).Single();
                                board[column, otherGap] = '1';
                            }
                        }
                    }

                    if (columnDetails.NumberOfGaps == 3 && columnDetails.NumberOf1s == 4)
                    {
                        var matchPattern = columnDetails.MatchPattern("  0");
                        if (matchPattern.Count() == 1)
                        {
                            var otherGap = columnDetails.Gaps.Except(matchPattern.Single()).Single();
                            board[column, otherGap] = '0';
                        }
                        else
                        {
                            matchPattern = columnDetails.MatchPattern("0  ");
                            if (matchPattern.Count() == 1)
                            {
                                var otherGap = columnDetails.Gaps.Except(matchPattern.Single()).Single();
                                board[column, otherGap] = '0';
                            }
                        }
                    }


                    if (columnDetails.NumberOfGaps != 0 && columnDetails.NumberOf0s == 5)
                    {
                        for (int row = 0; row < 10; row++)
                        {
                            if (board[column, row] == ' ')
                            {
                                board[column, row] = '1';
                            }
                        }
                    }

                    if (columnDetails.NumberOfGaps != 0 && columnDetails.NumberOf1s == 5)
                    {
                        for (int row = 0; row < 10; row++)
                        {
                            if (board[column, row] == ' ')
                            {
                                board[column, row] = '0';
                            }
                        }
                    }

                }

                for (int row = 0; row < 10; row++)
                {
                    var numberOf0sInRow = 0;
                    var numberOf1sInRow = 0;
                    var numberOfUnknownsInRow = 0;
                    var rowAsString = string.Empty;
                    var gaps = new List<int>();

                    for (int column = 0; column < 10; column++)
                    {
                        if (board[column, row] == '0') numberOf0sInRow++;
                        if (board[column, row] == '1') numberOf1sInRow++;
                        if (board[column, row] == ' ')
                        {
                            numberOfUnknownsInRow++;
                            gaps.Add(column);
                        }
                        rowAsString += board[column, row];
                    }

                    if (numberOfUnknownsInRow == 3 && numberOf0sInRow == 4)
                    {
                        // 1 x 0 to place
                        // 2 x 1 to place

                        var startOfMatch = rowAsString.IndexOf("  1");
                        if (startOfMatch != -1)
                        {
                            // 1 goes in the final gap
                            var otherGap = gaps.Except(Enumerable.Range(startOfMatch, 3)).Single();
                            board[otherGap, row] = '1';
                        }
                    }

                    if (numberOfUnknownsInRow != 0 && numberOf0sInRow == 5)
                    {
                        foreach (var gap in gaps)
                        {
                            board[gap, row] = '1';
                        }
                    }

                    if (numberOfUnknownsInRow != 0 && numberOf1sInRow == 5)
                    {
                        foreach (var gap in gaps)
                        {
                            board[gap, row] = '0';
                        }
                    }

                }

                Display(board);
                Console.ReadKey(true);
            }
        }

        private static List<Match> FindPatterns(char[,] board, string pattern)
        {
            var results = new List<Match>();

            // Search for pattern in each row
            for (int row = 0; row < 10; row++)
            {
                var rowAsString = string.Empty;
                for (int column = 0; column < 10; column++)
                {
                    rowAsString += board[column, row];
                }

                var startOfMatch = rowAsString.IndexOf(pattern);
                while (startOfMatch != -1)
                {
                    results.Add(new Match
                    {
                        Locations = Enumerable.Range(startOfMatch, pattern.Length)
                                              .Select(col => new Location { Column = col, Row = row })
                                              .ToList()
                    });

                    startOfMatch = rowAsString.IndexOf(pattern, startOfMatch + 1);
                }
            }


            // Search for pattern in each column
            for (int column = 0; column < 10; column++)
            {
                var columnDetails = LineDetails.ForColumn(board, column);
                var matches = columnDetails.MatchPattern(pattern)
                                           .Select(m => new Match() { Locations = m.Select(l => new Location { Column = column, Row = l }).ToList() });
                results.AddRange(matches);
            }

            return results;
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
