// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* RisRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Metadata.Ris;

//
// https://en.wikipedia.org/wiki/RIS_(file_format)
//
// RIS is a standardized tag format developed by Research Information
// Systems, Incorporated (the format name refers to the company)
// to enable citation programs to exchange data. It is supported
// by a number of reference managers. Many digital libraries,
// like IEEE Xplore, Scopus, the ACM Portal, Scopemed, ScienceDirect,
// SpringerLink, Rayyan QCRI and online library catalogs can export
// citations in this format. Major reference/citation manager
// applications, like Zotero, Citavi, Mendeley, and EndNote can
// export and import citations in this format.
//

/// <summary>
/// RIS record.
/// </summary>
public sealed class RisRecord
{
    #region Properties

    /// <summary>
    /// Field collection.
    /// </summary>
    public FieldCollection Fields { get; } = new ();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public RisRecord()
    {
    }

    #endregion
}
