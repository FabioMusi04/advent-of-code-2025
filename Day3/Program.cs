using AdventOfCode.Core;
using System.Numerics;

namespace Day3;

public class DaySolver : IDaySolver
{
    private readonly List<string> _input;

    public DaySolver()
    {
        _input = FileReader.ReadLines(3);
    }

    public string SolvePart1()
    {
        var combined = new List<int>();

        foreach (var line in _input)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Length < 2)
            {
                combined.Add(0);
                continue;
            }

            var digits = line.Select(c => c - '0').ToArray();
            int n = digits.Length;

            // suffixMax[i] = massimo valore nella sottosequenza da i a fine
            var suffixMax = new int[n];
            suffixMax[n - 1] = digits[n - 1];
            for (int i = n - 2; i >= 0; i--)
                suffixMax[i] = Math.Max(digits[i], suffixMax[i + 1]);

            int best = int.MinValue;

            // per ogni posizione i consideriamo la migliore cifra a destra (i+1..end)
            for (int i = 0; i < n - 1; i++)
            {
                int maxRight = suffixMax[i + 1];
                int value = digits[i] * 10 + maxRight;
                if (value > best) best = value;
            }

            combined.Add(best);
        }

        return combined.Sum().ToString();
    }

    private static BigInteger MaxKDigitsFromLine(string line, int k)
    {
        if (string.IsNullOrWhiteSpace(line) || line.Length < k)
            return BigInteger.Zero;

        var digits = line.Select(c => c - '0').ToArray();
        int n = digits.Length;
        int toRemove = n - k;
        var stack = new List<int>(n);

        for (int i = 0; i < n; i++)
        {
            int d = digits[i];
            while (stack.Count > 0 && toRemove > 0 && stack[stack.Count - 1] < d)
            {
                stack.RemoveAt(stack.Count - 1);
                toRemove--;
            }
            stack.Add(d);
        }

        while (toRemove > 0)
        {
            stack.RemoveAt(stack.Count - 1);
            toRemove--;
        }

        BigInteger value = BigInteger.Zero;
        for (int i = 0; i < k; i++)
        {
            value = value * 10 + stack[i];
        }

        return value;
    }

    public string SolvePart2()
    {
        var total = _input
            .Select(line => MaxKDigitsFromLine(line, 12))
            .Aggregate(BigInteger.Zero, (acc, v) => acc + v);

        return total.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var solver = new DaySolver();

        Console.WriteLine("Day 3");
        Console.WriteLine("Part 1: " + solver.SolvePart1());
        Console.WriteLine("Part 2: " + solver.SolvePart2());
    }
}
