// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EmlProgramNode.cs -- узел, описывающий программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Eml.Ast;

/// <summary>
/// Узел, описывающий программу в целом.
/// </summary>
[PublicAPI]
public sealed class EmlProgramNode
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

    #endregion
}
