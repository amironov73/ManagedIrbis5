// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* PropFile.cs -- файл с prop-строками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using AM.Reflection;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.IO.PropFiles;

/// <summary>
/// Файл с prop-строками.
/// </summary>
[PublicAPI]
public sealed class PropFile
{
    #region Private members

    private static Dictionary<string, PropertyOrField> GetProps
        (
            Type type
        )
    {
        var result = new Dictionary<string, PropertyOrField>();
        var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
        foreach (var propertyInfo in type.GetProperties (bindingFlags))
        {
            var propAttribute = propertyInfo.GetCustomAttribute<PropAttribute>();
            if (propAttribute is not null)
            {
                var keyName = propAttribute.KeyName.ThrowIfNullOrEmpty();
                var property = new PropertyOrField (propertyInfo);
                result[keyName] = property;
            }
        }

        foreach (var fieldInfo in type.GetFields())
        {
            var propAttribute = fieldInfo.GetCustomAttribute<PropAttribute>();
            if (propAttribute is not null)
            {
                var keyName = propAttribute.KeyName.ThrowIfNullOrEmpty();
                var property = new PropertyOrField (fieldInfo);
                result[keyName] = property;
            }
        }

        return result;
    }

    #endregion
    
    #region Public methods

    /// <summary>
    /// Чтение prop-файла в указанный объект.
    /// </summary>
    public static void Read<TTarget>
        (
            TTarget target,
            string fileName
        )
        where TTarget: class
    {
        Sure.NotNull (target);
        Sure.FileExists (fileName);

        var type = typeof (TTarget);
        var dictionary = GetProps (type);
        foreach (var line in File.ReadLines (fileName))
        {
            if (string.IsNullOrEmpty (line))
            {
                continue;
            }

            var parts = line.Split ('=', 2);
            if (parts.Length != 2)
            {
                continue;
            }

            var keyName = parts[0].Trim();
            var value = parts[1].Trim().Unquote();
            if (string.IsNullOrEmpty (keyName) || string.IsNullOrEmpty (value))
            {
                continue;
            }

            if (dictionary.TryGetValue (keyName, out var prop))
            {
                prop.SetValue (target, value);
            }
        }
    }

    #endregion
}
