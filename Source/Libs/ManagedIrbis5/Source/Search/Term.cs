// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Term.cs -- термин в поисковом словаре
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
    /// Термин в поисковом словаре.
    /// </summary>
    public sealed class Term
    {
        #region Properties

        /// <summary>
        /// Количество ссылок.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Поисковый термин.
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods


        #endregion
    }
}
