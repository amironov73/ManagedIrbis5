// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* NodeLeader64.cs -- record leader in N01/L01
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Record leader of L01/N01 file.
    /// </summary>
    [DebuggerDisplay("Number={Number}, Previous={Previous}, Next={Next}, "
        + "TermCount={TermCount}, FreeOffset={FreeOffset}")]
    public sealed class NodeLeader64
    {
        #region Properties

        /// <summary>
        /// Номер записи (начиная с 1; в N01 номер первой записи
        /// равен номеру корневой записи дерева
        /// </summary>
        [XmlAttribute("number")]
        [JsonPropertyName("number")]
        public int Number { get; set; }

        /// <summary>
        /// Номер предыдущей записи (-1, если нет)
        /// </summary>
        [XmlAttribute("previous")]
        [JsonPropertyName("previous")]
        public int Previous { get; set; }

        /// <summary>
        /// Номер следующей записи (-1, если нет)
        /// </summary>
        [XmlAttribute("next")]
        [JsonPropertyName("previous")]
        public int Next { get; set; }

        /// <summary>
        /// Число ключей в записи
        /// </summary>
        [XmlAttribute("term-count")]
        [JsonPropertyName("term-count")]
        public int TermCount { get; set; }

        /// <summary>
        /// Смещение на свободную позицию в записи
        /// (от начала записи)
        /// </summary>
        [XmlAttribute("free-offset")]
        [JsonPropertyName("free-offset")]
        public int FreeOffset { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Dump the leader.
        /// </summary>
        public void Dump
            (
                TextWriter writer
            )
        {
            writer.WriteLine("NUMBER: {0}", Number);
            writer.WriteLine("PREV  : {0}", Previous);
            writer.WriteLine("NEXT  : {0}", Next);
            writer.WriteLine("TERMS : {0}", TermCount);
            writer.WriteLine("FREE  : {0}", FreeOffset);
        }

        /// <summary>
        /// Считывание из потока.
        /// </summary>
        public static NodeLeader64 Read
            (
                Stream stream
            )
        {
            var result = new NodeLeader64
                {
                    Number = stream.ReadInt32Network(),
                    Previous = stream.ReadInt32Network(),
                    Next = stream.ReadInt32Network(),
                    TermCount = stream.ReadInt32Network(),
                    FreeOffset = stream.ReadInt32Network()
                };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Number: {0}, Previous: {1}, "
                    + "Next: {2}, TermCount: {3}, "
                    + "FreeOffset: {4}",
                    Number,
                    Previous,
                    Next,
                    TermCount,
                    FreeOffset
                );
        }

        #endregion
    }
}
