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

#nullable enable

namespace PdfSharpCore.Pdf.Annotations;

/// <summary>
/// Represents a generic annotation. Used for annotation dictionaries unknown to PdfSharpCore.
/// </summary>
internal sealed class PdfGenericAnnotation
    : PdfAnnotation
{
    #region Construction

    //DMH 6/7/06
    //Make this public so we can use it in PdfAnnotations to
    //get the Meta data from existings annotations.
    public PdfGenericAnnotation (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    internal new class Keys
        : PdfAnnotation.Keys
    {
        public static DictionaryMeta Meta => _meta ??= CreateMeta (typeof (Keys));

        private static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta => Keys.Meta;
}
