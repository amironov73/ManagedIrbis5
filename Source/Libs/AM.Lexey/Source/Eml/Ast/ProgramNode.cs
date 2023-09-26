// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ProgramNode.cs -- узел, описывающий программу
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
public sealed class ProgramNode
{
    #region Properties

    /// <summary>
    /// Импортируемые пространства имен.
    /// </summary>
    public List<ImportNode> Imports { get; } = new ();

    /// <summary>
    /// Корневой контрол, например, окно.
    /// </summary>
    public ControlNode? RootControl { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Запуск интерпретации программы.
    /// </summary>
    public object? Execute
        (
            Context context
        )
    {
        Sure.NotNull (context);

        return null;
    }

    #endregion
}
