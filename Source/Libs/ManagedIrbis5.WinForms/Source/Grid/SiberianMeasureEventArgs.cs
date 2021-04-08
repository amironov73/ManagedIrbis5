// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianMeasureEventArgs.cs --
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
    public class SiberianMeasureEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Dimensions.
        /// </summary>
        public SiberianDimensions Dimensions { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dimensions"></param>
        public SiberianMeasureEventArgs
            (
                SiberianDimensions dimensions
            )
        {
            Dimensions = dimensions;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString() => Dimensions.ToString();

        #endregion
    }
}
