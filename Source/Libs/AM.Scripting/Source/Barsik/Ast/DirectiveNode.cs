// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DirectiveNode.cs -- директива
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Директива
/// </summary>
internal sealed class DirectiveNode
    : StatementNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DirectiveNode
        (
            string code,
            string? text
        )
    {
        Sure.NotNullNorEmpty (code);

        _code = code;
        _text = text;
    }

    #endregion

    #region Private members

    private readonly string _code;
    private readonly string? _text;

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementNode.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        context.Output.WriteLine ($"{_code}: '{_text}'");
    }

    #endregion
}
