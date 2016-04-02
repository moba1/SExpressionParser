using System;

namespace SExpressionParser
{
    /// <summary>
    /// S式の解析時に発生する例外
    /// </summary>
    public class ParseException : Exception
    {
        public ParseException() { }
        public ParseException(string message) : base(message) { }
        public ParseException(string message, Exception inner) : base(message, inner) { }
    }
}
