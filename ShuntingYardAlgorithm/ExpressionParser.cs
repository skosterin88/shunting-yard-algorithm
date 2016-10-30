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
        private readonly Operator[] _operators = new Operator[]{
            new Operator("+", 1, 2, OperatorAssociativity.Left),
            new Operator("-", 1, 2, OperatorAssociativity.Left),
            new Operator("*", 2, 2, OperatorAssociativity.Left),
            new Operator("/", 2, 2, OperatorAssociativity.Left),
            new Operator("^", 3, 2, OperatorAssociativity.Right),
            new Operator("sqrt", 4, 1, OperatorAssociativity.Left),
            new Operator("sin", 4, 1,  OperatorAssociativity.Left),
            new Operator("cos", 4, 1,  OperatorAssociativity.Left),
            new Operator("tg", 4, 1,  OperatorAssociativity.Left),
            new Operator("ctg", 4, 1,  OperatorAssociativity.Left),
            new Operator("ln", 4, 1,  OperatorAssociativity.Left),
            new Operator("log", 4, 1,  OperatorAssociativity.Left),
            new Operator("exp", 4, 1,  OperatorAssociativity.Left)
        };

        private readonly Constant[] _constants = new Constant[]{
            new Constant("pi",Math.PI),
            new Constant("e",Math.E)
        };

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

                    if (isDouble)
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

        private string ReadOperator(string infixExpression, int index)
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

        public Queue<string> InfixToPostfix(string infixExpression)
        {
            string postfixExpression = "";
            Stack<string> operatorsStack = new Stack<string>();
            Queue<string> outputQueue = new Queue<string>();

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
                for (int i = 0; i < infixExpression.Length; i++)
                {
                    if (char.IsNumber(infixExpression[i]))
                    {
                        string currNumberString = ReadValueString(infixExpression, i);
                        outputQueue.Enqueue(currNumberString);
                        i += currNumberString.Length - 1;
                    }
                    else
                    {
                        string currOperator = ReadOperator(infixExpression, i);
                        if (currOperator != "")
                        {
                            OperatorAssociativity assoc = GetOperatorAssociativity(currOperator);
                            if (operatorsStack.Count == 0)
                            {
                                operatorsStack.Push(currOperator);
                            }
                            else
                            {
                                string topOperator = operatorsStack.Peek();
                                int stackTopPrecedence = GetOperatorPrecedence(topOperator);
                                int currPrecedence = GetOperatorPrecedence(currOperator);

                                if (currPrecedence <= stackTopPrecedence && assoc == OperatorAssociativity.Left ||
                                    currPrecedence < stackTopPrecedence && assoc == OperatorAssociativity.Right)
                                {
                                    string poppedOperator = operatorsStack.Pop();
                                    outputQueue.Enqueue(poppedOperator);
                                    operatorsStack.Push(currOperator);
                                }
                                else
                                {
                                    operatorsStack.Push(currOperator);
                                }
                            }
                        }
                        else
                        {
                            if (infixExpression[i] == '(')
                            {
                                operatorsStack.Push(infixExpression[i].ToString());
                            }
                            else if (infixExpression[i] == ')')
                            {
                                while (operatorsStack.Peek() != "(")
                                {
                                    outputQueue.Enqueue(operatorsStack.Pop());
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
                            else
                            {
                                string currConstant = ReadConstant(infixExpression, i);
                                if (currConstant != "")
                                {
                                    outputQueue.Enqueue(GetConstantValue(currConstant).ToString(CultureInfo.InvariantCulture));
                                    i += currConstant.Length - 1;
                                }
                            }
                        }
                    }

                    for (int j = 0; j < outputQueue.Count; j++)
                    {
                        Debug.Write(outputQueue.ElementAt(j).ToString() + " ");
                    }
                    Debug.WriteLine("");
                    Debug.Write("Operators Stack: ");
                    for (int j = 0; j < operatorsStack.Count; j++)
                    {
                        Debug.Write(operatorsStack.ElementAt(j).ToString() + " ");
                    }
                    Debug.WriteLine("");
                }


                while (operatorsStack.Count > 0)
                {
                    if (operatorsStack.Peek() == "(" || operatorsStack.Peek() == ")")
                    {
                        throw new ParenthesesMismatchException("Parentheses mismatch!");
                    }
                    else
                    {
                        outputQueue.Enqueue(operatorsStack.Pop());
                    }
                }


                for (int j = 0; j < outputQueue.Count; j++)
                {
                    Debug.Write(outputQueue.ElementAt(j).ToString() + " ");
                }
                Debug.WriteLine("");
                Debug.Write("Operators Stack: ");
                for (int j = 0; j < operatorsStack.Count; j++)
                {
                    Debug.Write(operatorsStack.ElementAt(j).ToString() + " ");
                }
                Debug.WriteLine("");
            }

            return outputQueue;
        }

        private double EvaluateOperator(double[] arguments, string operatorText)
        {
            if (operatorText == "+")
            {
                return arguments[0] + arguments[1];
            }
            else if (operatorText == "-")
            {
                return arguments[0] - arguments[1];
            }
            else if (operatorText == "*")
            {
                return arguments[0] * arguments[1];
            }
            else if (operatorText == "/")
            {
                return arguments[0] / arguments[1];
            }
            else if (operatorText == "^")
            {
                return Math.Pow(arguments[0], arguments[1]);
            }
            else if (operatorText == "sqrt")
            {
                return Math.Sqrt(arguments[0]);
            }
            else if (operatorText == "sin")
            {
                return Math.Sin(arguments[0]);
            }
            else if (operatorText == "cos")
            {
                return Math.Cos(arguments[0]);
            }
            else if (operatorText == "tg")
            {
                return Math.Tan(arguments[0]);
            }
            else if (operatorText == "ctg")
            {
                return 1.0 / Math.Tan(arguments[0]);
            }
            else if (operatorText == "ln")
            {
                return Math.Log(arguments[0]);
            }
            else if (operatorText == "log")
            {
                return Math.Log10(arguments[0]);
            }
            else if (operatorText == "exp")
            {
                return Math.Exp(arguments[0]);
            }

            return 0.0;
        }

        public double EvalPostfix(Queue<string> postfixQueue)
        {
            List<string> queueElements = postfixQueue.ToList();
            Stack<double> evalStack = new Stack<double>();
            double result = 0.0;

            int i = 0;

            while (postfixQueue.Count > 0)
            {
                string currElement = postfixQueue.Dequeue();
                if (!IsOperator(currElement))
                {
                    evalStack.Push(double.Parse(currElement, NumberStyles.Any, CultureInfo.InvariantCulture));
                }
                else
                {
                    int qArgsCurrOperator = GetOperatorArgumentsCount(currElement);
                    if (evalStack.Count < qArgsCurrOperator)
                    {
                        throw new InsufficientOperatorArgumentsException("Insufficient arguments count for operator" + " " + currElement + "!" + " " + "Arguments required" + ": " + qArgsCurrOperator.ToString());
                    }
                    else
                    {
                        double[] currOperatorArgs = new double[qArgsCurrOperator];
                        for (int j = qArgsCurrOperator - 1; j >= 0; j--)
                        {
                            currOperatorArgs[j] = evalStack.Pop();
                        }
                        double currResult = EvaluateOperator(currOperatorArgs, currElement);
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