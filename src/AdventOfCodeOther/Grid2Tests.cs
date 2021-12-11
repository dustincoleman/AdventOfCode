using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCodeOther
{
    public class Grid2Tests
    {
        [Theory]
        [InlineData("1,2/3,4", 1, 1)]
        [InlineData("1,2,3/4,5,6", 1, 2)]
        [InlineData("1,2,3,4,5,6/2,3,4,5,6,7/3,4,5,6,7,8/4,5,6,7,8,9", 2, 2)]
        [InlineData("1,2,3,4,5,6/2,3,4,5,6,7/3,4,5,6,7,8/4,5,6,7,8,9", 3, 2)]
        public void SplitAndCombine(string input, int sizeX, int sizeY)
        {
            Grid2<int> inputGrid = GridFromString(input);
            Grid2<int> roundTripGrid = Grid2<int>.Combine(inputGrid.Split(new Point2(sizeX, sizeY)));
            Assert.True(inputGrid == roundTripGrid);
        }

        [Theory]
        [InlineData("1,2/3,4", "3,4/1,2")]
        [InlineData("1,2,3/4,5,6", "4,5,6/1,2,3")]
        [InlineData("1,2/3,4/5,6", "5,6/3,4/1,2")]
        [InlineData("1,2,3/4,5,6/7,8,9", "7,8,9/4,5,6/1,2,3")]
        public void HorizontalFlip(string input, string expected)
        {
            Grid2<int> flippedGrid = GridFromString(input).FlipHorizontal();
            Grid2<int> expectedGrid = GridFromString(expected);
            Assert.True(flippedGrid == expectedGrid);
        }

        [Theory]
        [InlineData("1,2/3,4", "2,1/4,3")]
        [InlineData("1,2,3/4,5,6", "3,2,1/6,5,4")]
        [InlineData("1,2/3,4/5,6", "2,1/4,3/6,5")]
        [InlineData("1,2,3/4,5,6/7,8,9", "3,2,1/6,5,4/9,8,7")]
        public void VerticalFlip(string input, string expected)
        {
            Grid2<int> flippedGrid = GridFromString(input).FlipVertical();
            Grid2<int> expectedGrid = GridFromString(expected);
            Assert.True(flippedGrid == expectedGrid);
        }

        [Theory]
        [InlineData("1,2/3,4", "3,1/4,2")]
        [InlineData("1,2,3/4,5,6", "4,1/5,2/6,3")]
        [InlineData("1,2/3,4/5,6", "5,3,1/6,4,2")]
        [InlineData("1,2,3/4,5,6/7,8,9", "7,4,1/8,5,2/9,6,3")]
        public void Rotate(string input, string expected)
        {
            Grid2<int> flippedGrid = GridFromString(input).Rotate();
            Grid2<int> expectedGrid = GridFromString(expected);
            Assert.True(flippedGrid == expectedGrid);
        }

        private Grid2<int> GridFromString(string input)
        {
            int[][] values = input.Split('/')
                                  .Select(row => row.Split(",")
                                                    .Select(int.Parse)
                                                    .ToArray())
                                  .ToArray();

            Point2 bounds = new Point2(values[0].Length, values.Length);
            Grid2<int> grid = new Grid2<int>(bounds);

            foreach (Point2 point in Point2.Quadrant(bounds))
            {
                grid[point] = values[point.Y][point.X];
            }

            return grid;
        }
    }
}
