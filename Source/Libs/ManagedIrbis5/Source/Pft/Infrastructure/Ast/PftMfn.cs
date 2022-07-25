// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftMfn.cs -- вывод номера записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Для вывода номера записи в файле документов служит
/// команда MFN, формат которой:
///
/// MFN или MFN(d),
///
/// где d - количество выводимых на экран цифр.
/// Если параметр(d) опущен, то по умолчанию
/// предполагается 6 цифр.
/// </summary>
public sealed class PftMfn
    : PftNumeric
{
    #region Constants

    /// <summary>
    /// Default width.
    /// </summary>
    public const int DefaultWidth = 10;

    #endregion

    #region Properties

    /// <summary>
    /// Width of the output.
    /// </summary>
    public int Width { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftMfn()
    {
        Width = DefaultWidth;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftMfn
        (
            int width
        )
    {
        Sure.Positive (width);

        Width = width;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftMfn
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Mfn);

        Width = DefaultWidth;

        try
        {
            var text = token.Text;
            if (!string.IsNullOrEmpty (text))
            {
                if (text.Length > 3)
                {
                    text = text.Substring (3);
                    text = text.TrimStart ('(').TrimEnd (')');

                    Width = int.Parse (text);
                    if (Width <= 0)
                    {
                        Magna.Logger.LogError
                            (
                                nameof (PftMfn) + "::Constructor"
                                + ": width={Width}",
                                Width
                            );

                        throw new PftSyntaxException (token);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (PftMfn) + "::Constructor"
                );

            throw new PftSyntaxException (token, exception);
        }
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        base.Deserialize (reader);

        Width = reader.ReadPackedInt32();
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        Value = 0.0;

        if (context.Record is { } record)
        {
            Value = record.Mfn;

            var text = Width == 0
                ? record.Mfn.ToInvariantString()
                : record.Mfn.ToString
                    (
                        new string ('0', Width),
                        CultureInfo.InvariantCulture
                    );

            context.Write
                (
                    this,
                    text
                );
        }

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        writer.WritePackedInt32 (Width);
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer.Write ("mfn");
        if (Width > 0
            && Width != DefaultWidth)
        {
            printer.Write ('(')
                .Write (Width)
                .Write (')');
        }
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="PftNode.ToString"/>
    public override string ToString()
    {
        return Width.ToInvariantString ("mfn#");
    }

    #endregion
}
