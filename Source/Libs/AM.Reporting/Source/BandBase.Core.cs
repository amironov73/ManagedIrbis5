// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* BandBase.Core.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting;

partial class BandBase
{
    /// <inheritdoc cref="ReportComponentBase.Draw" />
    public override void Draw
        (
            FRPaintEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        DrawBackground (eventArgs);
        Border.Draw (eventArgs, new RectangleF (AbsLeft, AbsTop, Width, Height));
    }
}
