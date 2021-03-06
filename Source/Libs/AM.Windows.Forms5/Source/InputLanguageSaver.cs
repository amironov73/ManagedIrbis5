﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* InputLanguageSaver.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public sealed class InputLanguageSaver
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Previous language.
        /// </summary>
        public InputLanguage PreviousLanguage { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public InputLanguageSaver()
        {
            PreviousLanguage = InputLanguage.CurrentInputLanguage;

            Magna.Trace
                (
                    "InputLanguageSaver::Constructor: "
                    + "previous language="
                    + PreviousLanguage
                );
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Magna.Trace
                (
                    "InputLanguageSaver::Dispose: "
                    + "previous language="
                    + PreviousLanguage
                );

            InputLanguage.CurrentInputLanguage = PreviousLanguage;
        }

        #endregion

    }
}
