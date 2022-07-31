using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MyMasterMind.Interfaces;
using MyMasterMind.Model;

namespace MasterMindModelTest
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void NoMatch()
        {
            MyMasterMindCodeColors[] quessColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue
                };

            Code quess = new Code(quessColors);

            MyMasterMindCodeColors[] codeColors =
                new[]
                {
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Cyan
                };

            Code code = new Code(codeColors);

            Evaluation evaluation = quess.Compare(code);

            Assert.AreEqual(evaluation.Black,0);
            Assert.AreEqual(evaluation.White, 0);

        }

        [TestMethod]
        public void FullMatch()
        {
            MyMasterMindCodeColors[] guessColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue
                };

            Code quess = new Code(guessColors);

            MyMasterMindCodeColors[] codeColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue
                };

            Code code = new Code(codeColors);

            Evaluation evaluation = quess.Compare(code);

            Assert.AreEqual(evaluation.Black, MyMasterMindConstants.Columns );
            Assert.AreEqual(evaluation.White, 0);

        }

        [TestMethod]
        public void OneBlack()
        {
            MyMasterMindCodeColors[] guessColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red
                };

            Code quess = new Code(guessColors);

            MyMasterMindCodeColors[] codeColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Cyan
                };

            Code code = new Code(codeColors);

            Evaluation evaluation = quess.Compare(code);

            Assert.AreEqual(evaluation.Black, 1);
            Assert.AreEqual(evaluation.White, 0);

        }

        [TestMethod]
        public void OneBlack2()
        {
            MyMasterMindCodeColors[] guessColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red
                };

            Code quess = new Code(guessColors);

            MyMasterMindCodeColors[] codeColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Cyan
                };

            Code code = new Code(codeColors);

            Evaluation evaluation = quess.Compare(code);

            Assert.AreEqual(evaluation.Black, 1);
            Assert.AreEqual(evaluation.White, 0);

        }

        [TestMethod]
        public void OneWhite()
        {
            MyMasterMindCodeColors[] guessColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red
                };

            Code quess = new Code(guessColors);

            MyMasterMindCodeColors[] codeColors =
                new[]
                {
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Cyan
                };

            Code code = new Code(codeColors);

            Evaluation evaluation = quess.Compare(code);

            Assert.AreEqual(evaluation.Black, 0);
            Assert.AreEqual(evaluation.White, 1);

        }

        [TestMethod]
        public void OneWhite2()
        {
            MyMasterMindCodeColors[] guessColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red
                };

            Code quess = new Code(guessColors);

            MyMasterMindCodeColors[] codeColors =
                new[]
                {
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Cyan
                };

            Code code = new Code(codeColors);

            Evaluation evaluation = quess.Compare(code);

            Assert.AreEqual(evaluation.Black, 0);
            Assert.AreEqual(evaluation.White, 1);

        }

        [TestMethod]
        public void OneWhite3()
        {
            MyMasterMindCodeColors[] guessColors =
                new[]
                {
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red,
                    MyMasterMindCodeColors.Red
                };

            Code quess = new Code(guessColors);

            MyMasterMindCodeColors[] codeColors =
                new[]
                {
                    MyMasterMindCodeColors.Cyan,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Blue,
                    MyMasterMindCodeColors.Cyan
                };

            Code code = new Code(codeColors);

            Evaluation evaluation = quess.Compare(code);

            Assert.AreEqual(evaluation.Black, 0);
            Assert.AreEqual(evaluation.White, 1);

        }
    }
}
