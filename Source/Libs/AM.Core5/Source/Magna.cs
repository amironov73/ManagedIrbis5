// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Magna.cs -- организация среды для приложения в целом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Организация среды для приложения в целом.
    /// </summary>
    public sealed class Magna
    {
        #region Properites

        /// <summary>
        /// Хост приложения.
        /// </summary>
        public static IHost Host { get; private set; } = new HostBuilder().Build();

        /// <summary>
        /// Фабрика логгеров.
        /// </summary>
        public static ILoggerFactory Factory { get; private set; } = new LoggerFactory();

        /// <summary>
        /// Общий логгер.
        /// </summary>
        public static ILogger Logger { get; private set; } = new NullLogger<Magna>();

        #endregion

        #region Public methods

        /// <summary>
        /// Отладочное логирование.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Debug
            (
                string message,
                params object[] args
            )
        {
            Logger.LogDebug(message, args);
        }

        /// <summary>
        /// Логирует сообщение об ошибке.
        /// </summary>
        public static void Error
            (
                string message,
                params object[] args
            )
        {
            Logger.LogError(message, args);
        }

        /// <summary>
        /// Инициализация.
        /// </summary>
        public static void Initialize
            (
                string[] args,
                Action<IHostBuilder> configurationAction
            )
        {
            var builder = Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder(args);
            configurationAction(builder);

            Host = builder.Build();
            Factory = Host.Services.GetRequiredService<ILoggerFactory>();
            Logger = Factory.CreateLogger<Magna>();
        }

        /// <summary>
        /// Трассировочное логирование.
        /// </summary>
        [Conditional("TRACE")]
        public static void Trace
            (
                string message,
                params object[] args
            )
        {
            Logger.LogTrace(message, args);
        }

        /// <summary>
        /// Trace the exception.
        /// </summary>
        public static void TraceException
            (
                string text,
                Exception exception
            )
        {
            Logger.Log(LogLevel.Trace, exception, text);
        }

        #endregion
    }
}
