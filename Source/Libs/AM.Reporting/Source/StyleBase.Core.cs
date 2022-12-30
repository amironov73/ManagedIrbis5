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

using AM.Reporting.Utils;

using System.Drawing;

#endregion

#nullable enable

namespace AM.Reporting
{
    partial class StyleBase
    {
        #region Private Methods

        private Font GetDefaultFontInternal()
        {
            return DrawUtils.DefaultFont;
        }

        #endregion Private Methods
    }
}
