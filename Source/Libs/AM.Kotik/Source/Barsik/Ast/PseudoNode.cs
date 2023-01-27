// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PseudoNode.cs -- псевдо-узел, предназначенный, например, для функций
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Псевдо-узел AST, предназначенный, например, для функций.
/// </summary>
public abstract class PseudoNode
    : StatementBase
{
    #region Construction


    /// <summary>
    /// Конструктор.
    /// </summary>
    protected PseudoNode
        (
            int line
        )
        : base (line)
    {
        // пустое тело конструктора
    }
    
    #endregion
}
