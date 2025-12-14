using AdventOfCode.Core;

namespace Day7;

public class DaySolver : IDaySolver
{
    private readonly char[,] _matrix;
    private readonly int _width;
    private readonly int _height;

    private Dictionary<(int, int), long> beamCounts;
    private Queue<(int, int)> queue;

    public DaySolver()
    {
        var text = FileReader.ReadAsText(7) ?? string.Empty;

        var lines = text.Length == 0
            ? []
            : text.Split([Environment.NewLine], StringSplitOptions.None);

        _height = lines.Length;
        _width = _height == 0 ? 0 : lines.Max(l => l.Length);

        _matrix = new char[_height, _width];

        for (int r = 0; r < _height; r++)
        {
            string line = lines[r].PadRight(_width, ' ');
            for (int c = 0; c < _width; c++)
            {
                _matrix[r, c] = line[c];
            }
        }

        queue = new Queue<(int, int)>();
        beamCounts = [];
    }

    public string SolvePart1(params object[] args)
    {
        int totalSplits = 0;
        beamCounts = [];
        queue = new Queue<(int, int)>();

        var clonedMatrix = (char[,])_matrix.Clone();

        var processed = new HashSet<(int, int)>();

        for (int c = 0; c < _width; c++)
        {
            if (clonedMatrix[0, c] == 'S')
            {
                var start = (0, c);
                queue.Enqueue(start);
                beamCounts[start] = 1;
                break;
            }
        }

        while (queue.Count > 0)
        {
            var pos = queue.Dequeue();

            // Skip if this cell already processed
            if (processed.Contains(pos)) continue;
            processed.Add(pos);

            long count = beamCounts[pos];

            int row = pos.Item1 + 1; // move down
            int col = pos.Item2;

            if (row >= _height) continue;

            char ch = clonedMatrix[row, col];

            switch (ch)
            {
                case '.':
                case 'S':
                    clonedMatrix[row, col] = '|';
                    EnqueueBeam((row, col), count);
                    break;

                case '^':
                    totalSplits++; // each splitter counts once
                    int left = col - 1;
                    int right = col + 1;

                    if (left >= 0)
                    {
                        clonedMatrix[row, left] = '|';
                        EnqueueBeam((row, left), 1);
                    }

                    if (right < _width)
                    {
                        clonedMatrix[row, right] = '|';
                        EnqueueBeam((row, right), 1);
                    }
                    break;
            }
        }

        return totalSplits.ToString();
    }

    record Beam(int Row, int Col);

    public string SolvePart2(params object[] args)
    {
        long[,] timelineCount = new long[_height, _width];

        // find starting position 'S'
        for (int c = 0; c < _width; c++)
        {
            if (_matrix[0, c] == 'S')
            {
                timelineCount[0, c] = 1;
                break;
            }
        }

        for (int r = 0; r < _height; r++)
        {
            for (int c = 0; c < _width; c++)
            {
                long count = timelineCount[r, c];
                if (count == 0) continue;

                int nextRow = r + 1;
                if (nextRow >= _height)
                    continue;

                char ch = _matrix[nextRow, c];
                if (ch == '^')
                {
                    int left = c - 1;
                    int right = c + 1;
                    if (left >= 0) timelineCount[nextRow, left] += count;
                    if (right < _width) timelineCount[nextRow, right] += count;
                }
                else
                {
                    timelineCount[nextRow, c] += count;
                }
            }
        }

        long totalTimelines = 0;
        for (int c = 0; c < _width; c++)
            totalTimelines += timelineCount[_height - 1, c];

        return totalTimelines.ToString();
    }

    void EnqueueBeam((int, int) p, long c)
    {
        if (!beamCounts.ContainsKey(p)) beamCounts[p] = c;
        else beamCounts[p] += c;

        if (!queue.Contains(p)) queue.Enqueue(p);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var solver = new DaySolver();

        Console.WriteLine("Day 7");
        Console.WriteLine("Part 1: " + solver.SolvePart1());
        Console.WriteLine("Part 2: " + solver.SolvePart2());
    }
}
