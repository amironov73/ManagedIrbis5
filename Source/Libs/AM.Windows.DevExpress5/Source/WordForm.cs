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

/* WordForm.cs -- минимальный Word
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
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;

#endregion

#nullable enable

namespace AM.Windows.DevEx;

/// <summary>
/// Минимальный Word.
/// </summary>
public sealed partial class WordForm
    : XtraForm
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WordForm()
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
        Sure.FileExists (fileName);

        var content = File.ReadAllBytes (fileName);
        using var stream = new MemoryStream (content);
        richEditControl1.LoadDocument (stream);
    }

    /// <summary>
    /// Сохранение текста в указанный файл.
    /// </summary>
    public void SaveFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        using var stream = File.OpenWrite (fileName);
        richEditControl1.SaveDocument (stream, DocumentFormat.OpenXml);
    }

    #endregion
}
