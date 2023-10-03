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
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Linq;

using ManagedIrbis;
using ManagedIrbis.Batch;

#endregion

namespace EbscoFix;

internal sealed partial class Program
{
    private static HashSet<string> _epubIdentifiers = null!;
    private static HashSet<string> _pdfIdentifiers = null!;
    private static readonly Regex _regex = RegexForBookId();

    [GeneratedRegex ("\\d{4,}")]
    private static partial Regex RegexForBookId();

    private static bool ProcessRecord
        (
            Record record
        )
    {
        Console.WriteLine ($"MFN {record.Mfn}");
        var fields = record.EnumerateField (951).ToArray();
        string? bookId = null;
        foreach (var one in fields)
        {
            // удаляем старое поле, чтобы не мешалось под ногами
            record.Fields.Remove (one);

            var url = one.FM ('i');
            if (string.IsNullOrEmpty (url))
            {
                continue;
            }

            var match = _regex.Match (url);
            if (!match.Success)
            {
                continue;
            }

            bookId = match.Value;
            break;
        }

        if (string.IsNullOrEmpty (bookId))
        {
            // книга без идентификатора
            Console.WriteLine ("No book id");
            return false;
        }

        var kind = PdfOrEpub (bookId);
        if (string.IsNullOrEmpty (kind))
        {
            // если книги не существует, удаляем запись
            Console.WriteLine ($"Book with id {bookId} doesn't exist");
            record.Status |= RecordStatus.LogicallyDeleted;
            return true;
        }

        var coverUrl = kind == "pdf"
            ? $"/pdf/{bookId}.jpg"
            : $"/epub/{bookId}.jpg";
        var pathToCheck = @"D:\Temp1050" + coverUrl;
        if (File.Exists (pathToCheck))
        {
            // если обложка существует, добавляем ее
            coverUrl = $"http://elib.istu.edu/ebsco{coverUrl}";
            record.Add (new Field (951)
            {
                new ('h', "02a"),
                new ('i', coverUrl)
            });
        }

        var newUrl = kind == "pdf"
            ? $"http://elib.istu.edu/viewer/view.php?file=/ebsco/pdf/{bookId}.pdf"
            : $"http://elib.istu.edu/ebsco/epub/{bookId}.epub";
        record.Add ( new Field (951)
        {
            new ('h', "05"),
            new ('i', newUrl)
        });

        return true;
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

            var connection = ConnectionFactory.Shared.CreateSyncConnection();
            var connectionString = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=EBSCO;arm=C;";
            connection.ParseConnectionString (connectionString);
            connection.Connect();
            if (!connection.IsConnected)
            {
                Console.Error.WriteLine ("Can't connect");
                Console.Error.WriteLine (IrbisException.GetErrorDescription (connection.LastError));
                return 1;
            }

            using var writer = new BatchRecordWriter (connection, connection.EnsureDatabase(), 500);
            var reader = BatchRecordReader.WholeDatabase (connection);
            foreach (var record in reader)
            {
                if (ProcessRecord (record))
                {
                    writer.Append (record);
                }
            }

            Console.WriteLine ("ALL DONE");
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
