// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ControlNode.cs -- узел, описывающий контрол
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM.Lexey.Ast;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Eml.Ast;

/// <summary>
/// Узел, описывающий контрол.
/// </summary>
[PublicAPI]
public class ControlNode
    : AstNode
{
    #region Properties

    /// <summary>
    /// Имя типа.
    /// </summary>
    public string TypeName { get; }

    /// <summary>
    /// Свойства.
    /// </summary>
    public List<PropertyNode> Properties { get; }

    /// <summary>
    /// Вложенные контролы.
    /// </summary>
    public List<ControlNode> Children { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Имя типа.
    /// </summary>
    public ControlNode
        (
            string typeName,
            IList<PropertyNode> properties,
            IList<ControlNode> children
        )
    {
        Sure.NotNullNorEmpty (typeName);
        Sure.NotNull (properties);
        Sure.NotNull (children);

        TypeName = typeName;
        Properties = new (properties);
        Children = new (children);
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
        base.DumpHierarchyItem (name, level, writer, TypeName);

        foreach (var property in Properties)
        {
            property.DumpHierarchyItem ("Property", level + 1, writer);
        }

        foreach (var child in Children)
        {
            child.DumpHierarchyItem ("Child", level + 1, writer);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение контрола.
    /// </summary>
    public object? CreateControl
        (
            EmlContext context
        )
    {
        Sure.NotNull (context);

        return null;
    }

    #endregion
}
