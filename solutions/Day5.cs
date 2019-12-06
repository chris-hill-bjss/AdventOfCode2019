using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions 
{
    public class Day5
    {
        private readonly int _input;
        private readonly int[] _program;

        public Day5(int input, int[] program)
        {
            _input = input;
            _program = program;
        }

        public IEnumerable<int> ProcessIntCode()
        {
            var outputs = new List<int>();

            ProcessInstruction(0, outputs);

            return outputs;
        }

        private void ProcessInstruction(int position, ICollection<int> outputs)
        {
            while (true)
            {
                string opCode = _program[position].ToString().PadLeft(5, '0');
                if (opCode == "00099")
                    break;

                int instruction = Convert.ToInt16(opCode.Substring(opCode.Length-1));
                
                try
                {
                    position = instruction switch 
                    {
                        1 => HandleAddition(opCode),
                        2 => HandleMultiplication(opCode),
                        3 => HandleInput(),
                        4 => HandleOutput(opCode),
                        5 => HandleJumpIfTrue(opCode),
                        6 => HandleJumpIfFalse(opCode),
                        7 => HandleLessThan(opCode),
                        8 => HandleEquals(opCode),
                        _ => throw new NotSupportedException()
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
            
            int HandleAddition(string opCode)
            {
                int paramOne = _program[++position];
                int paramTwo = _program[++position];
                int store = _program[++position];

                if (opCode.Length > 1)
                {
                    char[] paramModes = opCode.Take(3).Reverse().ToArray();

                    int a = paramModes[0] == '0' ? _program[paramOne] : paramOne;
                    int b = paramModes[1] == '0' ? _program[paramTwo] : paramTwo;
                    
                    _program[store] = a + b;
                }
                else
                {
                    _program[store] = _program[paramOne] + _program[paramTwo];
                }

                return ++position;
            }

            int HandleMultiplication(string opCode)
            {
                int paramOne = _program[++position];
                int paramTwo = _program[++position];
                int store = _program[++position];

                if (opCode.Length > 1)
                {
                    char[] paramModes = opCode.Take(3).Reverse().ToArray();

                    int a = paramModes[0] == '0' ? _program[paramOne] : paramOne;
                    int b = paramModes[1] == '0' ? _program[paramTwo] : paramTwo;
                    
                    _program[store] = a * b;
                }
                else
                {
                    _program[store] = _program[paramOne] * _program[paramTwo];
                }

                return ++position;
            }

            int HandleInput()
            {
                int inputLocation = _program[++position];
                _program[inputLocation] = _input;  

                return ++position;              
            }

            int HandleOutput(string opCode)
            {
                if (opCode.Length > 1)
                {
                    char parameterMode = opCode.Reverse().Skip(2).First();

                    int output = parameterMode switch 
                    {
                        '0' => _program[_program[++position]],
                        '1' => _program[++position],
                        _ => throw new NotSupportedException()
                    };
                    
                    outputs.Add(output);
                }
                else
                {
                    outputs.Add(_program[_program[++position]]);
                }

                return ++position;
            }
        
            int HandleJumpIfTrue(string opCode)
            {
                int paramOne = _program[++position];
                int paramTwo = _program[++position];

                if (opCode.Length > 1)
                {
                    char[] paramModes = opCode.Take(3).Reverse().ToArray();

                    int a = paramModes[0] == '0' ? _program[paramOne] : paramOne;
                    int b = paramModes[1] == '0' ? _program[paramTwo] : paramTwo;
                    
                    return a != 0 ? b : ++position;
                }
                
                return _program[paramOne] != 0 ? _program[paramTwo] : ++position;
            }

            int HandleJumpIfFalse(string opCode)
            {
                int paramOne = _program[++position];
                int paramTwo = _program[++position];

                if (opCode.Length > 1)
                {
                    char[] paramModes = opCode.Take(3).Reverse().ToArray();

                    int a = paramModes[0] == '0' ? _program[paramOne] : paramOne;
                    int b = paramModes[1] == '0' ? _program[paramTwo] : paramTwo;
                    
                    return a == 0 ? b : ++position;
                }
                
                return _program[paramOne] == 0 ? _program[paramTwo] : ++position;
            }

            int HandleLessThan(string opCode)
            {
                int paramOne = _program[++position];
                int paramTwo = _program[++position];
                int store = _program[++position];

                if (opCode.Length > 1)
                {
                    char[] paramModes = opCode.Take(3).Reverse().ToArray();

                    int a = paramModes[0] == '0' ? _program[paramOne] : paramOne;
                    int b = paramModes[1] == '0' ? _program[paramTwo] : paramTwo;
                    
                   _program[store] = a < b ? 1 : 0;
                }
                else
                {
                    _program[store] = _program[paramOne] < _program[paramTwo] ? 1 : 0;
                }

                return ++position;
            }

            int HandleEquals(string opCode)
            {
                int paramOne = _program[++position];
                int paramTwo = _program[++position];
                int store = _program[++position];

                if (opCode.Length > 1)
                {
                    char[] paramModes = opCode.Take(3).Reverse().ToArray();

                    int a = paramModes[0] == '0' ? _program[paramOne] : paramOne;
                    int b = paramModes[1] == '0' ? _program[paramTwo] : paramTwo;
                    
                   _program[store] = a == b ? 1 : 0;
                }
                else
                {
                    _program[store] = _program[paramOne] == _program[paramTwo] ? 1 : 0;
                }

                return ++position;
            }
        }
    }
}