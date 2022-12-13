// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Epub.Options;

/// <summary>
/// Various options to configure how EPUB reader handles XML files.
/// </summary>
public class XmlReaderOptions
{
    /// <summary>
    ///
    /// </summary>
    public XmlReaderOptions()
    {
        SkipXmlHeaders = false;
    }

    /// <summary>
    /// Gets or sets a value indicating whether XML reader should skip XML headers for all schema files before attempting to read them.
    /// This is a workaround for handling XML 1.1 files due to the lack of their support in .NET (only XML 1.0 files are currently supported).
    /// If this property is set to true, XML reader will check if an XML file contains a declaration (&lt;?xml version="..." encoding="UTF-8"?&gt;)
    /// in which case the reader will skip it before passing the file to the underlying .NET XDocument class. This lets the library to handle
    /// EPUB files containing XML 1.1 files at the expense of an additional processing overhead for every schema file inside the EPUB file.
    /// If this property is set to false, then there is no overhead for processing XML files. However in an attempt to open an EPUB with an XML 1.1
    /// file, an XmlException "Version number '1.1' is invalid" will be thrown.
    /// Default value is false.
    /// </summary>
    public bool SkipXmlHeaders { get; set; }
}
