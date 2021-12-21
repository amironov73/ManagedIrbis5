// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

using AM.Windows.DevExpress;

#endregion

#nullable enable

static class Program
{
    // Класс с некими данными
    sealed class MyData
    {
        // День недели
        public string? Day { get; set; }

        // Количество
        public int Amount { get; set; }
    }

    public static void Main (string[] args)
    {
        MyData[] data =
        {
            new() { Day = "ПН", Amount = 10 },
            new() { Day = "ВТ", Amount = 20 },
            new() { Day = "СР", Amount = 30 }
        };

        using var report = new EasyReport (data);
        var width = report.Width / 4;

        var band = report.AddBand (10);
        band.Font = new Font (FontFamily.GenericSansSerif, 14F);

        var dayLabel = band.AddLabel(width);
        dayLabel.DataBindings.Add (nameof (dayLabel.Text), null, nameof (MyData.Day));

        var valueLabel = band.AddLabel (width);
        valueLabel.BackColor = Color.Bisque;
        valueLabel.Font = new Font (band.Font, FontStyle.Bold);
        valueLabel.DataBindings.Add (nameof (dayLabel.Text), null, nameof (MyData.Amount));

        report.ExportToPdf ("report.pdf");
        report.ExportToXlsx ("report.xlsx");
        // report.Report.ShowPreviewDialog();
    }
}
