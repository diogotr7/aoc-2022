using System.ComponentModel.DataAnnotations;

namespace AdventOfCode
{
    public class Day10 : ISolution
    {
        public enum Operation {  Noop, Addx };
        
        public readonly record struct Command (Operation Operation, int? Parameter);

        public string Part1(string data)
        {
            var commands = data.Split(Environment.NewLine)
                               .Select(l => l[..4] switch
                                {
                                    "noop" => new Command(Operation.Noop, null),
                                    "addx" => new Command(Operation.Addx, int.Parse(l[5..])),
                                    _ => throw new Exception()
                                }).ToList();
            var cpu = new Cpu(commands);
            cpu.Run();
            var strengths = new int[]
            {
                20, 60, 100, 140, 180, 220
            };
            var sum = strengths.Sum(s => cpu.SignalStrength[s]);
            return "";
        }

        public string Part2(string data)
        {
            var commands = data.Split(Environment.NewLine)
                   .Select(l => l[..4] switch
                   {
                       "noop" => new Command(Operation.Noop, null),
                       "addx" => new Command(Operation.Addx, int.Parse(l[5..])),
                       _ => throw new Exception()
                   }).ToList();
            var screen = new Crt(new Cpu(commands));
            screen.Draw();
            return "";
        }

        public class Cpu
        {
            public int X { get; private set; }
            public int Cycle { get; private set; }
            public List<Command> Commands { get; }
            public Dictionary<int, int> SignalStrength { get; }
            public bool Running => ptr < Commands.Count;

            private int ptr;
            private bool executingAddx;

            public Cpu(List<Command> commands)
            {
                X = 1;
                Cycle = 1;
                Commands = commands;
                SignalStrength = new();
            }

            public void Run()
            {
                while (ptr < Commands.Count)
                {
                    Tick();
                }
            }

            public void Tick()
            {
                if (ptr >= Commands.Count)
                    throw new Exception();

                SignalStrength[Cycle] = Cycle * X;

                var command = Commands[ptr];

                if (executingAddx)
                {
                    X += command.Parameter!.Value;
                    executingAddx = false;
                    ptr++;
                }
                else if (command.Operation == Operation.Noop)
                {
                    ptr++;
                }
                else if (command.Operation == Operation.Addx)
                {
                    executingAddx = true;
                }

                Cycle++;
            }
        }

        public class Crt
        {
            const int WIDTH = 40;
            const int HEIGHT = 6;
            private readonly char[] _pixels;
            private readonly Cpu _cpu;
            
            public Crt(Cpu cpu)
            {
                _cpu = cpu;
                _pixels = new char[HEIGHT * WIDTH];
            }

            public void Draw()
            {
                var cycle = 0;
                while (_cpu.Running)
                {
                    var spriteCenter = _cpu.X;
                    var drawingCoord = (cycle % WIDTH);

                    if (Math.Abs(drawingCoord - spriteCenter) <= 1)
                        _pixels[cycle] = '#';
                    else
                        _pixels[cycle] = '.';

                    ++cycle;
                    _cpu.Tick();
                }

                foreach (var chunk in _pixels.Chunk(WIDTH))
                    Console.WriteLine(string.Concat(chunk));
            }
        }
    }
}
