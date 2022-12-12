// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local

/* IBook.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.FictionBook.Entities;

/// <summary>
///
/// </summary>
public interface IBook:
    IComparable
{
    /// <summary>
    ///
    /// </summary>
    string BookFile { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookAuthorFirstName { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookAuthorLastName { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookAuthorMiddleName { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookGenre { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookEncoding { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookTitle { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookSequenceName { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookVersion { get; set; }

    /// <summary>
    ///
    /// </summary>
    int? BookSequenceNr { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookLang { get; set; }

    /// <summary>
    ///
    /// </summary>
    string BookSizeText { get; set; }
}
