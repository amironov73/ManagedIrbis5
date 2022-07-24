// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PftToken.cs -- токен PFT-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

/// <summary>
/// Токен PFT-скрипта.
/// </summary>
[DebuggerDisplay ("{Kind} {Text} {Line} {Column}")]
public sealed class PftToken
    : IHandmadeSerializable,
        ICloneable
{
    #region Properties

    /// <summary>
    /// Column number.
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Token kind.
    /// </summary>
    public PftTokenKind Kind { get; set; }

    /// <summary>
    /// Line number.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Token text.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Arbitrary user data.
    /// </summary>
    public object? UserData { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftToken()
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftToken
        (
            PftTokenKind kind,
            int line,
            int column,
            string? text
        )
    {
        Kind = kind;
        Line = line;
        Column = column;
        Text = text;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Требует совпадения с указанным типом токенов.
    /// </summary>
    public PftToken MustBe
        (
            PftTokenKind kind
        )
    {
        Sure.Defined (kind);

        if (Kind != kind)
        {
            Magna.Logger.LogError
                (
                    nameof (PftToken) + "::" + nameof (MustBe)
                    + "expecting {Expected}, got {Actual}",
                    kind,
                    Kind
                );

            throw new PftSyntaxException();
        }

        return this;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Column = reader.ReadPackedInt32();
        Kind = (PftTokenKind)reader.ReadPackedInt32();
        Line = reader.ReadPackedInt32();
        Text = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WritePackedInt32 (Column)
            .WritePackedInt32 ((int)Kind)
            .WritePackedInt32 (Line)
            .WriteNullable (Text);
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public object Clone()
    {
        return MemberwiseClone();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Kind
               + " ("
               + Line
               + ","
               + Column
               + ")"
               + ": "
               + Text.ToVisibleString();
    }

    #endregion
}
