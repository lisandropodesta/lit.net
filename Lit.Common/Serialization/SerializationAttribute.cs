using System;

namespace Lit
{
    /// <summary>
    /// Serialization attribute.
    /// </summary>
    public class SerializationAttribute : Attribute
    {
        /// <summary>
        /// Encoded text.
        /// </summary>
        public string EncodedText { get; private set; }

        #region Constructors

        public SerializationAttribute(string encodedText)
        {
            EncodedText = encodedText;
        }

        #endregion

        /// <summary>
        /// Check if the text matches a reference.
        /// </summary>
        public bool Match(string encodedText)
        {
            return EncodedText == encodedText;
        }
    }
}
