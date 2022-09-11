// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Annotations;

/// <summary>
/// Represents a rubber stamp annotation.
/// </summary>
public sealed class PdfRubberStampAnnotation
    : PdfAnnotation
{
    #region Construciton

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfRubberStampAnnotation"/> class.
    /// </summary>
    public PdfRubberStampAnnotation()
    {
        Initialize();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfRubberStampAnnotation"/> class.
    /// </summary>
    /// <param name="document">The document.</param>
    public PdfRubberStampAnnotation (PdfDocument document)
        : base (document)
    {
        Initialize();
    }

    #endregion

    private void Initialize()
    {
        Elements.SetName (Keys.Subtype, "/Stamp");
        Color = XColors.Yellow;
    }

    /// <summary>
    /// Gets or sets an icon to be used in displaying the annotation.
    /// </summary>
    public PdfRubberStampAnnotationIcon Icon
    {
        get
        {
            var value = Elements.GetName (Keys.Name);
            if (value == "")
            {
                return PdfRubberStampAnnotationIcon.NoIcon;
            }

            value = value.Substring (1);
            if (!Enum.IsDefined (typeof (PdfRubberStampAnnotationIcon), value))
            {
                return PdfRubberStampAnnotationIcon.NoIcon;
            }

            return (PdfRubberStampAnnotationIcon)Enum.Parse (typeof (PdfRubberStampAnnotationIcon), value, false);
        }
        set
        {
            if (Enum.IsDefined (typeof (PdfRubberStampAnnotationIcon), value) &&
                PdfRubberStampAnnotationIcon.NoIcon != value)
            {
                Elements.SetName (Keys.Name, "/" + value.ToString());
            }
            else
            {
                Elements.Remove (Keys.Name);
            }
        }
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    internal new class Keys : PdfAnnotation.Keys
    {
        /// <summary>
        /// (Optional) The name of an icon to be used in displaying the annotation. Viewer
        /// applications should provide predefined icon appearances for at least the following
        /// standard names:
        ///   Approved
        ///   AsIs
        ///   Confidential
        ///   Departmental
        ///   Draft
        ///   Experimental
        ///   Expired
        ///   Final
        ///   ForComment
        ///   ForPublicRelease
        ///   NotApproved
        ///   NotForPublicRelease
        ///   Sold
        ///   TopSecret
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.Optional)]
        public const string Name = "/Name";

        public static DictionaryMeta Meta => _meta ??= CreateMeta (typeof (Keys));

        private static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta => Keys.Meta;
}
