using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation
{
    /// <summary>
    /// Custom exception class
    /// </summary>
    public class CardFileException : Exception
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public CardFileException() : base() { }
        /// <summary>
        /// Constructor which takes one argument
        /// </summary>
        /// <param name="message">Message of the exception</param>
        public CardFileException(string message) : base(message) { }
        /// <summary>
        /// Constructor which takes two arguments
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="inner">Inner exception</param>
        public CardFileException(string message, Exception inner) : base(message, inner) { }
    }
}
