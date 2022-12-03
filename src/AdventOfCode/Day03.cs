using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day03 : ISolution
    {
        public string Part1(string data)
        {
            return data.Split(Environment.NewLine)
                       .Select(x => x.Chunk(x.Length / 2))   
                       .Select(y => y.First().Intersect(y.Last()).Single())
                       .Select(z => GetPriority(z))
                       .Sum()
                       .ToString();
        }

        public string Part2(string data)
        {
            return data.Split(Environment.NewLine)
                       .Chunk(3)
                       .Select(x => x.Aggregate((a,b) => string.Concat(a.Intersect(b))).Single())
                       .Select(y => GetPriority(y))
                       .Sum()
                       .ToString();
        }
        
        public static int GetPriority(char c) => c switch
        {
            >= 'a' and <= 'z' => c - 'a' + 1,
            >= 'A' and <= 'Z' => c - 'A' + 27,
            _ => throw new Exception("Invalid item")
        };
    }
}
