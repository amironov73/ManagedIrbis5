// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConstructorCache.cs -- кеш конструкторов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Reflection;

#endregion

namespace AM.Lexey.Types;

/// <summary>
/// Кеш конструкторов.
/// </summary>
public sealed class ConstructorCache
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ConstructorCache()
    {
        _dictionary = new ();
    }

    #endregion
    
    #region Private members

    private readonly Dictionary<ConstructorDescriptor, ConstructorInfo> _dictionary;

    #endregion
    
    #region Public methods

    /// <summary>
    /// Добавление конструктора в кэш.
    /// </summary>
    public void Add 
        (
            ConstructorDescriptor descriptor,
            ConstructorInfo constructor
        )
    {
        Sure.NotNull (descriptor);
        Sure.NotNull (constructor);

        _dictionary[descriptor.Clone()] = constructor;
    }
    
    /// <summary>
    /// Очистка кэша.
    /// </summary>
    public void Clear()
    {
        _dictionary.Clear();
    }

    /// <summary>
    /// Попытка получения конструктора по его дескриптору.
    /// </summary>
    public bool TryGetConstructor
        (
            ConstructorDescriptor descriptor,
            out ConstructorInfo? constructor
        )
    {
        Sure.NotNull (descriptor);

        return _dictionary.TryGetValue (descriptor, out constructor);
    }

    #endregion
}
