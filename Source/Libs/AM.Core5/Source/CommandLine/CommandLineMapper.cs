// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CommandLineMapper.cs -- утилита для маппинга командной строки на объекты
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Reflection;

#endregion

namespace AM.CommandLine;

/// <summary>
/// Утилита для простейшего отображения результата разбора
/// коммандной строки на объекты.
/// </summary>
public static class CommandLineMapper
{
    #region Private members

    /// <summary>
    /// Маппинг одного свойства или поля объекта.
    /// </summary>
    private static void MapValue
        (
            IReflect type,
            object target,
            string name,
            object? value
        )
    {
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
        var propertyInfo = type.GetProperty (name, flags);
        if (propertyInfo is not null)
        {
            propertyInfo.SetValue (target, value);
        }
        else
        {
            var fieldInfo = type.GetField (name, flags);
            if (fieldInfo is not null)
            {
                fieldInfo.SetValue (target, value);
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Отображение результата разбора командной строки.
    /// </summary>
    /// <param name="parseResult">Результат разбора.</param>
    /// <param name="target">Объект назначения.</param>
    public static void MapParseResult
        (
            ParseResult parseResult,
            object target
        )
    {
        var type = target.GetType();
        var children = parseResult.CommandResult.Children;
        foreach (var child in children)
        {
            string name;
            object? value;

            switch (child)
            {
                case ArgumentResult argument:
                    name = argument.Argument.Name;
                    value = argument.GetValueOrDefault();
                    break;

                case OptionResult option:
                    name = option.Option.Name;
                    value = option.GetValueOrDefault();
                    break;

                default: // мы не знаем, что это такое, пропускаем
                    continue;
            }

            MapValue (type, target, name, value);
        }
    }

    /// <summary>
    /// Разбор командной строки и отображение результатов на существующий объект.
    /// </summary>
    /// <param name="command">Команда (в т. ч. корневая).</param>
    /// <param name="arguments">Аргументы командной строки.</param>
    /// <param name="target">Объект назначения.</param>
    public static void MapCommandResult
        (
            Command command,
            string[] arguments,
            object target
        )
    {
        var parserResult = new CommandLineBuilder (command)
            .Build()
            .Parse (arguments);
        MapParseResult (parserResult, target);
    }

    /// <summary>
    /// Разбор командной строки и отображение результатов на вновь созданный объект.
    /// </summary>
    /// <param name="command">Команда (в т. ч. корневая).</param>
    /// <param name="arguments">Аргументы командной строки.</param>
    /// <typeparam name="T">Тип результата</typeparam>
    /// <returns>Результат отображения на вновь созданный объект.</returns>
    public static T MapCommandResult<T>
        (
            this Command command,
            string[] arguments
        )
        where T : class, new()
    {
        var result = new T();
        MapCommandResult (command, arguments, result);

        return result;
    }

    #endregion
}
