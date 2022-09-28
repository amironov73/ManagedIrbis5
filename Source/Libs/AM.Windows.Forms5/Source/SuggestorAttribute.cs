// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* SuggestorAttribute.cs -- атрибут, позволяющий задать подсказчик для свойства
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аттрибут, позволяющий задать подсказчик для свойства объекта.
/// </summary>
[AttributeUsage (AttributeTargets.Property )]
public sealed class SuggestorAttribute
    : Attribute
{
    #region Properties

    ///<summary>
    /// Тип подсказчика.
    ///</summary>
    public Type SuggestorType { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SuggestorAttribute
        (
            Type type
        )
    {
        SuggestorType = type;
    }

    #endregion
}
