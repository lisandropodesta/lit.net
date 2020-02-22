namespace Lit.Auditing
{
    /// <summary>
    /// Audit manager.
    /// </summary>
    public static class Audit
    {
        private static IAudit audit;

        /// <summary>
        /// Initialization.
        /// </summary>
        public static void Register(IAudit value)
        {
            audit = value;
        }

        /// <summary>
        /// Log a debug message.
        /// </summary>
        public static void Debug(string text)
        {
            audit?.Debug(text);
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        public static void Message(string text)
        {
            audit?.Message(text);
        }

        /// <summary>
        /// Log a warning.
        /// </summary>
        public static void Warning(string text)
        {
            audit?.Warning(text);
        }

        /// <summary>
        /// Log an error.
        /// </summary>
        public static void Error(string text)
        {
            audit?.Error(text);
        }
    }
}
