// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldReference.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Pft.Infrastructure;

using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

#nullable enable

namespace ManagedIrbis
{
    // Формат ссылки
    // "1" |2|+ v3/4^5[6-7]*8.9 +|10| "11"
    // где
    // 1 - условный префикс-литерал
    // 2 - повторяющийся префикс-литерал
    // v - один из символов: d, n, v
    // 3 - тег поля
    // 4 - тег встроенного поля
    // 5 - код подполя
    // 6 - начальный номер повторения
    // 7 - конечный номер повторения
    // 8 - смещение
    // 9 - длина
    // 10 - повторяющийся суффикс-литерал
    // 11 - условный суффикс-литерал

    // Примеры ссылок на поля
    // v200
    // v200^a
    // ". - "v200
    // v300+| - |
    // v701[1-2]
    // v701[2]
    // v701^a*2.2
    // "Отсутствует"n700


    /// <summary>
    ///
    /// </summary>
    [DebuggerDisplay("{" + nameof(Tag) + "}")]
    public sealed class FieldReference
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Нет кода.
        /// </summary>
        public const char NoCode = '\0';

        #endregion

        #region Properties

        /// <summary>
        /// Command.
        /// </summary>
        public char Command { get; set; }

        /// <summary>
        /// Embedded.
        /// </summary>
        public string? Embedded { get; set; }

        /// <summary>
        /// Отступ.
        /// </summary>
        public int Indent { get; set; }

        /// <summary>
        /// Смещение.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Длина.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Subfield.
        /// </summary>
        public char SubField { get; set; }

        /// <summary>
        /// Tag.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Tag specification.
        /// </summary>
        public string? TagSpecification { get; set; }

        /// <summary>
        /// Field repeat.
        /// </summary>
        public IndexSpecification FieldRepeat { get; set; }

        /// <summary>
        /// Subfield repeat.
        /// </summary>
        public IndexSpecification SubFieldRepeat { get; set; }

        /// <summary>
        /// Subfield specification.
        /// </summary>
        public string? SubFieldSpecification { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldReference()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldReference
            (
                int tag
            )
        {
            Tag = tag;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldReference
            (
                int tag,
                char subField
            )
        {
            Tag = tag;
            SubField = subField;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the specification.
        /// </summary>
        public void Apply
            (
                FieldSpecification specification
            )
        {
            Command = specification.Command;
            Embedded = specification.Embedded;
            Indent = specification.ParagraphIndent;
            FieldRepeat = specification.FieldRepeat;
            SubFieldRepeat = specification.SubFieldRepeat;
            Offset = specification.Offset;
            Length = specification.Length;
            SubField = specification.SubField;
            Tag = specification.Tag;
            TagSpecification = specification.TagSpecification;
            SubFieldSpecification = specification.SubFieldSpecification;
        }

        /// <summary>
        /// Format the reference against given record.
        /// </summary>
        public string? Format
            (
                Record record
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            var pft = new PftV();
            pft.Apply(this);
            pft.Execute(context);
            string result = context.Text;

            return result;
        }

        /// <summary>
        /// Get unique values of the field.
        /// </summary>
        public string[] GetUniqueValues
            (
                Record record
            )
        {
            var result = GetValues(record)
                .Distinct()
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get unique values of the field.
        /// </summary>
        public string[] GetUniqueValuesIgnoreCase
            (
                Record record
            )
        {
            var result = GetValues(record)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get values of the field.
        /// </summary>
        public string[] GetValues
            (
                Record record
            )
        {
            string[] result;

            int tag = Tag;
            if (tag <= 0)
            {
                result = Array.Empty<string>();
            }
            else
            {
                if (SubField == NoCode)
                {
                    result = record
                        .Fields
                        .GetField(tag)
                        .Select(field => field.ToText())
                        .NonEmptyLines()
                        .ToArray();
                }
                else
                {
                    result = record
                        .FMA(tag, SubField)
                        .NonEmptyLines()
                        .ToArray();
                }
            }

            return result;
        }

        /// <summary>
        /// Parse field specification.
        /// </summary>
        public static FieldReference Parse
            (
                string specification
            )
        {
            FieldReference result;

            try
            {
                var fs = new FieldSpecification();
                fs.Parse(specification);
                result = new FieldReference();
                result.Apply(fs);
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "FieldReference::Parse",
                        exception
                    );

                throw;
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            // TODO handle FieldRepeat and SubFieldRepeat

            Command = reader.ReadChar();
            Embedded = reader.ReadNullableString();
            Indent = reader.ReadPackedInt32();
            Length = reader.ReadPackedInt32();
            Offset = reader.ReadPackedInt32();
            SubField = reader.ReadChar();
            Tag = reader.ReadPackedInt32();
            TagSpecification = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            // TODO handle FieldRepeat and SubFieldRepeat

            writer.Write(Command);
            writer
                .WriteNullable(Embedded)
                .WritePackedInt32(Indent)
                .WritePackedInt32(Length)
                .WritePackedInt32(Offset)
                .Write(SubField);
            writer.WritePackedInt32(Tag);
            writer.WriteNullable(TagSpecification);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<FieldReference> verifier = new Verifier<FieldReference>
                (
                    this,
                    throwOnError
                );

            verifier
                .Positive(Tag, "Tag");

            return verifier.Result;
        }

        #endregion
    }
}
