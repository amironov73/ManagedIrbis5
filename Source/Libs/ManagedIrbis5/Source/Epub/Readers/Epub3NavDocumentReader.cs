// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Epub3NavDocumentReader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using ManagedIrbis.Epub.Environment;
using ManagedIrbis.Epub.Options;
using ManagedIrbis.Epub.Schema;
using ManagedIrbis.Epub.Utils;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Internal;

internal static class Epub3NavDocumentReader
{
    public static async Task<Epub3NavDocument?> ReadEpub3NavDocumentAsync
        (
            IZipFile epubFile,
            string contentDirectoryPath,
            EpubPackage package,
            EpubReaderOptions epubReaderOptions
        )
    {
        var result = new Epub3NavDocument();
        var navManifestItem = package.Manifest!.FirstOrDefault
            (
                item => item.Properties != null!
                        && item.Properties.Contains (ManifestProperty.NAV)
            );
        if (navManifestItem == null)
        {
            if (package.EpubVersion == EpubVersion.EPUB_2)
            {
                return null;
            }
            else
            {
                throw new Epub3NavException ("EPUB parsing error: NAV item not found in EPUB manifest.");
            }
        }

        var navFileEntryPath = ZipPathUtils.Combine (contentDirectoryPath, navManifestItem.Href!);
        var navFileEntry = epubFile.GetEntry (navFileEntryPath);
        if (navFileEntry == null)
        {
            throw new Epub3NavException
                (
                    $"EPUB parsing error: navigation file {navFileEntryPath} not found in the EPUB file."
                );
        }

        if (navFileEntry.Length > int.MaxValue)
        {
            throw new Epub3NavException
                (
                    $"EPUB parsing error: navigation file {navFileEntryPath} is larger than 2 GB."
                );
        }

        XDocument navDocument;
        using (var containerStream = navFileEntry.Open())
        {
            navDocument = await XmlUtils.LoadDocumentAsync (containerStream, epubReaderOptions.XmlReaderOptions)
                .ConfigureAwait (false);
        }

        var xhtmlNamespace = navDocument.Root!.Name.Namespace;
        var htmlNode = navDocument.Element (xhtmlNamespace + "html");
        if (htmlNode == null)
        {
            throw new Epub3NavException ("EPUB parsing error: navigation file does not contain html element.");
        }

        var bodyNode = htmlNode.Element (xhtmlNamespace + "body");
        if (bodyNode == null)
        {
            throw new Epub3NavException ("EPUB parsing error: navigation file does not contain body element.");
        }

        result.Navs = new List<Epub3Nav>();
        foreach (var navNode in bodyNode.Elements (xhtmlNamespace + "nav"))
        {
            var epub3Nav = ReadEpub3Nav (navNode);
            result.Navs.Add (epub3Nav);
        }

        return result;
    }

    private static Epub3Nav ReadEpub3Nav (XElement navNode)
    {
        var epub3Nav = new Epub3Nav();
        foreach (var navNodeAttribute in navNode.Attributes())
        {
            var attributeValue = navNodeAttribute.Value;
            switch (navNodeAttribute.GetLowerCaseLocalName())
            {
                case "type":
                    epub3Nav.Type = StructuralSemanticsPropertyParser.Parse (attributeValue);
                    break;

                case "hidden":
                    epub3Nav.IsHidden = true;
                    break;
            }
        }

        foreach (var navChildNode in navNode.Elements())
        {
            switch (navChildNode.GetLowerCaseLocalName())
            {
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                    epub3Nav.Head = navChildNode.Value.Trim();
                    break;

                case "ol":
                    var epub3NavOl = ReadEpub3NavOl (navChildNode);
                    epub3Nav.Ol = epub3NavOl;
                    break;
            }
        }

        return epub3Nav;
    }

    private static Epub3NavOl ReadEpub3NavOl (XElement epub3NavOlNode)
    {
        var epub3NavOl = new Epub3NavOl();
        foreach (var navOlNodeAttribute in epub3NavOlNode.Attributes())
        {
            switch (navOlNodeAttribute.GetLowerCaseLocalName())
            {
                case "hidden":
                    epub3NavOl.IsHidden = true;
                    break;
            }
        }

        epub3NavOl.Lis = new List<Epub3NavLi>();
        foreach (var navOlChildNode in epub3NavOlNode.Elements())
        {
            switch (navOlChildNode.GetLowerCaseLocalName())
            {
                case "li":
                    var epub3NavLi = ReadEpub3NavLi (navOlChildNode);
                    epub3NavOl.Lis.Add (epub3NavLi);
                    break;
            }
        }

        return epub3NavOl;
    }

    private static Epub3NavLi ReadEpub3NavLi (XElement epub3NavLiNode)
    {
        var epub3NavLi = new Epub3NavLi();
        foreach (var navLiChildNode in epub3NavLiNode.Elements())
        {
            switch (navLiChildNode.GetLowerCaseLocalName())
            {
                case "a":
                    var epub3NavAnchor = ReadEpub3NavAnchor (navLiChildNode);
                    epub3NavLi.Anchor = epub3NavAnchor;
                    break;

                case "span":
                    var epub3NavSpan = ReadEpub3NavSpan (navLiChildNode);
                    epub3NavLi.Span = epub3NavSpan;
                    break;

                case "ol":
                    var epub3NavOl = ReadEpub3NavOl (navLiChildNode);
                    epub3NavLi.ChildOl = epub3NavOl;
                    break;
            }
        }

        return epub3NavLi;
    }

    private static Epub3NavAnchor ReadEpub3NavAnchor (XElement epub3NavAnchorNode)
    {
        var epub3NavAnchor = new Epub3NavAnchor();
        foreach (var navAnchorNodeAttribute in epub3NavAnchorNode.Attributes())
        {
            var attributeValue = navAnchorNodeAttribute.Value;
            switch (navAnchorNodeAttribute.GetLowerCaseLocalName())
            {
                case "href":
                    epub3NavAnchor.Href = attributeValue;
                    break;

                case "title":
                    epub3NavAnchor.Title = attributeValue;
                    break;

                case "alt":
                    epub3NavAnchor.Alt = attributeValue;
                    break;

                case "type":
                    epub3NavAnchor.Type = StructuralSemanticsPropertyParser.Parse (attributeValue);
                    break;
            }
        }

        epub3NavAnchor.Text = epub3NavAnchorNode.Value.Trim();
        return epub3NavAnchor;
    }

    private static Epub3NavSpan ReadEpub3NavSpan (XElement epub3NavSpanNode)
    {
        var epub3NavSpan = new Epub3NavSpan();
        foreach (var navSpanNodeAttribute in epub3NavSpanNode.Attributes())
        {
            var attributeValue = navSpanNodeAttribute.Value;
            switch (navSpanNodeAttribute.GetLowerCaseLocalName())
            {
                case "title":
                    epub3NavSpan.Title = attributeValue;
                    break;

                case "alt":
                    epub3NavSpan.Alt = attributeValue;
                    break;
            }
        }

        epub3NavSpan.Text = epub3NavSpanNode.Value.Trim();
        return epub3NavSpan;
    }
}
