// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

/*
    Утилита для импорта метаданных EBSCO.

 */

#region Using directives

using System;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.Text;

using EbscoImport;

using ManagedIrbis;
using ManagedIrbis.ImportExport;

#endregion

#nullable enable

/// <summary>
/// Единственный класс, содержащий всю функциональность утилиты.
/// </summary>
internal sealed class Program
{
    private static void SetAuthor
        (
            Field field,
            string text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return;
        }

        var parts = text.Split (CommonSeparators.Space, 2);
        field.SetSubFieldValue ('a', parts[0]);
        if (parts.Length != 1)
        {
            field.SetSubFieldValue ('g', parts[1]);
        }
    }

    private static Record ConvertRecord
        (
            EbscoRecord source
        )
    {
        var result = new Record();

        var control = source.Header?.Control;
        if (control is null)
        {
            throw new Exception();
        }

        var book = control.Book;
        var publcation = control.Publication;
        var article = control.Article;
        if (book is null || publcation is null || article is null)
        {
            throw new Exception();
        }

        result.Add (new Field (900)
        {
            { 't', "a"  }, // тип: текстовый документ
            { 'b', "05" }, // вид: однотомное издание
        });
        result.AddNonEmptyField (101, control.Language?.Code);
        result.AddNonEmptyField (200, book.Title);
        result.AddNonEmptyField (331, article.Abstract);

        if (book.Authors is { } authors)
        {
            var counter = 0;
            foreach (var author in authors)
            {
                var tag = counter is 0 ? 700 : 701;
                var field = new Field (tag);
                SetAuthor (field, author);

                counter++;
            }
        }

        if (!string.IsNullOrEmpty (book.Series))
        {
            result.Add (new Field (225)
            {
                { 'a', book.Series }
            });
        }

        if (publcation.Date is {} date)
        {
            var field = new Field (215)
                .AddNonEmpty ('c', publcation.Publisher)
                .AddNonEmpty ('a', publcation.Place)
                .AddNonEmpty ('d', date.Year);
            if (!field.IsEmpty)
            {
                result.Add (field);
            }
        }

        if (book.Isbn is { } isbnArray)
        {
            foreach (var isbn in isbnArray)
            {
                if (isbn.Type is "print")
                {
                    result.Add (new Field (10)
                    {
                        { 'a', isbn.Value }
                    });
                }
            }
        }

        if (article.Subjects is {} subjects)
        {
            foreach (var subject in subjects)
            {
                if (subject.Type is "unclass")
                {
                    var parts = subject.Title?.Split ("--") ?? Array.Empty<string>();
                    if (parts.Length != 0)
                    {
                        var field = new Field (606)
                            .Add ('a', parts[0])
                            .AddNonEmpty ('b', parts.SafeAt (1))
                            .AddNonEmpty ('c', parts.SafeAt (2))
                            .AddNonEmpty ('d', parts.SafeAt (3));
                        result.Add (field);
                    }
                }
            }
        }

        return result;
    }

    private static void ProcessFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        // Console.WriteLine (fileName);
        using var textReader = File.OpenText (fileName);
        var serializer = new XmlSerializer (typeof (EbscoRecords));
        var records = (EbscoRecords?) serializer.Deserialize (textReader);
        // Console.WriteLine ($"Records found: {records?.Records?.Length ?? 0}");

        if (records?.Records is {} ebscoRecords)
        {
            foreach (var ebscoRecord in ebscoRecords)
            {
                var irbisRecord = ConvertRecord (ebscoRecord);
                PlainText.WriteRecord (Console.Out, irbisRecord);
            }
        }
    }

    /// <summary>
    /// Собственно точка входа в программу.
    /// </summary>
    static int Main()
    {
        try
        {
            var foundFiles = Directory.EnumerateFiles ("Input", "*.xml",
                SearchOption.TopDirectoryOnly);
            foreach (var fileName in foundFiles)
            {
                ProcessFile (fileName);
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
