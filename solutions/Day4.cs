using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace solutions 
{
    public class Day4
    {
        public IEnumerable<int> FindMatches(int start, int end) =>
            Enumerable
                .Range(start, end-start)
                .Where(i => {
                    string stringified = i.ToString();
                    string sorted = String.Join(String.Empty, stringified.OrderBy(_ => _));

                    return Regex.IsMatch(stringified, @"(\d{1})\1") && stringified == sorted;
                });

        public IEnumerable<int> FindMatchesStrict(int start, int end) =>
            Enumerable
                .Range(start, end-start)
                .Where(i => {
                    string stringified = i.ToString();
                    string sorted = String.Join(String.Empty, stringified.OrderBy(_ => _));
                    
                    return 
                        Regex.IsMatch(stringified, @"(\d{1})\1")
                        &&
                        sorted.GroupBy(_ => _).Any(grp => grp.Count() == 2)
                        && 
                        stringified == sorted;
                });
    }
}