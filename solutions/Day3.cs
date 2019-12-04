using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace solutions 
{
    public class Day3
    {
        private Point _initialPosition;
        public int CalculateClosestIntersection(string[] wirePaths)
        {
            IEnumerable<(int steps, Point position)>[] wirePositions = MapWirePathsToCircuitBoard(wirePaths);

            var pathA = wirePositions[0].Select(t => t.position);
            var pathB = wirePositions[1].Select(t => t.position);

            return
                pathA
                    .Intersect(pathB)
                    .Select(position => Math.Abs(position.X) + Math.Abs(position.Y))
                    .OrderBy(distance => distance)
                    .First();
        }

        public int CalculateStepsToIntersection(string[] wirePaths)
        {
            IEnumerable<(int steps, Point position)>[] wirePositions = MapWirePathsToCircuitBoard(wirePaths);

            var pathA = wirePositions[0];
            var pathB = wirePositions[1];

            var intersections = 
                pathA
                    .Join(
                        pathB,
                        a => a.position,
                        b => b.position,
                        (a, b) => (a, b));

            return
                intersections
                    .Select(t => t.a.steps + t.b.steps)
                    .OrderBy(stepTotal => stepTotal)
                    .First();
        }

        private IEnumerable<(int, Point)>[] MapWirePathsToCircuitBoard(string[] wirePaths) =>
            wirePaths
                .Select(
                    path => 
                    {
                        int x = 0;
                        int y = 0;
                        int steps = 0;
                        return
                            path
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(instruction => (direction: instruction[0], distance: Convert.ToInt16(instruction.Substring(1))))
                                .SelectMany(instruction => 
                                    Enumerable
                                        .Range(0, instruction.distance)
                                        .Select(i => 
                                        {
                                            switch (instruction.direction)
                                            {
                                                case 'U': 
                                                    ++y;
                                                    break;
                                                case 'D':
                                                    --y;
                                                    break;
                                                case 'R':
                                                    ++x;
                                                    break;
                                                case 'L':
                                                    --x;
                                                    break;
                                            }

                                            return (++steps, new Point(x, y));
                                        })
                                );
                    })
                .ToArray();
    }
}