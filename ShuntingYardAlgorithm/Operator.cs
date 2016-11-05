using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ShuntingYardAlgorithm
{
    public enum OperatorAssociativity
    {
        Left,
        Right
    }

    public class Operator : IToken
    {
        private string _text;
        private int _precedence;
        private OperatorAssociativity _assoc;
        private int _qArguments;
        private Func<double, double, double> _opFunc;

        public string Text
        {
            get
            {
                return _text;
            }
        }

        public int Precedence
        {
            get
            {
                return _precedence;
            }
        }

        public OperatorAssociativity Associativity
        {
            get
            {
                return _assoc;
            }
        }

        public Func<double, double, double> OperatorFunction
        {
            get
            {
                return _opFunc;
            }
        }

        public int ArgumentsCount
        {
            get
            {
                return _qArguments;
            }
        }

        public Operator(string text, int precedence, int qArguments, OperatorAssociativity assoc, Func<double,double,double> opFunc)
        {
            _text = text;
            _precedence = precedence;
            _qArguments = qArguments;
            _assoc = assoc;
            _opFunc = opFunc;
        }
    }

}