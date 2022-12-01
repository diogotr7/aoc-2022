using System.Linq;

namespace AdventOfCode
{
    public class Day01 : ISolution
    {
        public string Part1(string data)
        {
            var elfCalories = data.Split(Environment.NewLine + Environment.NewLine)
                                  .Select(l => l.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(a => int.Parse(a))
                                                .Sum());

            return elfCalories.Max().ToString();
        }

        public string Part2(string data)
        {
            var elfCalories = data.Split(Environment.NewLine + Environment.NewLine)
                                  .Select(l => l.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(a => int.Parse(a))
                                                .Sum());
            
            return elfCalories.OrderByDescending(d => d).Take(3).Sum().ToString();
        }
    }
}