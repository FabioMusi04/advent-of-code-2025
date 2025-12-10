using AdventOfCode.Core;

namespace Day1;

public class Day01Solver : IDaySolver
{
    private readonly List<string> _input;

    public Day01Solver()
    {
        _input = FileReader.ReadLines(1);
    }

    public string SolvePart1()
    {
        int start = 50;
        int zeroCount = 0;

        foreach (var value in _input)
        {
            int amount = int.Parse(value[1..]);
            if (value.Contains('R'))
                start = (start + amount) % 100;
            else
                start = (start - amount + 100) % 100;

            if (start == 0) zeroCount++;
        }

        return zeroCount.ToString();
    }

    public string SolvePart2()
    {
        int start = 50;
        int zeroCount = 0;

        foreach (var value in _input)
        {
            int amount = int.Parse(value[1..]);
            for (int i = 0; i < amount; i++)
            {
                start = value.Contains('R')
                    ? (start + 1) % 100
                    : (start - 1 + 100) % 100;

                if (start == 0) zeroCount++;
            }
        }

        return zeroCount.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var solver = new Day01Solver();

        Console.WriteLine("Day 1");
        Console.WriteLine("Part 1: " + solver.SolvePart1());
        Console.WriteLine("Part 2: " + solver.SolvePart2());
    }
}
