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
