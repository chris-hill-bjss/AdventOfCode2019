using System;

using FluentAssertions;
using solutions;
using Xunit;

namespace tests
{
    public class Day12Tests
    {
        [Theory]
        [InlineData(
            10,
            new[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" },
            new[] { "<x=2, y=1, z=-3>", "<x=1, y=-8, z=0>", "<x=3, y=-6, z=1>", "<x=2, y=0, z=4>"},
            new[] { "<x=-3, y=-2, z=1>", "<x=-1, y=1, z=3>", "<x=3, y=2, z=-3>", "<x=1, y=-1, z=-1>" }
        )]
        [InlineData(
            100,
            new[] { "<x=-8, y=-10, z=0>","<x=5, y=5, z=10>","<x=2, y=-7, z=3>","<x=9, y=-8, z=-3>" },
            new[] { "<x=  8, y=-12, z= -9>","<x= 13, y= 16, z= -3>","<x=2, y=-7, z=3>","<x=-29, y=-11, z= -1>" },
            new[] { "<x= -7, y=  3, z=  0>","<x=  3, y=-11, z= -5>","<x= -3, y=  7, z=  4>","<x=  7, y=  1, z=  1>" }
        )]
        public void PartOne(int iterations, string[] initialPositions, string[] expectedPositions, string[] expectedVelocities)
        {
            new Day12(initialPositions).SimulateMovements(iterations);
        }

        [Theory]
        [InlineData(
            new[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" },
            2772
        )]
        [InlineData(
            new[] { "<x=-8, y=-10, z=0>","<x=5, y=5, z=10>","<x=2, y=-7, z=3>","<x=9, y=-8, z=-3>" },
            4686774924
        )]
        public void PartTwo(string[] initialPositions, double expectedIterations)
        {
            double iterations = new Day12(initialPositions).SimulateUntilMatch();

            iterations.Should().Be(expectedIterations);
        }
    }
}