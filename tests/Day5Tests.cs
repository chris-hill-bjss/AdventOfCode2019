using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using solutions;
using Xunit;

namespace tests
{
    public class Day5Tests
    {
        [Theory]
        [InlineData(21, new[] { 3, 0, 1, 0, 0, 0, 1001, 0, 0, 0, 4, 0, 99 }, new[] { 84 })]
        public void PartOne(int input, int[] program, int[] expectedOutput) 
        {
            IEnumerable<int> actualOutput = new Day5(input, program).ProcessIntCode();

            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}