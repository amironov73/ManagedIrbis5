// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridPalette.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public sealed class TreeGridPalette
        : Palette
    {
        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public TreeGridPalette()
        {
            InitializeFromAttributes();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the foreground.
        /// </summary>
        /// <value>The foreground.</value>
        [PaletteColor("Black")]
        public Tube Foreground
        {
            get { return GetTubeFromReflection(); }
        }

        /// <summary>
        /// Gets the backrground.
        /// </summary>
        /// <value>The backrground.</value>
        [PaletteColor("White")]
        public Tube Backrground
        {
            get { return GetTubeFromReflection(); }
        }

        /// <summary>
        /// Gets the selected foreground.
        /// </summary>
        /// <value>The selected foreground.</value>
        [PaletteColor("White")]
        public Tube SelectedForeground
        {
            get { return GetTubeFromReflection(); }
        }

        /// <summary>
        /// Gets the selected background.
        /// </summary>
        /// <value>The selected background.</value>
        [PaletteColor("Blue")]
        public Tube SelectedBackground
        {
            get { return GetTubeFromReflection(); }
        }

        /// <summary>
        /// Gets the lines.
        /// </summary>
        /// <value>The lines.</value>
        [PaletteColor("DarkGray")]
        public Tube Lines
        {
            get { return GetTubeFromReflection(); }
        }

        /// <summary>
        /// Gets the disabled.
        /// </summary>
        /// <value>The disabled.</value>
        [PaletteColor("Gray")]
        public Tube Disabled
        {
            get { return GetTubeFromReflection(); }
        }

        /// <summary>
        /// Gets the header background.
        /// </summary>
        /// <value>The header background.</value>
        [PaletteColor("LightGray")]
        public Tube HeaderBackground
        {
            get { return GetTubeFromReflection(); }
        }

        /// <summary>
        /// Gets the header foreground.
        /// </summary>
        /// <value>The header foreground.</value>
        [PaletteColor("Black")]
        public Tube HeaderForeground
        {
            get { return GetTubeFromReflection(); }
        }

        #endregion
    }
}
