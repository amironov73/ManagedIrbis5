// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* WithNode.cs -- блок With
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Блок with.
/// </summary>
internal sealed class WithNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WithNode
        (
            int line,
            AtomNode center,
            StatementBase body
        )
        : base (line)
    {
        Sure.NotNull (center);
        Sure.NotNull (body);

        _center = center;
        _body = body;
    }

    #endregion

    #region Private members

    private readonly AtomNode _center;

    private readonly StatementBase _body;

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var previousCenter = context.With;
        context.With = _center;

        try
        {
            _body.Execute (context);
        }
        finally
        {
            context.With = previousCenter;
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

        _center.DumpHierarchyItem ("Center", level + 1, writer);
        _body.DumpHierarchyItem ("Body", level + 1, writer);
    }

    #endregion
}
