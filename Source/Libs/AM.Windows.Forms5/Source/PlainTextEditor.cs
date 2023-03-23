// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* PlainTextEditor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Windows.Forms;

using AM.Windows.Forms.Printing;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Редактор для плоского текста с панелью инструментов.
/// </summary>
[PublicAPI]
public partial class PlainTextEditor
    : UserControl
{
    #region Properties

    private string? FileName { get; set; }

    /// <inheritdoc cref="UserControl.Text"/>
    [AllowNull]
    public override string Text
    {
        get => _textBox.Text;
        set
        {
            _textBox.Text = value;
            _textBox.Select (0, 0);
        }
    }

    /// <summary>
    /// Контрол, содержащий текст.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public TextBox TextBox => _textBox;

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PlainTextEditor()
    {
        InitializeComponent();
    }

    #endregion

    #region Private members

    private void _newToolStripButton_Click
        (
            object sender,
            EventArgs eventArgs
        )
    {
        Clear();
    }

    private void _openToolStripButton_Click
        (
            object sender,
            EventArgs eventArgs
        )
    {
        LoadFromFile();
    }

    private void _saveToolStripButton_Click
        (
            object sender,
            EventArgs eventArgs
        )
    {
        SaveToFile();
    }

    private void _printToolStripButton_Click
        (
            object sender,
            EventArgs eventArgs
        )
    {
        Print();
    }

    private void _cutToolStripButton_Click
        (
            object sender,
            EventArgs eventArgs
        )
    {
        Cut();
    }

    private void _copyToolStripButton_Click
        (
            object sender,
            EventArgs eventArgs
        )
    {
        Copy();
    }

    private void _pasteToolStripButton_Click
        (
            object sender,
            EventArgs eventArgs
        )
    {
        Paste();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление кнопки на панель инструментов.
    /// </summary>
    public PlainTextEditor AddButton
        (
            ToolStripButton button
        )
    {
        Sure.NotNull (button);

        _toolStrip.Items.Add (button);

        return this;
    }

    /// <summary>
    /// Удаление всего текста.
    /// </summary>
    public void Clear()
    {
        _textBox.Clear();
    }

    /// <summary>
    /// Копирование выделенного фрагмента в буфер обмена.
    /// </summary>
    public void Copy()
    {
        _textBox.Copy();
    }

    /// <summary>
    /// Вырезание выделенного фрагмента
    /// и помещение его в буфер обмена.
    /// </summary>
    public void Cut()
    {
        _textBox.Cut();
    }

    /// <summary>
    /// Загрузка текста из файла с запросом имени файла.
    /// </summary>
    public void LoadFromFile()
    {
        if (_openFileDialog.ShowDialog() == DialogResult.OK)
        {
            FileName = _openFileDialog.FileName.ThrowIfNull();

            LoadFromFile (FileName);
        }
    }

    /// <summary>
    /// Загрузка текста из указанного файла.
    /// </summary>
    public void LoadFromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        Text = File.ReadAllText (fileName, Encoding.UTF8);
    }

    /// <summary>
    /// Вставка текста из буфера обмена.
    /// </summary>
    public void Paste()
    {
        _textBox.Paste();
    }

    /// <summary>
    /// Вывод на печать.
    /// </summary>
    public void Print()
    {
        var printer = new PlainTextPrinter();
        printer.Print (Text);
    }

    /// <summary>
    /// Сохранение текста в файл с запросом имени файла.
    /// </summary>
    public void SaveToFile()
    {
        if (string.IsNullOrEmpty (FileName))
        {
            if (_saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            FileName = _saveFileDialog.FileName.ThrowIfNull();
        }

        SaveToFile (FileName);
    }

    /// <summary>
    /// Сохранение текста в указанный фаул.
    /// </summary>
    public void SaveToFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        File.WriteAllText (fileName, Text, Encoding.UTF8);
    }

    #endregion
}
