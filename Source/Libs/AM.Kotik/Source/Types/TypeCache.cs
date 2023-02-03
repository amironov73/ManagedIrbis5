// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TypeCache.cs -- кэш типов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Кэш типов.
/// </summary>
public sealed class TypeCache
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TypeCache()
    {
        _dictionary = new ();
    }

    #endregion
    
    #region Private members

    private readonly Dictionary<TypeDescriptor, Type> _dictionary;

    #endregion
    
    #region Public methods

    /// <summary>
    /// Добавление типа в кэш.
    /// </summary>
    public void Add 
        (
            TypeDescriptor descriptor,
            Type type
        )
    {
        Sure.NotNull (descriptor);
        Sure.NotNull (type);

        _dictionary[descriptor] = type;
    }
    
    /// <summary>
    /// Очистка кэша.
    /// </summary>
    public void Clear()
    {
        _dictionary.Clear();
    }

    /// <summary>
    /// Попытка получения типа по его дескриптору.
    /// </summary>
    public bool TryGetType
        (
            TypeDescriptor descriptor,
            out Type? type
        )
    {
        Sure.NotNull (descriptor);

        return _dictionary.TryGetValue (descriptor, out type);
    }

    #endregion
}
