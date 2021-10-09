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

/* BibTexFile.cs -- файл с BibTex-записями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.BibTex
{
    //
    // BibTeX использует bib-файлы специального текстового формата
    // для хранения списков библиографических записей. Каждая запись
    // описывает ровно одну публикацию - статью, книгу, диссертацию,
    // и т. д.
    //

    /// <summary>
    /// Файл с BibTex-записями.
    /// </summary>
    public sealed class BibTexFile
    {
        #region Properties

        /// <summary>
        /// Записи.
        /// </summary>
        public List<BibTexRecord> Records { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BibTexFile()
        {
            Records = new List<BibTexRecord>();

        } // constructor

        #endregion

    } // class BibTexFile

} // namespace ManagedIrbis.BibTex
