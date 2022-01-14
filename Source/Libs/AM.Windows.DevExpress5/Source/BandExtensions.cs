// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BandExtensions.cs -- удобные расширения для работы с полосами отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

#endregion

#nullable enable

namespace AM.Windows.DevEx;

/// <summary>
/// Удобные расширения для работа с полосами отчета.
/// </summary>
public static class BandExtensions
{
    /// <summary>
    /// Добавление метки к полосе.
    /// </summary>
    public static XRLabel AddLabel
        (
            this Band band,
            int width = 0,
            BorderSide borders = BorderSide.All
        )
    {
        var result = new XRLabel()
        {
            CanGrow = true,
            CanShrink = true,
            Borders = borders,
            Padding = new PaddingInfo (3, 3, 3, 3)
        };

        if (width != 0)
        {
            result.Width = width;
        }

        XRControl? last = null;
        foreach (XRControl control in band.Controls)
        {
            last = control;
        }

        result.Left = last?.Right ?? 0;
        band.Controls.Add (result);

        return result;
    }
}
