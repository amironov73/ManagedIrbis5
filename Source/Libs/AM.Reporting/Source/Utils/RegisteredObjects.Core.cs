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

using System.Drawing;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    partial class ObjectInfo
    {
        #region Private Methods

        partial void UpdateDesign (Bitmap image, int imageIndex, int buttonIndex = -1);

        /// <summary>
        /// Does nothing.
        /// </summary>
        partial void UpdateDesign (int flags, bool multiInsert, Bitmap image, int imageIndex, int buttonIndex = -1);

        #endregion Private Methods
    }
}
