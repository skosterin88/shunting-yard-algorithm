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

        public static Operator[] Operators
        {
            get
            {
                return _operators;
            }
        }

    }
}
