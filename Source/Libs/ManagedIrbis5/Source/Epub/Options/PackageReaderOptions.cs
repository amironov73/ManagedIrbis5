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

#nullable enable

namespace ManagedIrbis.Epub.Options;

/// <summary>
/// Various options to configure the behavior of the EPUB package reader.
/// </summary>
public class PackageReaderOptions
{
    public PackageReaderOptions()
    {
        IgnoreMissingToc = false;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the package reader should ignore missing TOC attribute in the EPUB 2 spine.
    /// If it's set to false and the TOC attribute is not present, then the "Incorrect EPUB spine: TOC is missing" exception will be thrown.
    /// Default value is false.
    /// </summary>
    public bool IgnoreMissingToc { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the package reader should skip EPUB manifest items that are missing required attributes (id, href, or media-type).
    /// If it's set to false and one of the required attributes is not present, then one of the "Incorrect EPUB manifest: item ... is missing" exceptions will be thrown.
    /// Default value is false.
    /// </summary>
    public bool SkipInvalidManifestItems { get; set; }
}