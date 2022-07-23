// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* RepeatSpecification.cs -- спецификация повторения поля в записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text.Json.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure;

//
// Повторение поля задается одним из способов:
//
// * - все повторения
// F - если используется корректировка по формату
// N (число) – если корректируется N-ое повторение поля
// L – если корректируется последнее повторение поля
// L-N (число) – если корректируется N-ое с конца повторение поля
//
// Нумерация повторений начинается с 1.
//

/// <summary>
/// Спецификация повторения поля в записи.
/// </summary>
public struct RepeatSpecification
    : IHandmadeSerializable,
        IVerifiable
{
    #region Properties

    /// <summary>
    /// Вид повторения.
    /// </summary>
    [JsonPropertyName ("kind")]
    public RepeatKind Kind { get; set; }

    /// <summary>
    /// Number of the repeat.
    /// </summary>
    [JsonPropertyName ("index")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Index { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RepeatSpecification
        (
            RepeatKind kind
        )
        : this()
    {
        Sure.Defined (kind);

        Kind = kind;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RepeatSpecification
        (
            int index
        )
        : this()
    {
        Sure.Positive (index);

        Kind = RepeatKind.Explicit;
        Index = index;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RepeatSpecification
        (
            RepeatKind kind,
            int index
        )
        : this()
    {
        Sure.Defined (kind);
        Sure.NonNegative (index);

        Kind = kind;
        Index = index;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текстового представления.
    /// </summary>
    public static RepeatSpecification Parse
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        var result = new RepeatSpecification();
        switch (text)
        {
            case "*":
                result.Kind = RepeatKind.All;
                break;

            case "F":
            case "f":
                result.Kind = RepeatKind.ByFormat;
                break;

            case "L":
            case "l":
                result.Kind = RepeatKind.Last;
                break;

            default:
                if (uint.TryParse (text, out var index))
                {
                    if (index == 0)
                    {
                        throw new IrbisException
                            (
                                "Invalid repeat specification: "
                                + text
                            );
                    }

                    result.Kind = RepeatKind.Explicit;
                    result.Index = (int)index;
                }
                else if (text[0] == 'L'
                         && uint.TryParse
                             (
                                 text.Substring (2),
                                 out index
                             ))
                {
                    result.Kind = RepeatKind.Last;
                    result.Index = (int)index;
                }
                else
                {
                    Magna.Logger.LogError
                        (
                            nameof (RepeatSpecification) + "::" + nameof (Parse)
                            + ": invalid repeat specification={Specification}",
                            text
                        );


                    throw new IrbisException
                        (
                            "Invalid repeat specification: "
                            + text
                        );
                }

                break;
        }

        return result;
    }

    /// <summary>
    /// Нужно ли сериализовать поле <see cref="Index"/>?
    /// </summary>
    public bool ShouldSerializeIndex() => Index != 0;

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Kind = (RepeatKind)reader.ReadPackedInt32();
        Index = reader.ReadPackedInt32();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WritePackedInt32 ((int)Kind)
            .WritePackedInt32 (Index);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<RepeatSpecification> (this, throwOnError);

        switch (Kind)
        {
            case RepeatKind.All:
                verifier.Assert (Index == 0);
                break;

            case RepeatKind.ByFormat:
                verifier.Assert (Index == 0);
                break;

            case RepeatKind.Last:
                verifier.Assert (Index >= 0);
                break;

            case RepeatKind.Explicit:
                verifier.Assert (Index > 0);
                break;

            default:
                verifier.Result = false;
                break;
        }

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.ToString" />
    public override string ToString() => Kind switch
    {
        RepeatKind.All => "*",

        RepeatKind.ByFormat => "F",

        RepeatKind.Last => Index == 0 ? "L" : "L-" + Index.ToInvariantString(),

        RepeatKind.Explicit => Index.ToInvariantString(),

        _ => $"Kind={Kind}, Index={Index}"
    };

    #endregion
}
