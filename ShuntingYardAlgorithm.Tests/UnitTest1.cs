using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShuntingYardAlgorithm;
using System.Text;

namespace ShuntingYardAlgorithm.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InfixToPostfix_NullExpressionString_ExceptionThrown()
        {
            string expr = null;
            ExpressionParser parser = new ExpressionParser();

            try
            {
                Queue<IToken> postfix = parser.InfixToPostfix(expr);
                Assert.Fail();
            }
            catch (NullExpressionException)
            {

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void InfixToPostfix_EmptyExpressionString_ExceptionThrown()
        {
            string expr = "";
            ExpressionParser parser = new ExpressionParser();

            try
            {
                Queue<IToken> postfix = parser.InfixToPostfix(expr);
                Assert.Fail();
            }
            catch (EmptyExpressionException)
            {

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void InfixToPostfix_ParenthesesMismatch_ExceptionThrown()
        {
            string expr = "cos(pi/2)+ln(e^(0.5)";
            ExpressionParser parser = new ExpressionParser();

            try
            {
                Queue<IToken> postfix = parser.InfixToPostfix(expr);
                Assert.Fail();
            }
            catch (ParenthesesMismatchException)
            {

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void EvalPostfix_InsufficientOperatorArguments_ExceptionThrown()
        {
            string expr = "sin(pi)+sqrt()";
            ExpressionParser parser = new ExpressionParser();

            try
            {
                Queue<IToken> postfix = parser.InfixToPostfix(expr);
                double eval = parser.EvalPostfix(postfix);
                Assert.Fail();
            }
            catch (InsufficientOperatorArgumentsException)
            {

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void InfixToPostfix_ExpressionWithWhitespaces_WhitespacesRemoved()
        {
            string exprWithSpaces = "cos(   pi /     2.0    ) +   sqrt  (  4.0     )";
            string exprWithoutSpaces = "cos(pi/2.0)+sqrt(4.0)";
            ExpressionParser parser = new ExpressionParser();

            Queue<IToken> postfixSpaces = parser.InfixToPostfix(exprWithSpaces);
            Queue<IToken> postfixNoSpaces = parser.InfixToPostfix(exprWithoutSpaces);

            StringBuilder postfixNotationSpaces = new StringBuilder();
            for (int i = 0; i < postfixSpaces.Count; i++)
            {
                postfixNotationSpaces.Append(postfixSpaces.ElementAt(i).Text);
            }
            string postfixSpacesText = postfixNotationSpaces.ToString();

            StringBuilder postfixNotationNoSpaces = new StringBuilder();
            for (int i = 0; i < postfixNoSpaces.Count; i++)
            {
                postfixNotationNoSpaces.Append(postfixNoSpaces.ElementAt(i).Text);
            }
            string postfixNoSpacesText = postfixNotationNoSpaces.ToString();


            Assert.AreEqual(postfixNoSpacesText, postfixSpacesText);
        }

        [TestMethod]
        public void InfixToPostfix_InfixNotation_CorrectlyTransformed()
        {
            string expr = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
            ExpressionParser parser = new ExpressionParser();

            Queue<IToken> postfix = parser.InfixToPostfix(expr);

            StringBuilder postfixStringBuilder = new StringBuilder();
            for (int i = 0; i < postfix.Count; i++)
            {
                postfixStringBuilder.Append(postfix.ElementAt(i).Text);
                postfixStringBuilder.Append(" ");
            }

            postfixStringBuilder = postfixStringBuilder.Remove(postfixStringBuilder.Length-1, 1);
            string actualPostfixString = postfixStringBuilder.ToString();

            string correctPostfixString = "3 4 2 * 1 5 - 2 3 ^ ^ / +";

            Assert.AreEqual(correctPostfixString, actualPostfixString);
        }

        [TestMethod]
        public void EvalPostfix_Expression_CorrectlyEvaluated()
        {
            //string expr = "cos(pi/2) + sin(pi/6) * sqrt(4.0) - ln(e^(0.5))";
            string expr = "sin(pi/6)+cos(pi/3)*sqrt(4.0)+e^(ln(0.25))";

            ExpressionParser parser = new ExpressionParser();
            Queue<IToken> postfix = parser.InfixToPostfix(expr);
            double actualResult = parser.EvalPostfix(postfix);

            double correctResult = 1.75;

            bool isCorrect = (Math.Abs(correctResult - actualResult) < 0.001);

            Assert.AreEqual(true, isCorrect);
        }
    }
}
