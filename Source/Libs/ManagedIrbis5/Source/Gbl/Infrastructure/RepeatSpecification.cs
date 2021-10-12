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

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure
{
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
            Sure.Defined(kind, nameof(kind));

            Kind = kind;

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public RepeatSpecification
            (
                int index
            )
            : this()
        {
            Sure.Positive(index, nameof(index));

            Kind = RepeatKind.Explicit;
            Index = index;

        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RepeatSpecification
            (
                RepeatKind kind,
                int index
            )
            : this()
        {
            Sure.Defined(kind, nameof(kind));
            Sure.NonNegative(index, nameof(index));

            Kind = kind;
            Index = index;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the text.
        /// </summary>
        public static RepeatSpecification Parse
            (
                string text
            )
        {
            Sure.NotNullNorEmpty(text, nameof(text));

            RepeatSpecification result = new RepeatSpecification();
            switch (text)
            {
                case "*":
                    result.Kind = RepeatKind.All;
                    break;

                case "F":
                    result.Kind = RepeatKind.ByFormat;
                    break;

                case "L":
                    result.Kind = RepeatKind.Last;
                    break;

                default:
                    if (uint.TryParse(text, out var index))
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
                        result.Index = (int) index;
                    }
                    else if (text[0] == 'L'
                        && uint.TryParse
                            (
                                text.Substring(2),
                                out index
                            ))
                    {
                        result.Kind = RepeatKind.Last;
                        result.Index = (int) index;
                    }
                    else
                    {
                        Magna.Error
                            (
                                "RepeatSpecification::Parse: "
                               + "invalid repeat specification="
                               + text
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
        /// Should serialize <see cref="Index"/> field?
        /// </summary>
        public bool ShouldSerializeIndex()
        {
            return Index != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull(reader, nameof(reader));

            Kind = (RepeatKind) reader.ReadPackedInt32();
            Index = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer
                .WritePackedInt32((int) Kind)
                .WritePackedInt32(Index);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<RepeatSpecification> verifier
                = new Verifier<RepeatSpecification>(this, throwOnError);

            switch (Kind)
            {
                case RepeatKind.All:
                    verifier.Assert(Index == 0);
                    break;

                case RepeatKind.ByFormat:
                    verifier.Assert(Index == 0);
                    break;

                case RepeatKind.Last:
                    verifier.Assert(Index >= 0);
                    break;

                case RepeatKind.Explicit:
                    verifier.Assert(Index > 0);
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
        public override string ToString()
        {
            switch (Kind)
            {
                case RepeatKind.All:
                    return "*";

                case RepeatKind.ByFormat:
                    return "F";

                case RepeatKind.Last:
                    return Index == 0
                        ? "L"
                        : "L-" + Index.ToInvariantString();

                case RepeatKind.Explicit:
                    return Index.ToInvariantString();

                default:
                    return $"Kind={Kind}, Index={Index}";
            }

        } // method ToString

        #endregion

    } // struct RepeatSpecification

} // namespace ManagedIrbis.Gbl.Infrastructure
