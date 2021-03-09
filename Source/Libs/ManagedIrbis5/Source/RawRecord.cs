// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMethodReturnValue.Global

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

using static ManagedIrbis.RecordStatus;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Сырая (не разобранная) запись.
    /// </summary>
    /// <remarks>
    /// Основной сценарий, ради которых создан класс -- массированная
    /// выгрузка/загрузка записей из/в ИРБИС.
    /// </remarks>
    [DebuggerDisplay("[{" + nameof(Database) + "}] MFN={"
        + nameof(Mfn) + "} ({" + nameof(Version) + "})")]
    public sealed class RawRecord
    {
        #region Properties

        /// <summary>
        /// База данных, в которой хранится запись.
        /// Для вновь созданных записей -- <c>null</c>.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN (порядковый номер в базе данных) записи.
        /// Для вновь созданных записей равен <c>0</c>.
        /// Для хранящихся в базе записей нумерация начинается
        /// с <c>1</c>.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Статус записи. Для вновь созданных записей <c>None</c>.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Признак -- запись помечена как логически удаленная.
        /// </summary>
        public bool Deleted
        {
            get => (Status & LogicallyDeleted) != 0;
            set
            {
                if (value)
                {
                    Status |= LogicallyDeleted;
                }
                else
                {
                    Status &= ~LogicallyDeleted;
                }
            }
        } // property Deleted

        /// <summary>
        /// Версия записи. Для вновь созданных записей равна <c>0</c>.
        /// Для хранящихся в базе записей нумерация версий начинается
        /// с <c>1</c>.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Текстовые строки с полями.
        /// </summary>
        public List<string> Fields { get; } = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Encode record to text.
        /// </summary>
        public string EncodeRecord
            (
                string? delimiter = IrbisText.IrbisDelimiter
            )
        {
            var result = new StringBuilder(512);

            result.Append(Mfn.ToInvariantString());
            result.Append('#');
            result.Append(((int) Status).ToInvariantString());
            result.Append(delimiter);
            result.Append("0#");
            result.Append(Version.ToInvariantString());
            result.Append(delimiter);

            foreach (var line in Fields)
            {
                result.Append(line).Append(delimiter);
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
            var regex = new Regex(@"^(-?\d+)\#(\d*)?");
            var match = regex.Match(line1);
            record.Mfn = Math.Abs(int.Parse(match.Groups[1].Value));
            if (match.Groups[2].Length > 0)
            {
                record.Status = (RecordStatus) int.Parse
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
            var lines = IrbisText.SplitIrbisToLines(text);

            var startOffset = 0;
            if (lines[0] == lines[1])
            {
                startOffset = 1;
            }

            var result = Parse(lines, startOffset);

            return result;
        } // method Parse

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
            result.Fields.AddRange(lines.Skip(startOffset + 2));

            ParseMfnStatusVersion
                (
                    line1,
                    line2,
                    result
                );

            return result;
        } // method Parse

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => EncodeRecord(Environment.NewLine);

        #endregion

    } // class RawRecord

} // namespace ManagedIrbis
