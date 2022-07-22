// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable TemplateIsNotCompileTimeConstantProblem
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Magna.cs -- организация среды для приложения в целом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using AM.AppServices;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Splat;

using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

#endregion

#pragma warning disable CA2211 // поля, не являющиеся констрантами, не должны быть видны
#pragma warning disable CA2254 // шаблон сообщения должен быть константой

#nullable enable

namespace AM;

//
// .NET logging levels
//
// Trace = 0
// For information that is valuable only to a developer debugging
// an issue. These messages may contain sensitive application data
// and so should not be enabled in a production environment.
// Disabled by default.
// Example: Credentials: {"User":"someuser", "Password":"P@ssword"}
//
// Debug = 1
// or information that has short-term usefulness during development
// and debugging.
// Example: Entering method Configure with flag set to true.
//
// Information = 2
// For tracking the general flow of the application. These logs typically
// have some long-term value.
// Example: Request received for path /api/some
//
// Warning = 3
// For abnormal or unexpected events in the application flow. These may
// include errors or other conditions that do not cause the application
// to stop, but which may need to be investigated. Handled exceptions
// are a common place to use the Warning log level.
// Example: FileNotFoundException for file quotes.txt.
//
// Error = 4
// For errors and exceptions that cannot be handled. These messages
// indicate a failure in the current activity or operation (such as the
// current HTTP request), not an application-wide failure.
// Example log message: Cannot insert record due to duplicate key violation.
//
// Critical = 5
// For failures that require immediate attention. Examples: data
// loss scenarios, out of disk space.
//

/// <summary>
/// Организация среды для приложения в целом.
/// </summary>

// нельзя делать static -- слетят дженерики
public sealed class Magna
{
    #region Properites

    /// <summary>
    /// Файловая версия сборки.
    /// </summary>
    public static string FileVersion = ThisAssembly.AssemblyFileVersion;

    /// <summary>
    /// Версия сборки.
    /// </summary>
    public static string AssemblyVersion = ThisAssembly.AssemblyVersion;

    /// <summary>
    /// Аргументы командной строки.
    /// </summary>
    public static string[] Args { get; private set; } = Array.Empty<string>();

    /// <summary>
    /// Хост приложения.
    /// </summary>
    public static IHost Host { get; internal set; } = new HostBuilder().Build();

    /// <summary>
    /// Фабрика логгеров.
    /// </summary>
    public static ILoggerFactory Factory { get; private set; } = new LoggerFactory();

    /// <summary>
    /// Общий логгер для всего приложения.
    /// </summary>
    public static ILogger Logger { get; private set; } = new NullLogger<Magna>();

    /// <summary>
    /// Общая конфигурация для всего приложения.
    /// </summary>
    public static IConfiguration Configuration { get; set; } = new ConfigurationBuilder()
        .SetBasePath (AppContext.BaseDirectory)
        .AddJsonFile ("appsettings.json", true, true)
        .Build();

    /// <summary>
    /// Глобальные опции программы.
    /// </summary>
    public static Dictionary<string, object?> GlobalOptions { get; } = new ();

    /// <summary>
    /// Глобальный объект приложения.
    /// </summary>
    [AllowNull]
    public static MagnaApplication Application { get; internal set; }

    #endregion

    #region Public methods

    // /// <summary>
    // /// Критическая ситуация.
    // /// </summary>
    // public static void Critical
    //     (
    //         string message
    //     )
    // {
    //     Logger.LogCritical (message);
    // }
    //
    // /// <summary>
    // /// Критическая ситуация.
    // /// </summary>
    // public static void Critical
    //     (
    //         string message,
    //         params object[] args
    //     )
    // {
    //     Logger.LogCritical (message, args);
    // }

    // /// <summary>
    // /// Отладочное логирование.
    // /// </summary>
    // [Conditional ("DEBUG")]
    // public static void Debug
    //     (
    //         string message
    //     )
    // {
    //     Logger.LogDebug (message);
    // }
    //
    // /// <summary>
    // /// Отладочное логирование.
    // /// </summary>
    // [Conditional ("DEBUG")]
    // public static void Debug
    //     (
    //         string message,
    //         params object[] args
    //     )
    // {
    //     Logger.LogDebug (message, args);
    // }

    // /// <summary>
    // /// Отладочное логирование.
    // /// </summary>
    // [Conditional ("DEBUG")]
    // public static void Debug
    //     (
    //         Func<string> lazy
    //     )
    // {
    //     if (Logger.IsEnabled (LogLevel.Debug))
    //     {
    //         Logger.LogDebug (lazy());
    //     }
    // }

    // /// <summary>
    // /// Логирует сообщение об ошибке.
    // /// </summary>
    // public static void Error
    //     (
    //         string message
    //     )
    // {
    //     Logger.LogError (message);
    // }
    //
    // /// <summary>
    // /// Логирует сообщение об ошибке.
    // /// </summary>
    // public static void Error
    //     (
    //         string message,
    //         params object[] args
    //     )
    // {
    //     Logger.LogError (message, args);
    // }
    //
    // /// <summary>
    // /// Логирует сообщение об ошибке.
    // /// </summary>
    // public static void Error
    //     (
    //         Func<string> lazy
    //     )
    // {
    //     if (Logger.IsEnabled (LogLevel.Error))
    //     {
    //         Logger.LogError (lazy());
    //     }
    // }
    //
    // /// <summary>
    // /// Логирует информационное сообщение.
    // /// </summary>
    // public static void Info
    //     (
    //         string message
    //     )
    // {
    //     Logger.LogInformation (message);
    // }
    //
    // /// <summary>
    // /// Логирует информационное сообщение.
    // /// </summary>
    // public static void Info
    //     (
    //         string message,
    //         params object[] args
    //     )
    // {
    //     Logger.LogInformation (message, args);
    // }
    //
    // /// <summary>
    // /// Логирует информационное сообщение.
    // /// </summary>
    // public static void Info
    //     (
    //         Func<string> lazy
    //     )
    // {
    //     if (Logger.IsEnabled (LogLevel.Information))
    //     {
    //         Logger.LogInformation (lazy());
    //     }
    // }

    /// <summary>
    /// Инициализация.
    /// </summary>
    public static void Initialize
        (
            string[] args,
            Action<IHostBuilder>? configurationAction = null
        )
    {
        Args = args;

        var builder = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder (args);
        configurationAction?.Invoke (builder);

        Host = builder.Build();
        Factory = Host.Services.GetRequiredService<ILoggerFactory>();
        Logger = Factory.CreateLogger<Magna>();
    }

    /// <summary>
    /// Определение, не запущены ли мы в режиме Unit-тестирования.
    /// </summary>
    public static bool InUnitTestRunner()
    {
        return ModeDetector.InUnitTestRunner();
    }

    /// <summary>
    /// Определение, не запущены ли мы в режиме дизайнера.
    /// </summary>
    public static bool InDesignMode()
    {
        const string XamlDesignPropertiesType = "System.ComponentModel.DesignerProperties, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        const string XamlControlBorderType = "System.Windows.Controls.Border, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
        const string XamlDesignPropertiesDesignModeMethodName = "GetIsInDesignMode";
        const string WpfDesignerPropertiesType = "System.ComponentModel.DesignerProperties, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        const string WpfDesignerPropertiesDesignModeMethod = "GetIsInDesignMode";
        const string WpfDependencyPropertyType = "System.Windows.DependencyObject, WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        const string WinFormsDesignerPropertiesType = "Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime";
        const string WinFormsDesignerPropertiesDesignModeMethod = "DesignModeEnabled";

        var result = false;

        // Check Silverlight / WP8 Design Mode
        var type = Type.GetType (XamlDesignPropertiesType, false);
        if (type is not null)
        {
            var mInfo = type.GetMethod (XamlDesignPropertiesDesignModeMethodName);
            var dependencyObject = Type.GetType (XamlControlBorderType, false);

            if (mInfo is not null && dependencyObject is not null)
            {
                result = (bool)(mInfo.Invoke (null, new[] { Activator.CreateInstance (dependencyObject) }) ?? false);
            }
        }
        else if ((type = Type.GetType (WpfDesignerPropertiesType, false)) is not null)
        {
            // loaded the assembly, could be .net
            var mInfo = type.GetMethod (WpfDesignerPropertiesDesignModeMethod);
            var dependencyObject = Type.GetType (WpfDependencyPropertyType, false);
            if (mInfo is not null && dependencyObject is not null)
            {
                result = (bool)(mInfo.Invoke (null, new[] { Activator.CreateInstance (dependencyObject) }) ?? false);
            }
        }
        else if ((type = Type.GetType (WinFormsDesignerPropertiesType, false)) is not null)
        {
            // check WinRT next
            result = (bool)(type.GetProperty (WinFormsDesignerPropertiesDesignModeMethod)?.GetMethod
                ?.Invoke (null, null) ?? false);
        }
        else
        {
            var designEnvironments = new[] { "BLEND.EXE", "XDESPROC.EXE" };

            var entry = Assembly.GetEntryAssembly();
            if (entry is not null)
            {
                var exeName = new FileInfo (entry.Location).Name;

                if (designEnvironments.Any (x =>
                        x.IndexOf (exeName, StringComparison.InvariantCultureIgnoreCase) != -1))
                {
                    result = true;
                }
            }
        }

        return result;
    }

    // /// <summary>
    // /// Трассировочное логирование.
    // /// </summary>
    // [Conditional ("TRACE")]
    // public static void Trace
    //     (
    //         string message
    //     )
    // {
    //     Logger.LogTrace (message);
    // }
    //
    // /// <summary>
    // /// Трассировочное логирование.
    // /// </summary>
    // [Conditional ("TRACE")]
    // public static void Trace
    //     (
    //         string message,
    //         params object[] args
    //     )
    // {
    //     Logger.LogTrace (message, args);
    // }
    //
    // /// <summary>
    // /// Логирует сообщение об ошибке.
    // /// </summary>
    // [Conditional ("TRACE")]
    // public static void Trace
    //     (
    //         Func<string> lazy
    //     )
    // {
    //     if (Logger.IsEnabled (LogLevel.Trace))
    //     {
    //         Logger.LogTrace (lazy());
    //     }
    // }
    //
    // /// <summary>
    // /// Регистрация исключения в логах.
    // /// </summary>
    // public static void TraceException
    //     (
    //         string text,
    //         Exception exception
    //     )
    // {
    //     Logger.Log (LogLevel.Trace, exception, text);
    // }
    //
    // /// <summary>
    // /// Логирование предупреждения.
    // /// </summary>
    // public static void Warning
    //     (
    //         string message
    //     )
    // {
    //     Logger.LogWarning (message);
    // }
    //
    // /// <summary>
    // /// Логирование предупреждения.
    // /// </summary>
    // public static void Warning
    //     (
    //         string message,
    //         params object[] args
    //     )
    // {
    //     Logger.LogWarning (message, args);
    // }
    //
    // /// <summary>
    // /// Логирование предупреждения.
    // /// </summary>
    // public static void Warning
    //     (
    //         Func<string> lazy
    //     )
    // {
    //     if (Logger.IsEnabled (LogLevel.Warning))
    //     {
    //         Logger.LogWarning (lazy());
    //     }
    // }

    #endregion
}
