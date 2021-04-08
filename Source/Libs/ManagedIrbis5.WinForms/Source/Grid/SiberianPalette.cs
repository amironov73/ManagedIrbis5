// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianPalette.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Text.Json.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Palette of UI colors.
    /// </summary>
    public class SiberianPalette
    {
        #region Properties

        /// <summary>
        /// Background (non-selected) color.
        /// </summary>
        [JsonPropertyName("back-color")]
        public Color BackColor { get; set; }

        /// <summary>
        /// Background color for disabled elements.
        /// </summary>
        [JsonPropertyName("disabled-back-color")]
        public Color DisabledBackColor { get; set; }

        /// <summary>
        /// Foreground color for disabled elements.
        /// </summary>
        [JsonPropertyName("disabled-fore-color")]
        public Color DisabledForeColor { get; set; }

        /// <summary>
        /// Foreground (non-selected) color.
        /// </summary>
        [JsonPropertyName("fore-color")]
        public Color ForeColor { get; set; }

        /// <summary>
        /// Background color for header.
        /// </summary>
        [JsonPropertyName("header-back-color")]
        public Color HeaderBackColor { get; set; }

        /// <summary>
        /// Foreground color for header.
        /// </summary>
        [JsonPropertyName("header-fore-color")]
        public Color HeaderForeColor { get; set; }

        /// <summary>
        /// Line color.
        /// </summary>
        [JsonPropertyName("line-color")]
        public Color LineColor { get; set; }

        /// <summary>
        /// Name of the palette.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Selected background color.
        /// </summary>
        [JsonPropertyName("selected-back-color")]
        public Color SelectedBackColor { get; set; }

        /// <summary>
        /// Selected foreground color.
        /// </summary>
        [JsonPropertyName("selected-fore-color")]
        public Color SelectedForeColor { get; set; }

        /// <summary>
        /// Default palette.
        /// </summary>
        public static SiberianPalette DefaultPalette => _defaultPalette;

        #endregion

        #region Construction

        static SiberianPalette()
        {
            _defaultPalette = new SiberianPalette
            {
                Name = "Default",

                BackColor = Color.White,
                ForeColor = Color.Black,

                HeaderBackColor = Color.LightGray,
                HeaderForeColor = Color.Black,

                LineColor = Color.Gray,

                DisabledBackColor = Color.White,
                DisabledForeColor = Color.DarkGray,

                SelectedBackColor = Color.Blue,
                SelectedForeColor = Color.White
            };
        }

        #endregion

        #region Private members

        private static readonly SiberianPalette _defaultPalette;

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the palette.
        /// </summary>
        public SiberianPalette Clone()
        {
            return (SiberianPalette) MemberwiseClone();
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString() => Name.ToVisibleString();

        #endregion
    }
}
