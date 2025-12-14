using AdventOfCode.Core;

namespace Day8;

public class DaySolver : IDaySolver
{
    private readonly List<string> _input;

    public DaySolver()
    {
        _input = FileReader.ReadLines(8);
    }

    record Point(int X, int Y, int Z);

    public string SolvePart1(params object[] args)
    {
        var points = _input
                .Select(l => l.Split(','))
                .Select(p => new Point(
                    int.Parse(p[0]),
                    int.Parse(p[1]),
                    int.Parse(p[2])))
                .ToList();

        int n = points.Count;
        var edges = new List<(long dist, int a, int b)>();

        // Generate all pairs
        for (int i = 0; i < n; i++)
            for (int j = i + 1; j < n; j++)
                edges.Add((Dist2(points[i], points[j]), i, j));

        // Sort by distance
        edges.Sort((x, y) => x.dist.CompareTo(y.dist));

        var uf = new UnionFind(n);
        int connectionsToMake = points.Count == 20 ? 10 : 1000;

        for (int i = 0; i < connectionsToMake; i++)
        {
            var (_, a, b) = edges[i];
            uf.Union(a, b);
        }

        var largestThree = uf.ComponentSizes()
            .OrderByDescending(x => x)
            .Take(3)
            .ToList();

        while (largestThree.Count < 3)
            largestThree.Add(1);

        long result = 1;
        foreach (var v in largestThree)
            result *= v;

        return result.ToString();
    }

    private static long Dist2(Point a, Point b)
    {
        long dx = a.X - b.X;
        long dy = a.Y - b.Y;
        long dz = a.Z - b.Z;
        return dx * dx + dy * dy + dz * dz;
    }

    public string SolvePart2(params object[] args)
    {
        var points = _input
            .Select(l => l.Split(','))
            .Select(p => new Point(
                int.Parse(p[0]),
                int.Parse(p[1]),
                int.Parse(p[2])))
            .ToList();

        int n = points.Count;
        var edges = new List<(long dist, int a, int b)>();

        // All pairs
        for (int i = 0; i < n; i++)
            for (int j = i + 1; j < n; j++)
                edges.Add((Dist2(points[i], points[j]), i, j));

        edges.Sort((x, y) => x.dist.CompareTo(y.dist));

        var uf = new UnionFind(n);
        int unions = 0;

        foreach (var (_, a, b) in edges)
        {
            if (uf.Union(a, b))
            {
                unions++;

                // THIS is the moment everything becomes connected
                if (unions == n - 1)
                {
                    long result = (long)points[a].X * points[b].X;
                    return result.ToString();
                }
            }
        }

        return "";
    }

}

class Program
{
    static void Main(string[] args)
    {
        var solver = new DaySolver();

        Console.WriteLine("Day 8");
        Console.WriteLine("Part 1: " + solver.SolvePart1());
        Console.WriteLine("Part 2: " + solver.SolvePart2());
    }
}

class UnionFind
{
    private readonly int[] parent;
    private readonly int[] size;

    public UnionFind(int n)
    {
        parent = new int[n];
        size = new int[n];
        for (int i = 0; i < n; i++)
        {
            parent[i] = i;
            size[i] = 1;
        }
    }

    public int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]);
        return parent[x];
    }

    public bool Union(int a, int b)
    {
        int ra = Find(a);
        int rb = Find(b);
        if (ra == rb) return false;

        if (size[ra] < size[rb])
            (ra, rb) = (rb, ra);

        parent[rb] = ra;
        size[ra] += size[rb];
        return true;
    }

    public IEnumerable<int> ComponentSizes()
    {
        var dict = new Dictionary<int, int>();
        for (int i = 0; i < parent.Length; i++)
        {
            int r = Find(i);
            dict[r] = dict.GetValueOrDefault(r) + 1;
        }
        return dict.Values;
    }
}

