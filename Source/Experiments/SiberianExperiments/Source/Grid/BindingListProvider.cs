// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BindingListProvider.cs -- провайдер для грида, принимающий IBindingList
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid;

/// <summary>
/// Провайдер для грида, принимающий <see cref="IBindingList"/>.
/// </summary>
public sealed class BindingListProvider
    : ISiberianProvider
{
    #region Properties

    /// <summary>
    /// Список, подлежащий адаптации.
    /// </summary>
    public IBindingList List { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BindingListProvider (IBindingList list) => List = list;

    #endregion

    #region ISiberianProvider members

    /// <inheritdoc cref="AddData"/>
    public void AddData
        (
            object? data
        )
    {
        var lastIndex = DataLength;
        List.AddNew();
        PutData (lastIndex, data);
    }

    /// <inheritdoc cref="DataLength"/>
    public int DataLength => List.Count;

    /// <inheritdoc cref="ISiberianProvider.GetData"/>
    public object? GetData (int index) => List[index];

    /// <inheritdoc cref="ISiberianProvider.PutData"/>
    public void PutData (int index, object? data) => List[index] = data;

    #endregion
}
