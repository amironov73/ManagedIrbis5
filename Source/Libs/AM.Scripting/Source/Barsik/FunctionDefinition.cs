// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FunctionDefiniton.cs -- определение функции в скрипте Барсик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Определение функции в скрипте Барсик.
/// </summary>
sealed class FunctionDefinition
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FunctionDefinition
        (
            string name,
            IEnumerable<string> arguments,
            IEnumerable<StatementNode> body
        )
    {
        Sure.NotNull (name);
        Sure.NotNull (arguments);
        Sure.NotNull (body);

        _name = name;
        _arguments = arguments.ToArray();
        _body = new List<StatementNode> (body);
    }

    #endregion

    #region Private members

    // имя функции
    private readonly string _name;

    // имена аргументов
    private readonly string[] _arguments;

    // тело функции
    private readonly List<StatementNode> _body;

    #endregion

    #region Public methods

    /// <summary>
    /// Создание точки вызова.
    /// </summary>
    public Func<Context, dynamic?[], dynamic?> CreateCallPoint()
    {
        return Execute;
    }

    /// <summary>
    /// Выполнение функции.
    /// </summary>
    public dynamic? Execute
        (
            Context context,
            dynamic?[] argumentValues
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (argumentValues);

        _name.NotUsed ();

        try
        {
            var innerContext = context.CreateChild();
            var index = 0;
            foreach (var argumentName in _arguments)
            {
                if (index >= argumentValues.Length)
                {
                    break;
                }

                innerContext.Variables[argumentName] = argumentValues[index];
                ++index;
            }

            foreach (var statement in _body)
            {
                statement.Execute (innerContext);
            }
        }
        catch (ReturnException exception)
        {
            return exception.Value;
        }

        return null;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var args = string.Join (",", _arguments);
        return $"func {_name} ({args})";
    }

    #endregion
}
