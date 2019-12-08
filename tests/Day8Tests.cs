using System.Linq;
using FluentAssertions;
using solutions;
using Xunit;

namespace tests
{
    public class Day8Tests
    {
        [Theory]
        [InlineData("123456789012", 3, 2, 2)]
        public void PartOne(string digits, int width, int height, int expectedLayerCount) 
        {
            var layers = new Day8(digits, width, height).ConvertToLayers();

            layers.Count().Should().Be(expectedLayerCount);
            layers.First().Should().Be("123456");
            layers.Last().Should().Be("789012");
        }

        [Theory]
        [InlineData("0222112222120000", 2, 2)]
        public void PartTwo(string digits, int width, int height)
        {
            char[,] expectedOutput =  new char[,] {{'0','1'},{'1','0'}};

            var output = new Day8(digits, width, height).DecodeImage();

            output.Should().BeEquivalentTo(expectedOutput);
        }
    }
}