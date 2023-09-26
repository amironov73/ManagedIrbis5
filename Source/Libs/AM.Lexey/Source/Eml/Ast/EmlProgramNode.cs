// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EmlProgramNode.cs -- узел, описывающий программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Lexey.Ast;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Eml.Ast;

/// <summary>
/// Узел, описывающий программу в целом.
/// </summary>
[PublicAPI]
public sealed class EmlProgramNode
    : AstNode
{
    #region Properties

    /// <summary>
    /// Импортируемые пространства имен.
    /// </summary>
    public List<ImportNode> Imports { get; }

    /// <summary>
    /// Корневой контрол, например, окно.
    /// </summary>
    public ControlNode RootControl { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EmlProgramNode
        (
            IEnumerable<ImportNode> imports,
            ControlNode rootControl
        )
    {
        Sure.NotNull (imports);
        Sure.NotNull (rootControl);

        Imports = new (imports);
        RootControl = rootControl;
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

        foreach (var importNode in Imports)
        {
            importNode.DumpHierarchyItem ("Import", level + 1, writer);
        }

        RootControl.DumpHierarchyItem ("Root", level + 1, writer);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Запуск интерпретации программы - построение заказанного контрола.
    /// </summary>
    public object? CreateControl
        (
            EmlContext context
        )
    {
        Sure.NotNull (context);

        foreach (var node in Imports)
        {
            if (!context.Namespaces.Contains (node.Namespace))
            {
                context.Namespaces.Add (node.Namespace);
            }
        }

        return RootControl.CreateControl (context);
    }

    /// <summary>
    /// Дамп программы
    /// </summary>
    public void Dump()
    {
        DumpHierarchyItem ("Program", 0, Console.Out);
    }

    #endregion
}
