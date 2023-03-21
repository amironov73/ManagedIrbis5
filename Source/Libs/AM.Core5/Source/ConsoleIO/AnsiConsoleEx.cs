// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AnsiConsoleEx.cs -- полезные методы для Spectre.Console
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Spectre.Console;

#endregion

#nullable enable

namespace AM.ConsoleIO;

/// <summary>
/// Полезные методы для <c>Spectre.Console</c>.
/// </summary>
[PublicAPI]
public static class AnsiConsoleEx
{
    #region Public methods

    /// <summary>
    /// Запуск операции с прогрессом и дополнительными аргументами.
    /// </summary>
    public static void Start<TArg1>
        (
            this Progress progress,
            Action<ProgressContext, TArg1> action,
            TArg1 arg1
        )
    {
        Sure.NotNull (progress);
        Sure.NotNull (action);

        progress.Start (it => action (it, arg1));
    }

    /// <summary>
    /// Запуск операции с прогрессом и дополнительными аргументами.
    /// </summary>
    public static void Start<TArg1, TArg2>
        (
            this Progress progress,
            Action<ProgressContext, TArg1, TArg2> action,
            TArg1 arg1,
            TArg2 arg2
        )
    {
        Sure.NotNull (progress);
        Sure.NotNull (action);

        progress.Start (it => action (it, arg1, arg2));
    }

    /// <summary>
    /// Запуск операции с прогрессом и дополнительными аргументами.
    /// </summary>
    public static void Start<TArg1, TArg2, TArg3>
        (
            this Progress progress,
            Action<ProgressContext, TArg1, TArg2, TArg3> action,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3
        )
    {
        Sure.NotNull (progress);
        Sure.NotNull (action);

        progress.Start (it => action (it, arg1, arg2, arg3));
    }

    /// <summary>
    /// Запуск операции со статусом и дополнительными аргументами.
    /// </summary>
    public static void Start<TArg1>
        (
            this Status status,
            string title,
            Action<StatusContext, TArg1> action,
            TArg1 arg1
        )
    {
        Sure.NotNull (status);
        Sure.NotNull (action);

        status.Start (title, it => action (it, arg1));
    }

    /// <summary>
    /// Запуск операции со статусом и дополнительными аргументами.
    /// </summary>
    public static void Start<TArg1, TArg2>
        (
            this Status status,
            string title,
            Action<StatusContext, TArg1, TArg2> action,
            TArg1 arg1,
            TArg2 arg2
        )
    {
        Sure.NotNull (status);
        Sure.NotNull (action);

        status.Start (title, it => action (it, arg1, arg2));
    }

    /// <summary>
    /// Запуск операции со статусом и дополнительными аргументами.
    /// </summary>
    public static void Start<TArg1, TArg2, TArg3>
        (
            this Status status,
            string title,
            Action<StatusContext, TArg1, TArg2, TArg3> action,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3
        )
    {
        Sure.NotNull (status);
        Sure.NotNull (action);

        status.Start (title, it => action (it, arg1, arg2, arg3));
    }

    /// <summary>
    /// Запуск операции с прогрессом и дополнительными аргументами.
    /// </summary>
    public static Task StartAsync<TArg1>
        (
            this Progress progress,
            Func<ProgressContext, TArg1, Task> func,
            TArg1 arg1
        )
    {
        Sure.NotNull (progress);
        Sure.NotNull (func);

        return progress.StartAsync (it => func (it, arg1));
    }

    /// <summary>
    /// Запуск операции с прогрессом и дополнительными аргументами.
    /// </summary>
    public static Task StartAsync<TArg1, TArg2>
        (
            this Progress progress,
            Func<ProgressContext, TArg1, TArg2, Task> func,
            TArg1 arg1,
            TArg2 arg2
        )
    {
        Sure.NotNull (progress);
        Sure.NotNull (func);

        return progress.StartAsync (it => func (it, arg1, arg2));
    }

    /// <summary>
    /// Запуск операции с прогрессом и дополнительными аргументами.
    /// </summary>
    public static Task StartAsync<TArg1, TArg2, TArg3>
        (
            this Progress progress,
            Func<ProgressContext, TArg1, TArg2, TArg3, Task> func,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3
        )
    {
        Sure.NotNull (progress);
        Sure.NotNull (func);

        return progress.StartAsync (it => func (it, arg1, arg2, arg3));
    }

    /// <summary>
    /// Запуск операции со статусом и дополнительными аргументами.
    /// </summary>
    public static Task StartAsync<TArg1>
        (
            this Status status,
            string title,
            Func<StatusContext, TArg1, Task> func,
            TArg1 arg1
        )
    {
        Sure.NotNull (status);
        Sure.NotNull (func);

        return status.StartAsync (title, it => func (it, arg1));
    }

    /// <summary>
    /// Запуск операции со статусом и дополнительными аргументами.
    /// </summary>
    public static Task StartAsync<TArg1, TArg2>
        (
            this Status status,
            string title,
            Func<StatusContext, TArg1, TArg2, Task> func,
            TArg1 arg1,
            TArg2 arg2
        )
    {
        Sure.NotNull (status);
        Sure.NotNull (func);

        return status.StartAsync (title, it => func (it, arg1, arg2));
    }

    /// <summary>
    /// Запуск операции со статусом и дополнительными аргументами.
    /// </summary>
    public static Task StartAsync<TArg1, TArg2, TArg3>
        (
            this Status status,
            string title,
            Func<StatusContext, TArg1, TArg2, TArg3, Task> func,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3
        )
    {
        Sure.NotNull (status);
        Sure.NotNull (func);

        return status.StartAsync (title, it => func (it, arg1, arg2, arg3));
    }

    #endregion
}
