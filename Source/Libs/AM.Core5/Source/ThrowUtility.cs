// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ThrowUtility.cs -- вспомогательные методы для выбрасывания исключений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Вспомогательные методы для выбрасывания исключений
/// </summary>
[PublicAPI]
public static class ThrowUtility
{
    #region Public methods

    /// <summary>
    /// Создание исключения с предварительным выполнением
    /// некоторого действия.
    /// </summary>
    public static TException CreateException<TException>
        (
            Action action
        )
        where TException: Exception, new()
    {
        Sure.NotNull (action);

        action();

        return new TException();
    }

    /// <summary>
    /// Создание исключения с предварительным выполнением
    /// некоторого действия.
    /// </summary>
    public static TException CreateException<TException, TArgument>
        (
            Action<TArgument> action,
            TArgument argument
        )
        where TException: Exception, new()
    {
        Sure.NotNull (action);

        action (argument);

        return new TException();
    }

    /// <summary>
    /// Создание исключения с предварительным выполнением
    /// некоторого действия.
    /// </summary>
    public static TException CreateException<TException>
        (
            Action action,
            string message
        )
        where TException: Exception
    {
        Sure.NotNull (action);

        action();

        return (TException) Activator.CreateInstance
            (
                typeof (TException),
                message
            )
            .ThrowIfNull();
    }

    /// <summary>
    /// Создание исключения с предварительным выполнением
    /// некоторого действия.
    /// </summary>
    public static TException CreateException<TException, TArgument>
        (
            Action<TArgument> action,
            TArgument argument,
            string message
        )
        where TException: Exception
    {
        Sure.NotNull (action);

        action (argument);

        return (TException) Activator.CreateInstance
            (
                typeof (TException),
                message
            )
            .ThrowIfNull();
    }

    /// <summary>
    /// Создание исключения с предварительным выполнением
    /// некоторого действия.
    /// </summary>
    public static TException CreateException<TException, TArgument1, TArgument2>
        (
            Action<TArgument1, TArgument2> action,
            TArgument1 argument1,
            TArgument2 argument2
        )
        where TException: Exception, new()
    {
        Sure.NotNull (action);

        action (argument1, argument2);

        return new TException();
    }

    /// <summary>
    /// Создание исключения с предварительным выполнением
    /// некоторого действия.
    /// </summary>
    public static TException CreateException<TException, TArgument1, TArgument2>
        (
            Action<TArgument1, TArgument2> action,
            TArgument1 argument1,
            TArgument2 argument2,
            string message
        )
        where TException: Exception
    {
        Sure.NotNull (action);

        action (argument1, argument2);

        return (TException) Activator.CreateInstance
            (
                typeof (TException),
                message
            )
            .ThrowIfNull();
    }

    #endregion
}
