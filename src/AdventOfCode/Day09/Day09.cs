using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdventOfCode
{
    public class Day09 : ISolution
    {
        public string Part1(string data)
        {
            var commands = data.Split(Environment.NewLine)
                               .Select(s =>
                               {
                                   var d = s[0] switch
                                   {
                                       'U' => Direction.Up,
                                       'D' => Direction.Down,
                                       'L' => Direction.Left,
                                       'R' => Direction.Right,
                                       _ => throw new NotImplementedException(),
                                   };
                                   var steps = int.Parse(s[1..]);
                                   return (d, steps);
                               });
            var simulator = new RopeSimulator(2);

            foreach (var (direction, steps) in commands)
                simulator.PerformMove(direction, steps);

            return simulator.VisitedPositions.Count.ToString();
        }

        public string Part2(string data)
        {
            var commands = data.Split(Environment.NewLine)
                   .Select(s =>
                   {
                       var d = s[0] switch
                       {
                           'U' => Direction.Up,
                           'D' => Direction.Down,
                           'L' => Direction.Left,
                           'R' => Direction.Right,
                           _ => throw new NotImplementedException(),
                       };
                       var steps = int.Parse(s[1..]);
                       return (d, steps);
                   });
            var simulator = new RopeSimulator(10);

            foreach (var (direction, steps) in commands)
                simulator.PerformMove(direction, steps);

            return simulator.VisitedPositions.Count.ToString();
        }

        public readonly record struct Position(int X, int Y);

        public enum Direction { Up, Down, Left, Right }

        public class RopeSimulator
        {
            public readonly HashSet<Position> VisitedPositions;
            public Position[] Pieces;

            public RopeSimulator(int ropeSize)
            {
                VisitedPositions = new();
                VisitedPositions.Add(new Position(0,0));

                Pieces = new Position[ropeSize];
            }

            public void PerformMove(Direction direction, int steps)
            {
                for (int i = 0; i < steps; i++)
                {
                    Pieces[0] = direction switch
                    {
                        Direction.Up => new Position(Pieces[0].X + 1, Pieces[0].Y),
                        Direction.Down => new Position(Pieces[0].X - 1, Pieces[0].Y),
                        Direction.Left => new Position(Pieces[0].X, Pieces[0].Y - 1),
                        Direction.Right => new Position(Pieces[0].X, Pieces[0].Y + 1),
                        _ => throw new ArgumentException()
                    };

                    AdjustTail();
                }
            }

            private void AdjustTail()
            {
                for (int i = 0; i < Pieces.Length - 1; i++)
                {
                    //same position, don't do anything else
                    if (Pieces[i] == Pieces[i + 1])
                        return;

                    //already touching, do nothing
                    if (Math.Abs(Pieces[i].X - Pieces[i + 1].X) <= 1 &&
                        Math.Abs(Pieces[i].Y - Pieces[i + 1].Y) <= 1)
                        return;

                    //same vertical line, move up / down
                    if (Pieces[i].X == Pieces[i + 1].X)
                    {
                        if (Pieces[i].Y > Pieces[i + 1].Y)
                            Pieces[i + 1] = new Position(Pieces[i + 1].X, Pieces[i + 1].Y + 1);
                        else
                            Pieces[i + 1] = new Position(Pieces[i + 1].X, Pieces[i + 1].Y - 1);
                    }

                    //same horizontal line, move left / right
                    else if (Pieces[i].Y == Pieces[i + 1].Y)
                    {
                        if (Pieces[i].X > Pieces[i + 1].X)
                            Pieces[i + 1] = new Position(Pieces[i + 1].X + 1, Pieces[i + 1].Y);
                        else
                            Pieces[i + 1] = new Position(Pieces[i + 1].X - 1, Pieces[i + 1].Y);
                    }

                    else
                    {
                        if (Pieces[i].X > Pieces[i + 1].X && Pieces[i].Y > Pieces[i + 1].Y)
                            Pieces[i + 1] = new Position(Pieces[i + 1].X + 1, Pieces[i + 1].Y + 1);
                        else if (Pieces[i].X > Pieces[i + 1].X && Pieces[i].Y < Pieces[i + 1].Y)
                            Pieces[i + 1] = new Position(Pieces[i + 1].X + 1, Pieces[i + 1].Y - 1);
                        else if (Pieces[i].X < Pieces[i + 1].X && Pieces[i].Y > Pieces[i + 1].Y)
                            Pieces[i + 1] = new Position(Pieces[i + 1].X - 1, Pieces[i + 1].Y + 1);
                        else if (Pieces[i].X < Pieces[i + 1].X && Pieces[i].Y < Pieces[i + 1].Y)
                            Pieces[i + 1] = new Position(Pieces[i + 1].X - 1, Pieces[i + 1].Y - 1);
                    }
                }

                VisitedPositions.Add(Pieces[^1]);
            }
        }
    }
}
