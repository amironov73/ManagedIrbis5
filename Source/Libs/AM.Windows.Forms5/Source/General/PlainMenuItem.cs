// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#pragma warning disable CS0067

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PlainMenuItem.cs -- простой элемент меню
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms.General;

/// <summary>
/// Простой элемент меню, реализованный
/// стандартными компонентами WinForms.
/// </summary>
public class PlainMenuItem
    : IGeneralItem
{
    #region IGeneralItem members

    /// <inheritdoc cref="IGeneralItem.Id"/>
    public string Id => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralItem.Caption"/>
    public string Caption => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralItem.Enabled"/>
    public bool Enabled => throw new NotImplementedException();

    /// <summary>
    /// Основное действие.
    /// </summary>
    public event EventHandler? Execute;

    /// <summary>
    /// Обновление состояния.
    /// </summary>
    public event EventHandler? Update;

    #endregion
}
