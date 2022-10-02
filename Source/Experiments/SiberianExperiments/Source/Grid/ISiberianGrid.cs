// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverInvoked.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedType.Global

// Событие никогда не используется
#pragma warning disable CS0067

/* ISiberianGrid.cs -- интерфейс грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid;

/// <summary>
/// Интерфейс грида.
/// </summary>
public interface ISiberianGrid
{
    #region Events

    /// <summary>
    /// Событие, возникающее при щелчке по гриду.
    /// </summary>
    event EventHandler<SiberianClickEventArgs>? GridClick;

    #endregion

    #region Properties

    /// <summary>
    /// Колонки.
    /// </summary>
    ISiberianColumnCollection Columns { get; }

    /// <summary>
    /// Строки.
    /// </summary>
    ISiberianRowCollection Rows { get; }

    #endregion

    #region Public methods

    /// <inheritdoc cref="Control.Invalidate(System.Drawing.Region)"/>
    public void Invalidate();

    #endregion
}
