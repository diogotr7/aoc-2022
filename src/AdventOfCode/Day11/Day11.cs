
using System.Diagnostics;

namespace AdventOfCode
{
    public class Day11 : ISolution
    {
        public enum Operator
        {
            Add,
            Multiply,
        }

        public class Operation 
        {
            public Operator Operator { get; }
            public long? Value { get; }

            public Operation(Operator @operator, long? value)
            {
                Operator = @operator;
                Value = value;
            }

        }

        public class Monkey
        {
            public int TrueDestination { get; }
            public int FalseDestination { get; }
            public Queue<long> Items { get; }
            public Operation Operation { get; }
            public int Divisor { get; }
            public int Inspected { get; set; }

            public Monkey(List<long> items, Operation op, int divisor, int trueInt, int falseInt)
            {
                Operation = op;
                Divisor = divisor;
                TrueDestination = trueInt;
                FalseDestination = falseInt;

                Items = new();
                foreach (var item in items)
                    Items.Enqueue(item);
            }

            public void AddItem(long worryLevel)
            {
                Items.Enqueue(worryLevel);
            }

            public long PerformOperation(long worry) => Operation.Operator switch
            {
                Operator.Add => worry + (Operation.Value ?? worry),
                Operator.Multiply => worry * (Operation.Value ?? worry),
                _ => throw new NotImplementedException()
            };

            public int GetTargetMonkey(long worry) => worry % Divisor == 0 ? TrueDestination : FalseDestination;
        }

        public class MonkeyChase
        {
            private readonly Monkey[] _monkeys;
            private readonly int _rounds;
            private readonly long? _superModulo;

            public MonkeyChase(Monkey[] monkeys, int rounds, long? superModulo = null)
            {
                _monkeys = monkeys;
                _rounds = rounds;
                _superModulo = superModulo;
            }

            public void Run()
            {
                for (int round = 0; round < _rounds; round++)
                {
                    foreach (var monkey in _monkeys)
                    {
                        while (monkey.Items.TryDequeue(out var item))
                        {
                            monkey.Inspected++;

                            if (_superModulo.HasValue)
                                item = monkey.PerformOperation(item) % _superModulo.Value;
                            else
                                item = monkey.PerformOperation(item) / 3;

                            var targetMonkey = monkey.GetTargetMonkey(item);

                            _monkeys[targetMonkey].AddItem(item);
                        }
                    }
                }
            }

            public long GetMonkeyBusiness()
            {
                var highest = _monkeys.Select(a => a.Inspected)
                               .OrderByDescending(i => i)
                               .Take(2).ToArray();

                return highest[0] * (long)highest[1];
            }
        }

        public string Part1(string data)
        {
            var monkeys = Parse(data).ToArray();

            var monkeyChase = new MonkeyChase(monkeys, 20);
            monkeyChase.Run();

            var business = monkeyChase.GetMonkeyBusiness();

            return business.ToString();
        }

        public string Part2(string data)
        {
            var monkeys = Parse(data).ToArray();

            long superModulo = 1;
            foreach (var item in monkeys)
                superModulo *= item.Divisor;

            var monkeyChase = new MonkeyChase(monkeys, 10000, superModulo);
            monkeyChase.Run();

            var business = monkeyChase.GetMonkeyBusiness();

            return business.ToString();
        }

        public static IEnumerable<Monkey> Parse(string data)
        {
            return data.Split(Environment.NewLine + Environment.NewLine)
                      .Select(m =>
                      {
                          var lines = m.Split(Environment.NewLine);

                          var items = lines[1][18..].Split(',').Select(a => long.Parse(a)).ToList();
                          var operationText = lines[2][23..];
                          var op = operationText[0] switch
                          {
                              '+' => Operator.Add,
                              '*' => Operator.Multiply,
                              _ => throw new FormatException()
                          };
                          int? operationValue = int.TryParse(operationText[2..], out var gay) ? gay : null;
                          var operation = new Operation(op, operationValue);
                          var divisible = int.Parse(lines[3][21..]);
                          var trueMonkey = int.Parse(lines[4][28..]);
                          var falseMonkey = int.Parse(lines[5][29..]);

                          return new Monkey(items, operation, divisible, trueMonkey, falseMonkey);
                      });
        }
    }
}
