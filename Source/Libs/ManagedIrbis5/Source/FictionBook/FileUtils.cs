// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* FileUtils.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;

using System.Xml.Schema;
using System.Xml.XPath;

using AM;

using Exception = System.Exception;

#endregion

namespace ManagedIrbis.FictionBook;

internal sealed class FB2EncoderFallbackBuffer
    : EncoderFallbackBuffer
{
    #region FB2EncoderFallbackBuffer

    // Store our fallback string
    private string strFallback = string.Empty;
    int fallbackCount = -1;

    int fallbackIndex = -1;

    // Fallback Methods
    public override bool Fallback (char charUnknown, int index)
    {
        // If we had a buffer already we're being recursive, throw, it's probably at the suspect
        // character in our array.
        if (fallbackCount >= 1)

            // Presumably you'd want a prettier exception:
        {
            throw new Exception ("Recursive Fallback Exception");
        }

        // Go ahead and get our fallback
        strFallback = $"&#{(int)charUnknown};";
        fallbackCount = strFallback.Length;
        fallbackIndex = -1;

        return fallbackCount != 0;
    }

    public override bool Fallback (char charUnknownHigh, char charUnknownLow, int index)
    {
        // In this example, we didn't really expect surrogates.

        // If we had a buffer already we're being recursive, throw, it's probably at the suspect
        // character in our array.
        if (fallbackCount >= 1)

            // Presumably you'd want a prettier exception:
        {
            throw new Exception ("Recursive Fallback Exception");
        }

        // Go ahead and get our fallback
        // Note that we're doing this 2X, once for each char.  That won't effect the
        // EncoderNumberFallback.MaxCharCount though because it is counting per char,
        // and although we're 2X that here, we also have 2x chars.
        strFallback = $"&#{(int)charUnknownHigh};&#{(int)charUnknownLow};";
        fallbackCount = strFallback.Length;
        fallbackIndex = -1;

        return fallbackCount != 0;
    }

    public override char GetNextChar()
    {
        // We want it to get < 0 because == 0 means that the current/last character is a fallback
        // and we need to detect recursion.  We could have a flag but we already have this counter.
        fallbackCount--;
        fallbackIndex++;

        // Do we have anything left? 0 is now last fallback char, negative is nothing left
        if (fallbackCount < 0)
        {
            return (char)0;
        }

        // Need to get it out of the buffer.
        return strFallback[fallbackIndex];
    }

    public override bool MovePrevious()
    {
        // Back up one, only if we just processed the last character (or earlier)
        if (fallbackCount >= -1 && fallbackIndex >= 0)
        {
            fallbackIndex--;
            fallbackCount++;
            return true;
        }

        // Return false 'cause we couldn't do it.
        return false;
    }

    // How many characters left to output?
    public override int Remaining =>

        // Our count is 0 for 1 character left.
        (fallbackCount < 0) ? 0 : fallbackCount;

    #endregion
}

/// <summary>
///
/// </summary>
public class FBEncoderFallback
    : EncoderFallback
{
    #region FBEncoderFallback

    /// <inheritdoc cref="EncoderFallback.CreateFallbackBuffer"/>
    public override EncoderFallbackBuffer CreateFallbackBuffer()
    {
        return new FB2EncoderFallbackBuffer();
    }

    /// <inheritdoc cref="EncoderFallback.MaxCharCount"/>
    public override int MaxCharCount => 8;

    #endregion
}

internal static class NativeMethods
{
    #region Private

    private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr (-1);

    [StructLayout (LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct WIN32_FIND_DATA
    {
        private uint dwFileAttributes;
        private System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
        private System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
        private System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
        private uint nFileSizeHigh;
        private uint nFileSizeLow;
        private uint dwReserved0;
        private uint dwReserved1;

        [MarshalAs (UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;

        [MarshalAs (UnmanagedType.ByValTStr, SizeConst = 14)]
        private string cAlternateFileName;
    }

    [DllImport ("kernel32.dll", CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
    private static extern IntPtr FindFirstFile (string lpFileName, out WIN32_FIND_DATA lpFindFileData);

    [DllImport ("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern bool FindNextFile (IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

    [DllImport ("kernel32.dll")]
    private static extern bool FindClose (IntPtr hFindFile);

    #endregion

    public static bool CheckDirectoryEmpty_Fast (string path)
    {
        if (string.IsNullOrEmpty (path))
        {
            throw new ArgumentNullException (path);
        }

        if (Directory.Exists (path))
        {
            if (path.EndsWith (Path.DirectorySeparatorChar.ToString()))
            {
                path += "*";
            }
            else
            {
                path += Path.DirectorySeparatorChar + "*";
            }

            var findHandle = FindFirstFile (path, out var findData);

            if (findHandle != INVALID_HANDLE_VALUE)
            {
                try
                {
                    var empty = true;
                    do
                    {
                        if (findData.cFileName != "." && findData.cFileName != "..")
                        {
                            empty = false;
                        }
                    } while (empty && FindNextFile (findHandle, out findData));

                    return empty;
                }
                finally
                {
                    FindClose (findHandle);
                }
            }

            throw new Exception ("Failed to get directory first file");
        }

        throw new DirectoryNotFoundException();
    }
}

/// <summary>
///
/// </summary>
public class FileProperties
{
    /// <summary>
    ///
    /// </summary>
    public bool AuthorFirstNameChange { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? AuthorFirstName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool AuthorLastNameChange { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? AuthorLastName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool AuthorMiddleNameChange { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? AuthorMiddleName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool GengeChange { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Genre { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool SeriesChange { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Series { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool NumberChange { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool TitleChange { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Title { get; set; }
}

/// <summary>
///
/// </summary>
public class FileOperationResult
{
    /// <summary>
    ///
    /// </summary>
    public string? NewFullName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? NewFileName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool Skipped { get; set; }
}

/// <summary>
///
/// </summary>
public enum DescriptionElements
{
    #region DescriptionElements elements

    /// <summary>
    ///
    /// </summary>
    Genre,

    /// <summary>
    ///
    /// </summary>
    Title,

    /// <summary>
    ///
    /// </summary>
    SequenceName,

    /// <summary>
    ///
    /// </summary>
    SequenceNr,

    /// <summary>
    ///
    /// </summary>
    Version,

    /// <summary>
    ///
    /// </summary>
    AuthorFirstName,

    /// <summary>
    ///
    /// </summary>
    AuthorMiddleName,

    /// <summary>
    ///
    /// </summary>
    AuthorLastName,

    /// <summary>
    ///
    /// </summary>
    AuthorFirstName1,

    /// <summary>
    ///
    /// </summary>
    AuthorMiddleName1,

    /// <summary>
    ///
    /// </summary>
    AuthorLastName1,

    /// <summary>
    ///
    /// </summary>
    TranslatorFirstName,

    /// <summary>
    ///
    /// </summary>
    TranslatorMiddleName,

    /// <summary>
    ///
    /// </summary>
    TranslatorLastName,

    /// <summary>
    ///
    /// </summary>
    TranslatorFirstName1,

    /// <summary>
    ///
    /// </summary>
    TranslatorMiddleName1,

    /// <summary>
    ///
    /// </summary>
    TranslatorLastName1,

    /// <summary>
    ///
    /// </summary>
    Lang

    #endregion
}

/// <summary>
///
/// </summary>
public class FileMetadata
{
    #region Private

    private readonly Dictionary<string, string> _metadataItems = new ();
    private string _description = string.Empty;
    private bool _initialized;

    #endregion

    private void InternalAddMetadata
        (
            string key,
            string value
        )
    {
        var _key = key;
        var _value = string.IsNullOrEmpty (value) ? string.Empty : value.Trim();
        if (Metadata.ContainsKey (_key))
        {
            Metadata[_key] = _value;
        }
        else
        {
            Metadata.Add (_key, _value);
        }
    }

    private Dictionary<string, string> Metadata
    {
        get
        {
            if (_metadataItems.Count == 0)
            {
                InternalInitialize();
            }

            return _metadataItems;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="description"></param>
    protected void SetDescription (string description)
    {
        _description = description;
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void InternalInitialize()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;
        var elements = Enum.GetValues (typeof (DescriptionElements)) as DescriptionElements[];
        foreach (var element in elements!)
        {
            AddMetadata (element, string.Empty);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="description"></param>
    protected virtual void InternalParseDescription (string description)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="description"></param>
    protected void ParseDescription (string description)
    {
        InternalParseDescription (description);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    protected virtual bool CheckRequiredAttribute (string part)
    {
        foreach (var item in Metadata)
        {
            if (part.Contains ($"({item.Key})") || part.Contains ($"[{item.Key}]"))
            {
                if (string.IsNullOrEmpty (item.Value))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    protected string NormalizeString (string s)
    {
        if (string.IsNullOrEmpty (s))
        {
            return string.Empty;
        }

        var a = s.ToLower().ToCharArray();
        a[0] = char.ToUpper (a[0]);
        return new string (a);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public void AddMetadata
        (
            DescriptionElements key,
            int index,
            string value
        )
    {
        var name = Enum.GetName (typeof (DescriptionElements), key);
        if (index == 0)
        {
            InternalAddMetadata (name!, value);
        }

        name = name + Convert.ToString (index + 1);
        InternalAddMetadata (name, value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddMetadata (DescriptionElements key, string value)
    {
        var name = Enum.GetName (typeof (DescriptionElements), key);
        InternalAddMetadata (name!, value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetMetadata (DescriptionElements key)
    {
        var name = Enum.GetName (typeof (DescriptionElements), key);
        var _key = $"{name}";
        return Metadata[_key];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public string SubstitutePart (string part)
    {
        if (CheckRequiredAttribute (part))
        {
            foreach (var item in Metadata)
            {
                part = part.Replace ($"({item.Key})", item.Value);
                part = part.Replace ($"[{item.Key}]", string.Empty);
            }

            return part;
        }

        return string.Empty;
    }

    /// <summary>
    ///
    /// </summary>
    public string Description => _description;
}

/// <summary>
///
/// </summary>
public class FB2Metadata
    : FileMetadata
{
    #region Private

    private int bookTitleAuthor;
    private int bookTitleTranslator;
    private int bookTitleGenre;

    #endregion

    private void ParseBookTitleAuthor
        (
            string author
        )
    {
        using (var reader = XmlReader.Create (new StringReader (author)))
        {
            while (reader.Read())
            {
                if (reader is { NodeType: XmlNodeType.Element, Name: "first-name" })
                {
                    var bookAuthorFirstName = reader.ReadString();
                    if (FB2Config.Current.NormalizeNames)
                    {
                        bookAuthorFirstName = NormalizeString (bookAuthorFirstName);
                    }

                    AddMetadata (DescriptionElements.AuthorFirstName, bookTitleAuthor, bookAuthorFirstName);
                    AddMetadata (DescriptionElements.AuthorFirstName1, bookTitleAuthor,
                        string.IsNullOrEmpty (bookAuthorFirstName)
                            ? string.Empty
                            : bookAuthorFirstName[0].ToString());
                }
                else if (reader is { NodeType: XmlNodeType.Element, Name: "last-name" })
                {
                    var bookAuthorLastName = reader.ReadString();
                    if (FB2Config.Current.NormalizeNames)
                    {
                        bookAuthorLastName = NormalizeString (bookAuthorLastName);
                    }

                    AddMetadata (DescriptionElements.AuthorLastName, bookTitleAuthor, bookAuthorLastName);
                    AddMetadata (DescriptionElements.AuthorLastName1, bookTitleAuthor,
                        string.IsNullOrEmpty (bookAuthorLastName)
                            ? string.Empty
                            : bookAuthorLastName[0].ToString());
                }
                else if (reader is { NodeType: XmlNodeType.Element, Name: "middle-name" })
                {
                    var bookAuthorMiddleName = reader.ReadString();
                    if (FB2Config.Current.NormalizeNames)
                    {
                        bookAuthorMiddleName = NormalizeString (bookAuthorMiddleName);
                    }

                    AddMetadata (DescriptionElements.AuthorMiddleName, bookTitleAuthor, bookAuthorMiddleName);
                    AddMetadata (DescriptionElements.AuthorMiddleName1, bookTitleAuthor,
                        string.IsNullOrEmpty (bookAuthorMiddleName)
                            ? string.Empty
                            : bookAuthorMiddleName[0].ToString());
                }
            }
        }
    }

    private void ParseBookTitleTranslator
        (
            string translator
        )
    {
        using (var reader = XmlReader.Create (new StringReader (translator)))
        {
            while (reader.Read())
            {
                if (reader is { NodeType: XmlNodeType.Element, Name: "first-name" })
                {
                    var bookTranslatorFirstName = reader.ReadString();
                    if (FB2Config.Current.NormalizeNames)
                    {
                        bookTranslatorFirstName = NormalizeString (bookTranslatorFirstName);
                    }

                    AddMetadata (DescriptionElements.TranslatorFirstName, bookTitleTranslator,
                        bookTranslatorFirstName);
                    AddMetadata (DescriptionElements.TranslatorFirstName1, bookTitleTranslator,
                        string.IsNullOrEmpty (bookTranslatorFirstName)
                            ? string.Empty
                            : bookTranslatorFirstName[0].ToString());
                }
                else if (reader is { NodeType: XmlNodeType.Element, Name: "last-name" })
                {
                    var bookTranslatorLastName = reader.ReadString();
                    if (FB2Config.Current.NormalizeNames)
                    {
                        bookTranslatorLastName = NormalizeString (bookTranslatorLastName);
                    }

                    AddMetadata (DescriptionElements.TranslatorLastName, bookTitleTranslator,
                        bookTranslatorLastName);
                    AddMetadata (DescriptionElements.TranslatorLastName1, bookTitleTranslator,
                        string.IsNullOrEmpty (bookTranslatorLastName)
                            ? string.Empty
                            : bookTranslatorLastName[0].ToString());
                }
                else if (reader is { NodeType: XmlNodeType.Element, Name: "middle-name" })
                {
                    var bookTranslatorMiddleName = reader.ReadString();
                    if (FB2Config.Current.NormalizeNames)
                    {
                        bookTranslatorMiddleName = NormalizeString (bookTranslatorMiddleName);
                    }

                    AddMetadata (DescriptionElements.TranslatorMiddleName, bookTitleTranslator,
                        bookTranslatorMiddleName);
                    AddMetadata (DescriptionElements.TranslatorMiddleName1, bookTitleTranslator,
                        string.IsNullOrEmpty (bookTranslatorMiddleName)
                            ? string.Empty
                            : bookTranslatorMiddleName[0].ToString());
                }
            }
        }
    }

    private void ParseTitleInfo (string titleInfo)
    {
        using (var reader = XmlReader.Create (new StringReader (titleInfo)))
        {
            while (reader.Read())
            {
                if (reader is { NodeType: XmlNodeType.Element, Name: "author" })
                {
                    var author = reader.ReadOuterXml();
                    author = author.Trim();
                    ParseBookTitleAuthor (author);
                    bookTitleAuthor++;
                }

                if (reader is { NodeType: XmlNodeType.Element, Name: "translator" })
                {
                    var translator = reader.ReadOuterXml();
                    translator = translator.Trim();
                    ParseBookTitleTranslator (translator);
                    bookTitleTranslator++;
                }

                if (reader is { NodeType: XmlNodeType.Element, Name: "book-title" })
                {
                    var bookTitle = reader.ReadString();
                    AddMetadata (DescriptionElements.Title, bookTitle);
                }
                else if (reader is { NodeType: XmlNodeType.Element, Name: "genre" })
                {
                    var bookGenre = reader.ReadString();
                    bookGenre = FB2Config.Current.GenreSubstitutions.FindSubstitution (bookGenre);
                    if (bookTitleGenre == 0)
                    {
                        AddMetadata (DescriptionElements.Genre, bookGenre);
                    }

                    AddMetadata (DescriptionElements.Genre, bookTitleGenre, bookGenre);
                    bookTitleGenre++;
                }
                else if (reader is { NodeType: XmlNodeType.Element, Name: "sequence" })
                {
                    var bookSequenceName = reader.GetAttribute ("name");
                    bookSequenceName = bookSequenceName!.Trim();
                    AddMetadata (DescriptionElements.SequenceName, bookSequenceName);
                    var tmp = reader.GetAttribute ("number");
                    try
                    {
                        var tmpi = int.Parse (tmp!);
                        if ((tmpi > 0) && !string.IsNullOrEmpty (bookSequenceName))
                        {
                            AddMetadata (DescriptionElements.SequenceNr, Convert.ToString (tmpi));
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine (exception.Message);
                    }
                }
                else if (reader is { NodeType: XmlNodeType.Element, Name: "lang" })
                {
                    var bookLang = reader.ReadString();
                    AddMetadata (DescriptionElements.Lang, bookLang);
                }
            }
        }
    }

    private void ParseDocumentInfo (string documentInfo)
    {
        using (var reader = XmlReader.Create (new StringReader (documentInfo)))
        {
            while (reader.Read())
            {
                if (reader is { NodeType: XmlNodeType.Element, Name: "version" })
                {
                    var bookVersion = reader.ReadString();
                    AddMetadata (DescriptionElements.Version, bookVersion);
                    break;
                }
            }
        }
    }

    /// <inheritdoc cref="FileMetadata.InternalParseDescription"/>
    protected override void InternalParseDescription (string description)
    {
        base.InternalParseDescription (description);
        SetDescription (description);
        using (var reader = XmlReader.Create (new StringReader (description)))
        {
            while (reader.Read())
            {
                if (reader is { NodeType: XmlNodeType.Element, Name: "title-info" })
                {
                    var titleInfo = reader.ReadOuterXml();
                    titleInfo = titleInfo.Trim();
                    ParseTitleInfo (titleInfo);
                }

                if (reader is { NodeType: XmlNodeType.Element, Name: "document-info" })
                {
                    var documentInfo = reader.ReadOuterXml();
                    documentInfo = documentInfo.Trim();
                    ParseDocumentInfo (documentInfo);
                }
            }
        }

        if (string.IsNullOrEmpty (GetMetadata (DescriptionElements.SequenceName)))
        {
            AddMetadata (DescriptionElements.SequenceNr, string.Empty);
        }
    }

    /// <inheritdoc cref="FileMetadata.InternalInitialize"/>
    protected override void InternalInitialize()
    {
        bookTitleAuthor = 0;
        bookTitleTranslator = 0;
        bookTitleGenre = 0;
        base.InternalInitialize();
    }

    /// <summary>
    ///
    /// </summary>
    public FB2Metadata()
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="description"></param>
    public FB2Metadata (string description)
        : this()
    {
        ParseDescription (description);
    }
}

/// <summary>
///
/// </summary>
public class FB2File
    : IComparable
{
    #region Private

    private const string fb2xmlns = "http://www.gribuser.ru/xml/fictionbook/2.0";
    private bool _isValid = true;
    private FileMetadata _metadata = new ();
    private string _errors = string.Empty;
    private readonly List<string> validationSchemaErrors = new ();

    private static XmlSchema? FictionBook { get; set; }
    private static XmlSchema? FictionBookGenres { get; set; }
    private static XmlSchema? FictionBookLang { get; set; }
    private static XmlSchema? FictionBookLinks { get; set; }

    #endregion

    #region IComparable Members

    /// <inheritdoc cref="IComparable.CompareTo"/>
    public int CompareTo (object? obj)
    {
        var fc = obj as FB2File;
        if (fc == null)
        {
            throw new InvalidCastException();
        }

        var result = string.Compare (BookAuthorLastName, fc.BookAuthorLastName,
            StringComparison.InvariantCultureIgnoreCase);
        if (result == 0)
        {
            result = string.Compare (BookAuthorFirstName, fc.BookAuthorFirstName,
                StringComparison.InvariantCultureIgnoreCase);
        }

        if (result == 0)
        {
            result = string.Compare (BookSequenceName, fc.BookSequenceName,
                StringComparison.InvariantCultureIgnoreCase);
        }

        if (result == 0)
        {
            result = Comparer<int?>.Default.Compare (BookSequenceNr, fc.BookSequenceNr);
        }

        if (result == 0)
        {
            result = string.Compare (BookTitle, fc.BookTitle, StringComparison.InvariantCultureIgnoreCase);
        }

        return result;
    }

    #endregion

    private static void LoadSchemas()
    {
        if (FictionBook == null)
        {
            FictionBook = GetEmbeddedSchema ("FB2Toolbox.Validation.FictionBook2.1.xsd");
            FictionBookGenres = GetEmbeddedSchema ("FB2Toolbox.Validation.FictionBookGenres.xsd");
            FictionBookLang = GetEmbeddedSchema ("FB2Toolbox.Validation.FictionBookLang.xsd");
            FictionBookLinks = GetEmbeddedSchema ("FB2Toolbox.Validation.FictionBookLinks.xsd");
        }
    }

    private void SchemaValidation
        (
            object? sender,
            ValidationEventArgs eventArgs
        )
    {
        //validationSchemaErrors.Add(string.Format(Properties.Resources.ValidationError, e.Exception.LineNumber, e.Exception.LinePosition, e.Message));
        validationSchemaErrors.Add ("Validation Error");
    }

    private static XmlSchema GetEmbeddedSchema (string resourceName)
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream (resourceName))
        {
            return XmlSchema.Read (stream!, null)!;
        }
    }

    private void ParseEncoding (string encoding)
    {
        BookEncoding = encoding;
        BookInternalEncoding = encoding;
        if (FB2Config.Current.Encodings.TranslateEncodings)
        {
            try
            {
                var enc = Encoding.GetEncoding (encoding);
                BookEncoding = enc.EncodingName;
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception.Message);
            }
        }
    }

    private void ParseStream (Stream stream)
    {
        stream.Position = 0;
        using (var reader = XmlReader.Create (stream))
        {
            ClearFields();

            // reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.XmlDeclaration)
                {
                    ParseEncoding (reader.GetAttribute ("encoding")!);
                }

                if (reader is { NodeType: XmlNodeType.Element, Name: "description" })
                {
                    var description = reader.ReadOuterXml();
                    description = description.Trim();
                    _metadata = new FB2Metadata (description);
                    break;
                }

                if (reader is { NodeType: XmlNodeType.Element, Name: "body" })
                {
                    break;
                }
            }
        }
    }

    private Stream GetFileReadStream (string fileName)
    {
        Stream stream = new MemoryStream();
        if (fileName.ToLower().EndsWith (FB2Config.Current.FB2Extension))
        {
            stream = new FileStream (fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        else if (fileName.ToLower().EndsWith (FB2Config.Current.FB2ZIPExtension))
        {
            var zipEncoding = Encoding.GetEncoding (FB2Config.Current.Encodings.CompressionEncoding);
            // var options = new ReadOptions
            // {
            //     Encoding = zipEncoding
            // };
            using (var zip = ZipFile.Open (fileName, ZipArchiveMode.Read, zipEncoding))
                   //ZipFile.Read (fileName, options))
            {
                if (zip.Entries.Count <= 0)
                {
                    // throw new Exception(Properties.Resources.ZipErrorNoFiles);
                    throw new Exception();
                }

                if (zip.Entries.Count > 1)
                {
                    // throw new Exception(Properties.Resources.ZipErrorMoreThanOneFile);
                    throw new Exception();
                }

                if (!zip.Entries[0].Name.ToLower().EndsWith (FB2Config.Current.FB2Extension))
                {
                    // throw new Exception(Properties.Resources.ZipErrorNoFB2);
                    throw new Exception();
                }

                foreach (var entry in zip.Entries)
                {
                    using var zipStream = entry.Open();
                    using var fileStream = File.Create (entry.Name);
                    zipStream.CopyTo (fileStream);
                    // entry.Extract (stream);
                }
            }
        }

        stream.Position = 0;
        return stream;
    }

    private void ParseFile (string fileName)
    {
        var stream = GetFileReadStream (fileName);
        ParseStream (stream);
        stream.Close();
        Validate();
    }

    private void Validate()
    {
        _errors = string.Empty;
        _isValid = (!string.IsNullOrEmpty (BookTitle)) && (!string.IsNullOrEmpty (BookAuthorLastName)) &&
                   (!string.IsNullOrEmpty (BookGenre));
        if (!IsValid)
        {
            if (string.IsNullOrEmpty (BookGenre))
            {
                // _errors += Properties.Resources.ParseFileErrorNoBookGenre + " ";
                _errors += "Parse file error: np book genre ";
            }

            if (string.IsNullOrEmpty (BookTitle))
            {
                // _errors += Properties.Resources.ParseFileErrorNoBookTitle + " ";
                _errors += "Parse file error: no book title ";
            }

            if (string.IsNullOrEmpty (BookAuthorLastName))
            {
                // _errors += Properties.Resources.ParseFileErrorNoAuthorLastName + " ";
                _errors += "Parse file error: no author last name ";
            }

            _errors = _errors.Trim();
        }
    }

    private void ClearFields()
    {
        BookEncoding = string.Empty;
        BookInternalEncoding = string.Empty;
    }

    internal bool IsValid => _isValid;

    internal string Error()
    {
        return _errors;
    }

    internal void Reload()
    {
        try
        {
            UpdateFileInfo (FileInformation.FullName);
            ParseFile (FileInformation.FullName);
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="substitutionCollection"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    protected string SubstituteCharacters
        (
            CharacterSubstitutionCollection? substitutionCollection,
            string value
        )
    {
        if (substitutionCollection == null)
        {
            return value;
        }

        if (substitutionCollection.Count == 0)
        {
            return value;
        }

        foreach (CharacterSubstitutionElement el in substitutionCollection)
        {
            for (var i = 0; i < el.Repeat; i++)
                value = value.Replace (el.From, el.To);
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="useTranslit"></param>
    /// <returns></returns>
    protected string CalculateNewFileName
        (
            RenameProfileElement profile,
            bool useTranslit
        )
    {
        var fn = string.Empty;
        var extension = string.Empty;
        if (FileInformation.Name.ToLower().EndsWith (FB2Config.Current.FB2Extension))
        {
            extension = FB2Config.Current.FB2Extension;
        }
        else if (FileInformation.Name.ToLower().EndsWith (FB2Config.Current.FB2ZIPExtension))
        {
            extension = FB2Config.Current.FB2ZIPExtension;
        }

        foreach (var part in profile.FileName.Split (new char[] { '|' }))
        {
            fn += Metadata.SubstitutePart (part);
        }

        fn = fn.Replace ("\\", string.Empty);
        fn = fn.Trim();
        fn = SubstituteCharacters (profile.CharacterSubstitution, fn);
        if (useTranslit)
        {
            fn = SubstituteCharacters (FB2Config.Current.RenameProfiles.GlobalTranslit, fn);
        }

        fn = SubstituteCharacters (FB2Config.Current.RenameProfiles.GlobalCharacterSubstitution, fn);
        if (!fn.ToLower().EndsWith (extension))
        {
            fn += extension;
        }

        return fn;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="useTranslit"></param>
    /// <returns></returns>
    protected string CalculateNewPath (RenameProfileElement profile, bool useTranslit)
    {
        var fn = string.Empty;
        foreach (var part in profile.Path.Split (new char[] { '|' }))
        {
            fn += Metadata.SubstitutePart (part);
        }

        fn = SubstituteCharacters (profile.CharacterSubstitution, fn);
        if (useTranslit)
        {
            fn = SubstituteCharacters (FB2Config.Current.RenameProfiles.GlobalTranslit, fn);
        }

        fn = SubstituteCharacters (FB2Config.Current.RenameProfiles.GlobalCharacterSubstitution, fn);
        return fn;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="folder"></param>
    protected void RemoveFolder
        (
            DirectoryInfo? folder
        )
    {
        try
        {
            if (folder == null)
            {
                return;
            }

            if (NativeMethods.CheckDirectoryEmpty_Fast (folder.FullName))
            {
                folder.Delete();
                if (folder.FullName != folder.Root.FullName)
                {
                    RemoveFolder (folder.Parent);
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool IsZIP()
    {
        return IsZIP (FileInformation.FullName);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool IsZIP (string fileName)
    {
        return fileName.ToLower().EndsWith (FB2Config.Current.FB2ZIPExtension);
    }

    /// <summary>
    ///
    /// </summary>
    public string BookSizeText =>
        // return string.Format(Properties.Resources.FileSizeText, FileInformation.Length / 1024);
        (FileInformation.Length / 1024).ToInvariantString();

    /// <summary>
    ///
    /// </summary>
    public FileInfo FileInformation { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public string BookAuthorFirstName => Metadata.GetMetadata (DescriptionElements.AuthorFirstName);

    /// <summary>
    ///
    /// </summary>
    public string BookAuthorLastName => Metadata.GetMetadata (DescriptionElements.AuthorLastName);

    /// <summary>
    ///
    /// </summary>
    public string BookAuthorMiddleName => Metadata.GetMetadata (DescriptionElements.AuthorMiddleName);

    /// <summary>
    ///
    /// </summary>
    public string BookGenre => Metadata.GetMetadata (DescriptionElements.Genre);

    /// <summary>
    ///
    /// </summary>
    public string BookEncoding { get; private set; }

    /// <summary>
    ///
    /// </summary>
    protected internal string BookInternalEncoding { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public string BookTitle => Metadata.GetMetadata (DescriptionElements.Title);

    /// <summary>
    ///
    /// </summary>
    public string BookSequenceName => Metadata.GetMetadata (DescriptionElements.SequenceName);

    /// <summary>
    ///
    /// </summary>
    public string BookVersion => Metadata.GetMetadata (DescriptionElements.Version);

    /// <summary>
    ///
    /// </summary>
    public int? BookSequenceNr
    {
        get
        {
            var bsn = Metadata.GetMetadata (DescriptionElements.SequenceNr);
            if (string.IsNullOrEmpty (bsn))
            {
                return null;
            }

            return int.Parse (bsn);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public string BookLang => Metadata.GetMetadata (DescriptionElements.Lang);

    /// <summary>
    ///
    /// </summary>
    public FileMetadata Metadata => _metadata;

    /// <summary>
    ///
    /// </summary>
    /// <param name="newFullName"></param>
    /// <param name="newFileName"></param>
    /// <returns></returns>
    public bool IsSkipFile (string newFullName, string newFileName)
    {
        return File.Exists (newFullName);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="targetFolder"></param>
    /// <param name="profile"></param>
    /// <param name="useTranslit"></param>
    /// <returns></returns>
    public FileOperationResult MoveTo (string targetFolder, RenameProfileElement profile, bool useTranslit)
    {
        var newPath = CalculateNewPath (profile, useTranslit);
        newPath = Path.Combine (targetFolder, newPath);
        var newFileName = CalculateNewFileName (profile, useTranslit);
        var newFullName = Path.Combine (newPath, newFileName);
        Directory.CreateDirectory (newPath);
        var di = FileInformation.Directory;
        var result = new FileOperationResult
        {
            NewFileName = newFileName, NewFullName = newFullName,
            Skipped = IsSkipFile (newFullName, newFileName)
        };
        if (!result.Skipped)
        {
            if (File.Exists (newFullName))
            {
                File.Delete (newFullName);
            }

            FileInformation.MoveTo (newFullName);
            UpdateFileInfo (newFullName);
            RemoveFolder (di);
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="targetFolder"></param>
    /// <param name="profile"></param>
    /// <param name="useTranslit"></param>
    /// <returns></returns>
    public FileOperationResult CopyTo (string targetFolder, RenameProfileElement profile, bool useTranslit)
    {
        var newPath = CalculateNewPath (profile, useTranslit);
        newPath = Path.Combine (targetFolder, newPath);
        var newFileName = CalculateNewFileName (profile, useTranslit);
        var newFullName = Path.Combine (newPath, newFileName);
        Directory.CreateDirectory (newPath);
        var result = new FileOperationResult
        {
            NewFileName = newFileName, NewFullName = newFullName,
            Skipped = IsSkipFile (newFullName, newFileName)
        };
        if (!result.Skipped)
        {
            FileInformation.CopyTo (newFullName, true);
            UpdateFileInfo (newFullName);
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool Extract()
    {
        var fileName = FileInformation.FullName;
        if (fileName.ToLower().EndsWith (FB2Config.Current.FB2ZIPExtension))
        {
            fileName = fileName.Substring (0, fileName.Length - FB2Config.Current.FB2ZIPExtension.Length) +
                       FB2Config.Current.FB2Extension;
            var zipEncoding = Encoding.GetEncoding (FB2Config.Current.Encodings.CompressionEncoding);
            //var options = new ReadOptions
            //{
            //    Encoding = zipEncoding
            //};
            using (var zip = ZipFile.Open (fileName, ZipArchiveMode.Read, zipEncoding))
                //(ZipFile zip = ZipFile.Read (FileInformation.FullName, options))
            {
                var zipEntry = zip.Entries[0];
                //zip[0].Extract (FileInformation.Directory!.FullName, ExtractExistingFileAction.Throw);
                fileName = Path.Combine (FileInformation.Directory!.FullName, zipEntry.Name);
                using var zipStream = zipEntry.Open();
                using var fileSteam = File.Create (zipEntry.Name);
                zipStream.CopyTo (fileSteam);
            }

            FileInformation.Delete();
            UpdateFileInfo (fileName);
            Reload();
            return true;
        }

        return false;
    }

    private void ValidateEmptyTags (XPathDocument xmlDoc)
    {
        var navigator = xmlDoc.CreateNavigator();
        var iterator = navigator.Select ("//*");
        while (iterator.MoveNext())
        {
            var curr = iterator.Current;
            if (!curr!.HasChildren && !curr.Name.EndsWith ("empty-line") && !curr.Name.EndsWith ("image") &&
                !curr.Name.EndsWith ("sequence") && string.IsNullOrEmpty (curr.Value))
            {
                var info = curr as IXmlLineInfo;
                // validationSchemaErrors.Add (string.Format (Properties.Resources.ValidationWarning, info.LineNumber,
                //     info.LinePosition, string.Format (Properties.Resources.ValidationErrorEmptyTag, curr.Name)));
            }
            validationSchemaErrors.Add ("Validation Warning");
        }
    }

    private void ValidateLinks (XPathDocument xmlDoc)
    {
        var defaultNamespace = "http://www.w3.org/1999/xlink";
        var navigator = xmlDoc.CreateNavigator();
        var nsm = new XmlNamespaceManager (navigator.NameTable);
        nsm.AddNamespace ("xlink", defaultNamespace);
        nsm.AddNamespace ("l", defaultNamespace);


        var idList = new Dictionary<string, string>();
        var hrefList = new Dictionary<string, string>();

        var ids = navigator.Select ("//*[@id]", nsm);
        while (ids.MoveNext())
        {
            var curr = ids.Current;
            var info = curr as IXmlLineInfo;
            var id = curr!.GetAttribute ("id", string.Empty);
            if (!idList.ContainsKey (id))
            {
                idList.Add (id, $"{info!.LineNumber}|{info.LinePosition}");
            }
            else
            {
                // validationSchemaErrors.Add (string.Format (Properties.Resources.ValidationWarning, info.LineNumber,
                //     info.LinePosition, string.Format (Properties.Resources.ValidationErrorDuplicateId, id)));
                validationSchemaErrors.Add ("Validation Warning");
            }
        }

        var iterator = navigator.Select ("//*[@xlink:href|@l:href]", nsm);
        while (iterator.MoveNext())
        {
            var curr = iterator.Current;
            var error = string.Empty;
            var href = curr!.GetAttribute ("href", defaultNamespace);
            var type = curr.GetAttribute ("type", defaultNamespace);
            if (string.IsNullOrEmpty (href))
            {
                // error = string.Format (Properties.Resources.ValidationErrorEmptyLink, curr.Name);
                error = "Validation error: empty link";
            }
            else if (!href.StartsWith ("#"))
            {
                if (curr.Name.EndsWith ("}image"))
                {
                    // error = string.Format (Properties.Resources.ValidationErrorExternalLink, href);
                    error = "Validation error: external link";
                }
                else if (type == "note")
                {
                    // error = string.Format (Properties.Resources.ValidationErrorExternalNote, href);
                    error = "Validation error: external note";
                }
                else if (!(href.StartsWith ("http:") || href.StartsWith ("https:") || href.StartsWith ("ftp:") ||
                           href.StartsWith ("mailto:")))
                {
                    // error = string.Format (Properties.Resources.ValidationErrorInvalidExternalLink, href);
                    error = "Validation error: invalid external link";
                }
                else
                {
                    // error = string.Format (Properties.Resources.ValidationErrorLocalExternalLink, href);
                    error = "Validation error: local external link";
                }
            }
            else
            {
                var id = href.Remove (0, 1);
                if (!hrefList.ContainsKey (id))
                {
                    hrefList.Add (id, string.Empty);
                }

                if (!idList.ContainsKey (id))
                {
                    // error = string.Format (Properties.Resources.ValidationErrorReferenceToAnUnknown, href);
                    error = "Validation error: reference to an unknown";
                }
            }

            if (!string.IsNullOrEmpty (error))
            {
                var info = curr as IXmlLineInfo;
                //validationSchemaErrors.Add (string.Format (Properties.Resources.ValidationWarning, info.LineNumber,
                //    info.LinePosition, error));
                validationSchemaErrors.Add ("Validation warning");
            }
        }

        foreach (var item in idList)
        {
            if (!hrefList.ContainsKey (item.Key))
            {
                var parts = item.Value.Split ('|');
                // validationSchemaErrors.Add (string.Format (Properties.Resources.ValidationWarning, parts[0],
                //     parts[1], string.Format (Properties.Resources.ValidationErrorNoLinksToObject, item.Key)));
                validationSchemaErrors.Add ("Validation warning");
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<string> ValidateSchema()
    {
        validationSchemaErrors.Clear();
        LoadSchemas();
        using (var stream = GetFileReadStream (FileInformation.FullName))
        {
            var settings = new XmlReaderSettings
            {
                CheckCharacters = true
            };
            settings.ValidationEventHandler += SchemaValidation;
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add (FictionBook!);
            settings.Schemas.Add (FictionBookGenres!);
            settings.Schemas.Add (FictionBookLang!);
            settings.Schemas.Add (FictionBookLinks!);
            var reader = XmlReader.Create (stream, settings);
            while (reader.Read())
            {
            }

            //reader.Close();
        }

        using (var vstream = GetFileReadStream (FileInformation.FullName))
        {
            var xmlDoc = new XPathDocument (vstream);

            ValidateEmptyTags (xmlDoc);
            ValidateLinks (xmlDoc);
        }

        return validationSchemaErrors;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool Compress()
    {
        var fileName = FileInformation.FullName;
        if (fileName.ToLower().EndsWith (FB2Config.Current.FB2Extension))
        {
            fileName = fileName.Substring (0, fileName.Length - FB2Config.Current.FB2Extension.Length) +
                       FB2Config.Current.FB2ZIPExtension;
            var zipEncoding = Encoding.GetEncoding (FB2Config.Current.Encodings.CompressionEncoding);
            using (var zip = ZipFile.Open (fileName, ZipArchiveMode.Update, zipEncoding))
            //(ZipFile zip = new ZipFile (fileName, zipEncoding))
            {
                var entry = zip.CreateEntry (FileInformation.Name);
                using var zipStream = entry.Open();
                using var fileStream = File.OpenRead (FileInformation.FullName);
                fileStream.CopyTo (zipStream);
                //zip.AddFile (FileInformation.FullName, string.Empty);
                //zip.Save();
            }

            FileInformation.Delete();
            UpdateFileInfo (fileName);
            Reload();
            return true;
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="useTranslit"></param>
    /// <returns></returns>
    public FileOperationResult RenameTo (RenameProfileElement profile, bool useTranslit)
    {
        var newFileName = CalculateNewFileName (profile, useTranslit);
        var newFullName = Path.Combine (FileInformation.Directory!.FullName, newFileName);
        var result = new FileOperationResult() { NewFileName = newFileName, NewFullName = newFullName };

        // Skip renaming if it is the same file
        if (newFullName.ToLowerInvariant() == FileInformation.FullName.ToLowerInvariant())
        {
            result.Skipped = true;
            return result;
        }

        result.Skipped = IsSkipFile (newFullName, newFileName);
        if (!result.Skipped)
        {
            if (File.Exists (newFullName))
            {
                File.Delete (newFullName);
            }

            FileInformation.MoveTo (newFullName);
            UpdateFileInfo (newFullName);
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fileName"></param>
    public void UpdateFileInfo (string fileName)
    {
        FileInformation = new FileInfo (fileName);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="props"></param>
    public void UpdateProperties (FileProperties props)
    {
        var stream = GetFileReadStream (FileInformation.FullName);
        var doc = new XmlDocument();
        doc.Load (stream);
        stream.Close();

        // Add the namespace.
        var nsmgr = new XmlNamespaceManager (doc.NameTable);
        nsmgr.AddNamespace ("FB2", fb2xmlns);

        // Select and display the first node in which the author's
        // last name is Kingsolver.
        var titleInfo = doc.SelectSingleNode (
            "/FB2:FictionBook/FB2:description/FB2:title-info", nsmgr);

        var author = doc.SelectSingleNode ("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author", nsmgr);
        if (author == null)
        {
            author = doc.CreateElement ("author", fb2xmlns);
            titleInfo!.AppendChild (author);
        }

        if (props.AuthorLastNameChange)
        {
            var lastName =
                doc.SelectSingleNode ("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author/FB2:last-name",
                    nsmgr);
            if (lastName == null)
            {
                lastName = doc.CreateElement ("last-name", fb2xmlns);
                author.AppendChild (lastName);
            }

            lastName.InnerText = props.AuthorLastName!;
        }

        if (props.AuthorFirstNameChange)
        {
            var firstName =
                doc.SelectSingleNode ("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author/FB2:first-name",
                    nsmgr);
            if (firstName == null)
            {
                firstName = doc.CreateElement ("first-name", fb2xmlns);
                author.AppendChild (firstName);
            }

            firstName.InnerText = props.AuthorFirstName!;
        }

        if (props.AuthorMiddleNameChange)
        {
            var middleName =
                doc.SelectSingleNode ("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:author/FB2:middle-name",
                    nsmgr);
            if (middleName == null)
            {
                middleName = doc.CreateElement ("middle-name", fb2xmlns);
                author.AppendChild (middleName);
            }

            middleName.InnerText = props.AuthorMiddleName!;
        }

        // <sequence name="100 великих" number="0" />
        if (props.NumberChange || props.SeriesChange)
        {
            var sequence = doc.SelectSingleNode ("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:sequence",
                nsmgr);
            if (sequence == null)
            {
                sequence = doc.CreateElement ("sequence", fb2xmlns);
                titleInfo!.AppendChild (sequence);
            }

            var nameA = (sequence as XmlElement)?.Attributes["name"];
            var numberA = (sequence as XmlElement)?.Attributes["number"];
            if (nameA == null)
            {
                nameA = doc.CreateAttribute ("name");
                (sequence as XmlElement)?.Attributes.Append (nameA);
            }

            if (numberA == null)
            {
                numberA = doc.CreateAttribute ("number");
                (sequence as XmlElement)?.Attributes.Append (numberA);
            }

            if (props.SeriesChange)
            {
                nameA.InnerText = props.Series!;
            }

            //(sequence as XmlElement).SetAttribute("name", props.Series);
            if (props.NumberChange)
            {
                numberA.InnerText = props.Number!;
            }

            //(sequence as XmlElement).SetAttribute("number", props.Number);
        }

        // <genre>ref_encyc</genre>
        if (props.GengeChange)
        {
            var genre = doc.SelectSingleNode ("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:genre", nsmgr);
            if (genre == null)
            {
                genre = doc.CreateElement ("genre", fb2xmlns);
                titleInfo!.AppendChild (genre);
            }

            genre.InnerText = props.Genre!;
        }

        if (props.TitleChange)
        {
            var title = doc.SelectSingleNode ("/FB2:FictionBook/FB2:description/FB2:title-info/FB2:book-title",
                nsmgr);
            if (title == null)
            {
                title = doc.CreateElement ("book-title", fb2xmlns);
                titleInfo!.AppendChild (title);
            }

            title.InnerText = props.Title!;
        }


        if (FileInformation.FullName.ToLower().EndsWith (FB2Config.Current.FB2Extension))
        {
            var fileName = FileInformation.FullName;
            using (Stream fileStream =
                   new FileStream (fileName + ".tmp", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var writer = new XmlTextWriter (fileStream, Encoding.GetEncoding (BookInternalEncoding));
                if (FB2Config.Current.Encodings.IndentFile)
                {
                    writer.Formatting = System.Xml.Formatting.Indented;
                }

                doc.Save (writer);
                writer.Flush();
            }

            FileInformation.Delete();
            var tmp = new FileInfo (fileName + ".tmp");
            tmp.MoveTo (fileName);
            FileInformation = new FileInfo (fileName);
        }
        else if (FileInformation.FullName.ToLower().EndsWith (FB2Config.Current.FB2ZIPExtension))
        {
            var inZipFileName = string.Empty;
            var zipEncoding = Encoding.GetEncoding (FB2Config.Current.Encodings.CompressionEncoding);
            //var options = new ReadOptions
            //{
            //    Encoding = zipEncoding
            //};
            using (var zip = ZipFile.Open (FileInformation.FullName, ZipArchiveMode.Read, zipEncoding))
                // (ZipFile zip = ZipFile.Read (FileInformation.FullName, options))
            {
                inZipFileName = zip.Entries[0].Name;
            }

            using (var zip = ZipFile.Open (FileInformation.FullName, ZipArchiveMode.Create))
                // (ZipFile zip = new ZipFile (zipEncoding))
            {
                var memStream = new MemoryStream();
                var writer = new XmlTextWriter (memStream, Encoding.GetEncoding (BookInternalEncoding));
                if (FB2Config.Current.Encodings.IndentFile)
                {
                    writer.Formatting = System.Xml.Formatting.Indented;
                }

                doc.Save (writer);
                writer.Flush();
                memStream.Position = 0;
                var entry = zip.CreateEntry(inZipFileName);
                using var zipStream = entry.Open();
                memStream.CopyTo (zipStream);
                writer.Close();
            }
        }

        Reload();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="enc"></param>
    public void EncodeTo (Encoding enc)
    {
        var stream = GetFileReadStream (FileInformation.FullName);
        var doc = new XmlDocument();
        doc.Load (stream);
        stream.Close();

        enc = Encoding.GetEncoding (enc.CodePage, new FBEncoderFallback(), Encoding.UTF8.DecoderFallback);

        if (FileInformation.FullName.ToLower().EndsWith (FB2Config.Current.FB2Extension))
        {
            var fileName = FileInformation.FullName;
            using (Stream fileStream =
                   new FileStream (fileName + ".tmp", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var writer = new XmlTextWriter (fileStream, enc);
                if (FB2Config.Current.Encodings.IndentFile)
                {
                    writer.Formatting = System.Xml.Formatting.Indented;
                }

                doc.Save (writer);
                writer.Flush();
            }

            FileInformation.Delete();
            var tmp = new FileInfo (fileName + ".tmp");
            tmp.MoveTo (fileName);
            FileInformation = new FileInfo (fileName);
        }
        else if (FileInformation.FullName.ToLower().EndsWith (FB2Config.Current.FB2ZIPExtension))
        {
            var inZipFileName = string.Empty;
            var zipEncoding = Encoding.GetEncoding (FB2Config.Current.Encodings.CompressionEncoding);
            //var options = new ReadOptions
            //{
            //    Encoding = zipEncoding
            //};
            using (var zip = ZipFile.Open (FileInformation.FullName, ZipArchiveMode.Read, zipEncoding))
                // (ZipFile zip = ZipFile.Read (FileInformation.FullName, options))
            {
                inZipFileName = zip.Entries[0].Name;
            }

            using (var zip = ZipFile.Open (FileInformation.FullName, ZipArchiveMode.Update, zipEncoding))
                // (ZipFile zip = new ZipFile (zipEncoding))
            {
                var memStream = new MemoryStream();
                var writer = new XmlTextWriter (memStream, enc);
                if (FB2Config.Current.Encodings.IndentFile)
                {
                    writer.Formatting = System.Xml.Formatting.Indented;
                }

                doc.Save (writer);
                writer.Flush();
                memStream.Position = 0;
                var entry = zip.CreateEntry (inZipFileName);
                using var zipStream = entry.Open();
                memStream.CopyTo (zipStream);
                writer.Close();
            }
        }

        Reload();
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return FileInformation.Name;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fileName"></param>
    public FB2File (string fileName)
    {
        BookEncoding = null!;
        BookInternalEncoding = null!;

        ClearFields();
        FileInformation = new FileInfo (fileName);
        ParseFile (fileName);
    }
}
