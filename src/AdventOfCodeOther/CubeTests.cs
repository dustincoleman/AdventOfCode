using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCodeOther
{
    public class CubeTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void UpDown(int times)
        {
            var cube = Cube();
            for (int i = 0; i < times; i++)
            {
                cube = cube.RotateUp();
            }
            for (int i = 0; i < times; i++)
            {
                cube = cube.RotateDown();
            }
            Assert.Equal(Grid(), cube.Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void DownUp(int times)
        {
            var cube = Cube();
            for (int i = 0; i < times; i++)
            {
                cube = cube.RotateDown();
            }
            for (int i = 0; i < times; i++)
            {
                cube = cube.RotateUp();
            }
            Assert.Equal(Grid(), cube.Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void LeftRight(int times)
        {
            var cube = Cube();
            for (int i = 0; i < times; i++)
            {
                cube = cube.RotateLeft();
            }
            for (int i = 0; i < times; i++)
            {
                cube = cube.RotateRight();
            }
            Assert.Equal(Grid(), cube.Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void RightLeft(int times)
        {
            var cube = Cube();
            for (int i = 0; i < times; i++)
            {
                cube = cube.RotateRight();
            }
            for (int i = 0; i < times; i++)
            {
                cube = cube.RotateLeft();
            }
            Assert.Equal(Grid(), cube.Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void UpLeftDown(int timesLeft)
        {
            var cube = Cube().RotateUp();
            for (int i = 0; i < timesLeft; i++)
            {
                cube = cube.RotateLeft();
            }
            var grid = Grid();
            for (int i = 0; i < timesLeft; i++)
            {
                grid = grid.Rotate();
            }
            Assert.Equal(grid, cube.RotateDown().Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void UpRightDown(int timesRight)
        {
            var cube = Cube().RotateUp();
            for (int i = 0; i < timesRight; i++)
            {
                cube = cube.RotateRight();
            }
            var grid = Grid();
            for (int i = 0; i < timesRight; i++)
            {
                grid = grid.RotateCCW();
            }
            Assert.Equal(grid, cube.RotateDown().Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void DownLeftUp(int timesLeft)
        {
            var cube = Cube().RotateDown();
            for (int i = 0; i < timesLeft; i++)
            {
                cube = cube.RotateLeft();
            }
            var grid = Grid();
            for (int i = 0; i < timesLeft; i++)
            {
                grid = grid.RotateCCW();
            }
            Assert.Equal(grid, cube.RotateUp().Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void DownRightUp(int timesRight)
        {
            var cube = Cube().RotateDown();
            for (int i = 0; i < timesRight; i++)
            {
                cube = cube.RotateRight();
            }
            var grid = Grid();
            for (int i = 0; i < timesRight; i++)
            {
                grid = grid.Rotate();
            }
            Assert.Equal(grid, cube.RotateUp().Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void LeftUpRight(int timesUp)
        {
            var cube = Cube().RotateLeft();
            for (int i = 0; i < timesUp; i++)
            {
                cube = cube.RotateUp();
            }
            var grid = Grid();
            for (int i = 0; i < timesUp; i++)
            {
                grid = grid.RotateCCW();
            }
            Assert.Equal(grid, cube.RotateRight().Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void RightUpLeft(int timesUp)
        {
            var cube = Cube().RotateRight();
            for (int i = 0; i < timesUp; i++)
            {
                cube = cube.RotateUp();
            }
            var grid = Grid();
            for (int i = 0; i < timesUp; i++)
            {
                grid = grid.Rotate();
            }
            Assert.Equal(grid, cube.RotateLeft().Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void LeftDownRight(int timesDown)
        {
            var cube = Cube().RotateLeft();
            for (int i = 0; i < timesDown; i++)
            {
                cube = cube.RotateDown();
            }
            var grid = Grid();
            for (int i = 0; i < timesDown; i++)
            {
                grid = grid.Rotate();
            }
            Assert.Equal(grid, cube.RotateRight().Face);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void RightDownLeft(int timesDown)
        {
            var cube = Cube().RotateRight();
            for (int i = 0; i < timesDown; i++)
            {
                cube = cube.RotateDown();
            }
            var grid = Grid();
            for (int i = 0; i < timesDown; i++)
            {
                grid = grid.RotateCCW();
            }
            Assert.Equal(grid, cube.RotateLeft().Face);
        }

        [Fact]
        public void Left4()
        {
            var cube = Cube();
            for (int i = 0; i < 4; i++)
            {
                cube = cube.RotateLeft();
            }
            Assert.Equal(Grid(), cube.Face);
        }

        [Fact]
        public void Right4()
        {
            var cube = Cube();
            for (int i = 0; i < 4; i++)
            {
                cube = cube.RotateRight();
            }
            Assert.Equal(Grid(), cube.Face);
        }

        [Fact]
        public void Up4()
        {
            var cube = Cube();
            for (int i = 0; i < 4; i++)
            {
                cube = cube.RotateUp();
            }
            Assert.Equal(Grid(), cube.Face);
        }

        [Fact]
        public void Down4()
        {
            var cube = Cube();
            for (int i = 0; i < 4; i++)
            {
                cube = cube.RotateDown();
            }
            Assert.Equal(Grid(), cube.Face);
        }

        [Fact]
        public void Up2Left2()
        {
            Assert.Equal(Grid().Rotate180(), Cube().RotateUp().RotateUp().RotateLeft().RotateLeft().Face);
        }

        [Fact]
        public void Up2Right2()
        {
            Assert.Equal(Grid().Rotate180(), Cube().RotateUp().RotateUp().RotateRight().RotateRight().Face);
        }

        [Fact]
        public void Down2Left2()
        {
            Assert.Equal(Grid().Rotate180(), Cube().RotateDown().RotateDown().RotateLeft().RotateLeft().Face);
        }

        [Fact]
        public void Down2Right2()
        {
            Assert.Equal(Grid().Rotate180(), Cube().RotateDown().RotateDown().RotateRight().RotateRight().Face);
        }

        [Fact]
        public void AllSides()
        {
            Cube<int> cube = new Cube<int>(2);

            cube[0, 0] = 1;
            cube[1, 0] = 1;
            cube[0, 1] = 1;
            cube[1, 1] = 1;

            cube = cube.RotateLeft();
            Assert.Equal(Grid(0), cube.Face);

            cube[0, 0] = 2;
            cube[1, 0] = 2;
            cube[0, 1] = 2;
            cube[1, 1] = 2;

            cube = cube.RotateLeft();
            Assert.Equal(Grid(0), cube.Face);

            cube[0, 0] = 3;
            cube[1, 0] = 3;
            cube[0, 1] = 3;
            cube[1, 1] = 3;

            cube = cube.RotateLeft();
            Assert.Equal(Grid(0), cube.Face);

            cube[0, 0] = 4;
            cube[1, 0] = 4;
            cube[0, 1] = 4;
            cube[1, 1] = 4;

            cube = cube.RotateLeft().RotateDown();
            Assert.Equal(Grid(0), cube.Face);

            cube[0, 0] = 5;
            cube[1, 0] = 5;
            cube[0, 1] = 5;
            cube[1, 1] = 5;

            cube = cube.RotateUp().RotateUp();
            Assert.Equal(Grid(0), cube.Face);

            cube[0, 0] = 6;
            cube[1, 0] = 6;
            cube[0, 1] = 6;
            cube[1, 1] = 6;

            cube = cube.RotateDown();
            Assert.Equal(Grid(1), cube.Face);

            cube = cube.RotateLeft();
            Assert.Equal(Grid(2), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(3), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(4), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(1), cube.Face);

            cube = cube.RotateUp();
            Assert.Equal(Grid(6), cube.Face);

            cube = cube.RotateLeft();
            Assert.Equal(Grid(2), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(5), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(4), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(6), cube.Face);

            cube = cube.RotateUp();
            Assert.Equal(Grid(3), cube.Face);

            cube = cube.RotateLeft();
            Assert.Equal(Grid(2), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(1), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(4), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(3), cube.Face);

            cube = cube.RotateUp();
            Assert.Equal(Grid(5), cube.Face);

            cube = cube.RotateLeft();
            Assert.Equal(Grid(2), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(6), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(4), cube.Face);
            cube = cube.RotateLeft();
            Assert.Equal(Grid(5), cube.Face);
        }

        private static Cube<int> Cube()
        {
            Cube<int> cube = new Cube<int>(2);
            cube[0, 0] = 1;
            cube[1, 0] = 2;
            cube[0, 1] = 3;
            cube[1, 1] = 4;
            return cube;
        }

        private static Grid2<int> Grid(int? value = null)
        {
            Grid2<int> grid = new Grid2<int>(2, 2);
            grid[0, 0] = value ?? 1;
            grid[1, 0] = value ?? 2;
            grid[0, 1] = value ?? 3;
            grid[1, 1] = value ?? 4;
            return grid;
        }
    }
}
