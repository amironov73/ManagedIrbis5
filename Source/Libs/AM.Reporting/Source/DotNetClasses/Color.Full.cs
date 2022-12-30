// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

#if !NETSTANDARD
namespace System.Drawing
{
    public class ColorExt
    {
        public static bool IsKnownColor (Color color)
        {
            return color.IsKnownColor;
        }

        public static KnownColor ToKnownColor (Color c)
        {
            return c.ToKnownColor();
        }

        public static Color FromKnownColor (KnownColor knownColor)
        {
            return Color.FromKnownColor (knownColor);
        }

        public static bool IsSystemColor (Color c)
        {
            return c.IsSystemColor;
        }
    }
}
#endif
