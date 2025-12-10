namespace AdventOfCode.Core;

public static class FileReader
{
    public static List<string> ReadLines(int day)
    {
        var projectDir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

        string path = Path.Combine(projectDir, $"day{day:00}.txt");
        return [.. File.ReadAllLines(path)];
    }

    public static string ReadAsText(int day)
    {
        var projectDir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

        string path = Path.Combine(projectDir, $"day{day:00}.txt");
        return File.ReadAllText(path);
    }

    public static IEnumerable<int> ReadInts(int day)
    {
        return ReadLines(day).Select(int.Parse);
    }
}
