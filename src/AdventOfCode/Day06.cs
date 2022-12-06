using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day06 : ISolution
    {
        public string Part1(string data)
        {
            return FindFirstDistinctSequence(data, 4).ToString();
        }

        public string Part2(string data)
        {
            return FindFirstDistinctSequence(data, 14).ToString();
        }

        public static int FindFirstDistinctSequence(ReadOnlySpan<char> chars, int count)
        {
            var seq = new HashSet<char>();
            
            for (int i = 0; i < chars.Length; i++)
            {
                if (!seq.Contains(chars[i]))
                {
                    seq.Add(chars[i]);
                }
                else
                {
                    //we hit a duplicate.
                    //backtrack to just after the
                    //previous appearence of the duplicate
                    i -= (seq.Count - 1);
                    seq.Clear();
                }

                if (seq.Count == count)
                    return i + 1;
            }

            return -1;
        }
    }
}
