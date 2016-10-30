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

    public class Operator
    {
        private string _text;
        private int _precedence;
        private OperatorAssociativity _assoc;
        private int _qArguments;

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

        public int ArgumentsCount
        {
            get
            {
                return _qArguments;
            }
        }

        public Operator(string text, int precedence, int qArguments, OperatorAssociativity assoc)
        {
            _text = text;
            _precedence = precedence;
            _qArguments = qArguments;
            _assoc = assoc;
        }
    }

}