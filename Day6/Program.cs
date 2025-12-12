using AdventOfCode.Core;
using System.Text;

namespace Day6;

public class DaySolver : IDaySolver
{
    private readonly string[,] _matrix;
    private readonly int _width;
    private readonly int _height;

    public DaySolver()
    {
        var text = FileReader.ReadAsText(6) ?? string.Empty;

        var lines = text.Length == 0
            ? []
            : text.Split([Environment.NewLine], StringSplitOptions.None);

        _height = lines.Length;
        _width = _height == 0 ? 0 : lines.Max(l => l.Length);

        _matrix = new string[_height, _width];

        for (int r = 0; r < _height; r++)
        {
            string line = lines[r].PadRight(_width, ' ');
            for (int c = 0; c < _width; c++)
            {
                _matrix[r, c] = line[c].ToString();
            }
        }
    }

    public string SolvePart1(params object[] args)
    {
        long total = 0;

        int c = 0;
        while (c < _width)
        {
            if (IsEmptyColumn(c))
            {
                c++;
                continue;
            }

            int start = c;
            while (c < _width && !IsEmptyColumn(c))
                c++;
            int end = c - 1;

            List<long> numbers = [];

            for (int r = 0; r < _height - 1; r++)
            {
                string num = "";

                for (int x = start; x <= end; x++)
                {
                    char ch = _matrix[r, x][0];
                    if (char.IsDigit(ch))
                        num += ch;
                    else if (num.Length > 0)
                        break;
                }

                if (num.Length > 0)
                    numbers.Add(long.Parse(num));
            }

            char op = ' ';
            for (int x = start; x <= end; x++)
            {
                char ch = _matrix[_height - 1, x][0];
                if (ch == '+' || ch == '*')
                {
                    op = ch;
                    break;
                }
            }

            long value = op switch
            {
                '+' => numbers.Sum(),
                '*' => numbers.Aggregate(1L, (a, b) => a * b),
                _ => 0
            };

            total += value;
        }

        return total.ToString();
    }

    public string SolvePart2(params object[] args)
    {
        long total = 0;
        int c = _width - 1;

        while (c >= 0)
        {
            if (IsEmptyColumn(c))
            {
                c--;
                continue;
            }

            int start = c;
            while (start >= 0 && !IsEmptyColumn(start))
                start--;
            start++;
            int end = c;

            int numRows = _height - 1;
            int regionWidth = end - start + 1;

            string[] values = new string[numRows];
            for (int r = 0; r < numRows; r++)
            {
                StringBuilder sb = new();
                for (int col = start; col <= end; col++)
                    sb.Append(_matrix[r, col][0]);
                values[r] = sb.ToString();
            }

            char op = ' ';
            for (int col = start; col <= end; col++)
            {
                char ch = _matrix[_height - 1, col][0];
                if (ch == '+' || ch == '*')
                {
                    op = ch;
                    break;
                }
            }

            int numNumbers = values[0].Length;
            long[] numbers = new long[numNumbers];

            for (int i = 0; i < numNumbers; i++)
            {
                StringBuilder sb = new();
                for (int r = 0; r < numRows; r++)
                    sb.Append(values[r][i]);
                numbers[i] = long.Parse(sb.ToString().Trim());
            }

            long value = op switch
            {
                '+' => numbers.Sum(),
                '*' => numbers.Aggregate(1L, (a, b) => a * b),
                _ => 0
            };

            total += value;

            c = start - 1;
        }

        return total.ToString();
    }

    private bool IsEmptyColumn(int c)
    {
        for (int r = 0; r < _height; r++)
            if (_matrix[r, c][0] != ' ')
                return false;
        return true;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var solver = new DaySolver();

        Console.WriteLine("Day 6");
        Console.WriteLine("Part 1: " + solver.SolvePart1());
        Console.WriteLine("Part 2: " + solver.SolvePart2());
    }
}
