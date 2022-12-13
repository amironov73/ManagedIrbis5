// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PackageReader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using ManagedIrbis.Epub.Environment;
using ManagedIrbis.Epub.Options;
using ManagedIrbis.Epub.Schema;
using ManagedIrbis.Epub.Utils;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Internal;

internal static class PackageReader
{
    public static async Task<EpubPackage> ReadPackageAsync
        (
            IZipFile epubFile,
            string rootFilePath,
            EpubReaderOptions epubReaderOptions
        )
    {
        var rootFileEntry = epubFile.GetEntry (rootFilePath);
        if (rootFileEntry == null)
        {
            throw new EpubContainerException ("EPUB parsing error: root file not found in the EPUB file.");
        }

        XDocument containerDocument;
        using (var containerStream = rootFileEntry.Open())
        {
            containerDocument = await XmlUtils.LoadDocumentAsync (containerStream, epubReaderOptions.XmlReaderOptions)
                .ConfigureAwait (false);
        }

        XNamespace opfNamespace = "http://www.idpf.org/2007/opf";
        var packageNode = containerDocument.Element (opfNamespace + "package");
        var result = new EpubPackage();
        var epubVersionValue = packageNode!.Attribute ("version")!.Value;
        EpubVersion epubVersion = epubVersionValue switch
        {
            "2.0" => EpubVersion.EPUB_2,
            "3.0" => EpubVersion.EPUB_3,
            "3.1" => EpubVersion.EPUB_3_1,
            _ => throw new EpubPackageException ($"Unsupported EPUB version: {epubVersionValue}.")
        };
        result.EpubVersion = epubVersion;
        var metadataNode = packageNode.Element (opfNamespace + "metadata");
        if (metadataNode == null)
        {
            throw new EpubPackageException ("EPUB parsing error: metadata not found in the package.");
        }

        var metadata = ReadMetadata (metadataNode);
        result.Metadata = metadata;
        var manifestNode = packageNode.Element (opfNamespace + "manifest");
        if (manifestNode == null)
        {
            throw new EpubPackageException ("EPUB parsing error: manifest not found in the package.");
        }

        var manifest = ReadManifest (manifestNode, epubReaderOptions.PackageReaderOptions);
        result.Manifest = manifest;
        var spineNode = packageNode.Element (opfNamespace + "spine");
        if (spineNode == null)
        {
            throw new EpubPackageException ("EPUB parsing error: spine not found in the package.");
        }

        var spine = ReadSpine (spineNode, epubVersion, epubReaderOptions.PackageReaderOptions);
        result.Spine = spine;
        var guideNode = packageNode.Element (opfNamespace + "guide");
        if (guideNode != null)
        {
            var guide = ReadGuide (guideNode);
            result.Guide = guide;
        }

        return result;
    }

    private static EpubMetadata ReadMetadata (XElement metadataNode)
    {
        var result = new EpubMetadata
        {
            Titles = new List<string>(),
            Creators = new List<EpubMetadataCreator>(),
            Subjects = new List<string>(),
            Publishers = new List<string>(),
            Contributors = new List<EpubMetadataContributor>(),
            Dates = new List<EpubMetadataDate>(),
            Types = new List<string>(),
            Formats = new List<string>(),
            Identifiers = new List<EpubMetadataIdentifier>(),
            Sources = new List<string>(),
            Languages = new List<string>(),
            Relations = new List<string>(),
            Coverages = new List<string>(),
            Rights = new List<string>(),
            MetaItems = new List<EpubMetadataMeta>()
        };
        foreach (var metadataItemNode in metadataNode.Elements())
        {
            var innerText = metadataItemNode.Value;
            switch (metadataItemNode.GetLowerCaseLocalName())
            {
                case "title":
                    result.Titles.Add (innerText);
                    break;

                case "creator":
                    var creator = ReadMetadataCreator (metadataItemNode);
                    result.Creators.Add (creator);
                    break;

                case "subject":
                    result.Subjects.Add (innerText);
                    break;

                case "description":
                    result.Description = innerText;
                    break;

                case "publisher":
                    result.Publishers.Add (innerText);
                    break;

                case "contributor":
                    var contributor = ReadMetadataContributor (metadataItemNode);
                    result.Contributors.Add (contributor);
                    break;

                case "date":
                    var date = ReadMetadataDate (metadataItemNode);
                    result.Dates.Add (date);
                    break;

                case "type":
                    result.Types.Add (innerText);
                    break;

                case "format":
                    result.Formats.Add (innerText);
                    break;

                case "identifier":
                    var identifier = ReadMetadataIdentifier (metadataItemNode);
                    result.Identifiers.Add (identifier);
                    break;

                case "source":
                    result.Sources.Add (innerText);
                    break;

                case "language":
                    result.Languages.Add (innerText);
                    break;

                case "relation":
                    result.Relations.Add (innerText);
                    break;

                case "coverage":
                    result.Coverages.Add (innerText);
                    break;
                case "rights":
                    result.Rights.Add (innerText);
                    break;

                case "meta":
                    var meta = ReadMetadataMeta (metadataItemNode);
                    result.MetaItems.Add (meta);
                    break;
            }
        }

        return result;
    }

    private static EpubMetadataCreator ReadMetadataCreator
        (
            XElement metadataCreatorNode
        )
    {
        var result = new EpubMetadataCreator();
        foreach (var metadataCreatorNodeAttribute in metadataCreatorNode.Attributes())
        {
            var attributeValue = metadataCreatorNodeAttribute.Value;
            switch (metadataCreatorNodeAttribute.GetLowerCaseLocalName())
            {
                case "id":
                    result.Id = attributeValue;
                    break;

                case "role":
                    result.Role = attributeValue;
                    break;

                case "file-as":
                    result.FileAs = attributeValue;
                    break;
            }
        }

        result.Creator = metadataCreatorNode.Value;
        return result;
    }

    private static EpubMetadataContributor ReadMetadataContributor (XElement metadataContributorNode)
    {
        var result = new EpubMetadataContributor();
        foreach (var metadataContributorNodeAttribute in metadataContributorNode.Attributes())
        {
            var attributeValue = metadataContributorNodeAttribute.Value;
            switch (metadataContributorNodeAttribute.GetLowerCaseLocalName())
            {
                case "id":
                    result.Id = attributeValue;
                    break;
                case "role":
                    result.Role = attributeValue;
                    break;
                case "file-as":
                    result.FileAs = attributeValue;
                    break;
            }
        }

        result.Contributor = metadataContributorNode.Value;
        return result;
    }

    private static EpubMetadataDate ReadMetadataDate (XElement metadataDateNode)
    {
        var result = new EpubMetadataDate();
        var eventAttribute = metadataDateNode.Attribute (metadataDateNode.Name.Namespace + "event");
        if (eventAttribute != null)
        {
            result.Event = eventAttribute.Value;
        }

        result.Date = metadataDateNode.Value;
        return result;
    }

    private static EpubMetadataIdentifier ReadMetadataIdentifier (XElement metadataIdentifierNode)
    {
        var result = new EpubMetadataIdentifier();
        foreach (var metadataIdentifierNodeAttribute in metadataIdentifierNode.Attributes())
        {
            var attributeValue = metadataIdentifierNodeAttribute.Value;
            switch (metadataIdentifierNodeAttribute.GetLowerCaseLocalName())
            {
                case "id":
                    result.Id = attributeValue;
                    break;
                case "opf:scheme":
                    result.Scheme = attributeValue;
                    break;
            }
        }

        result.Identifier = metadataIdentifierNode.Value;
        return result;
    }

    private static EpubMetadataMeta ReadMetadataMeta
        (
            XElement metadataMetaNode
        )
    {
        var result = new EpubMetadataMeta();
        foreach (var metadataMetaNodeAttribute in metadataMetaNode.Attributes())
        {
            var attributeValue = metadataMetaNodeAttribute.Value;
            switch (metadataMetaNodeAttribute.GetLowerCaseLocalName())
            {
                case "name":
                    result.Name = attributeValue;
                    break;
                case "content":
                    result.Content = attributeValue;
                    break;
                case "id":
                    result.Id = attributeValue;
                    break;
                case "refines":
                    result.Refines = attributeValue;
                    break;
                case "property":
                    result.Property = attributeValue;
                    break;
                case "scheme":
                    result.Scheme = attributeValue;
                    break;
            }
        }

        result.Content ??= metadataMetaNode.Value;

        return result;
    }

    private static EpubManifest ReadManifest
        (
            XElement manifestNode,
            PackageReaderOptions packageReaderOptions
        )
    {
        var result = new EpubManifest();
        foreach (var manifestItemNode in manifestNode.Elements())
        {
            if (manifestItemNode.CompareNameTo ("item"))
            {
                var manifestItem = new EpubManifestItem();
                foreach (var manifestItemNodeAttribute in manifestItemNode.Attributes())
                {
                    var attributeValue = manifestItemNodeAttribute.Value;
                    switch (manifestItemNodeAttribute.GetLowerCaseLocalName())
                    {
                        case "id":
                            manifestItem.Id = attributeValue;
                            break;
                        case "href":
                            manifestItem.Href = Uri.UnescapeDataString (attributeValue);
                            break;
                        case "media-type":
                            manifestItem.MediaType = attributeValue;
                            break;
                        case "required-namespace":
                            manifestItem.RequiredNamespace = attributeValue;
                            break;
                        case "required-modules":
                            manifestItem.RequiredModules = attributeValue;
                            break;
                        case "fallback":
                            manifestItem.Fallback = attributeValue;
                            break;
                        case "fallback-style":
                            manifestItem.FallbackStyle = attributeValue;
                            break;
                        case "properties":
                            manifestItem.Properties = ReadManifestProperties (attributeValue);
                            break;
                    }
                }

                if (string.IsNullOrWhiteSpace (manifestItem.Id))
                {
                    if (packageReaderOptions.SkipInvalidManifestItems)
                    {
                        continue;
                    }

                    throw new EpubPackageException ("Incorrect EPUB manifest: item ID is missing");
                }

                if (string.IsNullOrWhiteSpace (manifestItem.Href))
                {
                    if (packageReaderOptions.SkipInvalidManifestItems)
                    {
                        continue;
                    }

                    throw new EpubPackageException ("Incorrect EPUB manifest: item href is missing");
                }

                if (string.IsNullOrWhiteSpace (manifestItem.MediaType))
                {
                    if (packageReaderOptions.SkipInvalidManifestItems)
                    {
                        continue;
                    }

                    throw new EpubPackageException ("Incorrect EPUB manifest: item media type is missing");
                }

                result.Add (manifestItem);
            }
        }

        return result;
    }

    private static List<ManifestProperty> ReadManifestProperties (string propertiesAttributeValue)
    {
        var result = new List<ManifestProperty>();
        foreach (var propertyStringValue in propertiesAttributeValue.Split (new[] { ' ' },
                     StringSplitOptions.RemoveEmptyEntries))
        {
            result.Add (ManifestPropertyParser.Parse (propertyStringValue));
        }

        return result;
    }

    private static EpubSpine ReadSpine (XElement spineNode, EpubVersion epubVersion,
        PackageReaderOptions packageReaderOptions)
    {
        var result = new EpubSpine();
        foreach (var spineNodeAttribute in spineNode.Attributes())
        {
            var attributeValue = spineNodeAttribute.Value;
            switch (spineNodeAttribute.GetLowerCaseLocalName())
            {
                case "id":
                    result.Id = attributeValue;
                    break;
                case "page-progression-direction":
                    result.PageProgressionDirection = PageProgressionDirectionParser.Parse (attributeValue);
                    break;
                case "toc":
                    result.Toc = attributeValue;
                    break;
            }
        }

        if (epubVersion == EpubVersion.EPUB_2 && string.IsNullOrWhiteSpace (result.Toc) &&
            !packageReaderOptions.IgnoreMissingToc)
        {
            throw new EpubPackageException ("Incorrect EPUB spine: TOC is missing");
        }

        foreach (var spineItemNode in spineNode.Elements())
        {
            if (spineItemNode.CompareNameTo ("itemref"))
            {
                var spineItemRef = new EpubSpineItemRef();
                foreach (var spineItemNodeAttribute in spineItemNode.Attributes())
                {
                    var attributeValue = spineItemNodeAttribute.Value;
                    switch (spineItemNodeAttribute.GetLowerCaseLocalName())
                    {
                        case "id":
                            spineItemRef.Id = attributeValue;
                            break;
                        case "idref":
                            spineItemRef.IdRef = attributeValue;
                            break;
                        case "properties":
                            spineItemRef.Properties = SpinePropertyParser.ParsePropertyList (attributeValue);
                            break;
                    }
                }

                if (string.IsNullOrWhiteSpace (spineItemRef.IdRef))
                {
                    throw new EpubPackageException ("Incorrect EPUB spine: item ID ref is missing");
                }

                var linearAttribute = spineItemNode.Attribute ("linear");
                spineItemRef.IsLinear = linearAttribute == null || !linearAttribute.CompareValueTo ("no");
                result.Add (spineItemRef);
            }
        }

        return result;
    }

    private static EpubGuide ReadGuide (XElement guideNode)
    {
        var result = new EpubGuide();
        foreach (var guideReferenceNode in guideNode.Elements())
        {
            if (guideReferenceNode.CompareNameTo ("reference"))
            {
                var guideReference = new EpubGuideReference();
                foreach (var guideReferenceNodeAttribute in guideReferenceNode.Attributes())
                {
                    var attributeValue = guideReferenceNodeAttribute.Value;
                    switch (guideReferenceNodeAttribute.GetLowerCaseLocalName())
                    {
                        case "type":
                            guideReference.Type = attributeValue;
                            break;
                        case "title":
                            guideReference.Title = attributeValue;
                            break;
                        case "href":
                            guideReference.Href = Uri.UnescapeDataString (attributeValue);
                            break;
                    }
                }

                if (string.IsNullOrWhiteSpace (guideReference.Type))
                {
                    throw new EpubPackageException ("Incorrect EPUB guide: item type is missing");
                }

                if (string.IsNullOrWhiteSpace (guideReference.Href))
                {
                    throw new EpubPackageException ("Incorrect EPUB guide: item href is missing");
                }

                result.Add (guideReference);
            }
        }

        return result;
    }
}
