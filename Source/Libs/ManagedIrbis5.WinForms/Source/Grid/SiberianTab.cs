// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianTab.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianTab
    {
        #region Properties

        /// <summary>
        /// Tab title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Fields.
        /// </summary>
        public NonNullCollection<SiberianField> Fields { get; private set; }

        /// <summary>
        /// Modified?
        /// </summary>
        public bool Modified { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianTab()
        {
            Fields = new NonNullCollection<SiberianField>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
