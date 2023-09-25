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

namespace AM.Lexey.Barsik.Ast;

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
            IList<string> argumentNames,
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

    internal readonly IList<string> argumentNames;
    internal readonly StatementBase body;

    #endregion

    #region Public methods

    /// <summary>
    /// Переходник для обработки нативных событий.
    /// </summary>
    public dynamic? Adapter
        (
            Context context,
            IList<dynamic?> arguments
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
                if (index >= arguments.Count)
                {
                    break;
                }

                lambdaContext.Variables[argumentName] = arguments[index];
                ++index;
            }

            body.Execute (lambdaContext);
        }
        catch (GotoException exception)
        {
            // не позволяем goto сбежать из лямбды
            throw new BarsikException ($"Can't find label {exception.Label}");
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
        catch (GotoException exception)
        {
            // не позволяем goto сбежать из лямбды
            throw new BarsikException ($"Can't find label {exception.Label}");
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
