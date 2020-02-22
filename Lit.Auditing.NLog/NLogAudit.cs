using System;
using System.IO;

namespace Lit.Auditing
{
    /// <summary>
    /// NLog audit implementation.
    /// </summary>
    public class NLogAudit : IAudit
    {
        /// <summary>
        /// Register this class.
        /// </summary>
        public static void Register(AuditType minType, params AuditTarget[] targets)
        {
            new NLogAudit(minType, targets);
        }

        private NLog.Logger logger;

        /// <summary>
        /// Register the
        /// </summary>
        private NLogAudit(AuditType minType, AuditTarget[] targets)
        {
            var config = new NLog.Config.LoggingConfiguration();
            var minLevel = Translate(minType);

            foreach (var t in targets)
            {
                NLog.Targets.Target target = null;

                switch (t)
                {
                    case AuditTarget.Console:
                        target = new NLog.Targets.ColoredConsoleTarget("console");
                        break;

                    case AuditTarget.File:
                        target = new NLog.Targets.FileTarget("logfile")
                        {
                            FileName = GetDefaultLogFileName()
                        };
                        break;

                    case AuditTarget.Db:
                    default:
                        throw new NotImplementedException($"Audit target {t}");
                }

                config.AddRule(minLevel, NLog.LogLevel.Fatal, target);
            }

            NLog.LogManager.Configuration = config;
            logger = NLog.LogManager.GetCurrentClassLogger();

            Audit.Register(this);
        }

        /// <summary>
        /// Process a debug message.
        /// </summary>
        public void Debug(string text)
        {
            logger.Debug(text);
        }

        /// <summary>
        /// Process a message.
        /// </summary>
        public void Message(string text)
        {
            logger.Info(text);
        }

        /// <summary>
        /// Process a warning.
        /// </summary>
        public void Warning(string text)
        {
            logger.Warn(text);
        }

        /// <summary>
        /// Process an error.
        /// </summary>
        public void Error(string text)
        {
            logger.Error(text);
        }

        /// <summary>
        /// Process a fatal error.
        /// </summary>
        public void FatalError(string text)
        {
            logger.Fatal(text);
        }

        private string GetDefaultLogFileName()
        {
            return GetDefaultLogFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        private string GetDefaultLogFileName(string fullFileName)
        {
            var path = Path.GetDirectoryName(fullFileName);
            var fileName = Path.GetFileName(fullFileName);
            return Path.Combine(path, "log", fileName + ".log");
        }

        /// <summary>
        /// Translates a NLog type.
        /// </summary>
        private static NLog.LogLevel Translate(AuditType type)
        {
            switch (type)
            {
                case AuditType.Debug:
                default:
                    return NLog.LogLevel.Debug;

                case AuditType.Message:
                    return NLog.LogLevel.Info;

                case AuditType.Warning:
                    return NLog.LogLevel.Warn;

                case AuditType.Error:
                    return NLog.LogLevel.Error;

                case AuditType.Fatal:
                    return NLog.LogLevel.Fatal;

                case AuditType.Event:
                    return NLog.LogLevel.Fatal;
            }
        }
    }
}
