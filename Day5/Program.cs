using AdventOfCode.Core;

namespace Day5;

public class DaySolver : IDaySolver
{

    public class Range
    {
        public long Min { get; set; }
        public long Max { get; set; }
    }

    private readonly List<Range> _idsRange;
    private readonly List<long> _input;

    public DaySolver()
    {
        List<string> lines = FileReader.ReadLines(5);
        _idsRange = [];
        _input = [];

        foreach (var line in lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)))
        {
            var parts = line.Split('-');
            long start = long.Parse(parts[0]);
            long end = long.Parse(parts[1]);

            _idsRange.Add(new Range { Min = start, Max = end });
        }

        foreach (var line in lines.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(1))
        {
            if (!string.IsNullOrWhiteSpace(line))
                _input.Add(long.Parse(line));
        }
    }

    public string SolvePart1(params object[] args)
    {
        long count = 0;
        foreach (var id in _input)
        {
            if (_idsRange.Any(r => id >= r.Min && id <= r.Max))
                count++;
        }

        return count.ToString();
    }

    // Piano (pseudocodice dettagliato):
    // 2. Ordinare i range per valore Min crescente.
    // 3. Iterare i range ordinati e unirli in segmenti non sovrapposti:
    //    - Tenere un segmento corrente con start = corrente.Min e end = corrente.Max.
    //    - Per ogni prossimo range:
    //        a) Se next.Min <= currentEnd + 1 (si sovrappone o è contiguo) allora
    //           espandere currentEnd = max(currentEnd, next.Max).
    //        b) Altrimenti (nessuna sovrapposizione):
    //           - Aggiungere la lunghezza del segmento corrente (currentEnd - start + 1)
    //             al totale.
    //           - Impostare il segmento corrente al prossimo range.
    // 4. Dopo il ciclo aggiungere la lunghezza dell'ultimo segmento al totale.
    // 5. Restituire il totale come stringa.

    public string SolvePart2(params object[] args)
    {
        _idsRange.Sort((a, b) => a.Min.CompareTo(b.Min));

        long total = 0;
        long curStart = _idsRange[0].Min;
        long curEnd = _idsRange[0].Max;

        foreach (var r in _idsRange.Skip(1))
        {
            if (r.Min <= curEnd + 1)
            {
                if (r.Max > curEnd)
                    curEnd = r.Max;
            }
            else
            {
                total += (curEnd - curStart + 1);
                curStart = r.Min;
                curEnd = r.Max;
            }
        }

        total += (curEnd - curStart + 1);

        return total.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var solver = new DaySolver();

        Console.WriteLine("Day 5");
        Console.WriteLine("Part 1: " + solver.SolvePart1());
        Console.WriteLine("Part 2: " + solver.SolvePart2());
    }
}
