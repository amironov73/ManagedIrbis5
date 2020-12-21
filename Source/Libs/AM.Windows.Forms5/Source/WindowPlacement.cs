// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* WindowPlacement.cs -- места, где может быть расположена форма
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Места, где может быть расположена форма.
    /// </summary>
    public enum WindowPlacement
    {
        /// <summary>
        /// Center of the screen.
        /// </summary>
        [Description("Screen center")]
        ScreenCenter,

        /// <summary>
        /// Top left corner.
        /// </summary>
        [Description("Top left corner")]
        TopLeftCorner,

        /// <summary>
        /// Top right corner.
        /// </summary>
        [Description("Top right corner")]
        TopRightCorner,

        /// <summary>
        /// Center of the top side.
        /// </summary>
        [Description("Center of the top side")]
        TopSide,

        /// <summary>
        /// Center of the left side.
        /// </summary>
        [Description("Center of the left side")]
        LeftSide,

        /// <summary>
        /// Center of the right side.
        /// </summary>
        [Description("Center of the right side")]
        RightSide,

        /// <summary>
        /// Center of the bottom side.
        /// </summary>
        [Description("Center of the bottom side")]
        BottomSide,

        /// <summary>
        /// Bottom left corner.
        /// </summary>
        [Description("Bottom left corner")]
        BottomLeftCorner,

        /// <summary>
        /// Bottom right corner.
        /// </summary>
        [Description("Bottom right corner")]
        BottomRightCorner

    } // enum WindowPlacement

} // namespace AM.Windows.Forms
