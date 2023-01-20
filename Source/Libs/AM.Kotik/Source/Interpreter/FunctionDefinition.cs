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
using System.Linq;

#endregion

#nullable enable

namespace AM.Kotik;

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
            string[] arguments,
            StatementBase body
        )
    {
        Sure.NotNull (name);
        Sure.NotNull (arguments);
        Sure.NotNull (body);

        _name = name;
        _arguments = arguments.ToArray();
        _body = body;
    }

    #endregion

    #region Private members

    // имя функции
    private readonly string _name;

    // имена аргументов
    private readonly string[] _arguments;

    // тело функции
    private readonly StatementBase _body;

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
            var innerContext = context.CreateChildContext();
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

            _body.Execute (innerContext);
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
