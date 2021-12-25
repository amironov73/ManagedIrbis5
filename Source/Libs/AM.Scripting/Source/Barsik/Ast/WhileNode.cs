// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* WhileNode.cs -- цикл while
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable


namespace AM.Scripting.Barsik;

/// <summary>
/// Цикл while.
/// </summary>
internal sealed class WhileNode : StatementNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="condition">Условие.</param>
    /// <param name="body">Тело цикла.</param>
    public WhileNode
        (
            AtomNode condition,
            IEnumerable<StatementNode> body
        )
    {
        _condition = condition;
        _body = new List<StatementNode> (body);
    }

    #endregion

    #region Private members

    private readonly AtomNode _condition;
    private readonly List<StatementNode> _body;

    #endregion

    #region AstNode members

    /// <inheritdoc cref="StatementNode.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        while (BarsikUtility.ToBoolean (_condition.Compute (context)))
        {
            foreach (var statement in _body)
            {
                statement.Execute (context);
            }
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.AppendLine ($"While: {_condition}");
        builder.Append ("Statements: ");
        foreach (var statement in _body)
        {
            builder.AppendLine (statement.ToString());
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
