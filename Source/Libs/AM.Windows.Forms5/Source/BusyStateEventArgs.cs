// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BusyStateEventArgs.cs -- аргументы для события "занято"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Threading;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргументы для события "занято".
/// </summary>
public sealed class BusyStateEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Состояние.
    /// </summary>
    public BusyState State { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BusyStateEventArgs 
        (
            BusyState state
        )
    {
        Sure.NotNull (state);
        
        State = state;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => State.ToString();

    #endregion
}
