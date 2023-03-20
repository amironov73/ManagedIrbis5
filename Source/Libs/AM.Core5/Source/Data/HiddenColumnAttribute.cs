// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* HiddenColumnAttribute.cs -- скрывает колонку в гриде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Data;

/// <summary>
/// Скрывает колонку в гриде.
/// </summary>
[AttributeUsage (AttributeTargets.Property)]
public sealed class HiddenColumnAttribute
    : Attribute
{
    #region Properties

    ///<summary>
    /// Признак скрытой колонки.
    ///</summary>
    public bool Hidden { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public HiddenColumnAttribute()
        : this (true)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HiddenColumnAttribute
        (
            bool hidden
        )
    {
        Hidden = hidden;
    }

    #endregion
}
