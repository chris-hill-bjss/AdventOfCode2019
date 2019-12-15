using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace solutions
{
    public class Day13
    {
        private enum InstructionType
        {
            PosX,
            PosY,
            TileType
        }

        private enum TileType
        {
            Empty,
            Wall,
            Block,
            Paddle,
            Ball
        }

        private readonly long[] _program;

        public Day13(long[] program)
        {
            Array.Resize(ref program, program.Length * 100);
            _program = program;
        }

        public void RunGame()
        {
            InstructionType currentInstruction = InstructionType.PosX;
            var commands = new List<(PointF position, TileType tile)>();
            (PointF position, TileType tile) currentCommand = (new PointF(0, 0), TileType.Empty);
            long currentScore = 0;

            RunProgram(
                _program,
                0,
                () => {
                    var ball = commands.Last(c => c.tile == TileType.Ball);
                    var paddle = commands.Where(c => c.tile == TileType.Paddle).Last();

                    long movePaddle = 
                        paddle.position.X == ball.position.X 
                        ? 0
                        : paddle.position.X > ball.position.X 
                            ? -1 
                            : 1;

                    return movePaddle;
                },
                value => {
                    currentInstruction = currentInstruction switch
                    {
                        InstructionType.PosX => HandlePosX(value),
                        InstructionType.PosY => HandlePosY(value),
                        InstructionType.TileType => HandleTileType(value),
                        _ => throw new NotSupportedException()
                    };
                });

            foreach(var command in commands)
                Console.WriteLine(command);

            Console.WriteLine(currentScore);

            InstructionType HandlePosX(long value)
            {
                currentCommand.position.X = value;
                return InstructionType.PosY;
            }

            InstructionType HandlePosY(long value)
            {
                currentCommand.position.Y = value;
                return InstructionType.TileType;
            }

            InstructionType HandleTileType(long value)
            {
                if (value <= 4)
                {
                    currentCommand.tile = value switch
                    {
                        0 => TileType.Empty,
                        1 => TileType.Wall,
                        2 => TileType.Block,
                        3 => TileType.Paddle,
                        4 => TileType.Ball,
                        _ => throw new NotSupportedException()
                    };
                }
                else
                {
                    currentScore = value;
                }

                commands.Add(currentCommand);
                currentCommand = (new PointF(0, 0), TileType.Empty);
                return InstructionType.PosX;
            }
        }

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