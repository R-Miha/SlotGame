using System;
using System.Linq;

namespace SlotGame
{
    class SlotGame
    {

        static decimal[] probability = new decimal[8];
        static decimal[] payout = new decimal[8];
        static void Main(string[] args)
        {
            ReadSymbolData("Resources/Symbol data.csv");

            List<ReelCombination> reelCombinations = GenerateReelCombinations();

            // calculate baseRTP, which ignores scatters for now.
            decimal baseRTP = CalculateBaseRTP(reelCombinations);
            Console.WriteLine("BaseRTP is: " + baseRTP);

            // EV of free rolls awarded by scatters
            decimal numberOfFreeRolls = CalculateFreeRolls(reelCombinations);
            Console.WriteLine("Expected value of free rolls is: " + numberOfFreeRolls);

            // RTP = baseRTP + (numberOfFreeRolls * RTP), and we solve for RTP
            decimal RTP = baseRTP / (1 - numberOfFreeRolls);
            Console.WriteLine("RTP is: " + RTP);
        }
        private static decimal CalculateBaseRTP(List<ReelCombination> reelCombinations)
        {
            decimal sum = 0;
            // we go through every possible board, calculate the payout it gives and the probability of it occuring. 
            foreach (var i in reelCombinations)
            {
                foreach (var j in reelCombinations)
                {
                    foreach (var k in reelCombinations)
                    {
                        decimal payout = calculatePayout(i.occurences, j.occurences, k.occurences);
                        decimal boardProbability = i.probability * j.probability * k.probability;
                        sum += payout * boardProbability;
                    }
                }
            }

            return sum;
        }

        private static decimal CalculateFreeRolls(List<ReelCombination> reelCombinations)
        {
            decimal sum = 0;
            // we go through every possible board, calculate the free rolls it gives and the probability of it occuring. 
            foreach (var i in reelCombinations)
            {
                foreach (var j in reelCombinations)
                {
                    foreach (var k in reelCombinations)
                    {
                        decimal payout = 5 * i.occurences[0] * j.occurences[0] * k.occurences[0];
                        decimal boardProbability = i.probability * j.probability * k.probability;
                        sum += payout * boardProbability;
                    }
                }
            }

            return sum;
        }


        // left, middle, right
        private static decimal calculatePayout(int[] l, int[] m, int[] r)
        {
            decimal sum = 0;
            // we do wilds first, because we need the number of ways with 3 wilds in calculations later
            decimal wildHits = l[1] * m[1] * r[1];
            sum += wildHits * payout[1];
            // starting at 2, because both of the first symbols are special (scatter, wild2x)
            for (int i = 2; i < 8; i++)
            {
                // hits is the number of times we have to pay out on symbol i. This includes the wild multipliers
                // at the end we substract 8 * wildHits, because we don't want to count a wildway as a way where all of the wilds were replaced by another symbol.
                decimal hits = (l[i] + 2 * l[1]) * (m[i] + 2 * m[1]) * (r[i] + 2 * r[1]) - 8 * (wildHits);
                // decimal hits = l[i] * m[i] * r[i];
                // decimal hits = ((l[1] * m[i] * r[i]) + (l[i] * m[1] * r[i]) + (l[i] * m[i] * r[1])) * 2;
                sum += hits * payout[i];
            }
            return sum;
        }

        private static List<ReelCombination> GenerateReelCombinations()
        {
            List<ReelCombination> result = new();
            for (int i = 0; i < 8; i++)
            {
                for (int j = i; j < 8; j++)
                {
                    for (int k = j; k < 8; k++)
                    {
                        int permutations = 6;
                        if (i == j && j == k)
                        {
                            permutations = 1;
                        }
                        else if (i == j || j == k)
                        {
                            permutations = 3;
                        }
                        else
                        {
                            permutations = 6;
                        }
                        decimal probabilityCombination = probability[i] * probability[j] * probability[k] * permutations;
                        ReelCombination reel = new(i, j, k, probabilityCombination);
                        result.Add(reel);
                    }
                }
            }
            return result;
        }

        private static void ReadSymbolData(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            for (int i = 1; i < 9; i++)
            {
                var split = lines[i].Split(";");
                probability[i - 1] = decimal.Parse(split[1]);
                if (i > 1)
                {
                    payout[i - 1] = decimal.Parse(split[2]);
                }
            }
        }
    }

    class ReelCombination
    {
        public int[] occurences = new int[8];
        public decimal probability;
        public ReelCombination(int i, int j, int k, decimal probability)
        {
            occurences[i]++;
            occurences[j]++;
            occurences[k]++;

            this.probability = probability;
        }
    }
}