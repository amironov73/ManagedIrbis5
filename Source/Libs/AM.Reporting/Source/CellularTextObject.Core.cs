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
using System;

#endregion

#nullable enable

namespace AM.Reporting
{
    partial class CellularTextObject
    {
        private float GetCellWidthInternal(float fontHeight)
        {
            return (int)Math.Round((fontHeight + 10) / (0.25f * Units.Centimeters)) * (0.25f * Units.Centimeters);
        }
    }
}
