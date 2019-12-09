using System;
using System.Linq;
using System.Collections.Generic;

namespace solutions
{
    public class Day9
    {
        private readonly long[] _program;

        public Day9(long[] program)
        {
            Array.Resize(ref program, program.Length * 100);
            _program = program;
        }

        public IEnumerable<long> RunProgram()
        {
            var outputs = new List<long>();

            RunProgram(_program.Select(instruction => instruction).ToArray(), new Stack<long>(new long[]{ 2 }), 0, outputs);

            return outputs;
        }

        private (long[] program, long position, bool isComplete) RunProgram(long[] program, Stack<long> inputs, long position, ICollection<long> outputs)
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
                if (opCode == "00099" || opCode == "00003" && inputs.Count == 0)
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
                WriteValue(paramModes[0], inputs.Pop());
                
                return ++position;              
            }

            long HandleOutput(char[] paramModes)
            {
                long output = GetValue(paramModes[0]);
                outputs.Add(output);

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