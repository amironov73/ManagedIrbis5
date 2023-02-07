// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConstructorDescriptor.cs -- описание конструктора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;
using AM.Text;

#endregion

namespace AM.Kotik.Types;

/// <summary>
/// Описание конструктора.
/// </summary>
public sealed class ConstructorDescriptor
{
    #region Properties

    /// <summary>
    /// Аргументы.
    /// </summary>
    public Type[]? Arguments { get; init; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        if (obj is not ConstructorDescriptor other)
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
            return $"Constructor()";
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("Constructor");
        builder.Append ('(');
        builder.AppendList (Arguments);
        builder.Append (')');

        return builder.ReturnShared();
    }

    #endregion
}
