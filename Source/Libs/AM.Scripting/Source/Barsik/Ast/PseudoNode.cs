// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PseudoNode.cs -- псевдо-узел, предназначенный для функций
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Псевдо-узел, предназначенный для функций.
/// </summary>
internal /* not sealed */ class PseudoNode
    : StatementNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PseudoNode()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PseudoNode
        (
            SourcePosition startPosition
        )
        : base (startPosition)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PseudoNode
        (
            SourcePos position
        )
        : base (position)
    {
        // пустое тело конструктора
    }

    #endregion
}
