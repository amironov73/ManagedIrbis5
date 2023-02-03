// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LabelNode.cs -- псевдо-узел: метка для оператора goto
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Псевдо-узел: метка для оператора goto.
/// </summary>
internal sealed class LabelNode
    : PseudoNode
{
    #region Properties

    /// <summary>
    /// Имя метки.
    /// </summary>
    public string Name { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LabelNode
        (
            int line, 
            string name
        ) 
        : base (line)
    {
        Sure.NotNullNorEmpty (name);
        
        Name = name;
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
        
        DumpHierarchyItem ("Name", level + 1, writer, Name);
    }

    #endregion
}
