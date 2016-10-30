using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ShuntingYardAlgorithm
{
    public class Constant : IToken
    {
        private string _text;
        private double _value;

        public string Text
        {
            get
            {
                return _text;
            }
        }

        public double Value
        {
            get
            {
                return _value;
            }
        }

        public Constant(string text, double value)
        {
            _text = text;
            _value = value;
        }
    }
}