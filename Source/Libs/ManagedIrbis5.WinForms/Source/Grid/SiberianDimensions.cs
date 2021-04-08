// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianDimensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianDimensions
    {
        #region Properties

        /// <summary>
        /// Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Width.
        /// </summary>
        public int Width { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianDimensions()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianDimensions
            (
                int width,
                int height
            )
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianDimensions
            (
                Size size
            )
        {
            Width = size.Width;
            Height = size.Height;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert to <see cref="Size"/>.
        /// </summary>
        public Size ToSize()
        {
            var result = new Size(Width, Height);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString() => $"{{Width={Width}, Height={Height}}}";

        #endregion
    }
}
