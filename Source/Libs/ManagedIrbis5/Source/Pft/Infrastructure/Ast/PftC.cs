// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftC.cs -- табуляция в указанную позицию строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Команда горизонтального позиционирования.
/// Перемещает виртуальный курсор в n-ю позицию строки
/// (табуляция в указанную позицию строки).
/// </summary>
public sealed class PftC
    : PftNode
{
    #region Properties

    /// <inheritdoc cref="PftNode.ConstantExpression" />
    public override bool ConstantExpression => true;

    /// <inheritdoc cref="PftNode.RequiresConnection" />
    public override bool RequiresConnection => false;

    /// <summary>
    /// Новая позиция курсора.
    /// </summary>
    public int NewPosition { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftC()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftC
        (
            int position
        )
    {
        NewPosition = position;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftC
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.C);

        try
        {
            NewPosition = int.Parse
                (
                    token.Text.ThrowIfNull()
                );
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (PftC) + "::Constructor"
                );

            throw new PftSyntaxException (token, exception);
        }
    }

    #endregion

    #region Private members

    private void _Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        var current = context.Output.GetCaretPosition();
        var delta = NewPosition - current;
        if (delta > 0)
        {
            context.Write
                (
                    this,
                    new string (' ', delta)
                );
        }
        else
        {
            context.WriteLine (this);
            context.Write
                (
                    this,
                    new string (' ', NewPosition - 1)
                );
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

        var otherC = (PftC)otherNode;
        var result = NewPosition == otherC.NewPosition;

        if (!result)
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

        compiler
            .WriteIndent()
            .WriteLine ("if (CurrentField != null)")
            .WriteIndent()
            .WriteLine ("{")
            .WriteIndent()
            .IncreaseIndent()
            .WriteIndent()
            .WriteLine ("if (FirstRepeat(CurrentField))")
            .WriteIndent()
            .WriteLine ("{")
            .IncreaseIndent()
            .WriteIndent()
            .WriteLine ("Goto({0});", NewPosition)
            .DecreaseIndent()
            .WriteIndent()
            .WriteLine ("}")
            .DecreaseIndent()
            .WriteIndent()
            .WriteLine ("}")
            .WriteIndent()
            .WriteLine ("else")
            .WriteIndent()
            .WriteLine ("{")
            .IncreaseIndent()
            .WriteIndent()
            .WriteLine ("Goto({0});", NewPosition)
            .DecreaseIndent()
            .WriteIndent()
            .WriteLine ("}");

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

        NewPosition = reader.ReadPackedInt32();
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (context.CurrentField is not null)
        {
            if (context.CurrentField.IsFirstRepeat (context))
            {
                _Execute (context);
            }
        }
        else
        {
            _Execute (context);
        }

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        // Всегда в нижнем регистре
        printer
            .SingleSpace()
            .Write
                (
                    "c{0}",
                    NewPosition.ToInvariantString()
                )
            .SingleSpace();
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        writer.WritePackedInt32 (NewPosition);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="PftNode.ToString" />
    public override string ToString()
    {
        return "c" + NewPosition.ToInvariantString();
    }

    #endregion
}
