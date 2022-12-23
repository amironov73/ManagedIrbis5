// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ITypeDescriptorCache.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public class TypeDescriptorCache
    : ITypeDescriptorCache
{
    #region Construciton

    static TypeDescriptorCache()
    {
        _cache = new Cache<Type, IDictionary<string, PropertyInfo>> (type =>
            type.GetProperties (BindingFlags.Public | BindingFlags.Instance)
                .Where (propertyInfo => propertyInfo.CanWrite)
                .ToDictionary (propertyInfo => propertyInfo.Name));
    }

    #endregion

    #region Private members

    private static readonly Cache<Type, IDictionary<string, PropertyInfo>> _cache;

    #endregion

    #region ITypeDescriptorCache

    /// <inheritdoc cref="ITypeDescriptorCache.GetPropertiesFor{T}"/>
    public IDictionary<string, PropertyInfo> GetPropertiesFor<T>() => GetPropertiesFor (typeof (T));

    /// <inheritdoc cref="ITypeDescriptorCache.GetPropertiesFor"/>
    public IDictionary<string, PropertyInfo> GetPropertiesFor (Type itemType) => _cache[itemType];

    /// <inheritdoc cref="ITypeDescriptorCache.ForEachProperty"/>
    public void ForEachProperty
        (
            Type itemType,
            Action<PropertyInfo> action
        )
    {
        Sure.NotNull (itemType);
        Sure.NotNull (action);

        _cache[itemType].Values.Each (action);
    }

    /// <inheritdoc cref="ITypeDescriptorCache.ClearAll"/>
    public void ClearAll() => _cache.ClearAll();

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelType"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static PropertyInfo? GetPropertyFor
        (
            Type modelType,
            string propertyName
        )
    {
        Sure.NotNull (modelType);
        Sure.NotNullNorEmpty (propertyName);

        var dict = _cache[modelType];

        return dict.ContainsKey (propertyName) ? dict[propertyName] : null;
    }

    #endregion
}
