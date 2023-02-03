// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MethodDescriptor.cs -- описание метода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;
using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

// TODO поддержать generic-методы

/// <summary>
/// Описание метода.
/// </summary>
public sealed class MethodDescriptor
{
    #region Properties

    /// <summary>
    /// Имя метода.
    /// </summary>
    public required string Name { get; init; }
    
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
        if (obj is not MethodDescriptor other)
        {
            return false;
        }

        if (Name != other.Name)
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
            return $"{Name}()";
        }
        
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (Name);
        builder.Append ('(');
        builder.AppendList (Arguments);
        builder.Append (')');

        return builder.ReturnShared();
    }

    #endregion
}
