using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShuntingYardAlgorithm
{
    [Serializable()]
    public class NullExpressionException : Exception
    {
        public NullExpressionException() : base() { }
        public NullExpressionException(string message) : base(message) { }
        public NullExpressionException(string message, Exception innerException) : base(message, innerException) { }

        protected NullExpressionException(SerializationInfo info, StreamingContext context) { }
    }

    [Serializable()]
    public class EmptyExpressionException : Exception
    {
        public EmptyExpressionException() : base() { }
        public EmptyExpressionException(string message) : base(message) { }
        public EmptyExpressionException(string message, Exception innerException) : base(message, innerException) { }

        protected EmptyExpressionException(SerializationInfo info, StreamingContext context) { }
    }

    [Serializable()]
    public class ParenthesesMismatchException : Exception
    {
        public ParenthesesMismatchException() : base() { }
        public ParenthesesMismatchException(string message) : base(message) { }
        public ParenthesesMismatchException(string message, Exception innerException) : base(message, innerException) { }

        protected ParenthesesMismatchException(SerializationInfo info, StreamingContext context) { }
    }

    [Serializable()]
    public class InsufficientOperatorArgumentsException : Exception
    {
        public InsufficientOperatorArgumentsException() : base() { }
        public InsufficientOperatorArgumentsException(string message) : base(message) { }
        public InsufficientOperatorArgumentsException(string message, Exception innerException) : base(message, innerException) { }

        protected InsufficientOperatorArgumentsException(SerializationInfo info, StreamingContext context) { }
    }

}
