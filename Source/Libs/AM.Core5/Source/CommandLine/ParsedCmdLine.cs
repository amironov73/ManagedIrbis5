// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ParsedCmdLine.cs -- результат разбора командной строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

#endregion

namespace AM.CommandLine;

/// <summary>
/// Результат разбора командной строки.
/// </summary>
[PublicAPI]
public sealed class ParsedCmdLine
{
    #region Properties

    /// <summary>
    /// Нечувствителен к регистру символов?
    /// </summary>
    public bool CaseInsensitive { get; set; }

    /// <summary>
    /// Позиционные аргументы командной строки.
    /// </summary>
    public List<string> PositionalArguments { get; } = new ();

    /// <summary>
    /// Опции.
    /// </summary>
    public List<CmdOption> Options { get; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка на то, что все опции использованы,
    /// не осталось неизвестных.
    /// </summary>
    public bool CheckForUnknownOptions
        (
            bool throwOnError = true
        )
    {
        foreach (var option in Options)
        {
            if (!option.Used)
            {
                if (throwOnError)
                {
                    throw new ArsMagnaException ($"Unknown option: {option}");
                }

                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Получение опции с указанным именем.
    /// </summary>
    public CmdOption? GetOption
        (
            string name,
            string? defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (name);

        var comparer = CaseInsensitive
            ? StringComparer.InvariantCultureIgnoreCase
            : StringComparer.InvariantCulture;
        foreach (var option in Options)
        {
            if (comparer.Compare (name, option.Name) == 0)
            {
                option.Used = true;
                return option;
            }
        }

        return defaultValue is null ? default
            : new CmdOption { Name = name, Value = defaultValue };
    }

    /// <summary>
    /// Отображает неиспользованные опции на свойства объекта.
    /// </summary>
    public void MapOptions<TTarget>
        (
            TTarget targetObject
        )
        where TTarget: class
    {
        Sure.NotNull (targetObject);

        var type = typeof (TTarget);
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var properties = type.GetProperties (flags);

        foreach (var option in Options)
        {
            if (option.Used)
            {
                continue;
            }

            PropertyInfo? targetProperty = default;

            // сначала ищем по атрибутам
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<CmdOptionAttribute>();
                if (option.Name.SameString (attribute?.Name))
                {
                    targetProperty = property;
                    break;
                }
            }

            // если не удалось, то по совпадению имени опции и свойства
            if (targetProperty is null)
            {
                foreach (var property in properties)
                {
                    if (option.Name.SameString (property.Name))
                    {
                        targetProperty = property;
                        break;
                    }
                }
            }

            if (targetProperty is not null)
            {
                option.Used = true;
                var targetValue = Utility.ConvertTo
                    (
                        option.Value!,
                        targetProperty.PropertyType
                    );
                targetProperty.SetValue (targetObject, targetValue);
            }
        }
    }

    /// <summary>
    /// Удаляет указанное количество первых позиционных аргументов.
    /// </summary>
    public void RemoveFirstArguments
        (
            int count
        )
    {
        while (PositionalArguments.Count != 0 && count > 0)
        {
            PositionalArguments.RemoveAt (0);
            count--;
        }
    }

    /// <summary>
    /// Удаляет из списка опции, которые уже были использованы.
    /// </summary>
    public void RemoveUsedOptions()
    {
        for (var i = 0; i < Options.Count; i++)
        {
            if (Options[i].Used)
            {
                Options.RemoveAt (i);
            }
            else
            {
                i++;
            }
        }
    }

    #endregion
}
