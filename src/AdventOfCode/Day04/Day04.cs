using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day04 : ISolution
    {
        public string Part1(string data)
        {
            return ProcessData(data).Count(x => FullyContains(x.Item1, x.Item2)).ToString();
        }

        public string Part2(string data)
        {
            return ProcessData(data).Count(x => AnyOverlap(x.Item1, x.Item2)).ToString();
        }

        public static IEnumerable<(Range, Range)> ProcessData(string data)
        {
            return data.Split(Environment.NewLine)
                       .Select(l =>
                       {
                           var elves = l.Split(',');
                           var elf0 = elves[0].Split('-');
                           var elf1 = elves[1].Split('-');
                           return (
                               new Range(int.Parse(elf0[0]), int.Parse(elf0[1])),
                               new Range(int.Parse(elf1[0]), int.Parse(elf1[1]))
                           );
                       });
        }

        public static bool FullyContains(Range a, Range b)
        {
            return (a.Start.Value <= b.Start.Value && a.End.Value >= b.End.Value)
                || (b.Start.Value <= a.Start.Value && b.End.Value >= a.End.Value);
        }

        public static bool AnyOverlap(Range a, Range b)
        {
            return (a.Start.Value <= b.End.Value && a.End.Value >= b.Start.Value)
                || (b.Start.Value <= a.End.Value && b.End.Value >= a.Start.Value);
        }
    }
}
