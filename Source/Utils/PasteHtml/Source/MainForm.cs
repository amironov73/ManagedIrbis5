// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable LocalizableElement

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion

#nullable enable

namespace PasteHtml;

public partial class MainForm
    : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private static int _GetPosition
        (
            string text,
            string name
        )
    {
        var regex = new Regex (name);
        var match = regex.Match (text);
        if (!match.Success)
        {
            return -1;
        }

        return int.Parse (match.Groups[1].Value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// HTML из Clipboard приезжает с заголовком, который нам не нужен.
    /// Пытаемся извлечь из этого только то, что нам нужно.
    /// </summary>
    private static string _ExtractFragment
        (
            string clipboardContent
        )
    {
        const string startFragment = "<!--StartFragment-->";
        const string endFragment = "<!--EndFragment-->";

        var invariantCulture = StringComparison.InvariantCulture;
        var startIndex = clipboardContent.IndexOf (startFragment, invariantCulture);
        if (startIndex > 0)
        {
            startIndex += startFragment.Length;
        }

        var endIndex = clipboardContent.LastIndexOf (endFragment, invariantCulture);
        if (startIndex < 0 || endIndex < 0)
        {
            startIndex = _GetPosition (clipboardContent, "StartFragment:(\\d+)");
            endIndex = _GetPosition (clipboardContent, "EndFragment:(\\d+)");
            if (startIndex < 0 || endIndex < 0)
            {
                return clipboardContent;
            }
        }

        var length = endIndex - startIndex;
        var result = clipboardContent;
        try
        {
            result = clipboardContent.Substring (startIndex, length);
        }
        catch
        {
            // на случай, если указаны неверные смещения
        }

        return result;
    }

    /// <summary>
    /// Попытка извлечь из Clipboard текста в формате HTML.
    /// </summary>
    private void _pasteMenuItem_Click
        (
            object sender,
            EventArgs e
        )
    {
        if (Clipboard.ContainsText (TextDataFormat.Html))
        {
            var clipboardContent = Clipboard.GetText(TextDataFormat.Html);
            _textBox.Text = _ExtractFragment (clipboardContent);
        }
        else
        {
            MessageBox.Show ("Clipboard doesn't contains HTML");
        }
    }

    /// <summary>
    /// Сохранение полученного текста в файл.
    /// </summary>
    private void _saveMenuItem_Click
        (
            object sender,
            EventArgs e
        )
    {
        if (_saveFileDialog.ShowDialog (this) == DialogResult.OK)
        {
            File.WriteAllText
                (
                    _saveFileDialog.FileName,
                    _textBox.Text,
                    Encoding.UTF8
                );
        }
    }

    /// <summary>
    /// Копирование полученного текста обратно в Clipboard,
    /// только теперь уже как обычного текста.
    /// </summary>
    private void _copyMenuItem_Click
        (
            object sender,
            EventArgs e
        )
    {
        Clipboard.SetText (_textBox.Text, TextDataFormat.Text);
    }
}
