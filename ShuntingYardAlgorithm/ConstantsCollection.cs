using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuntingYardAlgorithm
{
    public static class ConstantsCollection
    {
        private static readonly Constant[] _constants = new Constant[]{
            new Constant("pi",Math.PI),
            new Constant("e",Math.E)
        };

        public static Constant[] Constants
        {
            get
            {
                return _constants;
            }
        }
    }
}
