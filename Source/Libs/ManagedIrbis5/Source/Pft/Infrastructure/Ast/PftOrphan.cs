// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PftOrphan.cs -- сирота, ложная ссылка на поле/подполе.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Pft.Infrastructure.Compiler;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Сирота - ложная ссылка на поле/подполе.
/// </summary>
public sealed class PftOrphan
    : PftField
{
    #region Properties

    /// <inheritdoc cref="PftNode.ConstantExpression" />
    public override bool ConstantExpression => true;

    /// <inheritdoc cref="PftNode.RequiresConnection" />
    public override bool RequiresConnection => false;

    #endregion

    #region Private members

    private bool _traced;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftField.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        compiler.StartMethod (this);

        // Nothing to do here

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    }

    /// <inheritdoc cref="PftField.GetAffectedFields" />
    public override int[] GetAffectedFields() => Array.Empty<int>();

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        // Nothing to do here

        if (!_traced)
        {
            Magna.Logger.LogTrace
                (
                    nameof (PftOrphan) + "::" + nameof (Execute)
                    + ": at {Token}",
                    this
                );
            _traced = true;
        }

        OnAfterExecution (context);
    } // method Execute

    /// <inheritdoc cref="PftNode.Optimize" />
    public override PftNode? Optimize() => null;

    #endregion
}
