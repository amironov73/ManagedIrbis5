// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ProtocolText.cs -- текстовое представление записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    /// Текстовое представление записи, используемое в протоколе
    /// ИРБИС64-сервер.
    /// </summary>
    public static class ProtocolText
    {
        #region Private members

        private static void _AppendIrbisLine
            (
                StringBuilder builder,
                string format,
                params object[] args
            )
        {
            builder.AppendFormat(format, args);
            builder.Append(IrbisText.IrbisDelimiter);
        }

        private static string _ReadTo
            (
                StringReader reader,
                char delimiter
            )
        {
            var result = new StringBuilder();

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

            return result.ToString();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode subfield.
        /// </summary>
        public static void EncodeSubField
            (
                StringBuilder builder,
                SubField subField
            )
        {
            builder.AppendFormat
                (
                    "{0}{1}{2}",
                    SubField.Delimiter,
                    subField.Code,
                    subField.Value
                );
        }

        /// <summary>
        /// Encode field.
        /// </summary>
        public static void EncodeField
            (
                StringBuilder builder,
                Field field
            )
        {
            builder.AppendFormat
                (
                    "{0}#",
                    field.Tag
                );

            builder.Append(field.Value);

            foreach (var subField in field.Subfields)
            {
                EncodeSubField
                    (
                        builder,
                        subField
                    );
            }

            builder.Append(IrbisText.IrbisDelimiter);
        }

        /// <summary>
        /// Кодирование записи в клиентское представление.
        /// </summary>
        /// <param name="record">Запись для кодирования.</param>
        /// <returns>
        /// Закодированная запись.
        /// </returns>
        public static string EncodeRecord
            (
                Record record
            )
        {
            var result = new StringBuilder();

            _AppendIrbisLine
                (
                    result,
                    "{0}#{1}",
                    record.Mfn,
                    (int)record.Status
                );
            _AppendIrbisLine
                (
                    result,
                    "0#{0}",
                    record.Version
                );

            foreach (var field in record.Fields)
            {
                EncodeField
                    (
                        result,
                        field
                    );
            }

            return result.ToString();
        }

        /// <summary>
        /// Parse the line.
        /// </summary>
        public static Field ParseLine
            (
                string line
            )
        {
            Sure.NotNullNorEmpty(line, nameof(line));

            var reader = new StringReader(line);
            var result = new Field
            {
                Tag = FastNumber.ParseInt32(_ReadTo(reader, '#')),
                Value = _ReadTo(reader, '^').EmptyToNull()
            };

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                var code = char.ToLower((char)next);
                var text = _ReadTo(reader, '^');
                var subField = new SubField
                {
                    Code = code,
                    Value = text
                };
                result.Subfields.Add(subField);
            }

            return result;
        }

        /// <summary>
        /// Parse MFN, status and version of the record
        /// </summary>
        public static Record ParseMfnStatusVersion
            (
                string line1,
                string line2,
                Record record
            )
        {
            Sure.NotNullNorEmpty(line1, nameof(line1));
            Sure.NotNullNorEmpty(line2, nameof(line2));

            var regex = new Regex(@"^(-?\d+)\#(\d*)?");
            var match = regex.Match(line1);
            record.Mfn = Math.Abs(FastNumber.ParseInt32(match.Groups[1].Value));
            if (match.Groups[2].Length > 0)
            {
                record.Status = (RecordStatus) FastNumber.ParseInt32(match.Groups[2].Value);
            }
            match = regex.Match(line2);
            if (match.Groups[2].Length > 0)
            {
                record.Version = FastNumber.ParseInt32(match.Groups[2].Value);
            }

            return record;
        }

        /// <summary>
        /// Parse server response for ReadRecordCommand.
        /// </summary>
        public static Record ParseResponseForReadRecord
            (
                Response response,
                Record record
            )
        {
            try
            {
                //record.Fields.BeginUpdate();
                record.Fields.Clear();

                ParseMfnStatusVersion
                    (
                        response.RequireUtf(),
                        response.RequireUtf(),
                        record
                    );

                string? line;
                while (true)
                {
                    line = response.ReadUtf();
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    }
                    if (line == "#")
                    {
                        break;
                    }
                    var field = ParseLine(line);
                    if (field.Tag > 0)
                    {
                        record.Fields.Add(field);
                    }
                }
                if (line == "#")
                {
                    var returnCode = response.RequireInteger();
                    if (returnCode >= 0)
                    {
                        line = response.RequireUtf();
                        line = IrbisText.IrbisToWindows(line);
                        record.Description = line;
                    }
                }

            }
            finally
            {
                //record.Fields.EndUpdate();
                //record.Modified = false;
            }

            return record;
        }

        /// <summary>
        /// Parse server response for WriteRecordCommand.
        /// </summary>
        public static Record ParseResponseForWriteRecord
            (
                Response response,
                Record record
            )
        {
            // Если в БД нет autoin.gbl, сервер не присылает
            // обработанную запись.

            var first = response.ReadUtf();
            if (string.IsNullOrEmpty(first))
            {
                return record;
            }

            var second = response.ReadUtf();
            if (string.IsNullOrEmpty(second))
            {
                return record;
            }

            try
            {
                //record.Fields.BeginUpdate();
                record.Fields.Clear();

                var split = second.Split('\x1E');

                ParseMfnStatusVersion
                    (
                        first,
                        split[0],
                        record
                    );

                for (var i = 1; i < split.Length; i++)
                {
                    var line = split[i];
                    var field = ParseLine(line);
                    if (field.Tag > 0)
                    {
                        record.Fields.Add(field);
                    }
                }
            }
            finally
            {
                //record.Fields.EndUpdate();
                //record.Modified = false;
            }

            return record;
        }

        /// <summary>
        /// Parse server response for WriteRecordsCommand.
        /// </summary>
        public static Record ParseResponseForWriteRecords
            (
                Response response,
                Record record
            )
        {
            try
            {
                //record.Fields.BeginUpdate();
                record.Fields.Clear();

                var whole = response.RequireUtf();
                var split = whole.Split('\x1E');

                ParseMfnStatusVersion
                    (
                        split[0],
                        split[1],
                        record
                    );

                for (var i = 2; i < split.Length; i++)
                {
                    var line = split[i];
                    var field = ParseLine(line);
                    if (field.Tag > 0)
                    {
                        record.Fields.Add(field);
                    }
                }
            }
            finally
            {
                //record.Fields.EndUpdate();
                //record.Modified = false;
            }

            return record;
        }

        /// <summary>
        /// Parse server response for ALL-formatted record.
        /// </summary>
        public static Record? ParseResponseForAllFormat
            (
                Response response,
                Record record
            )
        {
            try
            {
                //record.Fields.BeginUpdate();
                record.Fields.Clear();

                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }

                var split = line.Split('\x1F');
                if (split.Length < 3)
                {
                    return null;
                }

                ParseMfnStatusVersion
                    (
                        split[1],
                        split[2],
                        record
                    );

                for (var i = 3; i < split.Length; i++)
                {
                    line = split[i];
                    if (!string.IsNullOrEmpty(line))
                    {
                        var field = ParseLine(line);
                        if (field.Tag > 0)
                        {
                            record.Fields.Add(field);
                        }
                    }
                }
            }
            finally
            {
                //record.Fields.EndUpdate();
                //record.Modified = false;
            }

            return record;
        }

        /// <summary>
        /// Parse server response for ALL-formatted record.
        /// </summary>
        public static Record? ParseResponseForAllFormat
            (
                string? line,
                Record record
            )
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            try
            {
                //record.Fields.BeginUpdate();
                record.Fields.Clear();

                var split = line.Split('\x1F');
                if (split.Length < 3)
                {
                    return null;
                }

                ParseMfnStatusVersion
                    (
                        split[1],
                        split[2],
                        record
                    );

                for (var i = 3; i < split.Length; i++)
                {
                    line = split[i];
                    if (!string.IsNullOrEmpty(line))
                    {
                        var field = ParseLine(line);
                        if (field.Tag > 0)
                        {
                            record.Fields.Add(field);
                        }
                    }
                }
            }
            finally
            {
                //record.Fields.EndUpdate();
                //record.Modified = false;
            }

            return record;
        }

        /// <summary>
        /// Parse response for global correction
        /// of virtual record.
        /// </summary>
        public static Record? ParseResponseForGblFormat
            (
                string? line,
                Record record
            )
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            try
            {
                //record.Fields.BeginUpdate();
                record.Fields.Clear();

                var split = line.Split('\x1E');
                for (var i = 1; i < split.Length; i++)
                {
                    line = split[i];
                    if (!string.IsNullOrEmpty(line))
                    {
                        var field = ParseLine(line);
                        if (field.Tag > 0)
                        {
                            record.Fields.Add(field);
                        }
                    }
                }
            }
            finally
            {
                //record.Fields.EndUpdate();
                //record.Modified = false;
            }

            return record;
        }

        /// <summary>
        /// Convert the record to the protocol text.
        /// </summary>
        public static string? ToProtocolText
            (
                this Record? record
            )
        {
            return ReferenceEquals(record, null)
                ? null
                : EncodeRecord(record);
        }

        #endregion
    }
}
