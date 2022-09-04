// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfAcroForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.AcroForms;

/// <summary>
/// Represents a interactive form (or AcroForm), a collection of fields for
/// gathering information interactively from the user.
/// </summary>
public sealed class PdfAcroForm : PdfDictionary
{
    /// <summary>
    /// Initializes a new instance of AcroForm.
    /// </summary>
    internal PdfAcroForm (PdfDocument document)
        : base (document)
    {
        _document = document;
    }

    internal PdfAcroForm (PdfDictionary dictionary)
        : base (dictionary)
    {
    }

    /// <summary>
    /// Gets the fields collection of this form.
    /// </summary>
    public PdfAcroField.PdfAcroFieldCollection Fields
    {
        get
        {
            if (_fields == null)
            {
                object o = Elements.GetValue (Keys.Fields, VCF.CreateIndirect);
                _fields = (PdfAcroField.PdfAcroFieldCollection)o;
            }

            return _fields;
        }
    }

    PdfAcroField.PdfAcroFieldCollection _fields;

    /// <summary>
    /// Predefined keys of this dictionary.
    /// The description comes from PDF 1.4 Reference.
    /// </summary>
    public sealed class Keys : KeysBase
    {
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// (Required) An array of references to the document�s root fields (those with
        /// no ancestors in the field hierarchy).
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Required, typeof (PdfAcroField.PdfAcroFieldCollection))]
        public const string Fields = "/Fields";

        /// <summary>
        /// (Optional) A flag specifying whether to construct appearance streams and
        /// appearance dictionaries for all widget annotations in the document.
        /// Default value: false.
        /// </summary>
        [KeyInfo (KeyType.Boolean | KeyType.Optional)]
        public const string NeedAppearances = "/NeedAppearances";

        /// <summary>
        /// (Optional; PDF 1.3) A set of flags specifying various document-level characteristics
        /// related to signature fields.
        /// Default value: 0.
        /// </summary>
        [KeyInfo ("1.3", KeyType.Integer | KeyType.Optional)]
        public const string SigFlags = "/SigFlags";

        /// <summary>
        /// (Required if any fields in the document have additional-actions dictionaries
        /// containing a C entry; PDF 1.3) An array of indirect references to field dictionaries
        /// with calculation actions, defining the calculation order in which their values will
        /// be recalculated when the value of any field changes.
        /// </summary>
        [KeyInfo (KeyType.Array)] public const string CO = "/CO";

        /// <summary>
        /// (Optional) A document-wide default value for the DR attribute of variable text fields.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional)]
        public const string DR = "/DR";

        /// <summary>
        /// (Optional) A document-wide default value for the DA attribute of variable text fields.
        /// </summary>
        [KeyInfo (KeyType.String | KeyType.Optional)]
        public const string DA = "/DA";

        /// <summary>
        /// (Optional) A document-wide default value for the Q attribute of variable text fields.
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Optional)]
        public const string Q = "/Q";

        /// <summary>
        /// Gets the KeysMeta for these keys.
        /// </summary>
        internal static DictionaryMeta Meta
        {
            get
            {
                if (s_meta == null)
                    s_meta = CreateMeta (typeof (Keys));
                return s_meta;
            }
        }

        static DictionaryMeta s_meta;

        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta
    {
        get { return Keys.Meta; }
    }
}
