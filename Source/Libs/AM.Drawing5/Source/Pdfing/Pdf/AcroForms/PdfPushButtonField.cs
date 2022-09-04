// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfPushButtonField.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.AcroForms;

/// <summary>
/// Represents the push button field.
/// </summary>
public sealed class PdfPushButtonField
    : PdfButtonField
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of PdfPushButtonField.
    /// </summary>
    internal PdfPushButtonField (PdfDocument document)
        : base (document)
    {
        _document = document;
    }

    internal PdfPushButtonField (PdfDictionary dict)
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
