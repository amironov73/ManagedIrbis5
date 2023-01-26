// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SwitchNode.cs -- switch стейтмент
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Switch-стейтмент.
/// </summary>
internal sealed class SwitchNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SwitchNode
        (
            int line,
            AtomNode value,
            IList<CaseNode> cases,
            StatementBase? defaultCase
        )
        : base (line)
    {
        Sure.NotNull (value);
        Sure.NotNull (value);
        Sure.NotNull (cases);

        _value = value;
        _cases = cases;
        _defaultCase = defaultCase;
    }

    #endregion

    #region Private members

    private readonly AtomNode _value;
    private readonly IList<CaseNode> _cases;
    private readonly StatementBase? _defaultCase;

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        base.Execute (context);

        var value = _value.Compute (context);
        var success = false;
        foreach (var caseNode in _cases)
        {
            if (caseNode.CheckAndExecute (context, value))
            {
                success = true;
                break;
            }
        }

        if (!success && _defaultCase is not null)
        {
            _defaultCase.Execute (context);
        }
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer);

        _value.DumpHierarchyItem ("Value", level + 1, writer);
        foreach (var caseNode in _cases)
        {
            caseNode.DumpHierarchyItem ("Case", level + 1, writer);
        }
    }

    #endregion
}
