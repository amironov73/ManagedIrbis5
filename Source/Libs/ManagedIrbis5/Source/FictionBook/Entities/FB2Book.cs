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
    public int CompareTo (object obj)
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

    public string BookAuthorFirstName { get; set; }
    public string BookAuthorLastName { get; set; }
    public string BookAuthorMiddleName { get; set; }
    public string BookGenre { get; set; }
    public string BookEncoding { get; set; }
    public string BookTitle { get; set; }
    public string BookSequenceName { get; set; }
    public string BookVersion { get; set; }
    public int? BookSequenceNr { get; set; }
    public string BookLang { get; set; }
    public string BookSizeText { get; set; }
    public string BookFile { get; set; }

    public FB2Book()
    {
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
