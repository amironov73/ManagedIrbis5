// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PftGraveAccent.cs -- обратная кавычка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

//
// ibatrak
// Оказывается, в isis есть 2 явных варианта безусловного литерала
// 1) с символами одинарной кавычки 'test me'
// 2) с символами обратной кавычки, не знаю как правильно назвать,
// в php этот символ используется для вызова шелл команд.
// `test me`
// Эта же фишка перекочевала в ирбис, но ... написать о ней
// в документации забыли :)
//

/// <summary>
/// Обратная кавычка.
/// </summary>
public sealed class PftGraveAccent
    : PftNode
{
    #region Properties

    /// <inheritdoc cref="PftNode.ConstantExpression" />
    public override bool ConstantExpression => true;

    /// <inheritdoc cref="PftNode.Text" />
    public override string? Text
    {
        get => base.Text;
        set => base.Text = PftUtility.PrepareText (value);
    }

    /// <inheritdoc cref="PftNode.RequiresConnection" />
    public override bool RequiresConnection => false;

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftGraveAccent()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftGraveAccent
        (
            string text
        )
    {
        Text = text;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftGraveAccent
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.GraveAccent);

        try
        {
            Text = token.Text.ThrowIfNull();
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (PftGraveAccent) + "::Constructor"
                );

            throw new PftSyntaxException (token, exception);
        }
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
            .WriteLine
                (
                    "Context.Write(null,\"{0}\");",
                    Text
                );
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

        // TODO just for a while
        var text = Text;

        // Never throw on empty literal

        if (context.UpperMode
            && !ReferenceEquals (text, null))
        {
            text = IrbisText.ToUpper (text);
        }

        context.Write
            (
                this,
                text
            );

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer.Write ('`')
            .Write (Text)
            .Write ('`');
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => '`' + Text + '`';

    #endregion
}
