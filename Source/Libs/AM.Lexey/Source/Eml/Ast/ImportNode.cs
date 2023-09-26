// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ImportNode.cs -- узел, импортирующий пространство имен
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Lexey.Ast;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Eml.Ast;

/// <summary>
/// Узел, импортирующий пространство имен.
/// </summary>
[PublicAPI]
public sealed class ImportNode
    : AstNode
{
    #region Properties

    /// <summary>
    /// Пространство имен.
    /// </summary>
    public string Namespace { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ImportNode
        (
            string ns
        )
    {
        Sure.NotNullNorEmpty (ns);

        Namespace = ns;
    }

    #endregion
}
