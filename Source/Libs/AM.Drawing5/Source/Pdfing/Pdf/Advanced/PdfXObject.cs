// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Base class for all PDF external objects.
/// </summary>
public abstract class PdfXObject
    : PdfDictionary
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfXObject"/> class.
    /// </summary>
    /// <param name="document">The document that owns the object.</param>
    protected PdfXObject (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }
    #endregion

    #region Nested classes

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    public class Keys
        : PdfStream.Keys
    {
        // пустое тело класса
    }

    #endregion

}
