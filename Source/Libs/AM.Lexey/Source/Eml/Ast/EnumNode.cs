// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EnumNode.cs -- узел со значением из перечисления
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Lexey.Ast;
using AM.Lexey.Barsik;
using AM.Lexey.Barsik.Ast;

#endregion

namespace AM.Lexey.Eml.Ast;

/// <summary>
/// Узел, хранящий значение из перечисления
/// </summary>
public sealed class EnumNode
    : AtomNode
{
    #region Properties

    /// <summary>
    /// Текстовое представление.
    /// </summary>
    public string Text{ get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EnumNode
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        Text = text;
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
        base.DumpHierarchyItem (name, level, writer, Text);
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="AstNode.ToString"/>
    public override string ToString() => $"EnumNode '{Text.ToVisibleString()}'";

    #endregion
}
