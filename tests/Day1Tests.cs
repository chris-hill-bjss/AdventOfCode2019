using FluentAssertions;
using solutions;
using Xunit;

namespace tests
{
    public class Day1Tests
    {
        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 654)]
        [InlineData(100756, 33583)]
        public void PartOne(decimal mass, decimal expectedFuelRequired)
        {
            var actualFuelRequired = new Day1().CalculateRequiredFuel(new[] { mass });

            actualFuelRequired.Should().Be(expectedFuelRequired);
        }

        [Theory]
        [InlineData(14, 2)]
        [InlineData(1969, 966)]
        [InlineData(100756, 50346)]
        public void PartTwo(decimal mass, decimal expectedFuelRequired)
        {
            var actualFuelRequired = new Day1(useSimpleModel: false).CalculateRequiredFuel(new[] { mass });

            actualFuelRequired.Should().Be(expectedFuelRequired);
        }
    }
}
