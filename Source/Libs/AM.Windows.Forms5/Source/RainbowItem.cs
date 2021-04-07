// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* RainbowItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    [ToolboxItem(false)]
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public class RainbowItem
        : Component
    {
        ///<summary>
        /// Color.
        ///</summary>
        public Color Color { get; set; }

        ///<summary>
        /// Position.
        ///</summary>
        public float Position { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RainbowItem()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RainbowItem
            (
                Color color,
                float position
            )
        {
            Color = color;
            Position = position;
        }

    }
}
