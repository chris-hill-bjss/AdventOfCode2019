using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace solutions
{
    public class Day10
    {
        private readonly string[] _map;

        public Day10(string[] map) => _map = map;

        public (PointF baseLocation, int visibleAsteroids) FindBestStationLocation()
        {
            IEnumerable<PointF> asteroidPositions = FindAsteroids();

            var linesOfSight = 
                asteroidPositions
                    .Select(proposedBase => (
                        position: proposedBase,
                        visible: GetVectorsFromProposedBase(proposedBase, asteroidPositions.Except(new[] {proposedBase})).Count()
                        )
                    )
                    .OrderByDescending(position => position.visible)
                    .ToArray();

            var proposedBase = linesOfSight.First();

            return (proposedBase.position, proposedBase.visible);
        }

        public void VaporiseAsteroids(PointF stationLocation)
        {
            var asteroidsToDestroy = FindAsteroids().Except(new[] {stationLocation}).ToList();
            var asteroidsOnVectors = GetVectorsFromProposedBase(stationLocation, asteroidsToDestroy).OrderByDescending(grp => grp.Key).ToArray();

            var targetList = new LinkedList<IGrouping<double, (PointF location, double distance, double radians)>>(asteroidsOnVectors);
            LinkedListNode<IGrouping<double, (PointF location, double distance, double radians)>> current = null;
            
            Func<LinkedListNode<IGrouping<double, (PointF location, double distance, double radians)>>> getNextTargetVector = () =>
            {
                current = 
                    current == null
                    ? targetList.Find(asteroidsOnVectors.First(grp => grp.Key == 0))
                    : current.Next ?? targetList.First;

                return current;
            };

            int destroyed = 0;
            while(asteroidsToDestroy.Any())
            {
                var grp = getNextTargetVector();   

                foreach(var target in grp.Value.OrderBy(target => target.distance))
                {
                    if (asteroidsToDestroy.Contains(target.location))
                    {
                        asteroidsToDestroy.Remove(target.location);
                        destroyed++;
                        
                        Console.WriteLine($"{destroyed}:{target.location}");

                        break;
                    }
                }
            }
        }

        private IEnumerable<PointF> FindAsteroids() =>
            _map
                .SelectMany((row, y) => row.Select((c, x) => (c, location: new PointF(x, y))))
                .Where(tuple => tuple.c == '#')
                .Select(tuple => tuple.location);             

        private IEnumerable<IGrouping<double, (PointF location, double distance, double radians)>> GetVectorsFromProposedBase(PointF proposedBase, IEnumerable<PointF> asteroids) =>
            asteroids
                .Select(asteroid => {
                    double distanceToAsteroid = GetDistance(proposedBase, asteroid);

                    double deltaY = proposedBase.Y - asteroid.Y;
                    double deltaX = proposedBase.X - asteroid.X;

                    double radians = Math.Atan2(deltaX, deltaY);
                    
                    return (asteroid, distanceToAsteroid, radians);
                })
                .GroupBy(a => a.radians);
         
        private double GetDistance(PointF a, PointF b) => Math.Sqrt(Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.X - b.X, 2));

        private bool HasDirectLineOfSight(double gradient, double intercept, IEnumerable<PointF> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}