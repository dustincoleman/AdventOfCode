namespace AdventOfCode2022
{
    public class Day15
    {
        [Fact]
        public void Part1()
        {
            int row = 2000000;

            List<Sensor> sensors = LoadPuzzle();
            List<Range> coverage = GetCoverageAtRow(sensors, row);

            int beaconsOnRow = sensors.Select(s => s.NearestBeacon).Where(b => b.Y == row).Distinct().Count();
            int columnsCovered = coverage.Select(r => r.End - r.Begin + 1).Sum();

            Assert.Equal(5112034, columnsCovered - beaconsOnRow);
        }

        [Fact]
        public void Part2()
        {
            int maxValue = 4000000;

            List<Sensor> sensors = LoadPuzzle();
            Point2 result = Point2.Zero;

            for (int row = 0; row <= maxValue; row++)
            {
                List<Range> coverage = GetCoverageAtRow(sensors, row, (0, maxValue));

                if (coverage.Count > 1)
                {
                    result = new Point2(Math.Min(coverage[0].End, coverage[1].End) + 1, row);
                    break;
                }
            }

            long tuningFrequency = ((long)result.X * 4000000) + result.Y;
            Assert.Equal(13172087230812, tuningFrequency);
        }

        private List<Sensor> LoadPuzzle()
        {
            List<Sensor> list = new List<Sensor>();

            foreach (string line in File.ReadAllLines("Day15.txt"))
            {
                string[] split = line.Split(new[] { ' ', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
                list.Add(
                    new Sensor(
                        new Point2(int.Parse(split[2].Substring("x=".Length)), int.Parse(split[3].Substring("y=".Length))),
                        new Point2(int.Parse(split[8].Substring("x=".Length)), int.Parse(split[9].Substring("y=".Length)))
                    ));
            }

            return list;
        }

        private List<Range> GetCoverageAtRow(List<Sensor> sensors, int row, (int lower, int upper)? bounds = null)
        {
            List<Range> list = new List<Range>();

            foreach (Sensor sensor in sensors)
            {
                Range range = sensor.CoverageAtRow(row);

                if (range != null)
                {
                    if (bounds.HasValue)
                    {
                        int lower = bounds.Value.lower;
                        int upper = bounds.Value.upper;
                        range = new Range(Math.Max(range.Begin, lower), Math.Min(range.End, upper));
                    }

                    list.Add(range);
                }
            }

            return Range.MergeOverlap(list);
        }

        private class Sensor
        {
            internal Sensor(Point2 location, Point2 nearestBeacon)
            {
                Location = location;
                NearestBeacon = nearestBeacon;
                Distance = (nearestBeacon - location).Manhattan();
            }

            internal Point2 Location { get; }
            internal Point2 NearestBeacon { get; }
            internal int Distance { get; }

            internal Range CoverageAtRow(int row)
            {
                int width = Distance - Math.Abs(Location.Y - row);
                
                if (width >= 0)
                {
                    return new Range(Location.X - width, Location.X + width);
                }

                return null;
            }
        }
    }
}