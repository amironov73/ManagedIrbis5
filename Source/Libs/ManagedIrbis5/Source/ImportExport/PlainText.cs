// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PlainText.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.ImportExport
{
    //
    // Текстовые файлы документов используются для
    // импорта/экспорта данных в режимах ИМПОРТ и ЭКСПОРТ
    // АРМов «Каталогизатор» и «Администратор».
    //
    // Структура
    //
    // Структура текстового файла документов удовлетворяет
    // следующим правилам:
    //
    // Документ
    //
    // * каждый документ начинается с новой строки
    // и может занимать произвольное количество строк
    // произвольной длины;
    // один документ от другого отделяется строкой,
    // содержащей в первых позициях символы *****;
    //
    // Поля
    //
    // документ состоит из полей, каждое из которых
    // начинается с новой строки и имеет следующую структуру:
    //
    // #МММ: <данные поля>
    // <данные поля>
    // ................
    //
    // где МММ - числовая метка поля (лидирующие нули
    // можно не указывать);
    // поля внутри документа могут следовать в произвольном
    // порядке, поля с одинаковыми метками могут повторяться;
    //
    // Подполя
    //
    // * данные поля могут содержать подполя, которые
    // начинаются с признака и разделителя подполя, например:
    //
    // ^A<данные подполя>^B<данные подполя>......
    //
    // * подполя с одинаковыми разделителями не могут
    // повторяться внутри поля.
    //

    /// <summary>
    /// Плоское текстовое представление записи,
    /// используемое, например, при импорте-экспорте
    /// в текстовом формате в АРМ Каталогизатор
    /// и Администратор.
    /// </summary>
    public static class PlainText
    {
        #region Private members

        private static string _ReadTo
            (
                TextReader reader,
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

        private static Field _ParseLine
            (
                string line
            )
        {
            var reader = new StringReader(line);

            var c = (char) reader.Read();
            if (c != '#')
            {
                Magna.Error
                    (
                        "PlainText::_ParseLine: "
                        + "format error: "
                        + line.ToVisibleString()
                    );

                throw new IrbisException();
            }
            var result = new Field
            {
                Tag = _ReadTo(reader, ':').ParseInt32()
            };

            c = (char) reader.Read();
            if (c != ' ')
            {
                Magna.Error
                    (
                        "PlainText::_ParseLine: "
                        + "whitespace required: "
                        + line
                    );
                throw new IrbisException();
            }
            result.Value = _ReadTo(reader, '^');

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

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение одной записи из потока.
        /// </summary>
        public static Record? ReadRecord
            (
                TextReader reader
            )
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            var result = new Record();

            if (line.StartsWith("*****"))
            {
                Magna.Error
                    (
                        "PlainText::ReadRecord: "
                        + "unexpected five stars"
                    );

                throw new IrbisException();
            }

            var field = _ParseLine(line);
            result.Fields.Add(field);

            var good = false;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("*****"))
                {
                    good = true;
                    break;
                }
                field = _ParseLine(line);
                result.Fields.Add(field);
            }

            if (!good)
            {
                Magna.Error
                    (
                        "PlainText::ReadRecord: "
                        + "bad record"
                    );

                throw new IrbisException();
            }

            return result;
        }

        /// <summary>
        /// Read one record from local file.
        /// </summary>
        public static Record? ReadOneRecord
            (
                string fileName,
                Encoding encoding
            )
        {
            using var reader = new StreamReader
                (
                    new FileStream
                        (
                            fileName,
                            FileMode.Open,
                            FileAccess.Read
                        ),
                    encoding
                );
            var result = ReadRecord(reader);

            return result;
        }

        /// <summary>
        /// Read some records from local file.
        /// </summary>
        public static Record[] ReadRecords
            (
                string fileName,
                Encoding encoding
            )
        {
            var result = new List<Record>();

            using (var reader = new StreamReader
                (
                    new FileStream
                    (
                        fileName,
                        FileMode.Open,
                        FileAccess.Read
                    ),
                    encoding
                ))
            {
                Record? record;
                while ((record = ReadRecord(reader)) != null)
                {
                    result.Add(record);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Формирует представление записи в формате ALL.
        /// </summary>
        public static string ToAllFormat
            (
                Record record
            )
        {
            var output = new StringBuilder();
            output.AppendLine("0");
            output.AppendLine($"{record.Mfn}#0");
            output.AppendLine($"0#{record.Version}");
            output.Append(record.ToPlainText());

            return output.ToString();
        }

        /// <summary>
        /// Формирует плоское текстовое представление записи.
        /// </summary>
        public static string ToPlainText
            (
                this Record record
            )
        {
            var result = new StringBuilder();

            foreach (var field in record.Fields)
            {
                result.AppendFormat("{0}#", field.Tag);
                if (!string.IsNullOrEmpty(field.Value))
                {
                    result.Append(field.Value);
                }
                foreach (var subField in field.Subfields)
                {
                    result.Append('^');
                    result.Append(subField.Code);
                    result.Append(subField.Value);
                }
                result.AppendLine();
            }

            return result.ToString();
        }

        /// <summary>
        /// Export the record.
        /// </summary>
        public static TextWriter WriteRecord
            (
                TextWriter writer,
                Record record
            )
        {
            var culture = CultureInfo.InvariantCulture;
            foreach (var field in record.Fields)
            {
                writer.Write('#');
                writer.Write(field.Tag.ToString(culture));
                writer.Write(": ");
                if (!string.IsNullOrEmpty(field.Value))
                {
                    writer.Write(field.Value);
                }

                foreach (var subField in field.Subfields)
                {
                    writer.Write("^{0}{1}", subField.Code, subField.Value);
                }

                writer.WriteLine();
            }

            writer.WriteLine("*****");

            return writer;
        }

        #endregion

    } // class PlainText

} // namespace ManagedIrbis.ImportExport
