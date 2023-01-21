// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* MinusNode.cs -- унарный минус (смена знака числа)
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System.IO;

namespace AM.Kotik;

/// <summary>
/// Унарный минус (смена знака числа).
/// </summary>
public sealed class MinusNode
    : UnaryNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MinusNode
        (
            AtomNode inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly AtomNode _inner;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        Sure.NotNull (context);

        var value = _inner.Compute (context);

        return -value;
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

        _inner.DumpHierarchyItem ("Inner", level + 1, writer);
    }

    #endregion
}
