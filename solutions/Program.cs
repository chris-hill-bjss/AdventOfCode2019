using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace solutions
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // await SolveDay(async () => Console.WriteLine($"Day1, Part1: {await SolveDayOne(useSimpleModel: true)}, Part2: {await SolveDayOne(useSimpleModel: false)}"));
            // await SolveDay(async () => Console.WriteLine($"Day2, Part1: {await SolveDayTwo()}, Part2: {await SolveDayTwoPartTwo()}"));
            // await SolveDay(async () => Console.WriteLine($"Day3, Part1: {await SolveDayThree()}, Part2: {await SolveDayThreePartTwo()}"));
            // await SolveDay(async () => await Task.Run(() => Console.WriteLine($"Day4, Part1: {SolveDayFour()}, Part2: {SolveDayFourPartTwo()}")));
            // await SolveDay(async () => await Task.Run(() => Console.WriteLine($"Day4-Fast, Part1: {SolveDayFourFast()}, Part2: {SolveDayFourPartTwoFast()}")));
            // await SolveDay(async () => Console.WriteLine($"Day5, Part1: {await SolveDayFive()}, Part2: {await SolveDayFivePartTwo()}"));
            // await SolveDay(async () => Console.WriteLine($"Day6, Part1: {await SolveDaySix()}, Part2: {await SolveDaySixPartTwo()}"));
            // await SolveDay(async () => Console.WriteLine($"Day7, Part1: {await SolveDaySeven()}, Part2: {await SolveDaySevenPartTwo()}"));
            // await SolveDay(async () => Console.WriteLine($"Day8, Part1: {await SolveDayEight()}, Part2: {await SolveDayEightPartTwo()}"));

            await SolveDay(async () => Console.WriteLine($"Day9, Part1: {await SolveDayNine()}"));
        }

        private static async Task SolveDay(Func<Task> action)
        {
            var sw = new Stopwatch();
            sw.Start();
            
            await action();
            
            sw.Stop();
            Console.WriteLine($"Executed in {sw.Elapsed}");
        }

        private static async Task<decimal> SolveDayOne(bool useSimpleModel)
        {
            IEnumerable<decimal> input = await ReadInput<decimal>(1, Environment.NewLine);

            return new Day1(useSimpleModel).CalculateRequiredFuel(input);
        }

        private static async Task<int> SolveDayTwo()
        {
            int[] input = (await ReadInput<int>(2, ",")).ToArray();
            input[1] = 12;
            input[2] = 2;

            return new Day2().ProcessIntCode(input)[0];
        }

        private static async Task<String> SolveDayTwoPartTwo()
        {
            int[] originalInput = (await ReadInput<int>(2, ",")).ToArray();
            int[] input = new int[originalInput.Length];
            
            for(int noun = 0; noun <= 99; noun++)
            {
                for(int verb = 0; verb <= 99; verb++)
                {
                    Array.Copy(originalInput, input, originalInput.Length);
                    input[1] = noun;
                    input[2] = verb;

                    int result = new Day2().ProcessIntCode(input)[0];
                    if (result == 19690720)
                    {
                        return $"noun({noun}):verb({verb}):{100 * noun + verb}";
                    }
                }
            }
            
            return "Failed to solve?";
        }

        private static async Task<int> SolveDayThree() 
        {
            string[] input = (await ReadInput<string>(3, Environment.NewLine)).ToArray();

            return new Day3().CalculateClosestIntersection(input);
        }

        private static async Task<int> SolveDayThreePartTwo() 
        {
            string[] input = (await ReadInput<string>(3, Environment.NewLine)).ToArray();

            return new Day3().CalculateStepsToIntersection(input);
        }

        private static int SolveDayFour() =>
            new Day4()
                .FindMatches(158126, 624574)
                .Count();

        private static int SolveDayFourFast() =>
            new Day4()
                .FindMatchesFast(158126, 624574);

        private static int SolveDayFourPartTwo() =>
            new Day4()
                .FindMatchesStrict(158126, 624574)
                .Count();

        private static int SolveDayFourPartTwoFast() =>
            new Day4()
                .FindMatchesStrictFast(158126, 624574);

        private static async Task<int> SolveDayFive()
        {
            int[] input = (await ReadInput<int>(5, ",")).ToArray();

            var outputs = new Day5(1, input).ProcessIntCode();
            
            return outputs.Last();
        }

        private static async Task<int> SolveDayFivePartTwo()
        {
            int[] input = (await ReadInput<int>(5, ",")).ToArray();

            var outputs = new Day5(5, input).ProcessIntCode();
            
            return outputs.Last();
        }

        private static async Task<int> SolveDaySix()
        {
            string[] input = (await ReadInput<string>(6, Environment.NewLine)).ToArray(); 

            return new Day6().CalculateTotalOrbits(input);
        }

        private static async Task<int> SolveDaySixPartTwo()
        {
            string[] input = (await ReadInput<string>(6, Environment.NewLine)).ToArray(); 

            return new Day6().CalculateTransfersRequired(input);
        }

        private static async Task<int> SolveDaySeven()
        {
            int[] input = (await ReadInput<int>(7, ",")).ToArray();

            var results = new Day7(input, 0, 5).ProcessIntCode();
            
            return results.OrderByDescending(r => r.output).First().output;
        }

        private static async Task<int> SolveDaySevenPartTwo()
        {
            int[] input = (await ReadInput<int>(7, ",")).ToArray();

            var results = new Day7(input, 5, 5).ProcessIntCode();
            
            return results.OrderByDescending(r => r.output).First().output;
        }

        private static async Task<int> SolveDayEight()
        {
            string input = (await ReadInput<string>(8, Environment.NewLine)).First();
            
            var layers = new Day8(input, 25, 6).ConvertToLayers().ToArray();

            var layerDetails = 
                layers
                    .Select(s => (
                        zeroes: s.Count(c => c == '0'), 
                        ones: s.Count(c => c == '1'), 
                        twos: s.Count(c => c == '2')))
                    .OrderBy(layer => layer.zeroes);
            
            var checksumLayer = layerDetails.First();

            return checksumLayer.ones * checksumLayer.twos;
        }

        private static async Task<int> SolveDayEightPartTwo()
        {
            string input = (await ReadInput<string>(8, Environment.NewLine)).First();
            
            char[,] image = new Day8(input, 25, 6).DecodeImage();

            for(int y = 0; y <= image.GetUpperBound(0); y++)
            {
                for(int x = 0; x <= image.GetUpperBound(1); x++)
                {
                    Console.Write(image[y,x]);
                }

                Console.WriteLine();
            }

            return image.Length;
        }

        private static async Task<long> SolveDayNine()
        {
            long[] input = (await ReadInput<long>(9, ",")).ToArray();

            var results = new Day9(input).RunProgram();
            
            foreach(var output in results)
            {
                Console.WriteLine($"{output}");
            }

            return results.Last();;
        }

        private static async Task<IEnumerable<T>> ReadInput<T>(int day, String separator)
        {
            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Cookie", "_ga=GA1.2.263501500.1575319642; _gid=GA1.2.344195514.1575319642; session=53616c7465645f5fea3f6b0f0952e18c9a0a858713f2eea45bad3e979f24b75ff5f56f392076e25ce885da2b02d16316");
                
                var response = await client.GetAsync($"https://adventofcode.com/2019/day/{day}/input");
                response.EnsureSuccessStatusCode();

                var rawResponse = await response.Content.ReadAsStringAsync();

                return
                    rawResponse
                        .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                        .Select(item => (T)Convert.ChangeType(item, typeof(T)));
            }
        }
    }
}