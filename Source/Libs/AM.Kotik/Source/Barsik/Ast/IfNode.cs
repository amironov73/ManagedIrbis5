// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* IfNode.cs -- условный оператор if-then-else
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Условный оператор if-then-else.
/// </summary>
/// <remarks>
/// Структура условного оператора:
/// <code>
/// if (condition)
/// {
///  // блок then
/// }
/// else if (condition)
/// {
///  // произвольное количество (включая 0) блоков else if
/// }
/// else
/// {
///   опциональный блок else
/// }
/// </code>
/// </remarks>
internal sealed class IfNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IfNode
        (
            int line,
            AtomNode condition,
            StatementBase thenBlock,
            IfNode[]? elseIfArray, 
            StatementBase? elseBlock
        )
        : base (line)
    {
        Sure.NotNull (condition);
        Sure.NotNull (thenBlock);
        
        _condition = condition;
        _elseIfArray = elseIfArray;
        _thenBlock = thenBlock;
        _elseBlock = elseBlock;
    }

    #endregion

    #region Private members

    private readonly AtomNode _condition;
    private readonly StatementBase _thenBlock;
    private readonly IfNode[]? _elseIfArray;
    private readonly StatementBase? _elseBlock;

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        if (KotikUtility.ToBoolean (_condition.Compute (context)))
        {
            _thenBlock.Execute (context);
        }
        else
        {
            var handled = false;

            if (_elseIfArray is not null)
            {
                foreach (var block in _elseIfArray)
                {
                    if (KotikUtility.ToBoolean (block._condition.Compute (context)))
                    {
                        block._thenBlock.Execute (context);
                        handled = true;
                    }
                }
            }

            if (!handled)
            {
                _elseBlock?.Execute (context);
            }
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
        _thenBlock.DumpHierarchyItem ("Then", level + 1, writer);
        if (_elseIfArray is not null)
        {
            foreach (var elseIf in _elseIfArray)
            {
                elseIf.DumpHierarchyItem ("ElseIf", level + 1, writer);
            }
        }

        _elseBlock?.DumpHierarchyItem ("Else", level + 1, writer);
    }

    #endregion
}
