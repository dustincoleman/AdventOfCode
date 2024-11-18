namespace AdventOfCode.Common
{
    public class Grid2Locations
    {
        private readonly IGrid2 grid;

        internal Grid2Locations(IGrid2 grid)
        {
            this.grid = grid;
        }

        public Point2 CenterPoint => (this.grid.Bounds % 2 == Point2.One) ? this.grid.Bounds / 2 : throw new InvalidOperationException("Grid does not have center");

        public Point2 NWCorner => (0, 0);

        public Point2 NECorner => (this.grid.Bounds.X - 1, 0);

        public Point2 SWCorner => (0, this.grid.Bounds.Y - 1);

        public Point2 SECorner => (this.grid.Bounds.X - 1, this.grid.Bounds.Y - 1);

        public Point2 NorthCenter => (this.grid.Bounds.X % 2 == 1) ? (this.grid.Bounds.X / 2, 0) : throw new InvalidOperationException("Grid does not have center");

        public Point2 SouthCenter => (this.grid.Bounds.X % 2 == 1) ? (this.grid.Bounds.X / 2, this.grid.Bounds.Y - 1) : throw new InvalidOperationException("Grid does not have center");

        public Point2 WestCenter => (this.grid.Bounds.Y % 2 == 1) ? (0, this.grid.Bounds.Y / 2) : throw new InvalidOperationException("Grid does not have center");

        public Point2 EastCenter => (this.grid.Bounds.Y % 2 == 1) ? (this.grid.Bounds.X - 1, this.grid.Bounds.Y / 2) : throw new InvalidOperationException("Grid does not have center");
    }
}
