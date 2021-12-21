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

using DevExpress.Utils;
using DevExpress.XtraPrinting;

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

    // ReSharper disable UnusedMember.Local
    private static void TestReporting()
    // ReSharper restore UnusedMember.Local
    {
        MyData[] data =
        {
            new () { Day = "ПН", Amount = 10 },
            new () { Day = "ВТ", Amount = 20 },
            new () { Day = "СР", Amount = 30 }
        };

        using var report = new EasyReport (data);
        var width = report.Width / 4;

        var band = report.AddBand (10);
        band.Font = new Font (FontFamily.GenericSansSerif, 14F);

        var dayLabel = band.AddLabel (width);
        dayLabel.DataBindings.Add (nameof (dayLabel.Text), null, nameof (MyData.Day));

        var valueLabel = band.AddLabel (width);
        valueLabel.BackColor = Color.Bisque;
        valueLabel.Font = new Font (band.Font, FontStyle.Bold);
        valueLabel.DataBindings.Add (nameof (dayLabel.Text), null, nameof (MyData.Amount));

        report.ExportToPdf ("report.pdf");
        report.ExportToXlsx ("report.xlsx");

        // report.Report.ShowPreviewDialog();
    }

    private static void TestPrinting()
    {
        var printing = new EasyPrinting();
        var graphics = printing.Graphics;
        graphics.BackColor = Color.White;
        graphics.ForeColor = Color.Blue;

        var table = new PageTableBrick();
        for (var i = 0; i < 10; i++)
        {
            var row = table.Rows.AddRow();
            for (int column = 0; column < 3; column++)
            {
                LabelBrick cell = new LabelBrick
                {
                    HorzAlignment = HorzAlignment.Center,
                    VertAlignment = VertAlignment.Center,
                    Size = new SizeF(100, 20),
                    Text = $"{i} x {column}"
                };
                row.Bricks.Add(cell);
            }
        }

        table.UpdateSize();
        graphics.DrawBrick(table);

        printing.ExportToImage ("printing.png");
        // printing.ShowPreview();
    }

    public static void Main (string[] args)
    {
        TestPrinting();
    }
}
