using System.Diagnostics;

namespace AdventOfCode
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ISolution solution = new Day07();

            var name = solution.GetType().Name;
            var data = File.ReadAllText(Path.Combine(name, name + ".txt"));
            Console.WriteLine(name);
            var p1 = RunTimed(() => solution.Part1(data));
            var p2 = RunTimed(() => solution.Part2(data));
            Console.WriteLine($"Part 1: {p1.Item2}. Took: {p1.Item1}");
            Console.WriteLine($"Part 2: {p2.Item2}. Took: {p2.Item1}");
        }
        
        public static (TimeSpan, T) RunTimed<T>(Func<T> action)
        {
            var sw = Stopwatch.StartNew();
            var ret = action();
            sw.Stop();
            return (sw.Elapsed, ret);
        }
    }
}