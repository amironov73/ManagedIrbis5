// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BreakNode.cs -- досрочный выход из цикла
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using AM.Kotik.Barsik.Diagnostics;

namespace AM.Kotik.Barsik;

/// <summary>
/// Досрочный выход из цикла.
/// </summary>
internal sealed class BreakNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BreakNode
        (
            int line
        )
        : base(line)
    {
        // пустое тело конструктора
    }

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        throw new BreakException();
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.GetNodeInfo"/>
    public override AstNodeInfo GetNodeInfo() => new (this)
    {
        Name = "break"
    };

    #endregion
}
