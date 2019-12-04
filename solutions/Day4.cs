using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace solutions 
{
    public class Day4
    {
        private readonly Regex _regex = new Regex(@"(\d{1})\1", RegexOptions.Compiled);

        public IEnumerable<int> FindMatches(int start, int end) =>
            Enumerable
                .Range(start, end-start)
                .Where(i => {
                    string stringified = i.ToString();
                    string sorted = String.Join(String.Empty, stringified.OrderBy(_ => _));

                    return _regex.IsMatch(stringified) && stringified == sorted;
                });

        public int FindMatchesFast(int start, int end)
        {
            int matches = 0;
            for (int i = start; i < end; i++)
            {
                string stringified = i.ToString();
                if (hasDouble(stringified) && isSequential(stringified))
                {
                    matches++;
                }
            }

            return matches;
        }

        public IEnumerable<int> FindMatchesStrict(int start, int end) =>
            Enumerable
                .Range(start, end-start)
                .Where(i => {
                    string stringified = i.ToString();
                    string sorted = String.Join(String.Empty, stringified.OrderBy(_ => _));
                    
                    return 
                        _regex.IsMatch(stringified)
                        &&
                        sorted.GroupBy(_ => _).Any(grp => grp.Count() == 2)
                        && 
                        stringified == sorted;
                });

        public int FindMatchesStrictFast(int start, int end)
        {
            int matches = 0;
            for (int i = start; i < end; i++)
            {
                string stringified = i.ToString();
                if (hasDoubleStrict(stringified) && isSequential(stringified))
                {
                    matches++;
                }
            }

            return matches;
        }

        bool hasDouble(string stringified) 
        {
            for (int i = 0; i < stringified.Length - 1; i++)
            {
                if (stringified[i] == stringified[i+1])
                    return true;
            }

            return false;
        }

        bool hasDoubleStrict(string stringified) 
        {
            int matches = 1;
            for (int i = 1; i < stringified.Length; i++)
            {
                if (stringified[i] == stringified[i-1])
                {
                    matches++;
                }
                else
                {
                    if (matches == 2)
                    {
                        return true;
                    }

                    matches = 1;
                }
            }

            return matches == 2;
        }

        bool isSequential(string stringified) 
        {
            for (int i = 0; i < stringified.Length - 1; i++)
            {
                if (stringified[i] > stringified[i+1])
                    return false;
            }

            return true;
        }
    }
}