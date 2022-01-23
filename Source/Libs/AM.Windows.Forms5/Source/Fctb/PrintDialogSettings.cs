// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PrintDialogSettings.cs -- настройки диалога печати
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Fctb;

/// <summary>
/// Настройки диалога печати.
/// </summary>
public sealed class PrintDialogSettings
{
    #region Properties

    /// <summary>
    /// Показывать диалог настройки страницы.
    /// </summary>
    public bool ShowPageSetupDialog { get; set; }

    /// <summary>
    /// Показывать диалог печати.
    /// </summary>
    public bool ShowPrintDialog { get; set; }

    /// <summary>
    /// Показывать диалог предварительного просмотра.
    /// </summary>
    public bool ShowPrintPreviewDialog { get; set; }

    /// <summary>
    /// Title of page. If you want to print Title on the page, insert code &amp;w in Footer or Header.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Footer of page.
    /// Here you can use special codes: &amp;w (Window title), &amp;D, &amp;d (Date), &amp;t(), &amp;4 (Time), &amp;p (Current page number), &amp;P (Total number of pages),  &amp;&amp; (A single ampersand), &amp;b (Right justify text, Center text. If &amp;b occurs once, then anything after the &amp;b is right justified. If &amp;b occurs twice, then anything between the two &amp;b is centered, and anything after the second &amp;b is right justified).
    /// </summary>
    /// <remarks>
    /// More detailed see http://msdn.microsoft.com/en-us/library/aa969429(v=vs.85).aspx
    /// </remarks>
    public string Footer { get; set; }

    /// <summary>
    /// Header of page
    /// Here you can use special codes: &amp;w (Window title), &amp;D, &amp;d (Date), &amp;t(), &amp;4 (Time), &amp;p (Current page number), &amp;P (Total number of pages),  &amp;&amp; (A single ampersand), &amp;b (Right justify text, Center text. If &amp;b occurs once, then anything after the &amp;b is right justified. If &amp;b occurs twice, then anything between the two &amp;b is centered, and anything after the second &amp;b is right justified).
    /// </summary>
    /// <remarks>
    /// More detailed see http://msdn.microsoft.com/en-us/library/aa969429(v=vs.85).aspx
    /// </remarks>
    public string Header { get; set; }

    /// <summary>
    /// Prints line numbers
    /// </summary>
    public bool IncludeLineNumbers { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PrintDialogSettings()
    {
        ShowPrintPreviewDialog = true;
        Title = string.Empty;
        Footer = string.Empty;
        Header = string.Empty;
    }

    #endregion
}
