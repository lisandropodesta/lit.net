namespace Lit.Auditing
{
    /// <summary>
    /// Audit interface.
    /// </summary>
    public interface IAudit
    {
        /// <summary>
        /// Process a debug message.
        /// </summary>
        void Debug(string text);

        /// <summary>
        /// Process a message.
        /// </summary>
        void Message(string text);

        /// <summary>
        /// Process a warning.
        /// </summary>
        void Warning(string text);

        /// <summary>
        /// Proces an error.
        /// </summary>
        void Error(string text);

        /// <summary>
        /// Process a fatal error.
        /// </summary>
        void FatalError(string text);
    }
}
