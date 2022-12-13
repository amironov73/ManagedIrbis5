// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubReaderOptions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Epub.Options;

/// <summary>
/// Various options to configure the behavior of the EPUB reader.
/// </summary>
public class EpubReaderOptions
{
    /// <summary>
    ///
    /// </summary>
    public EpubReaderOptions()
    {
        PackageReaderOptions = new PackageReaderOptions();
        Epub2NcxReaderOptions = new Epub2NcxReaderOptions();
        XmlReaderOptions = new XmlReaderOptions();
    }

    /// <summary>
    /// Gets or sets EPUB package reader options.
    /// </summary>
    public PackageReaderOptions PackageReaderOptions { get; set; }

    /// <summary>
    /// Gets or sets EPUB 2 NCX (Navigation Center eXtended) reader options.
    /// </summary>
    public Epub2NcxReaderOptions Epub2NcxReaderOptions { get; set; }

    /// <summary>
    /// Gets or sets XML reader options.
    /// </summary>
    public XmlReaderOptions XmlReaderOptions { get; set; }
}
