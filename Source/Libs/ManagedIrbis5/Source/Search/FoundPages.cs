// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FoundPages.cs -- один результат полнотекстового поиска
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Text;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /*
        Из официальной документации:

        MFN #Pages#результат_форматирования
        Где Pages это список найденных страниц в порядке релевантности, разделенных символом #30 ($1F)

     */

    /// <summary>
    /// Один результат полнотекстового поиска.
    /// </summary>
    public sealed class FoundPages
    {
        #region Properties

        /// <summary>
        /// MFN найденной записи.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Номера страниц найденной записи.
        /// </summary>
        public int[]? Pages { get; set; }

        /// <summary>
        /// Результат расформатирования.
        /// </summary>
        public string? Formatted { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор строки ответа сервера.
        /// </summary>
        /// <param name="line">Строка из ответа сервера.</param>
        public void Decode
            (
                ReadOnlySpan<char> line
            )
        {
            throw new NotImplementedException();
        } // method Decode

        #endregion

    } // class FoundPages

} // namespace ManagedIrbis
