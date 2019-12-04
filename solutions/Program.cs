using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace solutions
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Day1, Part1: {await SolveDayOne(useSimpleModel: true)}, Part2: {await SolveDayOne(useSimpleModel: false)}");
            Console.WriteLine($"Day2, Part1: {await SolveDayTwo()}, Part2: {await SolveDayTwoPartTwo()}");
            Console.WriteLine($"Day3, Part1: {await SolveDayThree()}, Part2: {await SolveDayThreePartTwo()}");
            Console.WriteLine($"Day4, Part1: {SolveDayFour()}, Part2: {SolveDayFourPartTwo()}");
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

        private static int SolveDayFourPartTwo() =>
            new Day4()
                .FindMatchesStrict(158126, 624574)
                .Count();

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