// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* LockMark.cs -- тикет блокировки базы данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Тикет блокировки базы данных.
/// </summary>
public readonly struct LockMark
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Провайдер.
    /// </summary>
    public DirectProvider Provider { get; }

    /// <summary>
    /// Стратегия.
    /// </summary>
    public IDirectLockingStrategy Strategy { get; }

    /// <summary>
    /// Имя заблокированной базы данных.
    /// </summary>
    public string Database { get; }

    /// <summary>
    /// Признак успешной блокировки.
    /// </summary>
    public bool Success { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LockMark
        (
            DirectProvider provider,
            IDirectLockingStrategy strategy,
            string database,
            bool success
        )
    {
        Provider = provider;
        Strategy = strategy;
        Database = database;
        Success = success;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Оператор неявного преобразования в логическое значение.
    /// </summary>
    public static implicit operator bool (LockMark mark) => mark.Success;

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        if (Success)
        {
            if (!Strategy.UnlockDatabase (Provider, Database))
            {
                Magna.Logger.LogWarning
                    (
                        nameof (LockMark) + "::" + nameof (Dispose)
                        + ": can't unlock database {Database}",
                        Database
                    );
            }
        }
    }

    #endregion
}
