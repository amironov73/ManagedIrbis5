// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* PdfTrailer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using AM;

using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Security;
using PdfSharpCore.Pdf.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents a PDF trailer dictionary. Even though trailers are dictionaries they never have a cross
/// reference entry in PdfReferenceTable.
/// </summary>
internal class PdfTrailer
    : PdfDictionary // Reference: 3.4.4  File Trailer / Page 96
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of PdfTrailer.
    /// </summary>
    public PdfTrailer (PdfDocument document)
        : base (document)
    {
        _document = document;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfTrailer"/> class from a <see cref="PdfCrossReferenceStream"/>.
    /// </summary>
    public PdfTrailer
        (
            PdfCrossReferenceStream trailer
        )
        : base (trailer._document)
    {
        _document = trailer._document;

        // /ID [<09F877EBF282E9408ED1882A9A21D9F2><2A4938E896006F499AC1C2EA7BFB08E4>]
        // /Info 7 0 R
        // /Root 1 0 R
        // /Size 10

        var iref = trailer.Elements.GetReference (Keys.Info);
        if (iref != null)
        {
            Elements.SetReference (Keys.Info, iref);
        }

        Elements.SetReference (Keys.Root, trailer.Elements.GetReference (Keys.Root)!);

        Elements.SetInteger (Keys.Size, trailer.Elements.GetInteger (Keys.Size));

        var id = trailer.Elements.GetArray (Keys.ID);
        if (id != null)
        {
            Elements.SetValue (Keys.ID, id);
        }
    }

    #endregion

    public int Size
    {
        get => Elements.GetInteger (Keys.Size);
        set => Elements.SetInteger (Keys.Size, value);
    }

    // TODO: needed when linearized...
    //public int Prev
    //{
    //  get {return Elements.GetInteger(Keys.Prev);}
    //}

    public PdfDocumentInformation Info =>
        (PdfDocumentInformation) Elements.GetValue (Keys.Info, VCF.CreateIndirect)
            .ThrowIfNull();

    /// <summary>
    /// (Required; must be an indirect reference)
    /// The catalog dictionary for the PDF document contained in the file.
    /// </summary>
    public PdfCatalog Root => (PdfCatalog) Elements.GetValue (Keys.Root, VCF.CreateIndirect)
        .ThrowIfNull();

    /// <summary>
    /// Gets the first or second document identifier.
    /// </summary>
    public string GetDocumentID (int index)
    {
        if (index is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException (nameof (index), index, "Index must be 0 or 1.");
        }

        if (Elements[Keys.ID] is not PdfArray array || array.Elements.Count < 2)
        {
            return "";
        }

        var item = array.Elements[index];
        if (item is PdfString)
        {
            return ((PdfString)item).Value;
        }

        return string.Empty;
    }

    /// <summary>
    /// Sets the first or second document identifier.
    /// </summary>
    public void SetDocumentID (int index, string value)
    {
        if (index is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException (nameof (index), index, "Index must be 0 or 1.");
        }

        if (Elements[Keys.ID] is not PdfArray array || array.Elements.Count < 2)
        {
            array = CreateNewDocumentIDs();
        }

        array.Elements[index] = new PdfString (value, PdfStringFlags.HexLiteral);
    }

    /// <summary>
    /// Creates and sets two identical new document IDs.
    /// </summary>
    internal PdfArray CreateNewDocumentIDs()
    {
        var array = new PdfArray (_document.ThrowIfNull());
        var docID = Guid.NewGuid().ToByteArray();
        var id = PdfEncoders.RawEncoding.GetString (docID, 0, docID.Length);
        array.Elements.Add (new PdfString (id, PdfStringFlags.HexLiteral));
        array.Elements.Add (new PdfString (id, PdfStringFlags.HexLiteral));
        Elements[Keys.ID] = array;
        return array;
    }

    /// <summary>
    /// Gets the standard security handler.
    /// </summary>
    public PdfStandardSecurityHandler SecurityHandler
    {
        get
        {
            return _securityHandler ??= (PdfStandardSecurityHandler) Elements
                .GetValue (Keys.Encrypt, VCF.CreateIndirect)
                .ThrowIfNull();
        }
    }

    internal PdfStandardSecurityHandler? _securityHandler;

    internal override void WriteObject (PdfWriter writer)
    {
        // Delete /XRefStm entry, if any.
        // HACK:
        _elements!.Remove (Keys.XRefStm);

        // Don't encrypt myself
        var securityHandler = writer.SecurityHandler;
        writer.SecurityHandler = null;
        base.WriteObject (writer);
        writer.SecurityHandler = securityHandler;
    }

    /// <summary>
    /// Replace temporary irefs by their correct counterparts from the iref table.
    /// </summary>
    internal void Finish()
    {
        var document = _document.ThrowIfNull();
        var irefTable = document._irefTable.ThrowIfNull();

        // /Root
        if (document._trailer!.Elements[Keys.Root] is PdfReference iref && iref.Value == null!)
        {
            iref = irefTable[iref.ObjectID]!;
            Debug.Assert (iref.Value != null);
            document._trailer.Elements[Keys.Root] = iref;
        }

        // /Info
        iref = (document._trailer.Elements[Keys.Info] as PdfReference)!;
        if (iref is { Value: null })
        {
            iref = irefTable[iref.ObjectID]!;
            Debug.Assert (iref.Value != null);
            document._trailer.Elements[Keys.Info] = iref;
        }

        // /Encrypt
        iref = (document._trailer.Elements[Keys.Encrypt] as PdfReference)!;
        if (iref != null!)
        {
            iref = irefTable[iref.ObjectID]!;
            Debug.Assert (iref.Value != null);
            document._trailer.Elements[Keys.Encrypt] = iref;

            // The encryption dictionary (security handler) was read in before the XRefTable construction
            // was completed. The next lines fix that state (it took several hours to find these bugs...).
            iref.Value = _document!._trailer!._securityHandler!;
            _document._trailer._securityHandler!.Reference = iref;
            iref.Value.Reference = iref;
        }

        Elements.Remove (Keys.Prev);

        Debug.Assert (irefTable.IsUnderConstruction == false);
        irefTable.IsUnderConstruction = false;
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    internal class Keys
        : KeysBase // Reference: TABLE 3.13  Entries in the file trailer dictionary / Page 97
    {
        /// <summary>
        /// (Required; must not be an indirect reference) The total number of entries in the file�s
        /// cross-reference table, as defined by the combination of the original section and all
        /// update sections. Equivalently, this value is 1 greater than the highest object number
        /// used in the file.
        /// Note: Any object in a cross-reference section whose number is greater than this value is
        /// ignored and considered missing.
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Required)]
        public const string Size = "/Size";

        /// <summary>
        /// (Present only if the file has more than one cross-reference section; must not be an indirect
        /// reference) The byte offset from the beginning of the file to the beginning of the previous
        /// cross-reference section.
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Optional)]
        public const string Prev = "/Prev";

        /// <summary>
        /// (Required; must be an indirect reference) The catalog dictionary for the PDF document
        /// contained in the file.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Required, typeof (PdfCatalog))]
        public const string Root = "/Root";

        /// <summary>
        /// (Required if document is encrypted; PDF 1.1) The document�s encryption dictionary.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional, typeof (PdfStandardSecurityHandler))]
        public const string Encrypt = "/Encrypt";

        /// <summary>
        /// (Optional; must be an indirect reference) The document�s information dictionary.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional, typeof (PdfDocumentInformation))]
        public const string Info = "/Info";

        /// <summary>
        /// (Optional, but strongly recommended; PDF 1.1) An array of two strings constituting
        /// a file identifier for the file. Although this entry is optional,
        /// its absence might prevent the file from functioning in some workflows
        /// that depend on files being uniquely identified.
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Optional)]
        public const string ID = "/ID";

        /// <summary>
        /// (Optional) The byte offset from the beginning of the file of a cross-reference stream.
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Optional)]
        public const string XRefStm = "/XRefStm";

        /// <summary>
        /// Gets the KeysMeta for these keys.
        /// </summary>
        public static DictionaryMeta Meta => _meta ??= CreateMeta (typeof (Keys));

        private static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta => Keys.Meta;
}
