namespace AdventOfCode2023;

public class Day06
{
    [Fact]
    public void Part1()
    {
        int answer = 1;

        foreach (RaceInfo timeAndRecord in LoadPuzzlePart1())
        {
            answer *= GetPossibleRaces(timeAndRecord.Time).Where(scenario => scenario.Distance > timeAndRecord.Distance).Count();
        }

        Assert.Equal(771628, answer);
    }

    [Fact]
    public void Part2()
    {
        RaceInfoBig timeAndRecord = LoadPuzzlePart2();
        int answer = GetPossibleRacesBig(timeAndRecord.Time).Where(scenario => scenario.Distance > timeAndRecord.Distance).Count();
        Assert.Equal(27363861, answer);
    }

    private List<RaceInfo> GetPossibleRaces(int time)
    {
        List<RaceInfo> list = new List<RaceInfo>();

        for (int i = 1; i < time; i++)
        {
            list.Add(new RaceInfo(i, (time - i) * i));
        }

        return list;
    }

    private List<RaceInfoBig> GetPossibleRacesBig(int time)
    {
        List<RaceInfoBig> list = new List<RaceInfoBig>();

        for (int i = 1; i < time; i++)
        {
            list.Add(new RaceInfoBig(i, ((long)(time - i)) * i));
        }

        return list;
    }

    private List<RaceInfo> LoadPuzzlePart1()
    {
        string[] lines = File.ReadAllLines("Day06.txt");
        int[] times = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
        int[] distances = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();

        List<RaceInfo> list = new List<RaceInfo>();
        for (int i = 0; i < times.Length; i++)
        {
            list.Add(new RaceInfo(times[i], distances[i]));
        }

        return list;
    }

    private RaceInfoBig LoadPuzzlePart2()
    {
        string[] lines = File.ReadAllLines("Day06.txt");
        int time = int.Parse(string.Concat(lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1)));
        long distance = long.Parse(string.Concat(lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1)));

        return new RaceInfoBig(time, distance);
    }

    private record RaceInfo(int Time, int Distance);
    private record RaceInfoBig(int Time, long Distance);
}