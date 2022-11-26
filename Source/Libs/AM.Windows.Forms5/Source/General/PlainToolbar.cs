// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#pragma warning disable CS0067

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PlainToolbar.cs -- простой тулбар
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms.General;

/// <summary>
/// Простой тулбар, реализованный целиком средствами
/// стандартных компонентов WinForms.
/// </summary>
public class PlainToolbar
    : IGeneralItem,
    IGeneralItemList
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

    #region IGeneralItemList members

    /// <inheritdoc cref="IGeneralItemList.Count"/>
    public int Count => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralItemList.this[int]"/>
    public IGeneralItem this [int index] => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralItemList.this[string]"/>
    public IGeneralItem this [string id] => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralItemList.Add"/>
    public void Add (IGeneralItem item) => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralItemList.Clear"/>
    public void Clear() => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralItemList.CreateItem"/>
    public IGeneralItem CreateItem (string id, string caption) => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralItemList.Contains"/>
    public bool Contains (IGeneralItem item) => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralItemList.Remove"/>
    public void Remove (IGeneralItem item) => throw new NotImplementedException();

    #endregion
}
