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
                Queue<string> postfix = parser.InfixToPostfix(expr);
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
                Queue<string> postfix = parser.InfixToPostfix(expr);
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
                Queue<string> postfix = parser.InfixToPostfix(expr);
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
                Queue<string> postfix = parser.InfixToPostfix(expr);
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

            Queue<string> postfixSpaces = parser.InfixToPostfix(exprWithSpaces);
            Queue<string> postfixNoSpaces = parser.InfixToPostfix(exprWithoutSpaces);

            StringBuilder postfixNotationSpaces = new StringBuilder();
            for (int i = 0; i < postfixSpaces.Count; i++)
            {
                postfixNotationSpaces.Append(postfixSpaces.ElementAt(i));
            }
            string postfixSpacesText = postfixNotationSpaces.ToString();

            StringBuilder postfixNotationNoSpaces = new StringBuilder();
            for (int i = 0; i < postfixNoSpaces.Count; i++)
            {
                postfixNotationNoSpaces.Append(postfixNoSpaces.ElementAt(i));
            }
            string postfixNoSpacesText = postfixNotationNoSpaces.ToString();


            Assert.AreEqual(postfixNoSpacesText, postfixSpacesText);
        }
    }
}
