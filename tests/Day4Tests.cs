using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using solutions;
using Xunit;

namespace tests
{
    public class Day4Tests
    {
        [Theory]
        [InlineData(111111, true)]
        [InlineData(223450, false)]
        [InlineData(123789, false)]
        public void PartOne(int value, bool expectedMatch) 
        {
            IEnumerable<int> matches = new Day4().FindMatches(value, value+1);

            matches.Any().Should().Be(expectedMatch);
        }

        [Theory]
        [InlineData(112233, true)]
        [InlineData(123444, false)]
        [InlineData(111122, true)]
        public void PartTwo(int value, bool expectedMatch) 
        {
            IEnumerable<int> matches = new Day4().FindMatchesStrict(value, value+1);

            matches.Any().Should().Be(expectedMatch);
        }
    }
}