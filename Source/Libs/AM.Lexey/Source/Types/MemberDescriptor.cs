// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable NonReadonlyMemberInGetHashCode

/* MemberDescriptor.cs -- описание члена типа (свойства или поля)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Lexey.Types;

/// <summary>
/// Описание члена типа (свойства или поля).
/// </summary>
public sealed class MemberDescriptor
{
    #region Properties

    /// <summary>
    /// Тип, которому принадлежит член.
    /// </summary>
    public Type? Type { get; set; }

    /// <summary>
    /// Имя члена.
    /// </summary>
    public string? Name { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание полной копии описания.
    /// </summary>
    public MemberDescriptor Clone()
    {
        return (MemberDescriptor) MemberwiseClone();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj) =>
        obj is MemberDescriptor other
        && Type == other.Type
        && Name == other.Name;

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => HashCode.Combine (Type, Name);

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Type?.FullName}::{Name}";

    #endregion
}
