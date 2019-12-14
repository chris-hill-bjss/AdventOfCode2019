using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace solutions
{
    public class Day12
    {
        private readonly Body[] _bodies;

        public Day12(string[] initialPositions) => 
            _bodies = initialPositions.Select(Body.FromString).ToArray();

        public void SimulateMovements(int iterations)
        {
            int iteration = 0;

            Console.WriteLine($"After {iteration} steps:");
            _bodies.ToList().ForEach(body => Console.WriteLine(body.ToString()));

            while(iteration < iterations)
            {
                foreach(var body in _bodies)
                {
                    body.Interact(_bodies.Except(new[] {body}));
                }

                foreach(var body in _bodies)
                {
                    body.ApplyVelocity();
                }

                Console.WriteLine($"After {++iteration} steps:");
                _bodies.ToList().ForEach(body => Console.WriteLine(body.ToString()));
            }
        
            Console.WriteLine("Total energy in system:");
            Console.WriteLine($"{_bodies.Sum(body => body.Energy.potential * body.Energy.kinetic)}");
        }

        public double SimulateUntilMatch()
        {
            double iteration = 0;
            double[] intersectsAt = new double[] { -1, -1, -1 };

            while (true)
            {
                foreach(var body in _bodies)
                {
                    body.Interact(_bodies.Except(new[] {body}));
                }

                foreach(var body in _bodies)
                {
                    body.ApplyVelocity();
                }

                iteration++;

                if (intersectsAt[0] == -1 && _bodies.All(x => x.Velocity.X == 0))
                    intersectsAt[0] = iteration;

                if (intersectsAt[1] == -1 && _bodies.All(x => x.Velocity.Y == 0))
                    intersectsAt[1] = iteration;

                if (intersectsAt[2] == -1 && _bodies.All(x => x.Velocity.Z == 0))
                    intersectsAt[2] = iteration;

                if (intersectsAt.All(time => time > -1))
                    break;                
            }

            var leastCommonMultiple = LCM(intersectsAt[0], LCM(intersectsAt[1], intersectsAt[2])) * 2;
            return leastCommonMultiple;
        }

        private static double LCM(double m, double n) => Math.Abs(m * n) / GCD(m, n);

        private static double GCD(double a, double b) => b == 0 ? Math.Abs(a) : GCD(b, a % b);

        class Body
        {
            private Body(int x, int y, int z)
            {
                Position = new Vector3(x, y, z);
                Velocity = new Vector3(0, 0, 0);
            }

            public Vector3 Position { get; private set; }
            public Vector3 Velocity { get; private set; }

            public string Info => $"{Position.X}{Position.Y}{Position.Z}{Velocity.X}{Velocity.Y}{Velocity.Z}";

            public (float potential, float kinetic) Energy =>
                (
                    potential: Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z),
                    kinetic: Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z)
                );

            public static Body FromString(string rawPosition)
            {
                string[] coords = rawPosition.Replace(" ", String.Empty).Trim('<','>').Split(',');

                int x = CoordinateToInt(coords[0]);
                int y = CoordinateToInt(coords[1]);
                int z = CoordinateToInt(coords[2]);

                return new Body(x, y, z);
            }

            internal void Interact(IEnumerable<Body> bodies)
            {
                foreach(var body in bodies)
                {
                    Velocity = 
                        new Vector3(
                            Velocity.X + (Position.X != body.Position.X ? Position.X > body.Position.X ? -1 : 1 : 0), 
                            Velocity.Y + (Position.Y != body.Position.Y ? Position.Y > body.Position.Y ? -1 : 1 : 0), 
                            Velocity.Z + (Position.Z != body.Position.Z ? Position.Z > body.Position.Z ? -1 : 1 : 0));
                }
            }

            internal void ApplyVelocity() =>
                Position = new Vector3(Position.X + Velocity.X, Position.Y + Velocity.Y, Position.Z + Velocity.Z);

            public override string ToString() => 
                String.Format($"pos=<x={Position.X}, y={Position.Y}, z={Position.Z}>, vel=<x={Velocity.X}, y={Velocity.Y}, z={Velocity.Z}>");

            private static int CoordinateToInt(string coordinate) => Convert.ToInt16(coordinate[2..]);
        }
    }
}