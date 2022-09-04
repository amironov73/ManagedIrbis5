// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfListBoxField.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.AcroForms;

/// <summary>
/// Represents the list box field.
/// </summary>
public sealed class PdfListBoxField
    : PdfChoiceField
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of PdfListBoxField.
    /// </summary>
    internal PdfListBoxField (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    internal PdfListBoxField (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets or sets the index of the selected item
    /// </summary>
    public int SelectedIndex
    {
        get
        {
            var value = Elements.GetString (Keys.V);
            return IndexInOptArray (value);
        }
        set
        {
            var key = ValueInOptArray (value);
            Elements.SetString (Keys.V, key);
        }
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// The description comes from PDF 1.4 Reference.
    /// </summary>
    public new class Keys : PdfAcroField.Keys
    {
        // List boxes have no additional entries.

        internal static DictionaryMeta Meta
        {
            get { return _meta ??= CreateMeta (typeof (Keys)); }
        }

        static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta
    {
        get { return Keys.Meta; }
    }
}
