namespace Lit
{
    /// <summary>
    /// Serialization mode.
    /// </summary>
    public enum SerializationMode
    {
        /// <summary>
        /// Compact serialization.
        /// </summary>
        Compact,

        /// <summary>
        /// Json compact (without spaces and new lines).
        /// </summary>
        JsonCompact,

        /// <summary>
        /// Json printable (with spaces and new lines).
        /// </summary>
        JsonPrintable
    }
}
