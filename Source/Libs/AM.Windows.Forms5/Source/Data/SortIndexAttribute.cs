// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* SortIndexAttribute.cs -- задает индекс для сортировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Задает индекс для сортировки.
/// </summary>
[Serializable]
[AttributeUsage (AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SortIndexAttribute
    : Attribute
{
    #region Properties

    /// <summary>
    /// Индекс для сортировки.
    /// </summary>
    public int SortIndex { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SortIndexAttribute
        (
            int index
        )
    {
        SortIndex = index;
    }

    #endregion
}
