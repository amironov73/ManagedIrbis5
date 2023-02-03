// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MemberCache.cs -- кэш свойств
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Reflection;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Кэш свойств и полей объектов.
/// </summary>
public sealed class MemberCache
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MemberCache()
    {
        _dictionary = new ();
    }

    #endregion
    
    #region Private members

    private readonly Dictionary<MemberDescriptor, PropertyOrField> _dictionary;

    #endregion
    
    #region Public methods

    /// <summary>
    /// Добавление метода в кэш.
    /// </summary>
    public void Add 
        (
            MemberDescriptor descriptor,
            PropertyOrField propertyOrField
        )
    {
        Sure.NotNull (descriptor);
        Sure.NotNull (propertyOrField);

        _dictionary[descriptor] = propertyOrField;
    }
    
    /// <summary>
    /// Очистка кэша.
    /// </summary>
    public void Clear()
    {
        _dictionary.Clear();
    }

    /// <summary>
    /// Попытка получения свойства по его дескриптору.
    /// </summary>
    public bool TryGetProperty
        (
            MemberDescriptor descriptor,
            out PropertyOrField? propertyOrField
        )
    {
        Sure.NotNull (descriptor);

        return _dictionary.TryGetValue (descriptor, out propertyOrField);
    }

    #endregion
}
