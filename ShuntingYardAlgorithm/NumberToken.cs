using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuntingYardAlgorithm
{
    public class NumberToken : IToken
    {
        private string _text;
        private double _value;

        public string Text
        {
            get { return _text; }
        }

        public double Value
        {
            get
            {
                return _value;
            }
        }

        public NumberToken(string text, double value)
        {
            _text = text;
            _value = value;
        }
    }
}
