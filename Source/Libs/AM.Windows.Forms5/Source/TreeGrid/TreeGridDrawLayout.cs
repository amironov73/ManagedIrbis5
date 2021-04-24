// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TreeGridDrawLayout.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    public sealed class TreeGridDrawLayout
    {
        #region Construction

        public TreeGridDrawLayout()
        {
            Expand = Rectangle.Empty;
            Check = Rectangle.Empty;
            Icon = Rectangle.Empty;
            Text = Rectangle.Empty;
        }

        #endregion

        #region Properties

        public Rectangle Expand { get; set; }

        public Rectangle Check { get; set; }

        public Rectangle Icon { get; set; }

        public Rectangle Text { get; set; }

        public string? TextOverride { get; set; }

        #endregion

        #region Public methods

        public TreeGridClickKind DetermineClickKind ( Point point )
        {
            TreeGridClickKind result = TreeGridClickKind.Unknown;

            if (Expand.Contains(point))
            {
                result = TreeGridClickKind.Expand;
            }
            else if (Check.Contains(point))
            {
                result = TreeGridClickKind.Check;
            }
            else if (Icon.Contains(point))
            {
                result = TreeGridClickKind.Icon;
            }
            else if (Text.Contains(point))
            {
                result = TreeGridClickKind.Text;
            }

            return result;
        }

        #endregion
    }
}
