// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfDocument.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM;

using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.Internal;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.Security;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents a PDF document.
/// </summary>
[DebuggerDisplay ("(Name={Name})")] // A name makes debugging easier
public sealed class PdfDocument
    : PdfObject, IDisposable
{
    internal DocumentState _state;
    internal PdfDocumentOpenMode _openMode;

#if DEBUG_
        static PdfDocument()
        {
            PSSR.TestResourceMessages();
            //string test = PSSR.ResMngr.GetString("SampleMessage1");
            //test.GetType();
        }
#endif

    /// <summary>
    /// Creates a new PDF document in memory.
    /// To open an existing PDF file, use the PdfReader class.
    /// </summary>
    public PdfDocument()
    {
        Tag = null!;
        _options = null!;
        _settings = null!;
        _handle = null!;
        _info = null!;
        _customValues = null!;
        _pages = null!;
        _securitySettings = null!;
        _fontTable = null!;
        _imageTable = null!;
        _formTable = null!;
        _extGStateTable = null!;
        _catalog = null!;
        _internals = null!;
        _irefTable = null!;
        _outStream = null!;
        _lexer = null!;

        //PdfDocument.Gob.AttatchDocument(Handle);

        _creation = DateTime.Now;
        _state = DocumentState.Created;
        _version = 14;
        Initialize();
        Info.CreationDate = _creation;
    }

    /// <summary>
    /// Creates a new PDF document with the specified file name. The file is immediately created and keeps
    /// locked until the document is closed, at that time the document is saved automatically.
    /// Do not call Save() for documents created with this constructor, just call Close().
    /// To open an existing PDF file and import it, use the PdfReader class.
    /// </summary>
    public PdfDocument
        (
            string filename
        )
    {
        filename.NotUsed();
        //PdfDocument.Gob.AttatchDocument(Handle);

        _creation = DateTime.Now;
        _state = DocumentState.Created;
        _version = 14;
        Initialize();
        Info.CreationDate = _creation;

        // TODO 4STLA: encapsulate the whole c'tor with #if !NETFX_CORE?
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a new PDF document using the specified stream.
    /// The stream won't be used until the document is closed, at that time the document is saved automatically.
    /// Do not call Save() for documents created with this constructor, just call Close().
    /// To open an existing PDF file, use the PdfReader class.
    /// </summary>
    public PdfDocument
        (
            Stream outputStream
        )
    {
        Tag = null!;
        _options = null!;
        _settings = null!;
        _handle = null!;
        _info = null!;
        _customValues = null!;
        _pages = null!;
        _securitySettings = null!;
        _fontTable = null!;
        _imageTable = null!;
        _formTable = null!;
        _extGStateTable = null!;
        _catalog = null!;
        _internals = null!;
        _irefTable = null!;
        _outStream = null!;
        _lexer = null!;

        //PdfDocument.Gob.AttatchDocument(Handle);

        _creation = DateTime.Now;
        _state = DocumentState.Created;
        Initialize();
        Info.CreationDate = _creation;

        _outStream = outputStream;
    }

    internal PdfDocument
        (
            Lexer lexer
        )
    {
        Tag = null!;
        _options = null!;
        _settings = null!;
        _handle = null!;
        _info = null!;
        _customValues = null!;
        _pages = null!;
        _securitySettings = null!;
        _fontTable = null!;
        _imageTable = null!;
        _formTable = null!;
        _extGStateTable = null!;
        _catalog = null!;
        _internals = null!;
        _irefTable = null!;
        _outStream = null!;
        _lexer = null!;

        //PdfDocument.Gob.AttatchDocument(Handle);

        _creation = DateTime.Now;
        _state = DocumentState.Imported;

        //_info = new PdfInfo(this);
        //_pages = new PdfPages(this);
        //_fontTable = new PdfFontTable();
        //_catalog = new PdfCatalog(this);
        ////_font = new PdfFont();
        //_objects = new PdfObjectTable(this);
        //_trailer = new PdfTrailer(this);
        _irefTable = new PdfCrossReferenceTable (this);
        _lexer = lexer;
    }

    private void Initialize()
    {
        //_info = new PdfInfo(this);
        _fontTable = new PdfFontTable (this);
        _imageTable = new PdfImageTable (this);
        _trailer = new PdfTrailer (this);
        _irefTable = new PdfCrossReferenceTable (this);
        _trailer.CreateNewDocumentIDs();
    }

    //~PdfDocument()
    //{
    //  Dispose(false);
    //}

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Dispose (true);

        //GC.SuppressFinalize(this);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose
        (
            bool disposing
        )
    {
        if (_state != DocumentState.Disposed)
        {
            if (disposing)
            {
                // Dispose managed resources.
            }

            //PdfDocument.Gob.DetatchDocument(Handle);
        }

        _state = DocumentState.Disposed;
    }

    /// <summary>
    /// Gets or sets a user defined object that contains arbitrary information associated with this document.
    /// The tag is not used by PdfSharpCore.
    /// </summary>
    public object? Tag { get; set; }

    /// <summary>
    /// Gets or sets a value used to distinguish PdfDocument objects.
    /// The name is not used by PdfSharpCore.
    /// </summary>
    private string Name { get; set; } = NewName();

    /// <summary>
    /// Get a new default name for a new document.
    /// </summary>
    private static string NewName()
    {
#if DEBUG_
            if (PdfDocument.nameCount == 57)
                PdfDocument.nameCount.GetType();
#endif
        return "Document " + _nameCount++;
    }

    private static int _nameCount;

    internal bool CanModify =>

        //get {return _state == DocumentState.Created || _state == DocumentState.Modifyable;}
        true;

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public void Close()
    {
        if (!CanModify)
        {
            throw new InvalidOperationException (PSSR.CannotModify);
        }

        if (_outStream != null)
        {
            // Get security handler if document gets encrypted
            PdfStandardSecurityHandler securityHandler = null;
            if (SecuritySettings.DocumentSecurityLevel != PdfDocumentSecurityLevel.None)
            {
                securityHandler = SecuritySettings.SecurityHandler;
            }

            var writer = new PdfWriter (_outStream, securityHandler);
            try
            {
                DoSave (writer);
            }
            finally
            {
                writer.Close();
            }
        }
    }

    /// <summary>
    /// Saves the document to the specified path. If a file already exists, it will be overwritten.
    /// </summary>
    public void Save (string path)
    {
        if (!CanModify)
        {
            throw new InvalidOperationException (PSSR.CannotModify);
        }


        using (Stream stream = new FileStream (path, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            Save (stream);
        }
    }

    /// <summary>
    /// Saves the document to the specified stream.
    /// </summary>
    public void Save
        (
            Stream stream,
            bool closeStream
        )
    {
        if (!CanModify)
        {
            throw new InvalidOperationException (PSSR.CannotModify);
        }

        // TODO: more diagnostic checks
        var message = "";
        if (!CanSave (ref message))
        {
            throw new PdfSharpException (message);
        }

        // Get security handler if document gets encrypted.
        PdfStandardSecurityHandler? securityHandler = null;
        if (SecuritySettings.DocumentSecurityLevel != PdfDocumentSecurityLevel.None)
        {
            securityHandler = SecuritySettings.SecurityHandler;
        }

        PdfWriter? writer = null;
        try
        {
            writer = new PdfWriter (stream, securityHandler);
            DoSave (writer);
        }
        finally
        {
            if (stream != null)
            {
                if (closeStream)
                {
                    stream.Dispose();
                }
                else
                {
                    stream.Position = 0; // Reset the stream position if the stream is kept open.
                }
            }

            if (writer != null)
            {
                writer.Close (closeStream);
            }
        }
    }

    /// <summary>
    /// Saves the document to the specified stream.
    /// The stream is not closed by this function.
    /// (Older versions of PDFsharp closes the stream. That was not very useful.)
    /// </summary>
    public void Save (Stream stream)
    {
        Save (stream, false);
    }

    /// <summary>
    /// Implements saving a PDF file.
    /// </summary>
    private void DoSave (PdfWriter writer)
    {
        if (_pages == null || _pages.Count == 0)
        {
            if (_outStream != null)
            {
                // Give feedback if the wrong constructor was used.
                throw new InvalidOperationException (
                    "Cannot save a PDF document with no pages. Do not use \"public PdfDocument(string filename)\" or \"public PdfDocument(Stream outputStream)\" if you want to open an existing PDF document from a file or stream; use PdfReader.Open() for that purpose.");
            }

            throw new InvalidOperationException ("Cannot save a PDF document with no pages.");
        }

        try
        {
            // HACK: Remove XRefTrailer
            if (_trailer is PdfCrossReferenceStream)
            {
                // HACK^2: Preserve the SecurityHandler.
                var securityHandler = _securitySettings.SecurityHandler;
                _trailer = new PdfTrailer ((PdfCrossReferenceStream)_trailer);
                _trailer._securityHandler = securityHandler;
            }

            var encrypt = _securitySettings.DocumentSecurityLevel != PdfDocumentSecurityLevel.None;
            if (encrypt)
            {
                var securityHandler = _securitySettings.SecurityHandler;
                if (securityHandler.Reference == null)
                {
                    _irefTable.Add (securityHandler);
                }
                else
                {
                    Debug.Assert (_irefTable.Contains (securityHandler.ObjectID));
                }

                _trailer.Elements[PdfTrailer.Keys.Encrypt] = _securitySettings.SecurityHandler.Reference;
            }
            else
            {
                _trailer.Elements.Remove (PdfTrailer.Keys.Encrypt);
            }

            PrepareForSave();

            if (encrypt)
            {
                _securitySettings.SecurityHandler.PrepareEncryption();
            }

            writer.WriteFileHeader (this);
            var irefs = _irefTable.AllReferences;
            var count = irefs.Length;
            for (var idx = 0; idx < count; idx++)
            {
                var iref = irefs[idx];
#if DEBUG_
                    if (iref.ObjectNumber == 378)
                        GetType();
#endif
                iref.Position = writer.Position;
                iref.Value.WriteObject (writer);
            }

            var startxref = writer.Position;
            _irefTable.WriteObject (writer);
            writer.WriteRaw ("trailer\n");
            _trailer.Elements.SetInteger ("/Size", count + 1);
            _trailer.WriteObject (writer);
            writer.WriteEof (this, startxref);

            //if (encrypt)
            //{
            //  state &= ~DocumentState.SavingEncrypted;
            //  //_securitySettings.SecurityHandler.EncryptDocument();
            //}
        }
        finally
        {
            if (writer != null)
            {
                writer.Stream.Flush();

                // DO NOT CLOSE WRITER HERE
                //writer.Close();
            }
        }
    }

    /// <summary>
    /// Dispatches PrepareForSave to the objects that need it.
    /// </summary>
    internal override void PrepareForSave()
    {
        var info = Info;

        // Add patch level to producer if it is not '0'.
        var pdfSharpProducer = VersionInfo.Producer;
        if (!ProductVersionInfo.VersionPatch.Equals ("0"))
        {
            pdfSharpProducer = ProductVersionInfo.Producer2;
        }

        // Set Creator if value is undefined.
        if (info.Elements[PdfDocumentInformation.Keys.Creator] == null)
        {
            info.Creator = pdfSharpProducer;
        }

        // Keep original producer if file was imported.
        var producer = info.Producer;
        if (producer.Length == 0)
        {
            producer = pdfSharpProducer;
        }
        else
        {
            // Prevent endless concatenation if file is edited with PDFsharp more than once.
            if (!producer.StartsWith (VersionInfo.Title))
            {
                producer = pdfSharpProducer + " (Original: " + producer + ")";
            }
        }

        info.Elements.SetString (PdfDocumentInformation.Keys.Producer, producer);

        // Prepare used fonts.
        if (_fontTable != null)
        {
            _fontTable.PrepareForSave();
        }

        // Let catalog do the rest.
        Catalog.PrepareForSave();

#if true

        // Remove all unreachable objects (e.g. from deleted pages)
        var removed = _irefTable.Compact();
        if (removed != 0)
        {
            Debug.WriteLine ("PrepareForSave: Number of deleted unreachable objects: " + removed);
        }

        _irefTable.Renumber();
#endif
    }

    /// <summary>
    /// Determines whether the document can be saved.
    /// </summary>
    public bool CanSave (ref string message)
    {
        if (!SecuritySettings.CanSave (ref message))
        {
            return false;
        }

        return true;
    }

    internal bool HasVersion (string version)
    {
        return string.Compare (Catalog.Version, version) >= 0;
    }

    /// <summary>
    /// Gets the document options used for saving the document.
    /// </summary>
    public PdfDocumentOptions Options
    {
        get
        {
            if (_options == null)
            {
                _options = new PdfDocumentOptions (this);
            }

            return _options;
        }
    }

    private PdfDocumentOptions _options;

    /// <summary>
    /// Gets PDF specific document settings.
    /// </summary>
    public PdfDocumentSettings Settings
    {
        get
        {
            if (_settings == null)
            {
                _settings = new PdfDocumentSettings (this);
            }

            return _settings;
        }
    }

    private PdfDocumentSettings _settings;

    /// <summary>
    /// NYI Indicates whether large objects are written immediately to the output stream to relieve
    /// memory consumption.
    /// </summary>
    internal bool EarlyWrite => false;

    /// <summary>
    /// Gets or sets the PDF version number. Return value 14 e.g. means PDF 1.4 / Acrobat 5 etc.
    /// </summary>
    public int Version
    {
        get => _version;
        set
        {
            if (!CanModify)
            {
                throw new InvalidOperationException (PSSR.CannotModify);
            }

            if (value < 12 || value > 17) // TODO not really implemented
            {
                throw new ArgumentException (PSSR.InvalidVersionNumber, "value");
            }

            _version = value;
        }
    }

    internal int _version;

    /// <summary>
    /// Gets the number of pages in the document.
    /// </summary>
    public int PageCount
    {
        get
        {
            if (CanModify)
            {
                return Pages.Count;
            }

            // PdfOpenMode is InformationOnly
            var pageTreeRoot = (PdfDictionary)Catalog.Elements.GetObject (PdfCatalog.Keys.Pages);
            return pageTreeRoot.Elements.GetInteger (PdfPages.Keys.Count);
        }
    }

    /// <summary>
    /// Gets the file size of the document.
    /// </summary>
    public long FileSize => _fileSize;

    internal long _fileSize; // TODO: make private

    /// <summary>
    /// Gets the full qualified file name if the document was read form a file, or an empty string otherwise.
    /// </summary>
    public string FullPath => _fullPath;

    internal string _fullPath = string.Empty; // TODO: make private

    /// <summary>
    /// Gets a Guid that uniquely identifies this instance of PdfDocument.
    /// </summary>
    public Guid Guid => _guid;

    private Guid _guid = Guid.NewGuid();

    internal DocumentHandle Handle
    {
        get
        {
            if (_handle == null)
            {
                _handle = new DocumentHandle (this);
            }

            return _handle;
        }
    }

    private DocumentHandle _handle;

    /// <summary>
    /// Returns a value indicating whether the document was newly created or opened from an existing document.
    /// Returns true if the document was opened with the PdfReader.Open function, false otherwise.
    /// </summary>
    public bool IsImported => (_state & DocumentState.Imported) != 0;

    /// <summary>
    /// Returns a value indicating whether the document is read only or can be modified.
    /// </summary>
    public bool IsReadOnly => (_openMode != PdfDocumentOpenMode.Modify);

    internal Exception DocumentNotImported()
    {
        return new InvalidOperationException ("Document not imported.");
    }

    /// <summary>
    /// Gets information about the document.
    /// </summary>
    public PdfDocumentInformation Info
    {
        get
        {
            if (_info == null)
            {
                _info = _trailer.Info;
            }

            return _info;
        }
    }

    private PdfDocumentInformation _info; // never changes if once created

    /// <summary>
    /// This function is intended to be undocumented.
    /// </summary>
    public PdfCustomValues CustomValues
    {
        get
        {
            if (_customValues == null)
            {
                _customValues = PdfCustomValues.Get (Catalog.Elements);
            }

            return _customValues;
        }
        set
        {
            if (value != null)
            {
                throw new ArgumentException ("Only null is allowed to clear all custom values.");
            }

            PdfCustomValues.Remove (Catalog.Elements);
            _customValues = null;
        }
    }

    private PdfCustomValues _customValues;

    /// <summary>
    /// Get the pages dictionary.
    /// </summary>
    public PdfPages Pages
    {
        get
        {
            if (_pages == null)
            {
                _pages = Catalog.Pages;
            }

            return _pages;
        }
    }

    private PdfPages _pages; // never changes if once created

    /// <summary>
    /// Gets or sets a value specifying the page layout to be used when the document is opened.
    /// </summary>
    public PdfPageLayout PageLayout
    {
        get => Catalog.PageLayout;
        set
        {
            if (!CanModify)
            {
                throw new InvalidOperationException (PSSR.CannotModify);
            }

            Catalog.PageLayout = value;
        }
    }

    /// <summary>
    /// Gets or sets a value specifying how the document should be displayed when opened.
    /// </summary>
    public PdfPageMode PageMode
    {
        get => Catalog.PageMode;
        set
        {
            if (!CanModify)
            {
                throw new InvalidOperationException (PSSR.CannotModify);
            }

            Catalog.PageMode = value;
        }
    }

    /// <summary>
    /// Gets the viewer preferences of this document.
    /// </summary>
    public PdfViewerPreferences ViewerPreferences => Catalog.ViewerPreferences;

    /// <summary>
    /// Gets the root of the outline (or bookmark) tree.
    /// </summary>
    public PdfOutlineCollection Outlines => Catalog.Outlines;

    /// <summary>
    /// Get the AcroForm dictionary.
    /// </summary>
    public PdfAcroForm AcroForm => Catalog.AcroForm;

    /// <summary>
    /// Gets or sets the default language of the document.
    /// </summary>
    public string Language
    {
        get => Catalog.Language;
        set => Catalog.Language = value;

        //get { return Catalog.Elements.GetString(PdfCatalog.Keys.Lang); }
        //set { Catalog.Elements.SetString(PdfCatalog.Keys.Lang, value); }
    }

    /// <summary>
    /// Gets the security settings of this document.
    /// </summary>
    public PdfSecuritySettings SecuritySettings => _securitySettings ?? (_securitySettings = new PdfSecuritySettings (this));

    internal PdfSecuritySettings _securitySettings;

    /// <summary>
    /// Gets the document font table that holds all fonts used in the current document.
    /// </summary>
    internal PdfFontTable FontTable => _fontTable ??= new PdfFontTable (this);

    private PdfFontTable _fontTable;

    /// <summary>
    /// Gets the document image table that holds all images used in the current document.
    /// </summary>
    internal PdfImageTable ImageTable
    {
        get
        {
            if (_imageTable == null)
            {
                _imageTable = new PdfImageTable (this);
            }

            return _imageTable;
        }
    }

    private PdfImageTable? _imageTable;

    /// <summary>
    /// Gets the document form table that holds all form external objects used in the current document.
    /// </summary>
    internal PdfFormXObjectTable FormTable => _formTable ??= new PdfFormXObjectTable (this); // TODO: Rename to ExternalDocumentTable.

    private PdfFormXObjectTable _formTable;

    /// <summary>
    /// Gets the document ExtGState table that holds all form state objects used in the current document.
    /// </summary>
    internal PdfExtGStateTable ExtGStateTable => _extGStateTable ??= new PdfExtGStateTable (this);

    private PdfExtGStateTable _extGStateTable;

    /// <summary>
    /// Gets the PdfCatalog of the current document.
    /// </summary>
    internal PdfCatalog Catalog => _catalog ??= _trailer.Root;

    private PdfCatalog _catalog; // never changes if once created

    /// <summary>
    /// Gets the PdfInternals object of this document, that grants access to some internal structures
    /// which are not part of the public interface of PdfDocument.
    /// </summary>
    public new PdfInternals Internals => _internals ?? (_internals = new PdfInternals (this));

    private PdfInternals _internals;

    /// <summary>
    /// Creates a new page and adds it to this document.
    /// Depending of the IsMetric property of the current region the page size is set to
    /// A4 or Letter respectively. If this size is not appropriate it should be changed before
    /// any drawing operations are performed on the page.
    /// </summary>
    public PdfPage AddPage()
    {
        if (!CanModify)
        {
            throw new InvalidOperationException (PSSR.CannotModify);
        }

        return Catalog.Pages.Add();
    }

    /// <summary>
    /// Adds the specified page to this document. If the page is from an external document,
    /// it is imported to this document. In this case the returned page is not the same
    /// object as the specified one.
    /// </summary>
    public PdfPage AddPage (PdfPage page, AnnotationCopyingType annotationCopying = AnnotationCopyingType.DoNotCopy)
    {
        if (!CanModify)
        {
            throw new InvalidOperationException (PSSR.CannotModify);
        }

        return Catalog.Pages.Add (page, annotationCopying);
    }

    /// <summary>
    /// Creates a new page and inserts it in this document at the specified position.
    /// </summary>
    public PdfPage InsertPage (int index)
    {
        if (!CanModify)
        {
            throw new InvalidOperationException (PSSR.CannotModify);
        }

        return Catalog.Pages.Insert (index);
    }

    /// <summary>
    /// Inserts the specified page in this document. If the page is from an external document,
    /// it is imported to this document. In this case the returned page is not the same
    /// object as the specified one.
    /// </summary>
    public PdfPage InsertPage (int index, PdfPage page,
        AnnotationCopyingType annotationCopying = AnnotationCopyingType.DoNotCopy)
    {
        if (!CanModify)
        {
            throw new InvalidOperationException (PSSR.CannotModify);
        }

        return Catalog.Pages.Insert (index, page, annotationCopying);
    }

    /// <summary>
    /// Gets the security handler.
    /// </summary>
    public PdfStandardSecurityHandler? SecurityHandler => _trailer?.SecurityHandler;

    internal PdfTrailer? _trailer;
    internal PdfCrossReferenceTable _irefTable;
    internal Stream? _outStream;

    // Imported Document
    internal Lexer _lexer;

    internal DateTime _creation;

    /// <summary>
    /// Occurs when the specified document is not used anymore for importing content.
    /// </summary>
    internal void OnExternalDocumentFinalized (DocumentHandle handle)
    {
        if (tls != null)
        {
            //PdfDocument[] documents = tls.Documents;
            tls.DetachDocument (handle);
        }

        if (_formTable != null)
        {
            _formTable.DetachDocument (handle);
        }
    }

    //internal static GlobalObjectTable Gob = new GlobalObjectTable();

    /// <summary>
    /// Gets the ThreadLocalStorage object. It is used for caching objects that should created
    /// only once.
    /// </summary>
    internal static ThreadLocalStorage Tls => tls ?? (tls = new ThreadLocalStorage());

    [ThreadStatic] private static ThreadLocalStorage tls;

    [DebuggerDisplay ("(ID={ID}, alive={IsAlive})")]
    internal class DocumentHandle
    {
        public DocumentHandle (PdfDocument document)
        {
            _weakRef = new WeakReference (document);
            ID = document._guid.ToString ("B").ToUpper();
        }

        public bool IsAlive => _weakRef.IsAlive;

        public PdfDocument? Target => _weakRef.Target as PdfDocument;

        private readonly WeakReference _weakRef;

        public string ID;

        public override bool Equals (object? obj)
        {
            return obj is DocumentHandle handle && ID == handle.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator == (DocumentHandle? left, DocumentHandle? right)
        {
            return left?.Equals (right) ?? ReferenceEquals (right, null);
        }

        public static bool operator != (DocumentHandle? left, DocumentHandle? right)
        {
            return !(left == right);
        }
    }
}
