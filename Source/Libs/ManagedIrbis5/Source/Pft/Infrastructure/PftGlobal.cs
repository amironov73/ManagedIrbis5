// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftGlobal.cs -- global variable
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Global variable.
    /// </summary>
    [DebuggerDisplay("{Number} {ToString()}")]
    public sealed class PftGlobal
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Number of the variable.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Fields.
        /// </summary>
        public NonNullCollection<Field> Fields { get; private set; } = new();

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGlobal()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGlobal
            (
                int number
            )
        {
            Number = number;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGlobal
            (
                int number,
                string text
            )
        {
            Number = number;
            Parse(text);
        }

        #endregion

        #region Private members

        private static ReadOnlyMemory<char> _ReadTo
            (
                StringReader reader,
                char delimiter
            )
        {
            StringBuilder result = new StringBuilder();

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                var c = (char)next;
                if (c == delimiter)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString().AsMemory();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse (possibly multiline) text.
        /// </summary>
        public PftGlobal Parse
            (
                string? text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {

                string[] lines = text.SplitLines();
                foreach (string line in lines)
                {
                    ParseLine(line);
                }

            }

            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public PftGlobal ParseLine
            (
                string line
            )
        {
            var reader = new StringReader(line);
            var field = new Field { Tag = Number };
            Fields.Add(field);
            field.Value = _ReadTo(reader, '^').EmptyToNull();

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }

                var code = char.ToLower((char)next);
                var text = _ReadTo(reader, '^').EmptyToNull();
                var subField = new SubField
                {
                    Code = code,
                    Value = text
                };
                field.Subfields.Add(subField);
            }

            return this;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Number = reader.ReadPackedInt32();
            reader.ReadCollection(Fields);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WritePackedInt32(Number);
            writer.WriteCollection(Fields);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();

            var first = true;

            foreach (var field in Fields)
            {
                if (!first)
                {
                    result.AppendLine();
                }
                first = false;
                result.Append(field.ToText());
            }

            return result.ToString();
        }

        #endregion
    }
}
