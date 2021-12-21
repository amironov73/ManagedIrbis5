// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EasyReport.cs -- легко и просто формируем отчет
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

#endregion

#nullable enable

namespace AM.Windows.DevExpress;

/// <summary>
/// Легко и просто формируем отчет силами PrintingSystem.
/// </summary>
public class EasyReport
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Отчет.
    /// </summary>
    public XtraReport Report { get; }

    /// <summary>
    /// Ширина рабочей области отчета.
    /// </summary>
    public int Width => Report.PageWidth - Report.Margins.Left - Report.Margins.Right;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public EasyReport()
    {
        Report = new XtraReport();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EasyReport
        (
            object? dataSource
        )
    {
        Report = new XtraReport()
        {
            DataSource = dataSource
        };
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление полосы.
    /// </summary>
    public Band AddBand
        (
            int height = 0,
            BorderSide borders = BorderSide.All
        )
    {
        var result = new DetailBand()
        {
            CanShrink = true,
            CanGrow = true,
            Borders = borders
        };

        if (height != 0)
        {
            result.Height = height;
        }

        Report.Bands.Add (result);

        return result;
    }

    /// <summary>
    /// Экспорт в формат PDF.
    /// </summary>
    public void ExportToPdf
        (
            string fileName
        )
    {
        Report.ExportToPdf (fileName);
    }

    /// <summary>
    /// Экспорт в формат XLSX.
    /// </summary>
    public void ExportToXlsx
        (
            string fileName
        )
    {
        Report.ExportToXlsx (fileName);
    }

    #endregion

    #region IDisposable members

    public void Dispose()
    {
        Report.Dispose();
    }

    #endregion
}
