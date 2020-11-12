// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Delimiters.cs -- широко используемые разделители значений в строке.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Широко используемые разделители в строке.
    /// </summary>
    public static class Delimiters
    {
        #region Public members

        /// <summary>
        /// Colon.
        /// </summary>
        [NotNull]
        public static readonly char[] Colon = { ':' };

        /// <summary>
        /// Comma.
        /// </summary>
        [NotNull]
        public static readonly char[] Comma = { ',' };

        /// <summary>
        /// Dot.
        /// </summary>
        [NotNull]
        public static readonly char[] Dot = { '.' };

        /// <summary>
        /// Semicolon.
        /// </summary>
        [NotNull]
        public static readonly char[] Semicolon = { ';' };

        /// <summary>
        /// Space.
        /// </summary>
        [NotNull]
        public static readonly char[] Space = { ' ' };

        /// <summary>
        /// Tab.
        /// </summary>
        [NotNull]
        public static readonly char[] Tab = { '\t' };

        /// <summary>
        /// Pipe sign.
        /// </summary>
        [NotNull]
        public static readonly char[] Pipe = { '|' };

        #endregion
    }
}
