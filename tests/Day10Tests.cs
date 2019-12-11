using System.Drawing;

using FluentAssertions;
using solutions;
using Xunit;

namespace tests
{
    public class Day10Tests
    {
        [Theory]
        [InlineData(new[] {".#..#",".....","#####","....#","...##"}, 3, 4, 8)]
        [InlineData(new[] {"......#.#.","#..#.#....","..#######.",".#.#.###..",".#..#.....","..#....#.#","#..#....#.",".##.#..###","##...#..#.",".#....####"}, 5, 8, 33)]
        [InlineData(new[] {"#.#...#.#.",".###....#.",".#....#...","##.#.#.#.#","....#.#.#.",".##..###.#","..#...##..","..##....##","......#...",".####.###."}, 1, 2, 35)]
        public void PartOne(string[] map, int expectedX, int expectedY, int expectedAsteroids)
        {
            (PointF asteroidPosition, int visibleAsteroids) = new Day10(map).FindBestStationLocation();

            asteroidPosition.X.Should().Be(expectedX);
            asteroidPosition.Y.Should().Be(expectedY);
            visibleAsteroids.Should().Be(expectedAsteroids);
        }

        [Theory]
        //[InlineData(new[] {".#..#",".....","#####","....#","...##"}, 3, 4)]
        [InlineData(new[] {".#..##.###...#######","##.############..##.",".#.######.########.#",".###.#######.####.#.","#####.##.#.##.###.##","..#####..#.#########","####################","#.####....###.#.#.##","##.#################","#####.##.###..####..","..######..##.#######","####.##.####...##..#",".#####..#.######.###","##...#.##########...","#.##########.#######",".####.#.###.###.#.##","....##.##.###..#####",".#.#.###########.###","#.#.#.#####.####.###","###.##.####.##.#..##" }, 11, 13)]
        public void PartTwo(string[] map, int stationX, int stationY)
        {
            new Day10(map).VaporiseAsteroids(new PointF(stationX, stationY));
        }
    }
}