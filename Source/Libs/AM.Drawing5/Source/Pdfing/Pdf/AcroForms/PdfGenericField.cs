// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfGenericField.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.AcroForms;

/// <summary>
/// Represents a generic field. Used for AcroForm dictionaries unknown to PdfSharpCore.
/// </summary>
public sealed class PdfGenericField : PdfAcroField
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of PdfGenericField.
    /// </summary>
    internal PdfGenericField (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    internal PdfGenericField (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Predefined keys of this dictionary.
    /// The description comes from PDF 1.4 Reference.
    /// </summary>
    public new class Keys : PdfAcroField.Keys
    {
        internal static DictionaryMeta Meta
        {
            get { return _meta ??= CreateMeta (typeof (Keys)); }
        }

        static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta => Keys.Meta;
}
