// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MemberDescriptor.cs -- описание члена типа (свойства или поля)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Описание члена типа (свойства или поля).
/// </summary>
public sealed class MemberDescriptor
{
    #region Properties

    /// <summary>
    /// Тип, которому принадлежит член.
    /// </summary>
    public required Type Type { get; init; }
    
    /// <summary>
    /// Имя члена.
    /// </summary>
    public required string MemberName { get; init; }

    #endregion
    
    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj) => 
        obj is MemberDescriptor other 
        && Type == other.Type 
        && MemberName == other.MemberName;

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => HashCode.Combine (Type, MemberName);

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Type}::{MemberName}";

    #endregion
}
