﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftTextSeparator.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using AM.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// <see cref="TextSeparator"/> for PFT.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftTextSeparator
        : TextSeparator
    {
        #region Properties

        /// <summary>
        /// Accumulates result text.
        /// </summary>
        [NotNull]
        public string Accumulator
        {
            get { return _accumulator.ToString(); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTextSeparator()
        {
            _accumulator = new StringBuilder();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTextSeparator
            (
                [NotNull] string open,
                [NotNull] string close
            )
            : base(open, close)
        {
            _accumulator = new StringBuilder();
        }

        #endregion

        #region Private members

        private readonly StringBuilder _accumulator;

        #endregion

        #region TextSeparator members

        /// <inheritdoc cref="TextSeparator.HandleChunk" />
        protected override void HandleChunk
            (
                bool inner,
                string text
            )
        {
            if (inner)
            {
                _accumulator.Append(text);
            }
            else
            {
                if (!string.IsNullOrEmpty(text))
                {
                    _accumulator.Append("<<<");
                    _accumulator.Append(text);
                    _accumulator.Append(">>>");
                }
            }
        }

        #endregion
    }
}
