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
/// Various options to configure the behavior of the EPUB 2 NCX (Navigation Center eXtended) reader.
/// </summary>
public class Epub2NcxReaderOptions
{
    public Epub2NcxReaderOptions()
    {
        IgnoreMissingContentForNavigationPoints = false;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the EPUB 2 NCX reader should ignore missing content attribute on the navigation points in the NCX.
    /// If it's set to false and the content attribute is not present, then the "navigation point X should contain at least one navigation label" exception will be thrown.
    /// Otherwise all navigation points without the content attribute and all their child navigation points will be excluded from the navigation map.
    /// Default value is false.
    /// </summary>
    public bool IgnoreMissingContentForNavigationPoints { get; set; }
}