using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;

namespace solutions 
{
    public class Day7
    {
        private readonly int[] _program;
        private readonly int _amplifierStartIndex;
        private readonly int _amplifierCount;

        public Day7(int[] program, int amplifierStartIndex, int amplifierCount)
        {
            _program = program;
            _amplifierStartIndex = amplifierStartIndex;
            _amplifierCount = amplifierCount;
        }

        public IEnumerable<(int output, int[] setting)> ProcessIntCode()
        {
            var results = new List<(int output, int[] setting)>();
            var phaseSettings = CalculatePhaseSettings().ToArray();
            
            foreach(var phaseSetting in phaseSettings)
            {
                int total = 0;
                var inputs = new Stack<int>(new[] {0});
                
                var amplifierStates = 
                    phaseSetting
                        .Select((phase, id) => (
                            id: id,
                            phase: phase, 
                            position: 0, 
                            program: _program.Select(i => i).ToArray(), 
                            isComplete: false))
                        .ToArray();

                var outputs = new List<int>();
                while(amplifierStates.Any(state => !state.isComplete))
                {
                    for(int i = 0; i < amplifierStates.Count(); i++)
                    {  
                        outputs.Clear();
                        var amplifier = amplifierStates[i];

                        if (amplifier.position == 0)
                            inputs.Push(amplifier.phase);

                        (int[] program, int position, bool isComplete) =
                            ProcessInstruction(amplifier.program, inputs, amplifier.position, outputs);

                        amplifierStates[i].program = program;
                        amplifierStates[i].position = position;
                        amplifierStates[i].isComplete = isComplete;

                        outputs.ForEach(output => {
                            total = output;
                            inputs.Push(output);
                        });
                    }
                }
                
                results.Add((total, phaseSetting));
            }

            return results;
        }

        private IEnumerable<int[]> CalculatePhaseSettings() 
        {
            var permutations = new Permutations<int>(Enumerable.Range(_amplifierStartIndex, _amplifierCount).ToArray());
            foreach(var permutation in permutations)
            {
                yield return permutation.ToArray();
            }
        }

        private (int[] program, int position, bool isComplete) ProcessInstruction(int[] program, Stack<int> inputs, int position, ICollection<int> outputs)
        {
            Func<int, int, int> add = (a, b) => a + b;
            Func<int, int, int> multiply = (a, b) => a * b;
            Func<int, int, int> equals = (a, b) => a == b ? 1 : 0;
            Func<int, int, int> lessThan = (a, b) => a < b ? 1 : 0;
            Func<int, int, int, int> jumpIfNotZero = (a, b, c) => a != 0 ? b : c;
            Func<int, int, int, int> jumpIfZero = (a, b, c) => a == 0 ? b : c;

            while (true)
            {
                string opCode = program[position].ToString().PadLeft(5, '0');
                if (opCode == "00099" || opCode == "00003" && inputs.Count == 0)
                    return (program, position, opCode == "00099");

                try
                {
                    position = opCode[^1] switch 
                    {
                        '1' => HandleOperation(opCode, add),
                        '2' => HandleOperation(opCode, multiply),
                        '3' => HandleInput(),
                        '4' => HandleOutput(opCode),
                        '5' => HandleJumpIfCondition(opCode, jumpIfNotZero),
                        '6' => HandleJumpIfCondition(opCode, jumpIfZero),
                        '7' => HandleOperation(opCode, lessThan),
                        '8' => HandleOperation(opCode, equals),
                        _ => throw new NotSupportedException()
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
            
            int HandleOperation(string opCode, Func<int, int, int> operation)
            {
                int paramOne = program[++position];
                int paramTwo = program[++position];
                int store = program[++position];

                if (opCode.Length > 1)
                {
                    char[] paramModes = opCode.Take(3).Reverse().ToArray();

                    int a = paramModes[0] == '0' ? program[paramOne] : paramOne;
                    int b = paramModes[1] == '0' ? program[paramTwo] : paramTwo;
                    
                    program[store] = operation(a, b);
                }
                else
                {
                    program[store] = operation(program[paramOne], program[paramTwo]);
                }

                return ++position;
            }

            int HandleInput()
            {
                int inputLocation = program[++position];
                program[inputLocation] = inputs.Pop();  

                return ++position;              
            }

            int HandleOutput(string opCode)
            {
                if (opCode.Length > 1)
                {
                    char parameterMode = opCode.Reverse().Skip(2).First();

                    int output = parameterMode switch 
                    {
                        '0' => program[program[++position]],
                        '1' => program[++position],
                        _ => throw new NotSupportedException()
                    };
                    
                    outputs.Add(output);
                }
                else
                {
                    outputs.Add(program[program[++position]]);
                }

                return ++position;
            }
        
            int HandleJumpIfCondition(string opCode, Func<int, int, int, int> jumpCondition)
            {
                int paramOne = program[++position];
                int paramTwo = program[++position];

                if (opCode.Length > 1)
                {
                    char[] paramModes = opCode.Take(3).Reverse().ToArray();

                    int a = paramModes[0] == '0' ? program[paramOne] : paramOne;
                    int b = paramModes[1] == '0' ? program[paramTwo] : paramTwo;
                    
                    return jumpCondition(a, b, ++position);
                }
                
                return jumpCondition(program[paramOne], program[paramTwo], ++position);
            }
        }
    }
}