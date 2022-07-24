// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftTrue.cs -- константа ИСТИНА
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

using AM;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Константа ИСТИНА.
/// </summary>
public sealed class PftTrue
    : PftCondition
{
    #region Properties

    /// <inheritdoc cref="PftNode.ConstantExpression" />
    public override bool ConstantExpression => true;

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    /// <inheritdoc cref="PftNode.RequiresConnection" />
    public override bool RequiresConnection => false;

    /// <inheritdoc cref="PftBoolean.Value" />
    public override bool Value
    {
        get => true;

        // ReSharper disable once ValueParameterNotUsed
        set
        {
            // Nothing to do here

            Magna.Logger.LogError (nameof (PftTrue) + "::" + nameof (Value) + "::set");
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftTrue()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftTrue
        (
            PftToken token
        )
        : base (token)
    {
        // пустое тело конструктора
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        compiler.StartMethod (this);

        compiler
            .WriteIndent()
            .WriteLine ("return true;");

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        // Nothing to do here

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.PrettyPrint"/>
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer.EatWhitespace();
        printer
            .SingleSpace()
            .Write ("true")
            .SingleSpace();
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    [DebuggerStepThrough]
    protected internal override bool ShouldSerializeText()
    {
        return false;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="PftNode.ToString"/>
    public override string ToString()
    {
        return "true";
    }

    #endregion
}
