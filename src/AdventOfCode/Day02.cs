using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day02 : ISolution
    {
        private enum RPS
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        private enum Result
        {
            Loss = 0,
            Draw = 3,
            Win = 6
        }

        public string Part1(string data)
        {
            var strategy = data.Split(Environment.NewLine)
                .Select(line => (
                    line[0] switch
                    {
                        'A' => RPS.Rock,
                        'B' => RPS.Paper,
                        'C' => RPS.Scissors,
                        _ => throw new NotImplementedException()
                    },
                    line[2] switch
                    {
                        'X' => RPS.Rock,
                        'Y' => RPS.Paper,
                        'Z' => RPS.Scissors,
                        _ => throw new NotImplementedException()
                    }));

            return strategy.Sum(s => GetScore(s.Item2, s.Item1)).ToString();
        }

        public string Part2(string data)
        {
            var strategy = data.Split(Environment.NewLine)
                 .Select(line => (
                     line[0] switch
                     {
                         'A' => RPS.Rock,
                         'B' => RPS.Paper,
                         'C' => RPS.Scissors,
                         _ => throw new NotImplementedException()
                     },
                     line[2] switch
                     {
                         'X' => Result.Loss,
                         'Y' => Result.Draw,
                         'Z' => Result.Win,
                         _ => throw new NotImplementedException()
                     }));

            return strategy.Sum(s => GetScore(GetMovementForResult(s.Item1, s.Item2), s.Item1)).ToString();
        }

        private static Result PlayRound(RPS me, RPS them)
        {
            if (me == them) return Result.Draw;
            if (me == RPS.Rock && them == RPS.Scissors) return Result.Win;
            if (me == RPS.Paper && them == RPS.Rock) return Result.Win;
            if (me == RPS.Scissors && them == RPS.Paper) return Result.Win;
            return Result.Loss;
        }

        private static int GetScore(RPS me, RPS them)
        {
            var res = PlayRound(me, them);
            return (int)me + (int)res;
        }

        private static RPS GetMovementForResult(RPS me, Result res)
        {
            if (res == Result.Draw) return me;
            if (res == Result.Win)
            {
                if (me == RPS.Rock) return RPS.Paper;
                if (me == RPS.Paper) return RPS.Scissors;
                if (me == RPS.Scissors) return RPS.Rock;
            }
            else
            {
                if (me == RPS.Rock) return RPS.Scissors;
                if (me == RPS.Paper) return RPS.Rock;
                if (me == RPS.Scissors) return RPS.Paper;
            }
            throw new NotImplementedException();
        }
    }
}
