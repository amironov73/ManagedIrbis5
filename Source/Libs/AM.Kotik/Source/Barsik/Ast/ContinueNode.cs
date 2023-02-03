// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ContinueNode.cs -- досрочное завершение текущей итерации цикла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Досрочное завершение текущей итерации цикла.
/// </summary>
internal sealed class ContinueNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ContinueNode
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

        throw new ContinueException();
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.GetNodeInfo"/>
    public override AstNodeInfo GetNodeInfo() => new (this)
    {
        Name = "continue"
    };

    #endregion
}
