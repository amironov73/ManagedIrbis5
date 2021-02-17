// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FlcResult.cs -- result of formal checking
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Flc
{
    /// <summary>
    /// Result of formal checking.
    /// </summary>
    [XmlRoot("flc-result")]
    public sealed class FlcResult
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Can continue?
        /// </summary>
        [JsonIgnore]
        public bool CanContinue => Status == FlcStatus.OK
                                   || Status == FlcStatus.Warning;

        /// <summary>
        /// Status.
        /// </summary>
        [XmlAttribute("status")]
        [JsonPropertyName("status")]
        public FlcStatus Status { get; set; }

        /// <summary>
        /// Messages.
        /// </summary>
        [XmlElement("message")]
        [JsonPropertyName("messages")]
        public NonNullCollection<string> Messages { get; } = new ();

        #endregion

        #region Private members

        private void _AddMessage
            (
                string? message
            )
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = message.Trim();
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            Messages.Add(message);
        }

        private bool _ParseLine
            (
                string? line
            )
        {
            if (string.IsNullOrEmpty(line))
            {
                return false;
            }

            line = line.Trim();
            if (string.IsNullOrEmpty(line))
            {
                return false;
            }

            var code = line.Substring(0, 1);
            var message = line.Substring(1, line.Length - 1);

            if (code != "0" && code != "1" && code != "2")
            {
                _AddMessage(line);

                return false;
            }

            if (code == "1")
            {
                Status = FlcStatus.Error;
            }
            else if (code == "2")
            {
                if (Status == FlcStatus.OK)
                {
                    Status = FlcStatus.Warning;
                }
            }
            _AddMessage(message);

            return true;
        } // method _AddMessage

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the text.
        /// </summary>
        public static FlcResult Parse
            (
                string? text
            )
        {
            var result = new FlcResult();

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            text = text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            var lines = text.SplitLines();
            int index;
            for (index = 0; index < lines.Length; index++)
            {
                if (!result._ParseLine(lines[index]))
                {
                    break;
                }
            }

            return result;
        } // method Parse

        /// <summary>
        /// Should serialize the <see cref="Messages"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeMessages()
        {
            return Messages.Count != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull(reader, "reader");

            Status = (FlcStatus)reader.ReadPackedInt32();
            Messages.Clear();
            Messages.AddRange(reader.ReadStringArray());
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, "writer");

            writer
                .WritePackedInt32((int)Status)
                .WriteArray(Messages.ToArray());
        } // method SaveToStream

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (Messages.Count == 0)
            {
                return $"Status: {Status}, No messages";
            }

            return $"Status: {Status}, Messages: {string.Join(Environment.NewLine, Messages)}";
        } // method ToString

        #endregion

    } // class FlcResult

} // namespace ManagedIrbis.Flc
