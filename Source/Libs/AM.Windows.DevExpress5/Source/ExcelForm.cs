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

/* ExcelForm.cs -- минимальный Excel
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;

#endregion

#nullable enable

namespace AM.Windows.DevEx;

/// <summary>
/// Минимальный Excel.
/// </summary>
public sealed partial class ExcelForm
    : XtraForm
{
    #region Construction

    public ExcelForm()
    {
        InitializeComponent();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Загрузка таблицы из указанного файла.
    /// </summary>
    public void LoadFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var content = File.ReadAllBytes (fileName);
        using var stream = new MemoryStream (content);
        _spreadsheetControl.LoadDocument (stream);
    }

    /// <summary>
    /// Сохранение таблицы в указанный файл.
    /// </summary>
    public void SaveFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        using var stream = File.OpenWrite (fileName);
        _spreadsheetControl.SaveDocument (stream, DocumentFormat.OpenXml);
    }

    #endregion
}
