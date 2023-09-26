// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PropertyNode.cs -- узел, описывающий свойство
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Lexey.Ast;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Eml.Ast;

/// <summary>
/// Узел, описывающий свойство.
/// </summary>
[PublicAPI]
public class PropertyNode
    : AstNode
{
    #region Properties

    /// <summary>
    /// Имя свойства.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Значение свойства.
    /// </summary>
    public object? Value { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PropertyNode
        (
            string name,
            object? value
        )
    {
        Sure.NotNullNorEmpty (name);

        Name = name;
        Value = value;
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
        base.DumpHierarchyItem (name, level, writer, $"{Name} = {Value.ToVisibleString()}");
    }

    #endregion
}
