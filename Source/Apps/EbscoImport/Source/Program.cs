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
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.Linq;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.ImportExport;

#endregion

#nullable enable

namespace EbscoImport;

internal sealed class Program
{
    private static HashSet<string> _epubIdentifiers = null!;
    private static HashSet<string> _pdfIdentifiers = null!;

    private static readonly string[] _collectiveWords =
    {
        "Association", "Institute", "Meeting", "Committee", "Academy",
        "Corporation", "Foundation", "Museum"
    };

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

        var position = text.LastIndexOf (' ');
        if (position < 0)
        {
            field.SetSubFieldValue ('a', text);
        }
        else
        {
            field.SetSubFieldValue ('a', text.Substring (position + 1));
            field.SetSubFieldValue ('g', text.Substring (0, position));
        }

        // var parts = text.Split (CommonSeparators.Space, 2);
        // if (parts.Length == 0)
        // {
        //     field.SetSubFieldValue ('a', text);
        // }
        // else
        // {
        //     field.SetSubFieldValue ('g', parts[0]);
        //     field.SetSubFieldValue ('a', parts[1]);
        // }
    }

    private static bool ContainsAnySubstring
        (
            string text,
            string[] valuesToCheck
        )
    {
        foreach (var substring in valuesToCheck)
        {
            if (text.Contains (substring, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static Field ConvertAuthor
        (
            string author,
            int counter
        )
    {
        int tag;
        Field field;

        if (ContainsAnySubstring (author, _collectiveWords))
        {
            tag = counter is 0 ? 710 : 711;
            field = new Field (tag);
            field.SetSubFieldValue ('a', author);
        }
        else
        {
            tag = counter is 0 ? 700 : 701;
            field = new Field (tag);
            SetAuthor (field, author);
        }

        return field;
    }

    private static Record? ConvertRecord
        (
            EbscoRecord source,
            string? id,
            string kind
        )
    {
        var control = source.Header?.Control;
        if (control is null)
        {
            return null;
        }

        var book = control.Book;
        var publcation = control.Publication;
        var article = control.Article;
        if (book is null || publcation is null || article is null)
        {
            return null;
        }

        var bookTitle = book.Title;
        if (string.IsNullOrEmpty (bookTitle))
        {
            return null;
        }

        var result = new Record();

        result.Add (new Field (900)
        {
            { 't', "a"  }, // тип: текстовый документ
            { 'b', "05" }, // вид: однотомное издание
        });
        result.Add (920, Constants.Pazk);
        result.AddNonEmptyField (101, control.Language?.Code);
        result.Add (200, 'a', bookTitle);
        result.AddNonEmptyField (331, article.Abstract);

        if (book.Authors is { } authors)
        {
            var counter = 0;
            if (authors.Length > 3)
            {
                counter++;
            }

            foreach (var author in authors)
            {
                var field = ConvertAuthor (author, counter);
                result.Add (field);
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
            var field = new Field (210)
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

        if (!string.IsNullOrEmpty (article.Abstract))
        {
            var field = new Field (331)
            {
                Value = article.Abstract
            };
            result.Add (field);
        }

        var url = kind == "pdf"
            ? $"http://elib.istu.edu/viewer.php?file=/ebsco/{id}.pdf"
            : $"http://elib.istu.edu/ebsco/{id}.epub";
        result.Add (new Field (951, 'i', url));

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
                var id = ebscoRecord.Header?.Term;
                var kind = PdfOrEpub (id);
                if (string.IsNullOrEmpty (kind))
                {
                    continue;
                }

                var irbisRecord = ConvertRecord (ebscoRecord, id, kind);
                if (irbisRecord is not null)
                {
                    PlainText.WriteRecord (Console.Out, irbisRecord);
                }
            }
        }
    }

    private static HashSet<string> LoadBookIdentifiersFromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var result = new HashSet<string>();
        var lines = File.ReadAllLines (fileName)
            .NonEmptyLines();
        foreach (var line in lines)
        {
            result.Add (line);
        }

        return result;
    }

    private static string? PdfOrEpub (string? id) =>
        string.IsNullOrEmpty (id) ? null
        : _pdfIdentifiers.Contains (id) ? "pdf"
        : _epubIdentifiers.Contains (id) ? "epub"
        : null;

    /// <summary>
    /// Собственно точка входа в программу.
    /// </summary>
    private static int Main()
    {
        try
        {
            _epubIdentifiers = LoadBookIdentifiersFromFile ("epub.txt");
            _pdfIdentifiers = LoadBookIdentifiersFromFile ("pdf.txt");

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
