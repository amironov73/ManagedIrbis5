// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* FB2Book.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.FictionBook.Entities;

/// <summary>
///
/// </summary>
public class FB2Book
    : IBook
{
    #region IComparable Members

    /// <summary>
    ///
    /// </summary>
    public int CompareTo (object? obj)
    {
        if (obj is not IBook fc)
        {
            throw new InvalidCastException();
        }

        var result = string.Compare (BookAuthorLastName, fc.BookAuthorLastName,
            StringComparison.InvariantCultureIgnoreCase);
        if (result == 0)
        {
            result = string.Compare (BookAuthorFirstName, fc.BookAuthorFirstName,
                StringComparison.InvariantCultureIgnoreCase);
        }

        if (result == 0)
        {
            result = string.Compare (BookSequenceName, fc.BookSequenceName,
                StringComparison.InvariantCultureIgnoreCase);
        }

        if (result == 0)
        {
            result = Comparer<int?>.Default.Compare (BookSequenceNr, fc.BookSequenceNr);
        }

        if (result == 0)
        {
            result = string.Compare (BookTitle, fc.BookTitle, StringComparison.InvariantCultureIgnoreCase);
        }

        return result;
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    public string BookAuthorFirstName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookAuthorLastName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookAuthorMiddleName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookGenre { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookEncoding { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookTitle { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookSequenceName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookVersion { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int? BookSequenceNr { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookLang { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookSizeText { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BookFile { get; set; }

    /// <summary>
    ///
    /// </summary>
    public FB2Book()
    {
        BookGenre = string.Empty;
        BookVersion = string.Empty;
        BookSizeText = string.Empty;
        BookFile = string.Empty;
        BookAuthorFirstName = string.Empty;
        BookAuthorLastName = string.Empty;
        BookAuthorMiddleName = string.Empty;
        BookEncoding = string.Empty;
        BookTitle = string.Empty;
        BookSequenceName = string.Empty;
        BookSequenceNr = null;
        BookLang = string.Empty;
    }
}
