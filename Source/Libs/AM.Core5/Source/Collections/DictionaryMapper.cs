// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* DictionaryMapper.cs -- отображает элементы словаря на свойства объекта и обратно
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Отображает элементы словаря на свойства объекта и обратно.
/// </summary>
public static class DictionaryMapper
{
    #region Constants

    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;

    #endregion

    #region Public methods

    /// <summary>
    /// Отображение элементов словаря на свойства объекта.
    /// </summary>
    public static void FromObject
        (
            object source,
            IDictionary target
        )
    {
        Sure.NotNull (target);
        Sure.NotNull (source);

        var properties = source.GetType().GetProperties (Flags);
        foreach (var property in properties)
        {
            var indexParameters = property.GetIndexParameters();
            if (indexParameters.Length == 0)
            {
                var value = property.GetValue (source);
                target[property.Name] = value;
            }
        }
    }

    /// <summary>
    /// Отображение элементов словаря на свойства объекта.
    /// </summary>
    public static void ToObject
        (
            IDictionary source,
            object target
        )
    {
        Sure.NotNull (source);
        Sure.NotNull (target);

        var type = target.GetType();
        foreach (DictionaryEntry entry in source)
        {
            var name = entry.Key as string;
            if (string.IsNullOrEmpty (name))
            {
                continue;
            }

            var property = type.GetProperty (name, Flags);
            if (property is null)
            {
                continue;
            }

            var indexParameters = property.GetIndexParameters();
            if (indexParameters.Length == 0)
            {
                property.SetValue (target, entry.Value);
            }
        }
    }

    #endregion
}
