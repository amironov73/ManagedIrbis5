// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LoggingUtility.cs -- утилиты для логирования
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

namespace AM;

/// <summary>
/// Утилиты для логирования.
/// </summary>
public static class LoggingUtility
{
    #region Public methods

    /// <summary>
    /// Отладочное логирование.
    /// </summary>
    [Conditional ("DEBUG")]
    public static void Debug
        (
            this ILogger logger,
            Func<string> lazy
        )
    {
        Sure.NotNull (logger);
        Sure.NotNull (lazy);

        if (logger.IsEnabled (LogLevel.Debug))
        {
            logger.LogDebug (lazy());
        }
    }

    /// <summary>
    /// Логирует сообщение об ошибке.
    /// </summary>
    public static void Error
        (
            this ILogger logger,
            Func<string> lazy
        )
    {
        Sure.NotNull (logger);
        Sure.NotNull (lazy);

        if (logger.IsEnabled (LogLevel.Error))
        {
            logger.LogError (lazy());
        }
    }

    /// <summary>
    /// Получение от хоста логгера для указанного типа.
    /// </summary>
    public static ILogger GetLogger
        (
            IHost host,
            Type type
        )
    {
        Sure.NotNull (host);
        Sure.NotNull (type);

        ILogger result = NullLogger.Instance;
        var factory = host.Services.GetService<ILoggerFactory>();
        if (factory is not null)
        {
            result = factory.CreateLogger (type);
        }

        return result;
    }

    /// <summary>
    /// Логирует информационное сообщение.
    /// </summary>
    public static void Info
        (
            this ILogger logger,
            Func<string> lazy
        )
    {
        Sure.NotNull (logger);
        Sure.NotNull (lazy);

        if (logger.IsEnabled (LogLevel.Information))
        {
            logger.LogInformation (lazy());
        }
    }

    /// <summary>
    /// Трассировочное сообщение сообщение.
    /// </summary>
    [Conditional ("TRACE")]
    public static void Trace
        (
            this ILogger logger,
            Func<string> lazy
        )
    {
        Sure.NotNull (logger);
        Sure.NotNull (lazy);

        if (logger.IsEnabled (LogLevel.Trace))
        {
            logger.LogTrace (lazy());
        }
    }

    /// <summary>
    /// Логирование предупреждения.
    /// </summary>
    public static void Warning
        (
            this ILogger logger,
            Func<string> lazy
        )
    {
        Sure.NotNull (logger);
        Sure.NotNull (lazy);

        if (logger.IsEnabled (LogLevel.Warning))
        {
            logger.LogWarning (lazy());
        }
    }

    #endregion
}
