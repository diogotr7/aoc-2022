using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdventOfCode
{
    public class Day08 : ISolution
    {
        public string Part1(string data)
        {
            var grid = ParseTable(data);

            var visibleTrees = CountVisibleTrees(grid);

            return visibleTrees.ToString();
        }

        
        public string Part2(string data)
        {
            var grid = ParseTable(data);

            var bestScore = GetBestScenicScore(grid);

            return bestScore.ToString();
        }

        private static int[,] ParseTable(string data)
        {
            var lines = data.Split(Environment.NewLine);

            int[,] grid = new int[lines[0].Length, lines.Length];
            for (int i = 0; i < lines[0].Length; i++)
                for (int j = 0; j < lines.Length; j++)
                    grid[i, j] = lines[i][j] - '0'; //convert char to int
            
            return grid;
        }

        private static int CountVisibleTrees(int[,] grid)
        {
            var length0 = grid.GetLength(0);
            var length1 = grid.GetLength(1);
            
            int count = 0;
            for (int i = 0; i < length0; i++)
                for (int j = 0; j < length1; j++)
                    if (IsVisibleFromOutside(i, j, grid))
                        count++;

            return count;
        }

        private static bool IsVisibleFromOutside(int i, int j, int[,] grid)
        {
            var height = grid[i, j];

            bool visibleFromNegativeI = true;
            for (int negI = 0; negI < i; negI++)
                if (grid[negI, j] >= height)
                    visibleFromNegativeI = false;

            bool visibleFromPositiveI = true;
            for (int posI = grid.GetLength(0) - 1; posI > i; posI--)
                if (grid[posI, j] >= height)
                    visibleFromPositiveI = false;

            bool visibleFromNegativeJ = true;
            for (int negJ = 0; negJ < j; negJ++)
                if (grid[i, negJ] >= height)
                    visibleFromNegativeJ = false;

            bool visibleFromPositiveJ = true;
            for (int posJ = grid.GetLength(1) - 1; posJ > j; posJ--)
                if (grid[i, posJ] >= height)
                    visibleFromPositiveJ = false;

            return visibleFromNegativeI ||
                   visibleFromPositiveI ||
                   visibleFromNegativeJ ||
                   visibleFromPositiveJ;
        }

        private static int GetBestScenicScore(int[,] grid)
        {
            var length0 = grid.GetLength(0);
            var length1 = grid.GetLength(1);

            int maxScore = 0;
            for (int i = 0; i < length0; i++)
                for (int j = 0; j < length1; j++)
                    maxScore = Math.Max(maxScore, GetScenicScore(i, j, grid));

            return maxScore;
        }

        private static int GetScenicScore(int i, int j, int[,] grid)
        {
            var height = grid[i, j];

            int scoreNegativeI = 0;
            for (int negI = i - 1; negI >= 0; negI--)
            {
                scoreNegativeI++;
                if (grid[negI, j] >= height)
                    break;
            }

            int scorePositiveI = 0;
            for (int posI = i + 1; posI < grid.GetLength(0); posI++)
            {
                scorePositiveI++;
                if (grid[posI, j] >= height)
                    break;
            }

            int scoreNegativeJ = 0;
            for (int negJ = j - 1 ; negJ >= 0; negJ--)
            {
                scoreNegativeJ++;
                if (grid[i, negJ] >= height)
                    break;
            }

            int scorePositiveJ = 0;
            for (int posJ = j + 1; posJ < grid.GetLength(1); posJ++)
            {
                scorePositiveJ++;
                if (grid[i, posJ] >= height)
                    break;
            }

            return scoreNegativeI *
                   scorePositiveI *
                   scoreNegativeJ *
                   scorePositiveJ;
        }
    }
}
