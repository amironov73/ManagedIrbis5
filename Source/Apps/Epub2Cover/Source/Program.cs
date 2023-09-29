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
    Утилита для создания обложек для EPUB-файлов.
    Фактически вытаскивается картинка, помеченная в метаданных как cover.
 */

#region Using directives

using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using SharpCompress.Archives;
using SharpCompress.Archives.Zip;

#endregion

namespace Epub2Cover;

/// <summary>
/// Единственный класс, содержащий всю функциональность утилиты.
/// </summary>
internal sealed class Program
{
    private static string? ExtractMetadataPath
        (
            XDocument document
        )
    {
        var ns = document.Root?.Name.Namespace;
        if (ns is null)
        {
            return null;
        }

        var navigator = document.CreateNavigator();
        var table = navigator.NameTable;
        var manager = new XmlNamespaceManager (table);
        manager.AddNamespace ("epub", ns.ToString());

        var expression = "/epub:container/epub:rootfiles/epub:rootfile/@full-path";
        var attributes = ((IEnumerable) document.XPathEvaluate (expression, manager))
            .Cast<XAttribute>();

        var result = attributes.FirstOrDefault()?.Value;
        return result;
    }

    private static string? ExtractCoverPath
        (
            XDocument document
        )
    {
        var root = document.Root;
        if (root is null)
        {
            return null;
        }

        var opf = root.Name.Namespace;
        var metadata = root.Element (opf + "metadata");
        if (metadata is null)
        {
            return null;
        }

        var coverName = metadata.Elements (opf + "meta")
            .FirstOrDefault (x => x.Attribute ("name")?.Value == "cover")
            ?.Attribute ("content")?.Value;
        if (string.IsNullOrEmpty (coverName))
        {
            return null;
        }

        var manifest = root.Element (opf + "manifest");
        if (manifest is null)
        {
            return null;
        }

        var result = manifest.Elements (opf + "item")
            .FirstOrDefault (x => x.Attribute ("id")?.Value == coverName)
            ?.Attribute ("href")?.Value;
        return result;
    }

    private static XDocument? ExtractXmlFromArchive
        (
            ZipArchive archive,
            string fileName,
            Encoding? encoding = null
        )
    {
        var content = ExtractTextFile (archive, fileName, encoding);
        return string.IsNullOrEmpty (content)
            ? null
            : XDocument.Parse (content);
    }

    private static string? ExtractTextFile
        (
            ZipArchive archive,
            string fileName,
            Encoding? encoding = null
        )
    {
        var bytes = ExtractBinaryFile (archive, fileName);
        if (bytes is not null)
        {
            encoding ??= Encoding.UTF8;
            var result = encoding.GetString (bytes);
            return result;
        }

        return null;
    }

    private static byte[]? ExtractBinaryFile
        (
            ZipArchive archive,
            string fileName
        )
    {
        foreach (var entry in archive.Entries)
        {
            if (entry.Key == fileName)
            {
                var memory = new MemoryStream();
                entry.WriteTo (memory);
                var result = memory.ToArray();
                return result;
            }
        }

        return null;
    }

    private static string? ProcessEpub
        (
            string filename
        )
    {
        using var archive = ZipArchive.Open (filename);
        var container = ExtractXmlFromArchive (archive, "META-INF/container.xml");
        if (container is null)
        {
            return "No container file";
        }

        var metadataPath = ExtractMetadataPath (container);
        if (string.IsNullOrEmpty (metadataPath))
        {
            return "Bad container file";
        }

        var metadata = ExtractXmlFromArchive (archive, metadataPath);
        if (metadata is null)
        {
            return "No metadata file";
        }

        var coverPath = ExtractCoverPath (metadata);
        if (string.IsNullOrEmpty (coverPath))
        {
            return "Bad metadata file";
        }

        var fullCoverPath = Path.GetDirectoryName (metadataPath);
        if (string.IsNullOrEmpty (fullCoverPath))
        {
            fullCoverPath = coverPath;
        }
        else
        {
            fullCoverPath = fullCoverPath + "/" + coverPath;
        }

        var coverBytes = ExtractBinaryFile (archive, fullCoverPath);
        if (coverBytes is null)
        {
            return "Error extracting image file";
        }

        var imageName = Path.ChangeExtension (filename, ".jpg");
        File.WriteAllBytes (imageName, coverBytes);

        return null;
    }

    /// <summary>
    /// Собственно точка входа в программу.
    /// </summary>
    private static int Main
        (
            string[] args
        )
    {
        try
        {
            if (args.Length != 0)
            {
                var errorMessage = ProcessEpub (args[0]);
                if (!string.IsNullOrEmpty (errorMessage))
                {
                    Console.Error.WriteLine (errorMessage);
                    return 1;
                }
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
