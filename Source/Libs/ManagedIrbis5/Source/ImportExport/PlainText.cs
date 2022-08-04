// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PlainText.cs -- плоское текстовое представление библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using AM;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.ImportExport;

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

    private static ReadOnlyMemory<char> _ReadTo
        (
            TextReader reader,
            char delimiter
        )
    {
        Sure.NotNull (reader);

        var builder = StringBuilderPool.Shared.Get();
        while (true)
        {
            var next = reader.Read();
            if (next < 0)
            {
                break;
            }

            var c = (char) next;
            if (c == delimiter)
            {
                break;
            }

            builder.Append (c);
        }

        var result = builder.ToString().AsMemory();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    private static Field _ParseLine
        (
            string line
        )
    {
        Sure.NotNull (line);

        var reader = new StringReader (line);
        var c = (char)reader.Read();
        if (c != '#')
        {
            Magna.Logger.LogError
                (
                    nameof (PlainText) + "::" + nameof (_ParseLine)
                    + ": format error: {Line}",
                    line.ToVisibleString()
                );

            throw new IrbisException();
        }

        var result = new Field
        {
            Tag = _ReadTo (reader, ':').ParseInt32()
        };

        c = (char)reader.Read();
        if (c != ' ')
        {
            Magna.Logger.LogError
                (
                    nameof (PlainText) + "::" + nameof (_ParseLine)
                    + ": whitespace required: {Line}",
                    line
                );
            throw new IrbisException();
        }

        result.Value = _ReadTo (reader, '^').EmptyToNull();

        while (true)
        {
            var next = reader.Read();
            if (next < 0)
            {
                break;
            }

            var code = char.ToLower ((char)next);
            var text = _ReadTo (reader, '^');
            var subField = new SubField
            {
                Code = code,
                Value = text.ToString()
            };
            result.Subfields.Add (subField);
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
        Sure.NotNull (reader);

        var line = reader.ReadLine();
        if (string.IsNullOrEmpty (line))
        {
            return null;
        }

        var result = new Record();

        if (line.StartsWith ("*****"))
        {
            Magna.Logger.LogError
                (
                    nameof (PlainText) + "::" + nameof (ReadRecord)
                    + ": unexpected five stars"
                );

            throw new IrbisException();
        }

        var field = _ParseLine (line);
        result.Fields.Add (field);

        var good = false;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith ("*****"))
            {
                good = true;
                break;
            }

            field = _ParseLine (line);
            result.Fields.Add (field);
        }

        if (!good)
        {
            Magna.Logger.LogError
                (
                    nameof (PlainText) + "::" + nameof (ReadRecord)
                    + ": bad record"
                );

            throw new IrbisException();
        }

        return result;
    }

    /// <summary>
    /// Чтение одной библиографической записи из локального файла.
    /// </summary>
    public static Record? ReadOneRecord
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNull (encoding);

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
        var result = ReadRecord (reader);

        return result;
    }

    /// <summary>
    /// Чтение нескольких библиографических записей из локального файла.
    /// </summary>
    public static Record[] ReadRecords
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNull (encoding);

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
            while (ReadRecord (reader) is { } record)
            {
                result.Add (record);
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
        Sure.NotNull (record);

        var builder = StringBuilderPool.Shared.Get();
        builder.AppendLine ("0");
        builder.AppendLine ($"{record.Mfn}#0");
        builder.AppendLine ($"0#{record.Version}");
        builder.Append (record.ToPlainText());

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Формирует плоское текстовое представление записи.
    /// </summary>
    public static string ToPlainText
        (
            this Record record
        )
    {
        Sure.NotNull (record);

        var builder = StringBuilderPool.Shared.Get();
        foreach (var field in record.Fields)
        {
            builder.AppendFormat ("{0}#", field.Tag);
            foreach (var subField in field.Subfields)
            {
                if (subField.Code != SubField.NoCode)
                {
                    builder.Append ('^');
                    builder.Append (subField.Code);
                }

                builder.Append (subField.Value);
            }

            builder.AppendLine();
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Экспорт записи.
    /// </summary>
    public static TextWriter WriteRecord
        (
            TextWriter writer,
            Record record
        )
    {
        Sure.NotNull (writer);
        Sure.NotNull (record);

        var culture = CultureInfo.InvariantCulture;
        foreach (var field in record.Fields)
        {
            writer.Write ('#');
            writer.Write (field.Tag.ToString (culture));
            writer.Write (": ");
            if (!field.Value.IsEmpty())
            {
                writer.Write (field.Value);
            }

            foreach (var subField in field.Subfields)
            {
                writer.Write ("^{0}{1}", subField.Code, subField.Value);
            }

            writer.WriteLine();
        }

        writer.WriteLine ("*****");

        return writer;
    }

    #endregion
}
