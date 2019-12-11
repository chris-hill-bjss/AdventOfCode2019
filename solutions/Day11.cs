using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace solutions
{
    public class Day11
    {
        private enum Facing
        {
            Up,
            Down,
            Left,
            Right
        }

        private readonly long[] _program;

        public Day11(long[] program)
        {
            Array.Resize(ref program, program.Length * 100);
            _program = program;
        }

        public void PaintAllTheThings()
        {
            var robotPositions = new Dictionary<Point, int>();
            robotPositions.Add(new Point(0, 0), 1);

            var robot = (facing: Facing.Up, location: new Point(0, 0));
            bool commandIsPaint = true;
            int paints = 0;
            RunProgram(
                _program, 
                0,
                () => robotPositions[robot.location],
                command => {
                    switch (commandIsPaint)
                    {
                        case true:
                            robotPositions[robot.location] = (int)command;
                            paints += (int)command;
                            commandIsPaint = false;
                        break;
                        case false:
                            robot.facing = Rotate(robot.facing, (int)command);
                            robot.location = Move(robot.location, robot.facing);

                            if (!robotPositions.ContainsKey(robot.location))
                                robotPositions.Add(robot.location, 0);

                            commandIsPaint = true;
                        break;
                    }
                });

            int x1 = robotPositions.Keys.Min(point => point.X);
            int x2 = robotPositions.Keys.Max(point => point.X);

            int y1 = robotPositions.Keys.Min(point => point.Y);
            int y2 = robotPositions.Keys.Max(point => point.Y);

            for(int y = y1; y <= y2; y++)
            {
                for(int x = x1; x <= x2; x++)
                {
                    var location = new Point(x, y);

                    char colour = 
                        robotPositions.ContainsKey(location)
                        ? robotPositions[location] == 0 ? ' ' : '#'
                        : ' ';
                        
                    Console.Write(colour);
                }
                Console.WriteLine();
            }
        }

        private Point Move(Point location, Facing facing) =>
            facing switch
            {
                Facing.Up => new Point(location.X, location.Y - 1),
                Facing.Left => new Point(location.X - 1, location.Y),
                Facing.Down => new Point(location.X, location.Y + 1),
                Facing.Right => new Point(location.X + 1, location.Y),
                _ => throw new NotSupportedException()
            };

        private Facing Rotate(Facing facing, int command) =>
            (facing, command) switch
            {
                (Facing.Up, 0) => Facing.Left,
                (Facing.Up, 1) => Facing.Right,
                (Facing.Left, 0) => Facing.Down,
                (Facing.Left, 1) => Facing.Up,
                (Facing.Down, 0) => Facing.Right,
                (Facing.Down, 1) => Facing.Left,
                (Facing.Right, 0) => Facing.Up,
                (Facing.Right, 1) => Facing.Down,
                _ => throw new NotSupportedException()
            };

        private (long[] program, long position, bool isComplete) RunProgram(long[] program, long position, Func<long> readInput, Action<long> handleOutput)
        {
            Func<long, long, long> add = (a, b) => a + b;
            Func<long, long, long> multiply = (a, b) => a * b;
            Func<long, long, long> equals = (a, b) => a == b ? 1 : 0;
            Func<long, long, long> lessThan = (a, b) => a < b ? 1 : 0;
            Func<long, long, long, long> jumpIfNotZero = (a, b, c) => a != 0 ? b : c;
            Func<long, long, long, long> jumpIfZero = (a, b, c) => a == 0 ? b : c;

            long relBase = 0;

            while (true)
            {
                string opCode = program[position].ToString().PadLeft(5, '0');
                if (opCode == "00099") // || opCode == "00003" && inputs.Count == 0)
                    return (program, position, opCode == "00099");

                try
                {
                    var parameterModes = opCode[..3].Reverse().ToArray();
                    position = opCode[^1] switch 
                    {
                        '1' => HandleOperation(parameterModes, add),
                        '2' => HandleOperation(parameterModes, multiply),
                        '3' => HandleInput(parameterModes),
                        '4' => HandleOutput(parameterModes),
                        '5' => HandleJumpIfCondition(parameterModes, jumpIfNotZero),
                        '6' => HandleJumpIfCondition(parameterModes, jumpIfZero),
                        '7' => HandleOperation(parameterModes, lessThan),
                        '8' => HandleOperation(parameterModes, equals),
                        '9' => HandleRelBase(parameterModes),
                        _ => throw new NotSupportedException()
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
            
            long HandleOperation(char[] paramModes, Func<long, long, long> operation)
            {
                long a = GetValue(paramModes[0]);
                long b = GetValue(paramModes[1]);

                WriteValue(paramModes[2], operation(a, b));

                return ++position;
            }

            long HandleInput(char[] paramModes)
            {
                WriteValue(paramModes[0], readInput());
                
                return ++position;              
            }

            long HandleOutput(char[] paramModes)
            {
                long output = GetValue(paramModes[0]);
                handleOutput(output);

                return ++position;
            }

            long HandleRelBase(char[] paramModes)
            {
                long offset = GetValue(paramModes[0]);
                relBase += offset;
                
                return ++position;
            }
        
            long HandleJumpIfCondition(char[] paramModes, Func<long, long, long, long> jumpCondition)
            {
                long a = GetValue(paramModes[0]);
                long b = GetValue(paramModes[1]);
                
                return jumpCondition(a, b, ++position);
            }

            long GetValue(char parameterMode) =>
                parameterMode switch 
                {
                    '0' => program[program[++position]],
                    '1' => program[++position],
                    '2' => program[program[++position] + relBase],
                    _ => throw new NotSupportedException()
                };

            void WriteValue(char parameterMode, long value)
            {
                long address = parameterMode switch 
                {
                    '0' => program[++position],
                    '2' => program[++position] + relBase,
                    _ => throw new NotSupportedException()
                };

                program[address] = value;
            }
        }
    }
}