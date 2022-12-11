// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToStaticMemberViaDerivedType
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfFileAttachmentAnnotation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using PdfSharpCore.Pdf.Advanced;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Annotations;

/// <summary>
/// Represent a file that is attached to the PDF
/// </summary>
public class PdfFileAttachmentAnnotation
    : PdfAnnotation
{
    /// <summary>
    /// Name of icons used in displaying the annotation.
    /// </summary>
    public enum IconType
    {
        /// <summary>
        ///
        /// </summary>
        Graph,

        /// <summary>
        ///
        /// </summary>
        PushPin,

        /// <summary>
        ///
        /// </summary>
        Paperclip,

        /// <summary>
        ///
        /// </summary>
        Tag
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfFileAttachmentAnnotation"/> class.
    /// </summary>
    public PdfFileAttachmentAnnotation()
    {
        Elements.SetName (Keys.Subtype, "/FileAttachment");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfFileAttachmentAnnotation"/> class.
    /// </summary>
    public PdfFileAttachmentAnnotation (PdfDocument document)
        : base (document)
    {
        Elements.SetName (Keys.Subtype, "/FileAttachment");
        Flags = PdfAnnotationFlags.Locked;
    }

    /// <summary>
    ///
    /// </summary>
    public IconType Icon
    {
        get
        {
            var iconName = Elements.GetName (Keys.Name);

            return iconName == null!
                ? IconType.PushPin
                : (IconType)(Enum.Parse (typeof (IconType), iconName));
        }
        set => Elements.SetName (Keys.Name, value.ToString());
    }

    /// <summary>
    ///
    /// </summary>
    public PdfFileSpecification? File
    {
        get
        {
            var reference = Elements.GetReference (Keys.FS);

            return reference?.Value as PdfFileSpecification;
        }
        set
        {
            if (value == null)
            {
                Elements.Remove (Keys.FS);
            }
            else
            {
                if (!value.IsIndirect)
                {
                    Owner!._irefTable.Add (value);
                }

                Elements.SetReference (Keys.FS, value);
            }
        }
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    internal new class Keys : PdfAnnotation.Keys
    {
        /// <summary>
        /// (Required) The file associated with this annotation.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Required)]
        public const string FS = "/FS";

        /// <summary>
        /// (Optional) The name of an icon to be used in displaying the annotation.
        /// Viewer applications should provide predefined icon appearances for at least
        /// the following standard names:
        ///
        /// Graph
        /// PushPin
        /// Paperclip
        /// Tag
        ///
        /// Additional names may be supported as well. Default value: PushPin.
        /// Note: The annotation dictionary’s AP entry, if present, takes precedence over
        /// the Name entry; see Table 8.15 on page 606 and Section 8.4.4, “Appearance Streams.”
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.Optional)]
        public const string Name = "/Name";

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
