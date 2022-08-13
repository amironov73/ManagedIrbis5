// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DisplayFormatAttribute.cs -- задает формат отображения значений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Задает формат отображения значений.
/// </summary>
[Serializable]
[AttributeUsage (AttributeTargets.Property | AttributeTargets.Field)]
public sealed class DisplayFormatAttribute
    : Attribute
{
    #region Properties

    /// <summary>
    /// Формат отображения значений.
    /// </summary>
    public string DisplayFormat { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DisplayFormatAttribute
        (
            string displayFormat
        )
    {
        DisplayFormat = displayFormat;
    }

    #endregion
}
