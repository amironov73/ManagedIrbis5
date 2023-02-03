// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MethodCache.cs -- кэш методов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Кэш методов.
/// </summary>
public sealed class MethodCache
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MethodCache()
    {
        _dictionary = new ();
    }

    #endregion
    
    #region Private members

    private readonly Dictionary<MethodDescriptor, MethodInfo> _dictionary;

    #endregion
    
    #region Public methods

    /// <summary>
    /// Добавление метода в кэш.
    /// </summary>
    public void Add 
        (
            MethodDescriptor descriptor,
            MethodInfo method
        )
    {
        Sure.NotNull (descriptor);
        Sure.NotNull (method);

        _dictionary[descriptor] = method;
    }
    
    /// <summary>
    /// Очистка кэша.
    /// </summary>
    public void Clear()
    {
        _dictionary.Clear();
    }

    /// <summary>
    /// Попытка получения метода по его дескриптору.
    /// </summary>
    public bool TryGetMethod
        (
            MethodDescriptor descriptor,
            out MethodInfo? method
        )
    {
        Sure.NotNull (descriptor);

        return _dictionary.TryGetValue (descriptor, out method);
    }

    #endregion
}
