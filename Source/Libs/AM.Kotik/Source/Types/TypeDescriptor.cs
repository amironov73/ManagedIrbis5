// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable NonReadonlyMemberInGetHashCode

/* TypeDescriptor.cs -- описание типа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Описание типа.
/// </summary>
public sealed class TypeDescriptor
{
    #region Properties

    /// <summary>
    /// Имя типа.
    /// </summary>
    public string? TypeName { get; set; }
    
    /// <summary>
    /// Параметры обобщенного типа (опционально).
    /// </summary>
    public IList<string>? GenericParameters { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание полной копии дескриптора.
    /// </summary>
    public TypeDescriptor Clone()
    {
        return (TypeDescriptor) MemberwiseClone();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals 
        (
            object? obj
        )
    {
        if (obj is not TypeDescriptor other)
        {
            return false;
        }

        if (TypeName != other.TypeName)
        {
            return false;
        }

        if (GenericParameters is null)
        {
            return other.GenericParameters is null;
        }

        if (GenericParameters.Count != other.GenericParameters!.Count)
        {
            return false;
        }

        for (var i = 0; i < GenericParameters.Count; i++)
        {
            if (GenericParameters[i] != other.GenericParameters![i])
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
        result.Add (TypeName);
        if (GenericParameters is { Count: not 0 })
        {
            foreach (var parameter in GenericParameters)
            {
                result.Add (parameter);
            }
        }

        return result.ToHashCode();
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        if (GenericParameters is { Count: not 0 })
        {
            return TypeName.ToVisibleString();
        }
        
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (TypeName);
        builder.Append ('<');
        builder.AppendEnumerable (GenericParameters, open: string.Empty, close: string.Empty);
        builder.Append ('>');

        return builder.ReturnShared();
    }

    #endregion
}
