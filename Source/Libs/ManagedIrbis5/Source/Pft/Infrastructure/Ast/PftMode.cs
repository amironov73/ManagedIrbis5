// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftMode.cs -- переключение режима вывода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM;
using AM.IO;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Переключение режима вывода полей/подполей.
/// </summary>
public sealed class PftMode
    : PftNode
{
    #region Properties

    /// <summary>
    /// Output mode.
    /// </summary>
    public PftFieldOutputMode OutputMode { get; set; }

    /// <summary>
    /// Upper-case mode.
    /// </summary>
    public bool UpperMode { get; set; }

    /// <inheritdoc cref="PftNode.ConstantExpression" />
    public override bool ConstantExpression => true;

    /// <inheritdoc cref="PftNode.RequiresConnection" />
    public override bool RequiresConnection => false;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftMode()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftMode
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        try
        {
            ParseText (text);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (PftMode) + "::Constructor"
                );

            throw new PftSyntaxException
                (
                    "bad mode text",
                    exception
                );
        }
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftMode
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Mpl);

        try
        {
            ParseText (token.Text.ThrowIfNull());
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (PftMode) + "::Constructor"
                );

            throw new PftSyntaxException (token, exception);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Parse specified text.
    /// </summary>
    public void ParseText
        (
            string text
        )
    {
        Sure.NotNull (text);

        text = text.ToLower();
        if (text.Length != 3
            || text[0] != 'm')
        {
            Magna.Logger.LogError
                (
                    nameof (PftMode) + "::" + nameof (ParseText)
                    + ": text.Length != 3 at {Token}",
                    text.ToVisibleString()
                );

            throw new ArgumentException ("mode");
        }

        switch (text[1])
        {
            case 'p':
                OutputMode = PftFieldOutputMode.PreviewMode;
                break;

            case 'h':
                OutputMode = PftFieldOutputMode.HeaderMode;
                break;

            case 'd':
                OutputMode = PftFieldOutputMode.DataMode;
                break;

            default:
                Magna.Logger.LogError
                    (
                        nameof (PftMode) + "::" + nameof (ParseText)
                        + ": unexpected mode={Mode}",
                        text.ToVisibleString()
                    );

                throw new ArgumentException();
        }

        switch (text[2])
        {
            case 'u':
                UpperMode = true;
                break;

            case 'l':
                UpperMode = false;
                break;

            default:
                Magna.Logger.LogError
                    (
                        nameof (PftMode) + "::" + nameof (ParseText)
                        + ": unexpected mode={Mode}",
                        text.ToVisibleString()
                    );

                throw new ArgumentException();
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
                    "Context.FieldOutputMode = PftFieldOutputMode.{0};",
                    OutputMode
                )
            .WriteIndent()
            .WriteLine
                (
                    "Context.UpperMode = {0};",
                    CompilerUtility.BooleanToText (UpperMode)
                );

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    }

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        base.Deserialize (reader);

        OutputMode = (PftFieldOutputMode)reader.ReadPackedInt32();
        UpperMode = reader.ReadBoolean();
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        context.FieldOutputMode = OutputMode;
        context.UpperMode = UpperMode;

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer.Write (ToString());
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        writer.WritePackedInt32 ((int) OutputMode);
        writer.Write (UpperMode);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    [DebuggerStepThrough]
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ('m');

        char c;
        switch (OutputMode)
        {
            case PftFieldOutputMode.PreviewMode:
                c = 'p';
                break;

            case PftFieldOutputMode.HeaderMode:
                c = 'h';
                break;

            case PftFieldOutputMode.DataMode:
                c = 'd';
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        builder.Append (c);

        builder.Append
            (
                UpperMode ? 'u' : 'l'
            );

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
