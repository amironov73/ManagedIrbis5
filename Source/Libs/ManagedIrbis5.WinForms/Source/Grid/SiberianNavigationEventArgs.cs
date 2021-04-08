// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianNavigationEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianNavigationEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Cancellation flag.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// New column number.
        /// </summary>
        public int NewColumn { get; set; }

        /// <summary>
        /// New row number.
        /// </summary>
        public int NewRow { get; set; }

        /// <summary>
        /// Old column number.
        /// </summary>
        public int OldColumn { get; set; }

        /// <summary>
        /// Old row number.
        /// </summary>
        public int OldRow { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
