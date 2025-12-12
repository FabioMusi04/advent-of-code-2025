using AdventOfCode.Core;

namespace Day4;

public class DaySolver : IDaySolver
{
    private readonly char[,] _matrix;
    private readonly int _width;
    private readonly int _height;

    public DaySolver()
    {
        var text = FileReader.ReadAsText(4) ?? string.Empty;

        var lines = text.Length == 0
            ? []
            : text.Split([Environment.NewLine], StringSplitOptions.None);

        _height = lines.Length;
        _width = _height == 0 ? 0 : lines.Max(l => l?.Length ?? 0);

        _matrix = new char[_height, _width];

        for (int r = 0; r < _height; r++)
        {
            var line = lines[r] ?? string.Empty;
            for (int c = 0; c < _width; c++)
            {
                _matrix[r, c] = c < line.Length ? line[c] : ' ';
            }
        }
    }

    public string SolvePart1(params object[] args)
    {
        char[,]? matrix = args.Length > 0 ? args[0] as char[,] : null;
        matrix ??= _matrix;

        int canBePicked = 0;
        for (int r = 0; r < _height; r++)
        {
            for (int c = 0; c < _width; c++)
            {
                var currentChar = matrix[r, c];
                if (currentChar == '@')
                {
                    int equalCount = 0;
                    if (r > 0 && matrix[r - 1, c] == currentChar) equalCount++; // up
                    if (r > 0 && c > 0 && matrix[r - 1, c - 1] == currentChar) equalCount++; // up-left
                    if (r > 0 && c < _width - 1 && matrix[r - 1, c + 1] == currentChar) equalCount++; // up-right
                    if (c > 0 && matrix[r, c - 1] == currentChar) equalCount++; // left
                    if (c < _width - 1 && matrix[r, c + 1] == currentChar) equalCount++; // right
                    if (r < _height - 1 && matrix[r + 1, c] == currentChar) equalCount++; // down
                    if (r < _height - 1 && c > 0 && matrix[r + 1, c - 1] == currentChar) equalCount++; // down-left
                    if (r < _height - 1 && c < _width - 1 && matrix[r + 1, c + 1] == currentChar) equalCount++; // down-right

                    if (equalCount < 4)
                    {
                        canBePicked++;
                    }
                }
            }
        }

        return canBePicked.ToString();
    }

    public string SolvePart2(params object[] args)
    {
        char[,]? matrix = args.Length > 0 ? args[0] as char[,] : null;
        matrix ??= _matrix;

        int canBePicked = 0;
        bool anyPicked = false;
        char[,] updatedMatrix = (char[,])matrix.Clone();

        for (int r = 0; r < _height; r++)
        {
            for (int c = 0; c < _width; c++)
            {
                var currentChar = matrix[r, c];
                updatedMatrix[r, c] = currentChar;
                if (currentChar == '@')
                {
                    int equalCount = 0;
                    if (r > 0 && matrix[r - 1, c] == currentChar) equalCount++; // up
                    if (r > 0 && c > 0 && matrix[r - 1, c - 1] == currentChar) equalCount++; // up-left
                    if (r > 0 && c < _width - 1 && matrix[r - 1, c + 1] == currentChar) equalCount++; // up-right
                    if (c > 0 && matrix[r, c - 1] == currentChar) equalCount++; // left
                    if (c < _width - 1 && matrix[r, c + 1] == currentChar) equalCount++; // right
                    if (r < _height - 1 && matrix[r + 1, c] == currentChar) equalCount++; // down
                    if (r < _height - 1 && c > 0 && matrix[r + 1, c - 1] == currentChar) equalCount++; // down-left
                    if (r < _height - 1 && c < _width - 1 && matrix[r + 1, c + 1] == currentChar) equalCount++; // down-right

                    if (equalCount < 4)
                    {
                        canBePicked++;
                        anyPicked = true;
                        updatedMatrix[r, c] = '.';
                    }
                }
            }
        }

        if (anyPicked)
        {
            canBePicked += int.Parse(SolvePart2(updatedMatrix));
        }

        return canBePicked.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var solver = new DaySolver();

        Console.WriteLine("Day 4");
        Console.WriteLine("Part 1: " + solver.SolvePart1());
        Console.WriteLine("Part 2: " + solver.SolvePart2());
    }
}
