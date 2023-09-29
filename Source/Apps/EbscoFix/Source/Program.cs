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

    [GeneratedRegex ("\\d{3,}")]
    private static partial Regex RegexForBookId();

    private static bool ProcessRecord
        (
            Record record
        )
    {
        var result = false;

        Console.WriteLine ($"{record.Mfn}");
        foreach (var field in record.EnumerateField (951))
        {
            var url = field.FM ('i');
            if (string.IsNullOrEmpty (url) ||
                !url.Contains ("ebscohost"))
            {
                continue;
            }

            var match = _regex.Match (url);
            if (!match.Success)
            {
                continue;
            }

            var bookId = match.Value;
            var kind = PdfOrEpub (bookId);

            var coverUrl = kind == "pdf"
                ? $"http://elib.istu.edu/ebsco/pdf/{bookId}.jpg"
                : $"http://elib.istu.edu/ebsco/epub/{bookId}.jpg";
            record.Add (new Field (951)
            {
                new SubField ('h', "02a"),
                new SubField ('i', coverUrl)
            });

            var newUrl = kind == "pdf"
                ? $"http://elib.istu.edu/viewer.php?file=/ebsco/pdf/{bookId}.pdf"
                : $"http://elib.istu.edu/ebsco/epub/{bookId}.epub";
            field.SetSubFieldValue ('i', newUrl);
            field.Add ('h', "05");

            result = true;
        }

        return result;
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

                Console.WriteLine ("ALL DONE");
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
