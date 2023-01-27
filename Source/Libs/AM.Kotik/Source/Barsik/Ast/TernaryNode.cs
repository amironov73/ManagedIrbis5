// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TernaryNode.cs -- тернарный оператор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Тернарный оператор `? условие : истина : ложь`.
/// </summary>
internal sealed class TernaryNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TernaryNode
        (
            AtomNode condition,
            AtomNode trueValue,
            AtomNode falseValue
        )
    {
        Sure.NotNull (condition);
        Sure.NotNull (trueValue);
        Sure.NotNull (falseValue);

        _condition = condition;
        _trueValue = trueValue;
        _falseValue = falseValue;
    }

    #endregion

    #region Private members

    private readonly AtomNode _condition;
    private readonly AtomNode _trueValue;
    private readonly AtomNode _falseValue;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var value = _condition.Compute (context);
        var result = KotikUtility.ToBoolean (value)
            ? _trueValue.Compute (context)
            : _falseValue.Compute (context);

        return result;
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
        
        _condition.DumpHierarchyItem ("Condition", level + 1, writer);
        _trueValue.DumpHierarchyItem ("True", level + 1, writer);
        _falseValue.DumpHierarchyItem ("False", level + 1, writer);
    }

    #endregion
}
