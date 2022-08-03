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

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.FictionBook.Entities;

public interface IBook:
    IComparable
{
    string BookFile { get; set; }
    string BookAuthorFirstName { get; set; }
    string BookAuthorLastName { get; set; }
    string BookAuthorMiddleName { get; set; }
    string BookGenre { get; set; }
    string BookEncoding { get; set; }
    string BookTitle { get; set; }
    string BookSequenceName { get; set; }
    string BookVersion { get; set; }
    int? BookSequenceNr { get; set; }
    string BookLang { get; set; }
    string BookSizeText { get; set; }
}
