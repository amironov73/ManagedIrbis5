// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfWidgetAnnotation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Pdf.Annotations;

/// <summary>
/// Represents a text annotation.
/// </summary>
internal sealed class PdfWidgetAnnotation
    : PdfAnnotation
{
    #region Construciton

    public PdfWidgetAnnotation()
    {
        Initialize();
    }

    public PdfWidgetAnnotation (PdfDocument document)
        : base (document)
    {
        Initialize();
    }

    #endregion

    private void Initialize()
    {
        Elements.SetName (Keys.Subtype, "/Widget");
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    internal new class Keys
        : PdfAnnotation.Keys
    {
        /// <summary>
        /// (Optional) The annotation�s highlighting mode, the visual effect to be used when
        /// the mouse button is pressed or held down inside its active area:
        ///   N (None) No highlighting.
        ///   I (Invert) Invert the contents of the annotation rectangle.
        ///   O (Outline) Invert the annotation�s border.
        ///   P (Push) Display the annotation�s down appearance, if any. If no down appearance is defined,
        ///     offset the contents of the annotation rectangle to appear as if it were being pushed below
        ///     the surface of the page.
        ///   T (Toggle) Same as P (which is preferred).
        /// A highlighting mode other than P overrides any down appearance defined for the annotation.
        /// Default value: I.
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.Optional)]
        public const string H = "/H";

        /// <summary>
        /// (Optional) An appearance characteristics dictionary to be used in constructing a dynamic
        /// appearance stream specifying the annotation�s visual presentation on the page.
        /// The name MK for this entry is of historical significance only and has no direct meaning.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional)]
        public const string MK = "/MK";

        public static DictionaryMeta Meta => _meta ??= CreateMeta (typeof (Keys));

        private static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta => Keys.Meta;
}
