// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* BibTexRecord.cs -- BibTex-запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.BibTex
{
    /// <summary>
    /// BibTex-запись.
    /// </summary>
    public sealed class BibTexRecord
    {
        #region Properties

        /// <summary>
        /// Тип записи.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Метка записи.
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
        /// Поля записи.
        /// </summary>
        public List<BibTexField> Fields { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BibTexRecord()
        {
            Fields = new List<BibTexField>();

        } // constructor

        #endregion

    } // class BibTexRecord

} // namespace ManagedIrbis.BibTex
