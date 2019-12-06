using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions 
{
    public class Day6
    {
        public int CalculateTotalOrbits(string[] orbitalMap)
        {
            var registry = BuildRegistry(orbitalMap);

            int directOrbits = registry.Keys.Count - 1;
            int indirectOrbits = 
                registry
                    .Keys
                    .Where(body => registry[body].parent != null)
                    .Select(body => WalkRegistry(body))
                    .Sum();

            return directOrbits + indirectOrbits;

            int WalkRegistry(string body)
            {
                int parents = 0;

                string currentBody = registry[body].parent;
                while(registry[currentBody].parent != null)
                {
                    parents++;
                    currentBody = registry[currentBody].parent;
                }

                return parents;
            }
        }

        public int CalculateTransfersRequired(string[] orbitalMap)
        {
            var registry = BuildRegistry(orbitalMap);

            var myTransfers = WalkRegistry("YOU");
            var santaTransfers = WalkRegistry("SAN");

            string intersection = myTransfers.Intersect(santaTransfers).First();

            int myDistanceToIntersection = myTransfers.TakeWhile(body => body != intersection).Count();
            int santaDistanceToIntersection = santaTransfers.TakeWhile(body => body != intersection).Count();
            
            return myDistanceToIntersection + santaDistanceToIntersection;

            IEnumerable<string> WalkRegistry(string body)
            {
                var transfers = new List<string>();

                string currentBody = registry[body].parent;
                while(registry[currentBody].parent != null)
                {
                    transfers.Add(currentBody);
                    currentBody = registry[currentBody].parent;
                }

                return transfers;
            }
        }

        private Dictionary<string, (string parent, string child)> BuildRegistry(string[] orbitalMap)
        {
            var registry = new Dictionary<string, (string parent, string child)>();
            foreach(string orbit in orbitalMap)
            {
                string[] bodies = orbit.Split(")", StringSplitOptions.RemoveEmptyEntries);
                string body = bodies[0];
                string satellite = bodies[1];

                if (!registry.ContainsKey(body))
                    registry.Add(body, (null, satellite));

                if (!registry.ContainsKey(satellite))
                    registry.Add(satellite, (body, null));

                (string parent, string _) = registry[body];
                registry[body] = (parent, satellite);

                (string _, string child) = registry[body];
                registry[satellite] = (body, child);
            }

            return registry;
        }
    }
}