// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

using ManagedIrbis.Epub.Environment;
using ManagedIrbis.Epub.Options;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Internal;

internal static class RootFilePathReader
{
    public static async Task<string> GetRootFilePathAsync(IZipFile epubFile, EpubReaderOptions epubReaderOptions)
    {
        const string EPUB_CONTAINER_FILE_PATH = "META-INF/container.xml";
        IZipFileEntry containerFileEntry = epubFile.GetEntry(EPUB_CONTAINER_FILE_PATH);
        if (containerFileEntry == null)
        {
            throw new EpubContainerException($"EPUB parsing error: {EPUB_CONTAINER_FILE_PATH} file not found in the EPUB file.");
        }
        XDocument containerDocument;
        using (Stream containerStream = containerFileEntry.Open())
        {
            containerDocument = await XmlUtils.LoadDocumentAsync(containerStream, epubReaderOptions.XmlReaderOptions).ConfigureAwait(false);
        }
        XNamespace cnsNamespace = "urn:oasis:names:tc:opendocument:xmlns:container";
        XAttribute fullPathAttribute = containerDocument.Element(cnsNamespace + "container")?.Element(cnsNamespace + "rootfiles")?.Element(cnsNamespace + "rootfile")?.Attribute("full-path");
        if (fullPathAttribute == null)
        {
            throw new EpubContainerException("EPUB parsing error: root file path not found in the EPUB container.");
        }
        return fullPathAttribute.Value;
    }
}