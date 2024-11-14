namespace AdventOfCode2022
{
    public class Day19
    {
        [Fact]
        public void Part1()
        {
            int result = 0;
            int number = 1;

            foreach (Blueprint blueprint in LoadPuzzle())
            {
                best = 0;
                scoreByKeyByResources.Clear();
                
                int geodes = Solve(blueprint, 24, Point3.UnitX);
                result += geodes * number++;
            }

            Assert.Equal(1650, result);
        }

        [Fact]
        public void Part2()
        {
            int result = 1;

            foreach (Blueprint blueprint in LoadPuzzle().Take(3))
            {
                best = 0;
                scoreByKeyByResources.Clear();

                int geodes = Solve(blueprint, 32, Point3.UnitX);
                result *= geodes;
            }

            Assert.Equal(5824, result);
        }

        Dictionary<int, Dictionary<int, int>> scoreByKeyByResources = new Dictionary<int, Dictionary<int, int>>();
        int best = 0;

        private int Solve(Blueprint blueprint, int remainingTime, Point3 robots, int geodeRobots = 0, Point3 resources = default(Point3), int runningTotal = 0)
        {
            if (remainingTime == 0)
            {
                best = Math.Max(best, runningTotal);
                return 0;
            }

            runningTotal += geodeRobots;

            int key = remainingTime << 24 | robots.X << 18 | robots.Y << 12 | robots.Z << 6 | geodeRobots;
            int resourcesKey = resources.X << 20 | resources.Y << 10 | resources.Z;

            if(scoreByKeyByResources.TryGetValue(resourcesKey, out Dictionary<int, int> scoreByResources))
            {
                if (scoreByResources.TryGetValue(key, out int cached2))
                {
                    return cached2;
                }
            }
            else
            {
                scoreByKeyByResources.Add(resourcesKey, new Dictionary<int, int>());
            }

            int totalGeodes = 0;
            int mostPossible = (geodeRobots > 0) ? (geodeRobots * remainingTime * remainingTime) + runningTotal : ((remainingTime - 1) * (remainingTime - 1)) + runningTotal;

            if (mostPossible > best)
            {
                int typesBuilt = 0;

                if (resources >= blueprint.OreRobotCost && resources.X + (robots.X * remainingTime) < blueprint.Max.X * remainingTime)
                {
                    totalGeodes = Math.Max(totalGeodes, Solve(blueprint, remainingTime - 1, robots + Point3.UnitX, geodeRobots, resources + robots - blueprint.OreRobotCost, runningTotal));
                    typesBuilt++;
                }

                if (resources >= blueprint.ClayRobotCost && resources.Y + (robots.Y * remainingTime) < blueprint.Max.Y * remainingTime)
                {
                    totalGeodes = Math.Max(totalGeodes, Solve(blueprint, remainingTime - 1, robots + Point3.UnitY, geodeRobots, resources + robots - blueprint.ClayRobotCost, runningTotal));
                    typesBuilt++;
                }

                if (resources >= blueprint.ObsidianRobotCost && resources.Z + (robots.Z * remainingTime) < blueprint.Max.Z * remainingTime)
                {
                    totalGeodes = Math.Max(totalGeodes, Solve(blueprint, remainingTime - 1, robots + Point3.UnitZ, geodeRobots, resources + robots - blueprint.ObsidianRobotCost, runningTotal));
                    typesBuilt++;
                }

                if (resources >= blueprint.GeodeRobotCost)
                {
                    totalGeodes = Math.Max(totalGeodes, Solve(blueprint, remainingTime - 1, robots, geodeRobots + 1, resources + robots - blueprint.GeodeRobotCost, runningTotal));
                    typesBuilt++;
                }

                if (typesBuilt < 4)
                {
                    totalGeodes = Math.Max(totalGeodes, Solve(blueprint, remainingTime - 1, robots, geodeRobots, resources + robots, runningTotal));
                }

                totalGeodes += geodeRobots;
            }

            scoreByKeyByResources[resourcesKey].Add(key, totalGeodes);

            return totalGeodes;
        }

        private List<Blueprint> LoadPuzzle()
        {
            List<Blueprint> list = new List<Blueprint>();
            Regex regex = new Regex(@"^Blueprint \d+: Each ore robot costs (?<OreRobotOreCost>\d+) ore. Each clay robot costs (?<ClayRobotOreCost>\d+) ore. Each obsidian robot costs (?<ObsidianRobotOreCost>\d+) ore and (?<ObsidianRobotClayCost>\d+) clay. Each geode robot costs (?<GeodeRobotOreCost>\d+) ore and (?<GeodeRobotObsidianCost>\d+) obsidian.$");

            foreach (Match match in File.ReadAllLines("Day19.txt").Select(s => regex.Match(s)))
            {
                var blueprint = new Blueprint()
                {
                    OreRobotCost = new Point3(int.Parse(match.Groups["OreRobotOreCost"].Value), 0, 0),
                    ClayRobotCost = new Point3(int.Parse(match.Groups["ClayRobotOreCost"].Value), 0, 0),
                    ObsidianRobotCost = new Point3(int.Parse(match.Groups["ObsidianRobotOreCost"].Value), int.Parse(match.Groups["ObsidianRobotClayCost"].Value), 0),
                    GeodeRobotCost = new Point3(int.Parse(match.Groups["GeodeRobotOreCost"].Value), 0, int.Parse(match.Groups["GeodeRobotObsidianCost"].Value))
                };

                blueprint.Max = Point3.Max(Point3.Max(blueprint.OreRobotCost, blueprint.ClayRobotCost), Point3.Max(blueprint.ObsidianRobotCost, blueprint.GeodeRobotCost));

                list.Add(blueprint);
            }

            return list;
        }


        private struct Blueprint
        {
            internal Point3 OreRobotCost;
            internal Point3 ClayRobotCost;
            internal Point3 ObsidianRobotCost;
            internal Point3 GeodeRobotCost;
            internal Point3 Max;
        }
    }
}