// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RawRecord.cs -- сырая (не разобранная) запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Raw (not decoded) record.
    /// </summary>
    [DebuggerDisplay("[{Database}] MFN={Mfn} ({Version})")]
    public sealed class RawRecord
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Признак удалённой записи.
        /// </summary>
        public bool Deleted
        {
            get => (Status & RecordStatus.LogicallyDeleted) != 0;
            set
            {
                if (value)
                {
                    Status |= RecordStatus.LogicallyDeleted;
                }
                else
                {
                    Status &= ~RecordStatus.LogicallyDeleted;
                }
            }
        }

        /// <summary>
        /// Version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Lines of text.
        /// </summary>
        public List<string>? Fields { get; set; }

        #endregion

        #region Private members

        private static void _AppendIrbisLine
            (
                StringBuilder builder,
                string? delimiter,
                string format,
                params object[] args
            )
        {
            builder.AppendFormat(format, args);
            builder.Append(delimiter);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode record to text.
        /// </summary>
        public string EncodeRecord()
        {
            return EncodeRecord(IrbisText.IrbisDelimiter);
        }

        /// <summary>
        /// Encode record to text.
        /// </summary>
        public string EncodeRecord
            (
                string? delimiter
            )
        {
            var result = new StringBuilder();

            _AppendIrbisLine
                (
                    result,
                    delimiter,
                    "{0}#{1}",
                    Mfn,
                    (int)Status
                );
            _AppendIrbisLine
                (
                    result,
                    delimiter,
                    "0#{0}",
                    Version
                );

            if (!ReferenceEquals(Fields, null))
            {
                foreach (var line in Fields)
                {
                    result.Append(line);
                    result.Append(delimiter);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Parse MFN, status and version of the record
        /// </summary>
        public static RawRecord ParseMfnStatusVersion
            (
                string line1,
                string line2,
                RawRecord record
            )
        {
            Sure.NotNullNorEmpty(line1, nameof(line1));
            Sure.NotNullNorEmpty(line2, nameof(line2));

            var regex = new Regex(@"^(-?\d+)\#(\d*)?");
            var match = regex.Match(line1);
            record.Mfn = Math.Abs(int.Parse(match.Groups[1].Value));
            if (match.Groups[2].Length > 0)
            {
                record.Status = (RecordStatus)int.Parse
                    (
                        match.Groups[2].Value
                    );
            }
            match = regex.Match(line2);
            if (match.Groups[2].Length > 0)
            {
                record.Version = int.Parse(match.Groups[2].Value);
            }

            return record;
        }

        /// <summary>
        /// Parse text.
        /// </summary>
        public static RawRecord Parse
            (
                string text
            )
        {
            Sure.NotNullNorEmpty(text, nameof(text));

            var lines = IrbisText.SplitIrbisToLines(text);

            var startOffset = 0;
            if (lines[0] == lines[1])
            {
                startOffset = 1;
            }

            var result = Parse(lines, startOffset);

            return result;
        }

        /// <summary>
        /// Parse text lines.
        /// </summary>
        public static RawRecord Parse
            (
                string[] lines,
                int startOffset = 0
            )
        {
            if (lines.Length < 2)
            {
                Magna.Error
                    (
                        "RawRecord::Parse: "
                        + "text too short"
                    );

                throw new IrbisException("Text too short");
            }

            var line1 = lines[startOffset + 0];
            var line2 = lines[startOffset + 1];

            var result = new RawRecord();
            result.Fields = new List<string>();
            result.Fields.AddRange(lines.Skip(startOffset + 2));

            ParseMfnStatusVersion
                (
                    line1,
                    line2,
                    result
                );

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return EncodeRecord(Environment.NewLine);
        }

        #endregion
    }
}
