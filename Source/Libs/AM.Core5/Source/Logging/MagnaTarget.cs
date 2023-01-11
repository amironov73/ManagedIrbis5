// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MagnaTarget.cs -- специальная цель для перехвата логов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using NLog;
using NLog.Config;
using NLog.Targets;

#endregion

#nullable enable

namespace AM.Logging;

/// <summary>
/// Специальная цель для перехвата логов в NLog.
/// </summary>
[Target ("Magna")]
public sealed class MagnaTarget
    : TargetWithLayout
{
    #region Private members

    private static event EventHandler<LogEventInfo>? Handlers;
    private static bool _targetRegistered;
    private static bool _rulesAdded;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление нашей цели в конфигурацию NLog.
    /// </summary>
    public static void AddToNlogConfiguration()
    {
        if (!_rulesAdded)
        {
            var target = new MagnaTarget();
            var configuration = LogManager.Configuration;
            if (configuration is null)
            {
                configuration = new LoggingConfiguration();
                LogManager.Configuration = configuration;
            }

            configuration.AddRuleForAllLevels (target);
            LogManager.ReconfigExistingLoggers();
            _rulesAdded = true;
        }
    }

    /// <summary>
    /// Регистрация цели в NLog.
    /// </summary>
    public static void RegisterForNlog()
    {
        if (!_targetRegistered)
        {
            Register<MagnaTarget> ("Magna");
            _targetRegistered = true;
        }
    }

    /// <summary>
    /// Регистрация обработчика.
    /// </summary>
    public static void Subscribe
        (
            EventHandler<LogEventInfo> handler
        )
    {
        Handlers += handler;
    }

    /// <summary>
    /// Разрегистрация обработчика.
    /// </summary>
    public static void Unsubscribe
        (
            EventHandler<LogEventInfo> handler
        )
    {
        Handlers -= handler;
    }

    #endregion

    #region Target members

    /// <inheritdoc cref="Target.Write(NLog.LogEventInfo)"/>
    protected override void Write (LogEventInfo logEvent)
    {
        Handlers?.Invoke (null, logEvent);
    }

    #endregion
}
