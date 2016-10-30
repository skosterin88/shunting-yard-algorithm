using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShuntingYardAlgorithm;

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
    }
}
