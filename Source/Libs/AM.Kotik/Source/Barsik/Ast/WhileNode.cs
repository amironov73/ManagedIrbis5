// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* WhileNode.cs -- цикл while
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Цикл while.
/// </summary>
internal sealed class WhileNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="line">Номер строки в исходном тексте.</param>
    /// <param name="condition">Условие.</param>
    /// <param name="body">Тело цикла.</param>
    /// <param name="elseBody">Выполняется, если тело цикла не сработало ни разу.</param>
    public WhileNode
        (
            int line,
            AtomNode condition,
            StatementBase body,
            StatementBase? elseBody
        )
        : base (line)
    {
        _condition = condition;
        _body = body;
        _elseBody = elseBody;
    }

    #endregion

    #region Private members

    private readonly AtomNode _condition;
    private readonly StatementBase _body;
    private readonly StatementBase? _elseBody;

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        try
        {
            var success = false;
            while (KotikUtility.ToBoolean (_condition.Compute (context)))
            {
                success = true;
                try
                {
                    _body.Execute (context);
                }
                catch (ContinueException)
                {
                    Debug.WriteLine ("while-continue");
                }
            }

            if (!success && _elseBody is not null)
            {
                _elseBody.Execute (context);
            }
        }
        catch (BreakException)
        {
            Debug.WriteLine ("while-break");
        }
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter,string?)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer, ToString());

        _condition.DumpHierarchyItem ("Condition", level + 1, writer);
        _body.DumpHierarchyItem ("Block", level + 1, writer);
    }

    #endregion
}
