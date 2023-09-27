// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

using SharpCompress.Readers.Zip;

#endregion

namespace Epuber;

internal static class Program
{
    private static string? GetOpf
        (
            string fileName
        )
    {
        using var stream = File.OpenRead (fileName);
        using var reader = ZipReader.Open (stream);
        while (reader.MoveToNextEntry())
        {
            var entry = reader.Entry;
            if (!entry.IsDirectory)
            {
                var key = entry.Key;
                var extension = Path.GetExtension (key);
                if (string.Compare (extension, ".opf",
                        StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    var memory = new MemoryStream();
                    reader.WriteEntryTo (memory);
                    var bytes = memory.ToArray();
                    var content = Encoding.UTF8.GetString (bytes);
                    return content;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Извлечение элемента метаданных.
    /// </summary>
    private static string? ExtractItem
        (
            XElement metadata,
            string name
        )
    {
        var found = metadata.Descendants()
            .LastOrDefault (x => x.Name.LocalName == name);
        return found?.Value;
    }

    /// <summary>
    /// Извлечение элемента метаданных.
    /// </summary>
    private static string[] ExtractItems
        (
            XElement metadata,
            string name
        )
    {
        var found = metadata.Descendants()
            .Where (x => x.Name.LocalName == name)
            .Select (x => x.Value)
            .ToArray();

        return found;
    }

    private static Identifier[] ExtractIdentifiers
        (
            XElement metadata
        )
    {
        var found = metadata.Descendants()
            .Where (x => x.Name.LocalName == "identifier")
            .ToArray();

        return found.Select (x => new Identifier
            {
                Id = x.Attributes().Where (x => x.Name.LocalName == "id")
                    .Select (x => x.Value).FirstOrDefault(),
                Scheme = x.Attributes().Where (x => x.Name.LocalName == "scheme")
                    .Select (x => x.Value).FirstOrDefault(),
                Value = x.Value
            })
            .ToArray();
    }

    private static DublinCore? DecodeOpf
        (
            string? opf
        )
    {
        if (string.IsNullOrEmpty (opf))
        {
            return null;
        }

        var document = XDocument.Parse (opf);
        var metadata = document.Root?.Descendants()
            .FirstOrDefault (x => x.Name.LocalName == "metadata");
        if (metadata is null)
        {
            return null;
        }

        var result = new DublinCore
        {
            Identifiers = ExtractIdentifiers (metadata),
            Title = ExtractItem (metadata, "title"),
            Creators = ExtractItems (metadata, "creator"),
            Publisher = ExtractItem (metadata, "publisher"),
            Date = ExtractItem (metadata, "date"),
            Language = ExtractItem (metadata, "language"),
            Description = ExtractItem (metadata, "description"),
            Source = ExtractItem (metadata, "source"),
        };

        return result;
    }

    private static void ProcessFile
        (
            string fileName
        )
    {
        Console.WriteLine ($"File: {fileName}");

        var opfEntry = GetOpf (fileName);
        if (opfEntry is not null)
        {
            try
            {
                var dublinCore = DecodeOpf (opfEntry);
                if (dublinCore is not null)
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };
                    var serialized = JsonSerializer.Serialize (dublinCore, options);
                    Console.WriteLine (serialized);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine ($"Exception: {exception.Message}");
            }
        }
    }

    private static int Main (string[] args)
    {
        if (args.Length == 0)
        {
            return 1;
        }

        foreach (var file in Directory.EnumerateFiles (args[0]))
        {
            ProcessFile (file);
        }

        return 0;
    }
}
