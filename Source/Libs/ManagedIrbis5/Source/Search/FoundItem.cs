// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FoundLine.cs -- одна строка в ответе на поисковый запрос
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
    /// Одна строка в ответе сервера на поисковый запрос.
    /// </summary>
    [DebuggerDisplay("{Mfn} {Text}")]
    public sealed class FoundItem
    {
        #region Properties

        /// <summary>
        /// Text.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        #endregion

        #region Public methods


        #endregion
    }
}
