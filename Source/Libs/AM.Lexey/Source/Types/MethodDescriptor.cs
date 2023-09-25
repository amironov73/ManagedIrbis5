// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable NonReadonlyMemberInGetHashCode

/* MethodDescriptor.cs -- описание метода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;
using AM.Text;

#endregion

#nullable enable

namespace AM.Lexey.Types;

// TODO поддержать generic-методы

/// <summary>
/// Описание метода.
/// </summary>
public sealed class MethodDescriptor
{
    #region Properties

    /// <summary>
    /// Тип, которому принадлежит член.
    /// </summary>
    public Type? Type { get; set; }

    /// <summary>
    /// Имя метода.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Аргументы.
    /// </summary>
    public Type[]? Arguments { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание полной копии дескриптора.
    /// </summary>
    /// <returns></returns>
    public MethodDescriptor Clone()
    {
        return (MethodDescriptor) MemberwiseClone();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
             object? obj
        )
    {
        if (obj is not MethodDescriptor other)
        {
            return false;
        }

        if (Type != other.Type || Name != other.Name)
        {
            return false;
        }

        if (Arguments is null)
        {
            return other.Arguments is null;
        }

        if (Arguments.Length != other.Arguments!.Length)
        {
            return false;
        }

        for (var i = 0; i < Arguments.Length; i++)
        {
            if (Arguments[i] != other.Arguments![i])
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        var result = new HashCode();
        result.Add (Type);
        result.Add (Name);
        if (!Arguments.IsNullOrEmpty())
        {
            foreach (var parameter in Arguments)
            {
                result.Add (parameter);
            }
        }

        return result.ToHashCode();
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        if (Arguments.IsNullOrEmpty())
        {
            return $"{Type?.FullName}.{Name}()";
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.Append (Type?.FullName);
        builder.Append ('.');
        builder.Append (Name);
        builder.Append ('(');
        builder.AppendList (Arguments);
        builder.Append (')');

        return builder.ReturnShared();
    }

    #endregion
}
