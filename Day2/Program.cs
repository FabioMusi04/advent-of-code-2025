using AdventOfCode.Core;

namespace Day2;

public class Day01Solver : IDaySolver
{
    private readonly string _input;

    public Day01Solver()
    {
        _input = FileReader.ReadLines(2).First();
    }

    public string SolvePart1()
    {
        var ranges = _input.Split(',');

        long sum = 0;

        foreach (var range in ranges)
        {
            var bounds = range.Split('-');
            long start = long.Parse(bounds[0]);
            long end = bounds.Length > 1 ? long.Parse(bounds[1]) : start;

            for (long id = start; id <= end; id++)
            {
                string idStr = id.ToString();
                int len = idStr.Length;
                if (len % 2 == 0)
                {
                    string firstHalf = idStr.Substring(0, len / 2);
                    string secondHalf = idStr.Substring(len / 2, len / 2);
                    if (firstHalf == secondHalf)
                    {
                        sum += id;
                    }
                }
            }
        }
        return sum.ToString();
    }

    public string SolvePart2()
    {
        var ranges = _input.Split(',');

        long sum = 0;

        foreach (var range in ranges)
        {
            var bounds = range.Split('-');
            long start = long.Parse(bounds[0]);
            long end = bounds.Length > 1 ? long.Parse(bounds[1]) : start;

            for (long id = start; id <= end; id++)
            {
                string idStr = id.ToString();
                int len = idStr.Length;
                bool invalid = false;

                for (int subLen = 1; subLen <= len / 2; subLen++)
                {
                    if (len % subLen != 0) continue;
                    string pattern = idStr.Substring(0, subLen);
                    int repeat = len / subLen;
                    string repeated = string.Concat(Enumerable.Repeat(pattern, repeat));
                    if (repeated == idStr && repeat >= 2)
                    {
                        invalid = true;
                        break;
                    }
                }

                if (invalid)
                {
                    sum += id;
                }
            }
        }

        return sum.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var solver = new Day01Solver();

        Console.WriteLine("Day 2");
        Console.WriteLine("Part 1: " + solver.SolvePart1());
        Console.WriteLine("Part 2: " + solver.SolvePart2());
    }
}
