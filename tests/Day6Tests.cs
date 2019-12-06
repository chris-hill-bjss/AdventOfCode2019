using FluentAssertions;
using solutions;
using Xunit;

namespace tests
{
    public class Day6Tests
    {
        [Theory]
        [InlineData(new[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L" }, 42)]
        [InlineData(new[] { "G)H", "B)C", "C)D", "D)E", "E)F", "COM)B", "B)G", "D)I", "E)J", "J)K", "K)L", "I)M"}, 47)]
        public void PartOne(string[] orbitalMap, int expectedTotalOrbits) 
        {
            var actualTotalOrbits = new Day6().CalculateTotalOrbits(orbitalMap);

            actualTotalOrbits.Should().Be(expectedTotalOrbits);
        }

        [Theory]
        [InlineData(new[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "I)SAN" }, 4)]
        public void PartTwo(string[] orbitalMap, int expectedTransfers)
        {
            var actualTotalOrbits = new Day6().CalculateTransfersRequired(orbitalMap);

            actualTotalOrbits.Should().Be(expectedTransfers);
        }
    }
}