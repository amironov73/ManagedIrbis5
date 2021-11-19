// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Header.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Header
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public Sender? Sender { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Addressee? Addressee { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? SentDateTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? DefaultLanguageOfText { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? DefaultPriceType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? DefaultCurrencyCode { get; set; }

        #endregion
    }
}
