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

            int min = coverage.Select(r => r.Begin).Min();
            int max = coverage.Select(r => r.End).Max();
            int count = 0;

            for (int i = min; i <= max; i++)
            {
                if (coverage.Any(r => i >= r.Begin && i <= r.End) && !sensors.Any(s => s.NearestBeacon.X == i && s.NearestBeacon.Y == row))
                {
                    count++;
                }
            }

            Assert.Equal(5112034, count);
        }

        [Fact]
        public void Part2()
        {
            int maxValue = 4000000;

            List<Sensor> sensors = LoadPuzzle();

            Point2 result = Point2.Zero;

            

            for (int row = 0; row <= maxValue; row++)
            {
                List<Range> coverage = GetCoverageAtRow(sensors, row);

                coverage.Sort((left, right) =>
                {
                    int result = left.Begin.CompareTo(right.Begin);

                    if (result == 0)
                    {
                        result = left.End.CompareTo(right.End);
                    }

                    return result;
                });

                int current = 0;
                int confirmedThrough = 0;

                while (coverage[current].End < 0)
                {
                    current++;
                }

                if (coverage[current].Begin > 0)
                {
                    result = new Point2(0, row);
                    break;
                }

                confirmedThrough = coverage[current].End;

                while (current < coverage.Count && coverage[current].Begin <= confirmedThrough)
                {
                    confirmedThrough = Math.Max(coverage[current].End, confirmedThrough);
                    current++;
                }

                if (current < coverage.Count && coverage[current].Begin > confirmedThrough + 1)
                {
                    result = new Point2(confirmedThrough + 1, row);
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

        private List<Range> GetCoverageAtRow(List<Sensor> sensors, int row)
        {
            List<Range> list = new List<Range>();

            foreach (Sensor sensor in sensors)
            {
                Range range = sensor.CoverageAtRow(row);
                if (range != null)
                {
                    list.Add(range);
                }
            }

            return list;
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