using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShuntingYardAlgorithm
{

    public static class OperatorsCollection
    {
        private static readonly Operator[] _operators = new Operator[]{
            new Operator("+", 1, 2, OperatorAssociativity.Left, (a,b) => {return a+b;}),
            new Operator("-", 1, 2, OperatorAssociativity.Left, (a,b) => {return a-b;}),
            new Operator("*", 2, 2, OperatorAssociativity.Left, (a,b) => {return a*b;}),
            new Operator("/", 2, 2, OperatorAssociativity.Left, (a,b) => {return a/b;}),
            new Operator("^", 3, 2, OperatorAssociativity.Right, (a,b) => {return Math.Pow(a,b);}),
            new Operator("sqrt", 4, 1, OperatorAssociativity.Left, (a,b) => {return Math.Sqrt(a);}),
            new Operator("sin", 4, 1,  OperatorAssociativity.Left, (a,b) => {return Math.Sin(a);}),
            new Operator("cos", 4, 1,  OperatorAssociativity.Left, (a,b) => {return Math.Cos(a);}),
            new Operator("tg", 4, 1,  OperatorAssociativity.Left, (a,b) => {return Math.Tan(a);}),
            new Operator("ctg", 4, 1,  OperatorAssociativity.Left, (a,b) => {return 1.0/Math.Tan(a);}),
            new Operator("ln", 4, 1,  OperatorAssociativity.Left, (a,b) => {return Math.Log(a);}),
            new Operator("log", 4, 1,  OperatorAssociativity.Left, (a,b) => {return Math.Log10(a);}),
            new Operator("exp", 4, 1,  OperatorAssociativity.Left, (a,b) => {return Math.Exp(a);})
        };

        public static Operator[] Operators
        {
            get
            {
                return _operators;
            }
        }

    }
}
