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

/* PdfForm.cs -- минимальный Acrobat
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

using DevExpress.XtraBars;

#endregion

#nullable enable

namespace AM.Windows.DevEx;

/// <summary>
/// Минимальный Acrobat.
/// </summary>
public sealed partial class PdfForm
    : DevExpress.XtraEditors.XtraForm
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PdfForm()
    {
        InitializeComponent();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Загрузка текста из указанного файла.
    /// </summary>
    public void LoadFile
        (
            string fileName
        )
    {
        Sure.FileExists(fileName);

        pdfViewer1.LoadDocument (fileName);
    }

    #endregion
}
