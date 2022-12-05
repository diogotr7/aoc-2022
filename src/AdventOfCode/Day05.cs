using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day05 : ISolution
    {
        public readonly struct Move
        {
            public int Amount { get; }
            public int Origin { get; }
            public int Destination { get; }

            public Move(int amount, int origin, int destination)
            {
                Amount = amount;
                Origin = origin;
                Destination = destination;
            }

            public static Move Parse(string line)
            {
                var amountString = string.Concat(line.Skip(5).TakeWhile(c => c != ' '));
                var originString = string.Concat(line.Skip(amountString.Length + 11).TakeWhile(c => c != ' '));
                var destinationString = string.Concat(line.Skip(amountString.Length + originString.Length + 15).TakeWhile(c => c != ' '));

                return new Move(
                    int.Parse(amountString),
                    int.Parse(originString) - 1,
                    int.Parse(destinationString) - 1
                );
            }
        }

        public class Drawing
        {
            public int Size { get; }
            public Stack<char>[] Stacks { get; }

            public Drawing(int size)
            {
                Size = size;
                Stacks = new Stack<char>[size];
                for (int i = 0; i < size; i++)
                {
                    Stacks[i] = new Stack<char>();
                }
            }

            public void PerformMoves(IEnumerable<Move> moves)
            {
                foreach (var move in moves)
                {
                    if (Stacks[move.Origin].Count < move.Amount)
                        throw new Exception();

                    for (int i = 0; i < move.Amount; i++)
                    {
                        var item = Stacks[move.Origin].Pop();
                        Stacks[move.Destination].Push(item);
                    }
                }
            }

            public void PerformMovesCrateMover9001(IEnumerable<Move> moves)
            {
                foreach (var move in moves)
                {
                    if (Stacks[move.Origin].Count < move.Amount)
                        throw new Exception();

                    var tempStack = new Stack<char>();

                    for (int i = 0; i < move.Amount; i++)
                    {
                        var item = Stacks[move.Origin].Pop();
                        tempStack.Push(item);
                    }

                    while(tempStack.TryPop(out var item))
                    {
                        Stacks[move.Destination].Push(item);
                    }
                }
            }

            public IEnumerable<char> GetTopOfStacks()
            {
                foreach (var stack in Stacks)
                {
                    yield return stack.Peek();
                }
            }

            public static Drawing Parse(string data)
            {
                var lines = data.Split(Environment.NewLine);
                var size = (lines.Last().Length + 1) / 4;

                var drawing = new Drawing(size);
                for (int i = (lines.Length - 2); i >= 0; i--)
                {
                    var line = lines[i];
                    for (int j = 0; j < size; j++)
                    {
                        char possibleLetter = line[1 + j * 4];
                        if (char.IsLetter(possibleLetter))
                            drawing.Stacks[j].Push(possibleLetter);
                    }
                }

                return drawing;
            }
        }
        

        public string Part1(string data)
        {
            var parts = data.Split(Environment.NewLine + Environment.NewLine);

            var drawing = Drawing.Parse(parts[0]);
            var moves = ParseMoves(parts[1]);

            drawing.PerformMoves(moves);

            return string.Concat(drawing.GetTopOfStacks());
        }

        public string Part2(string data)
        {
            var parts = data.Split(Environment.NewLine + Environment.NewLine);

            var drawing = Drawing.Parse(parts[0]);
            var moves = ParseMoves(parts[1]);

            drawing.PerformMovesCrateMover9001(moves);

            return string.Concat(drawing.GetTopOfStacks());
        }

        private static IEnumerable<Move> ParseMoves(string moves)
        {
            return moves.Split(Environment.NewLine).Select(Move.Parse);
        }

    }
}
