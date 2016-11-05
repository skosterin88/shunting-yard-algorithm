using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ShuntingYardAlgorithm
{
    public class ExpressionParser
    {
        private Operator[] _operators = OperatorsCollection.Operators;
        private Constant[] _constants = ConstantsCollection.Constants;

        private int GetOperatorPrecedence(string operatorText)
        {
            string[] opTexts = _operators.Select(op => op.Text).ToArray();
            if (opTexts.Contains(operatorText))
            {
                int inxOperator = Array.IndexOf(opTexts, operatorText);
                return _operators[inxOperator].Precedence;
            }

            return -1;
        }

        private int GetMaxOperatorLength()
        {
            return _operators.Max(op => op.Text.Length);
        }

        private bool IsOperator(string str)
        {
            string[] operatorTexts = _operators.Select(op => op.Text).ToArray();
            return operatorTexts.Contains(str);
        }

        private bool IsConstant(string str)
        {
            string[] constantTexts = _constants.Select(c => c.Text).ToArray();
            return constantTexts.Contains(str);
        }

        private string ReadValueString(string infixExpression, int index)
        {
            string valStr = "";

            int exprLength = infixExpression.Length;

            for (int i = 0; i < infixExpression.Length; i++)
            {
                if (index >= 0 && (index + infixExpression.Length - i) <= exprLength)
                {
                    var temp = infixExpression.Substring(index, infixExpression.Length - i);
                    double val = 0.0d;
                    bool isDouble = double.TryParse(temp, NumberStyles.Any, CultureInfo.InvariantCulture, out val);
                    
                    if (isDouble && temp[0] != '+' && temp[0] != '-' && temp[0] != '(' && temp[temp.Length-1] != ')')
                    {
                        if (temp[temp.Length - 1] == '+' || temp[temp.Length - 1] == '-')
                        {
                            temp = temp.Substring(0, temp.Length - 1);
                        }
                        valStr = temp;
                        break;
                    }
                }
            }

            return valStr;
        }

        private string ReadConstant(string infixExpression, int index)
        {
            string valStr = "";

            int exprLength = infixExpression.Length;

            for (int i = 0; i < infixExpression.Length; i++)
            {
                if (index >= 0 && (index + infixExpression.Length - i) <= exprLength)
                {
                    var temp = infixExpression.Substring(index, infixExpression.Length - i);

                    if (IsConstant(temp))
                    {
                        valStr = temp;
                        break;
                    }
                }
            }

            return valStr;
        }

        private string ReadCurrentOperator(string infixExpression, int index)
        {
            string op = "";

            int maxOperatorLength = GetMaxOperatorLength();

            int exprLength = infixExpression.Length;

            //3+ln(5)

            for (int i = 0; i < maxOperatorLength; i++)
            {
                if (index >= 0 && (index + maxOperatorLength - i) <= exprLength)
                {
                    var temp = infixExpression.Substring(index, maxOperatorLength - i);
                    if (IsOperator(temp))
                    {
                        op = temp;
                        break;
                    }
                }
            }

            return op;
        }

        private OperatorAssociativity GetOperatorAssociativity(string operatorText)
        {
            OperatorAssociativity assoc = OperatorAssociativity.Left;

            for (int i = 0; i < _operators.Length; i++)
            {
                if (_operators[i].Text == operatorText)
                {
                    assoc = _operators[i].Associativity;
                    break;
                }
            }

            return assoc;
        }

        private int GetOperatorArgumentsCount(string operatorText)
        {
            int qArguments = 0;

            for (int i = 0; i < _operators.Length; i++)
            {
                if (_operators[i].Text == operatorText)
                {
                    qArguments = _operators[i].ArgumentsCount;
                    break;
                }
            }


            return qArguments;
        }

        private double GetConstantValue(string constantText)
        {
            double constVal = 0.0;

            for (int i = 0; i < _constants.Length; i++)
            {
                if (_constants[i].Text == constantText)
                {
                    constVal = _constants[i].Value;
                    break;
                }
            }

            return constVal;
        }

        private IToken ReadCurrentToken(string infixExpression, int index)
        {
            string currNumberString = ReadValueString(infixExpression, index);
            string currOperatorString = ReadCurrentOperator(infixExpression, index);
            string currConstantString = ReadConstant(infixExpression, index);

            bool isNumber = (currNumberString != "");
            bool isOperator = (currOperatorString != "");
            bool isConstant = (currConstantString != "");

            //if (infixExpression[index] != '(' && infixExpression[index] != ')')
            //{
                if (isNumber)
                {
                    double currNumber = double.Parse(currNumberString, NumberStyles.Any, CultureInfo.InvariantCulture);
                    return new NumberToken(currNumberString, currNumber);
                }
                else if (isOperator)
                {
                    int inxCurrOperator = -1;
                    for (int i = 0; i < _operators.Length; i++)
                    {
                        if (_operators[i].Text == currOperatorString)
                        {
                            inxCurrOperator = i;
                            break;
                        }
                    }
                    return _operators[inxCurrOperator];
                }
                else if (isConstant)
                {
                    int inxCurrConstant = -1;
                    for (int i = 0; i < _constants.Length; i++)
                    {
                        if (_constants[i].Text == currConstantString)
                        {
                            inxCurrConstant = i;
                            break;
                        }
                    }
                    return _constants[inxCurrConstant];
                }
            //}

            return null;
        }

        public IToken GetOperatorByText(string opText)
        {
            int inxOperator = -1;
            for (int j = 0; j < _operators.Length; j++)
            {
                if (_operators[j].Text == opText)
                {
                    inxOperator = j;
                    break;
                }
            }

            return _operators[inxOperator];
        }

        public Queue<IToken> InfixToPostfix(string infixExpression)
        {

            Stack<string> operatorsStack = new Stack<string>();
            Queue<IToken> outputQueue = new Queue<IToken>();

            if (infixExpression == null)
            {
                throw new NullExpressionException("Null expression to parse!");
            }
            else if (infixExpression == "")
            {
                throw new EmptyExpressionException("Empty expression to parse!");
            }
            else
            {
                string exprWithoutSpaces = infixExpression.Replace(" ", "");
                for (int i = 0; i < exprWithoutSpaces.Length; i++)
                {
                    IToken currToken = ReadCurrentToken(exprWithoutSpaces, i);
                    if (currToken is NumberToken)
                    {
                        //string currNumberString = ReadValueString(exprWithoutSpaces, i);
                        outputQueue.Enqueue(currToken);
                        i += currToken.Text.Length - 1;
                    }
                    else if (currToken is Operator)
                    {
                        OperatorAssociativity assoc = (currToken as Operator).Associativity;
                        
                        //else
                        //{
                        //    string topOperator = operatorsStack.Peek();
                        //    int stackTopPrecedence = GetOperatorPrecedence(topOperator);
                        //    int currPrecedence = GetOperatorPrecedence(currToken.Text);

                        //    if (currPrecedence <= stackTopPrecedence && assoc == OperatorAssociativity.Left ||
                        //        currPrecedence < stackTopPrecedence && assoc == OperatorAssociativity.Right)
                        //    {
                        //        string poppedOperatorString = operatorsStack.Pop();

                        //        IToken poppedOperator = GetOperatorByText(poppedOperatorString);

                        //        outputQueue.Enqueue(poppedOperator);
                        //        operatorsStack.Push(currToken.Text);

                        //    }
                        //    else
                        //    {
                        //        operatorsStack.Push(currToken.Text);
                        //    }
                        //}

                        bool isFinish = false;
                        if (operatorsStack.Count == 0)
                        {
                            operatorsStack.Push(currToken.Text);
                        }
                        else
                        {
                            while (!isFinish)
                            {
                                string topOperator = operatorsStack.Peek();
                                if (IsOperator(topOperator))
                                {
                                    int stackTopPrecedence = GetOperatorPrecedence(topOperator);
                                    int currPrecedence = GetOperatorPrecedence(currToken.Text);

                                    if (currPrecedence <= stackTopPrecedence && assoc == OperatorAssociativity.Left ||
                                        currPrecedence < stackTopPrecedence && assoc == OperatorAssociativity.Right)
                                    {
                                        string poppedOperatorString = operatorsStack.Pop();

                                        IToken poppedOperator = GetOperatorByText(poppedOperatorString);

                                        outputQueue.Enqueue(poppedOperator);
                                    }
                                    else
                                    {
                                        isFinish = true;
                                    }

                                    if (operatorsStack.Count > 0)
                                    {
                                        topOperator = operatorsStack.Peek();
                                    }
                                    else
                                    {
                                        isFinish = true;
                                    }
                                }
                                else
                                {
                                    isFinish = true;
                                }
                            }
                            operatorsStack.Push(currToken.Text);

                            i += currToken.Text.Length - 1;
                        }
                    }
                    else if (currToken is Constant)
                    {
                        outputQueue.Enqueue(currToken);
                        i += currToken.Text.Length - 1;
                    }
                    else
                    {
                        if (exprWithoutSpaces[i] == '(')
                        {
                            operatorsStack.Push(exprWithoutSpaces[i].ToString());
                        }
                        else if (exprWithoutSpaces[i] == ')')
                        {
                            while (operatorsStack.Peek() != "(")
                            {
                                outputQueue.Enqueue(GetOperatorByText(operatorsStack.Pop()));
                                if (operatorsStack.Count == 1 && operatorsStack.Peek() != "(")
                                {
                                    throw new ParenthesesMismatchException("Parentheses mismatch!");
                                }
                            }
                            operatorsStack.Pop();
                            //if (IsOperator(operatorsStack.Peek()))
                            //{
                            //    outputQueue.Enqueue(operatorsStack.Pop());
                            //}
                        }

                    }

                }
                      

                while (operatorsStack.Count > 0)
                {
                    if (operatorsStack.Peek() == "(" || operatorsStack.Peek() == ")")
                    {
                        throw new ParenthesesMismatchException("Parentheses mismatch!");
                    }
                    else
                    {
                        outputQueue.Enqueue(GetOperatorByText(operatorsStack.Pop()));
                    }
                }

            }

            return outputQueue;
        }

        private double EvaluateOperator(double[] arguments, string operatorText)
        {
            for (int i = 0; i < _operators.Length; i++)
            {
                if (_operators[i].Text == operatorText)
                {
                    if (arguments.Length < _operators[i].ArgumentsCount){
                        throw new InsufficientOperatorArgumentsException("Insufficient arguments count for operator" + " " + operatorText + "!" + " " + "Arguments required" + ": " + _operators[i].ArgumentsCount.ToString());
                    }
                    if (arguments.Length == 1)
                    {
                        return _operators[i].OperatorFunction(arguments[0], 0.0);
                    }
                    else if (arguments.Length == 2)
                    {
                        return _operators[i].OperatorFunction(arguments[0], arguments[1]);
                    }
                }
            }

            return 0.0;
        }

        public double EvalPostfix(Queue<IToken> postfixQueue)
        {
            Stack<double> evalStack = new Stack<double>();
            double result = 0.0;

            int i = 0;

            while (postfixQueue.Count > 0)
            {
                IToken currElement = postfixQueue.Dequeue();
                if (currElement is NumberToken)
                {
                    evalStack.Push((currElement as NumberToken).Value);
                }
                else if (currElement is Constant)
                {
                    evalStack.Push((currElement as Constant).Value);
                }
                else
                {
                    int qArgsCurrOperator = (currElement as Operator).ArgumentsCount;
                    if (evalStack.Count < qArgsCurrOperator)
                    {
                        throw new InsufficientOperatorArgumentsException("Insufficient arguments count for operator" + " " + currElement.Text + "!" + " " + "Arguments required" + ": " + qArgsCurrOperator.ToString());
                    }
                    else
                    {
                        double[] currOperatorArgs = new double[qArgsCurrOperator];
                        for (int j = qArgsCurrOperator - 1; j >= 0; j--)
                        {
                            currOperatorArgs[j] = evalStack.Pop();
                        }
                        double currResult = EvaluateOperator(currOperatorArgs, currElement.Text);
                        evalStack.Push(currResult);

                    }
                }
            }

            if (evalStack.Count == 1)
            {
                result = evalStack.Pop();
            }

            return result;
        }

    }

}