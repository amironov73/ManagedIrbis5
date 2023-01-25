// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LambdaNode.cs -- определение лямбда-функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Определение лямбда-функции.
/// </summary>
internal sealed class LambdaNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LambdaNode
        (
            string[] argumentNames,
            StatementBase body
        )
    {
        Sure.NotNull (argumentNames);
        Sure.NotNull (body);

        this.argumentNames = argumentNames;
        this.body = body;
    }

    #endregion

    #region Private members

    internal readonly string[] argumentNames;
    internal readonly StatementBase body;

    #endregion

    #region Public methods

    /// <summary>
    /// Переходник для обработки нативных событий.
    /// </summary>
    public dynamic? Adapter
        (
            Context context,
            dynamic?[] arguments
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (arguments);

        try
        {
            var lambdaContext = context.CreateChildContext();
            var index = 0;
            foreach (var argumentName in argumentNames)
            {
                if (index >= arguments.Length)
                {
                    break;
                }

                lambdaContext.Variables[argumentName] = arguments[index];
                ++index;
            }

            body.Execute (lambdaContext);
        }
        catch (ReturnException exception)
        {
            return exception.Value;
        }

        return null;
    }

    /// <summary>
    /// Выполнение функции.
    /// </summary>
    public dynamic? Execute
        (
            Context context,
            IList<AtomNode> arguments
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (arguments);

        try
        {
            var innerContext = context.CreateChildContext();
            var index = 0;
            foreach (var argumentName in argumentNames)
            {
                if (index >= arguments.Count)
                {
                    break;
                }

                innerContext.Variables[argumentName] = arguments[index].Compute (context);
                ++index;
            }

            body.Execute (innerContext);
        }
        catch (ReturnException exception)
        {
            return exception.Value;
        }

        return null;
    }

    #endregion

    #region AtomNode members

    /// <summary>
    /// Выполнение функции.
    /// </summary>
    public override dynamic Compute
        (
            Context context
        )
    {
        return this;
    }

    #endregion

}
