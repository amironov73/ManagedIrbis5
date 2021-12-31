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

/* EndNoteRecord.cs --
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

namespace ManagedIrbis.Metadata.EndNote;

//
// https://en.wikipedia.org/wiki/EndNote
//
// EndNote is a commercial reference management software package,
// used to manage bibliographies and references when writing essays
// and articles. It is produced by Clarivate Analytics (previously
// by Thomson Reuters).
//

/// <summary>
///
/// </summary>
public sealed class EndNoteRecord
{
    #region Properties

    /// <summary>
    /// Fields.
    /// </summary>
    public FieldCollection Fields { get; } = new ();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public EndNoteRecord()
    {
    }

    #endregion
}
