// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

/* ITypeDescriptorCache.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public interface ITypeDescriptorCache
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IDictionary<string, PropertyInfo> GetPropertiesFor<T>();

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    IDictionary<string, PropertyInfo> GetPropertiesFor (Type itemType);

    /// <summary>
    ///
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="action"></param>
    void ForEachProperty (Type itemType, Action<PropertyInfo> action);

    /// <summary>
    ///
    /// </summary>
    void ClearAll();
}
