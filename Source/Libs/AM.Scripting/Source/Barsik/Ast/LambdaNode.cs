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

namespace AM.Scripting.Barsik;

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
            IEnumerable<string> theArguments,
            IEnumerable<StatementNode> theBody
        )
    {
        this.theArguments = new (theArguments);
        this.theBody = new (theBody);
    }

    #endregion

    #region Private members

    internal readonly List<string> theArguments;
    internal readonly List<StatementNode> theBody;

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
            var innerContext = context.CreateChild();
            var index = 0;
            foreach (var argumentName in theArguments)
            {
                if (index >= arguments.Length)
                {
                    break;
                }

                innerContext.Variables[argumentName] = arguments[index];
                ++index;
            }

            foreach (var statement in theBody)
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
            var innerContext = context.CreateChild();
            var index = 0;
            foreach (var argumentName in theArguments)
            {
                if (index >= arguments.Count)
                {
                    break;
                }

                innerContext.Variables[argumentName] = arguments[index].Compute (context);
                ++index;
            }

            foreach (var statement in theBody)
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

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        return this;
    }

    #endregion
}
