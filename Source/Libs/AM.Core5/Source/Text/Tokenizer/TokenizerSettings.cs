﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* TokenizerSettings.cs -- settings for StringTokenizer
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Text.Tokenizer
{
    /// <summary>
    /// Settings for <see cref="StringTokenizer"/>
    /// </summary>
    [DebuggerDisplay("IgnoreNewLine={IgnoreNewLine} "
        + "IgnoreWhitespace={IgnoreWhitespace}")]
    public sealed class TokenizerSettings
    {
        #region Properties

        /// <summary>
        /// Ignore newline.
        /// </summary>
        [DefaultValue(true)]
        public bool IgnoreNewLine { get; set; }

        /// <summary>
        /// Ignore whitespace.
        /// </summary>
        [DefaultValue(true)]
        public bool IgnoreWhitespace { get; set; }

        /// <summary>
        /// Ignore EOF in AllTokens().
        /// </summary>
        [DefaultValue(true)]
        public bool IgnoreEOF { get; set; }

        /// <summary>
        /// Symbol characters.
        /// </summary>
        public char[] SymbolChars { get; set; }

        /// <summary>
        /// Unescape strings.
        /// </summary>
        [DefaultValue(false)]
        public bool UnescapeStrings { get; set; }

        /// <summary>
        /// Trim delimiter
        /// </summary>
        [DefaultValue(true)]
        public bool TrimDelimiter { get; set; }

        /// <summary>
        /// Array of the combined symbols.
        /// </summary>
        public string[] CombinedSymbols { get; set; }

        /// <summary>
        /// Trim quotes.
        /// </summary>
        public bool TrimQuotes { get; set; }

        /// <summary>
        /// Accept floating point number.
        /// </summary>
        [DefaultValue(true)]
        public bool AcceptFloatingPoint { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TokenizerSettings()
        {
            IgnoreNewLine = true;
            IgnoreWhitespace = true;
            TrimDelimiter = true;
            AcceptFloatingPoint = true;
            SymbolChars = new []
            {
                '=', '+', '-', '/', ',', '.', '*', '~', '!', '@',
                '#', '$', '%', '^', '&', '(', ')', '{', '}', '[',
                ']', ':', ';', '<', '>', '?', '|', '\\'
            };
            CombinedSymbols = new[]
            {
                "+=", "-=", "*=", "/=", "%=", "<=", ">=", "<<",
                ">>", "==", "!=", "++", "--"
            };
        }

        #endregion

        #region Public methods

        #endregion
    }
}
