// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TermPosting.cs -- постинг термина
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Постинг термина.
    /// </summary>
    public sealed class TermPosting
    {
        #region Properties

        /// <summary>
        /// MFN записи с искомым термом.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Тег поля с искомым термом.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Количество повторений.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Результат форматирования.
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods


        #endregion
    }
}
