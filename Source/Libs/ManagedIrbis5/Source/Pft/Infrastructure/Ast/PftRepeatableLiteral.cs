// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftRepeatableLiteral.cs -- повторяющийся литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Определяет текст, который будет выведен только
/// в случае присутствия в записи соответствующего
/// ему поля или подполя. Однако, если поле
/// повторяющееся, литерал будет выводиться
/// для каждого экземпляра поля/подполя.
/// Повторяющиеся литералы заключаются
/// в вертикальные черты (|), например, |Автор: |.
/// </summary>
public sealed class PftRepeatableLiteral
    : PftNode
{
    #region Properties

    /// <summary>
    /// Prefix or postfix?
    /// </summary>
    public bool IsPrefix { get; set; }

    /// <summary>
    /// Plus?
    /// </summary>
    public bool Plus { get; set; }

    /// <inheritdoc cref="PftNode.Text" />
    public override string? Text
    {
        get => base.Text;
        set => base.Text = PftUtility.PrepareText (value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftRepeatableLiteral()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftRepeatableLiteral
        (
            string text
        )
    {
        Sure.NotNull (text);

        Text = text;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftRepeatableLiteral
        (
            string text,
            bool isPrefix,
            bool plus
        )
    {
        Sure.NotNull (text);

        Text = text;
        IsPrefix = isPrefix;
        Plus = plus;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftRepeatableLiteral
        (
            string text,
            bool isPrefix
        )
    {
        Sure.NotNull (text);

        Text = text;
        IsPrefix = isPrefix;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftRepeatableLiteral
        (
            PftToken token,
            bool isPrefix
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.RepeatableLiteral);

        IsPrefix = isPrefix;

        try
        {
            Text = token.Text.ThrowIfNull();
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (PftRepeatableLiteral) + "::Constructor"
                );

            throw new PftSyntaxException (token, exception);
        }
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.CompareNode" />
    internal override void CompareNode
        (
            PftNode otherNode
        )
    {
        Sure.NotNull (otherNode);

        base.CompareNode (otherNode);

        var otherLiteral
            = (PftRepeatableLiteral)otherNode;
        if (IsPrefix != otherLiteral.IsPrefix
            || Plus != otherLiteral.Plus)
        {
            throw new PftSerializationException();
        }
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        compiler.StartMethod (this);

        if (!string.IsNullOrEmpty (Text))
        {
            var field = Parent as PftField
                        ?? throw new PftCompilerException();

            var info = compiler.Fields.Get (field)
                       ?? throw new PftCompilerException();

            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "DoRepeatableLiteral(\"{0}\", {1}, {2}, {3});",
                        CompilerUtility.Escape (Text),
                        info.Reference,
                        CompilerUtility.BooleanToText (IsPrefix),
                        CompilerUtility.BooleanToText (Plus)
                    );
        }

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

        IsPrefix = reader.ReadBoolean();
        Plus = reader.ReadBoolean();
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        var field = context.CurrentField;
        if (field is not null)
        {
            var flag = field.HaveRepeat (context);

            if (flag)
            {
                var value = field.GetValue (context);

                flag = field.CanOutput (value);
            }

            if (flag && Plus)
            {
                flag = IsPrefix
                    ? !field.IsFirstRepeat (context)
                    : !field.IsLastRepeat (context);
            }

            if (flag)
            {
                var text = Text;
                if (context.UpperMode
                    && !ReferenceEquals (text, null))
                {
                    text = IrbisText.ToUpper (text);
                }

                context.WriteAndSetFlag (this, text);
            }
        }

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.Optimize" />
    public override PftNode? Optimize()
    {
        if (string.IsNullOrEmpty (Text))
        {
            // Take the node away from the AST

            return null;
        }

        return this;
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        writer.Write (IsPrefix);
        writer.Write (Plus);
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        if (!IsPrefix && Plus)
        {
            printer.Write ('+');
        }

        printer.Write ('|');
        printer.Write (Text);
        printer.Write ('|');
        if (IsPrefix && Plus)
        {
            printer.Write ('+');
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        if (!IsPrefix && Plus)
        {
            builder.Append ('+');
        }

        builder.Append ('|');
        builder.Append (Text);
        builder.Append ('|');
        if (IsPrefix && Plus)
        {
            builder.Append ('+');
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
