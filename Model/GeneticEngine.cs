using System;
using System.Collections.Generic;
using System.Linq;
using MyMasterMind.Interfaces;

namespace MyMasterMind.Model
{
	/// <summary>
	/// Genetic search for a guess consistent with the guesses so far. Evolves a
	/// population of candidate codes; the fitness of a candidate is the total
	/// deviation of its comparison against each previous guess from the recorded
	/// evaluation (0 = consistent). The search is allowed to fail: after
	/// MaxGenerations the best individual found so far is used, even if it is
	/// not consistent. Each individual carries the origin of its genes (parent,
	/// mutation, ...) so the recombination can be visualized.
	/// </summary>
	public class GeneticEngine : IGeneticGenerationInfo
	{
		public const int PopulationSize = 60;
		public const int MaxGenerations = 30;
		private const int EliteCount = 2;
		private const int TournamentSize = 3;
		private const double MutationRate = 0.15;

		private static readonly Random Random = new Random();

		private class Individual
		{
			public Code Code;
			public GeneticGeneOrigin[] Origins;
			public int CrossoverPoint;
			public int Fitness;

			public static Individual WithUniformOrigin(Code code, GeneticGeneOrigin origin)
			{
				return new Individual
				{
					Code = code,
					Origins = Enumerable.Repeat(origin, MyMasterMindConstants.Columns).ToArray(),
					CrossoverPoint = 0,
				};
			}
		}

		private readonly List<Guess> PreviousGuesses;
		private List<Individual> Population;

		public int Generation { get; private set; }
		public int BestFitness { get; private set; }
		public bool Consistent => BestFitness == 0;
		public Code Best { get; private set; }
		public GeneticGeneOrigin[] BestOrigins { get; private set; }
		public int BestCrossoverPoint { get; private set; }

		public GeneticEngine(List<Guess> previousGuesses)
		{
			PreviousGuesses = previousGuesses;
			Population = Enumerable.Range(0, PopulationSize)
				.Select(_ => Individual.WithUniformOrigin(Code.GetRandomCode(), GeneticGeneOrigin.Random))
				.ToList();
			Generation = 0;
			Best = Population[0].Code;
			BestOrigins = Population[0].Origins;
			BestCrossoverPoint = 0;
			BestFitness = int.MaxValue;
		}

		/// <summary>
		/// Evolve by one generation. Returns true when the search is finished:
		/// the best individual is consistent or the generation limit is reached.
		/// </summary>
		public bool NextGeneration()
		{
			Generation++;

			foreach (Individual individual in Population)
				individual.Fitness = Fitness(individual.Code);
			List<Individual> scored = Population.OrderBy(individual => individual.Fitness).ToList();

			Best = scored[0].Code;
			BestFitness = scored[0].Fitness;
			BestOrigins = scored[0].Origins;
			BestCrossoverPoint = scored[0].CrossoverPoint;

			if (Consistent || Generation >= MaxGenerations)
				return true;

			List<Individual> next = scored.Take(EliteCount)
				.Select(elite => Individual.WithUniformOrigin(elite.Code.Copy(), GeneticGeneOrigin.Carried))
				.ToList();
			while (next.Count < PopulationSize)
				next.Add(Breed(Tournament(scored), Tournament(scored)));
			Population = next;

			return false;
		}

		private int Fitness(Code candidate)
		{
			int fitness = 0;
			foreach (Guess guess in PreviousGuesses)
			{
				Evaluation evaluation = candidate.Compare(guess.Code);
				fitness += Math.Abs(evaluation.Black - guess.Evaluation.Black)
						 + Math.Abs(evaluation.White - guess.Evaluation.White);
			}

			return fitness;
		}

		private static Individual Tournament(List<Individual> scored)
		{
			Individual best = scored[Random.Next(scored.Count)];
			for (int i = 1; i < TournamentSize; i++)
			{
				Individual candidate = scored[Random.Next(scored.Count)];
				if (candidate.Fitness < best.Fitness)
					best = candidate;
			}

			return best;
		}

		private static Individual Breed(Individual first, Individual second)
		{
			Code childCode = first.Code.Copy();
			GeneticGeneOrigin[] origins = new GeneticGeneOrigin[MyMasterMindConstants.Columns];

			int point = Random.Next(1, MyMasterMindConstants.Columns);
			for (int i = 0; i < MyMasterMindConstants.Columns; i++)
			{
				if (i >= point)
					childCode[i] = second.Code[i];
				origins[i] = i < point ? GeneticGeneOrigin.FirstParent : GeneticGeneOrigin.SecondParent;
			}

			for (int i = 0; i < MyMasterMindConstants.Columns; i++)
			{
				if (Random.NextDouble() < MutationRate)
				{
					childCode[i] = (MyMasterMindCodeColors)Random.Next(1, (int)MyMasterMindConstants.MaxColor + 1);
					origins[i] = GeneticGeneOrigin.Mutation;
				}
			}

			return new Individual { Code = childCode, Origins = origins, CrossoverPoint = point };
		}
	}
}
