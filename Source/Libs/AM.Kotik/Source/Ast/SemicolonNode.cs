// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SemicolonNode.cs -- псевдо-узел: точка с запятой
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Псевдо-узел AST: точка с запятой.
/// Она не выполняет никаких действий
/// и введена исключительно для совместимости.
/// </summary>
public sealed class SemicolonNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SemicolonNode
        (
            int line
        )
        : base(line)
    {
        // пустое тело конструктора
    }

    #endregion
}
