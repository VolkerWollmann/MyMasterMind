using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MyMasterMind.Interfaces;
using MyMasterMind.Model;

namespace MasterMindModelTest
{
    [TestClass]
    public class GeneticEngineTests
    {
        private static readonly MyMasterMindCodeColors[] SecretColors =
            new[]
            {
                MyMasterMindCodeColors.Blue,
                MyMasterMindCodeColors.Red,
                MyMasterMindCodeColors.Green,
                MyMasterMindCodeColors.Cyan
            };

        private static Guess MakeEvaluatedGuess(Code secret, params MyMasterMindCodeColors[] colors)
        {
            Guess guess = new Guess(colors);
            guess.Evaluate(secret);
            return guess;
        }

        [TestMethod]
        public void FirstGenerationWithoutPreviousGuessesIsConsistent()
        {
            GeneticEngine engine = new GeneticEngine(new List<Guess>());

            bool finished = engine.NextGeneration();

            Assert.IsTrue(finished);
            Assert.IsTrue(engine.Consistent);
            Assert.AreEqual(1, engine.Generation);
        }

        [TestMethod]
        public void SearchTerminatesWithinGenerationLimit()
        {
            Code secret = new Code(SecretColors);
            List<Guess> previousGuesses = new List<Guess>
            {
                MakeEvaluatedGuess(secret,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red),
                MakeEvaluatedGuess(secret,
                    MyMasterMindCodeColors.Green,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Yellow,
                    MyMasterMindCodeColors.Magenta),
            };

            GeneticEngine engine = new GeneticEngine(previousGuesses);

            int steps = 0;
            while (!engine.NextGeneration())
            {
                steps++;
                Assert.IsTrue(steps <= GeneticEngine.MaxGenerations);
            }

            Assert.IsNotNull(engine.Best);
            Assert.IsTrue(engine.Generation <= GeneticEngine.MaxGenerations);
        }

        [TestMethod]
        public void ConsistentResultReproducesAllPreviousEvaluations()
        {
            Code secret = new Code(SecretColors);
            List<Guess> previousGuesses = new List<Guess>
            {
                MakeEvaluatedGuess(secret,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Green,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Yellow),
                MakeEvaluatedGuess(secret,
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Blue),
            };

            GeneticEngine engine = new GeneticEngine(previousGuesses);
            while (!engine.NextGeneration()) { }

            if (engine.Consistent)
            {
                foreach (Guess guess in previousGuesses)
                {
                    Evaluation evaluation = engine.Best.Compare((Code)guess.GetCode());
                    Assert.IsTrue(evaluation.Compare((Evaluation)guess.GetEvaluation()));
                }
            }
            else
            {
                // the genetic search is allowed to fail, but only at the generation limit
                Assert.AreEqual(GeneticEngine.MaxGenerations, engine.Generation);
            }
        }

        [TestMethod]
        public void BestOriginsAreTracked()
        {
            Code secret = new Code(SecretColors);
            List<Guess> previousGuesses = new List<Guess>
            {
                MakeEvaluatedGuess(secret,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Green,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Yellow),
            };

            GeneticEngine engine = new GeneticEngine(previousGuesses);

            bool finished = false;
            while (!finished)
            {
                finished = engine.NextGeneration();

                Assert.IsNotNull(engine.BestOrigins);
                Assert.AreEqual(MyMasterMindConstants.Columns, engine.BestOrigins.Length);

                if (engine.BestCrossoverPoint == 0)
                {
                    // not bred: random start individual or carried-over elite
                    Assert.IsTrue(
                        engine.BestOrigins[0] == GeneticGeneOrigin.Random ||
                        engine.BestOrigins[0] == GeneticGeneOrigin.Carried);
                }
                else
                {
                    // bred: genes before the crossover point come from the first
                    // parent, from it on from the second, unless mutated
                    for (int i = 0; i < MyMasterMindConstants.Columns; i++)
                    {
                        GeneticGeneOrigin expected = i < engine.BestCrossoverPoint
                            ? GeneticGeneOrigin.FirstParent
                            : GeneticGeneOrigin.SecondParent;
                        Assert.IsTrue(
                            engine.BestOrigins[i] == expected ||
                            engine.BestOrigins[i] == GeneticGeneOrigin.Mutation);
                    }
                }
            }
        }

        [TestMethod]
        public void GeneticGamePlaysAtMostAllRows()
        {
            MyMasterMindGame game = new MyMasterMindGame();

            for (int row = 0; row < MyMasterMindConstants.Rows; row++)
            {
                game.StartGeneticGuess();
                while (!game.GeneticStep()) { }
                game.CommitGeneticGuess();

                if (game.Finished())
                    break;
            }

            // the genetic strategy may lose the game, but must never leave the board
            Assert.IsTrue(game.GetCurrentGuessRow() < MyMasterMindConstants.Rows);
        }
    }
}
