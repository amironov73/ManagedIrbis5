// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReturnNode.cs -- возврат значения из функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Возврат значения из функции.
/// </summary>
internal sealed class ReturnNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReturnNode
        (
            int line,
            AtomNode? value
        )
        : base (line)
    {
        _value = value;
    }

    #endregion

    #region Private members

    private readonly AtomNode? _value;

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var value = _value?.Compute (context) ?? null;

        throw new ReturnException (value);
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

        if (_value is not null)
        {
            _value.DumpHierarchyItem ("Value", level + 1, writer);
        }
    }

    #endregion
}
