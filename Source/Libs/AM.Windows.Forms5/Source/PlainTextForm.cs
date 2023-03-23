// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement

/* PlainTextForm.cs -- форма для демонстрации простого текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Форма для демонстрации пользователю
/// некоторого простого текста, например,
/// лицензионного соглашения или простейшего отчета.
/// </summary>
[PublicAPI]
public sealed class PlainTextForm
    : Form
{
    #region Properties

    /// <summary>
    /// Текстовый редактор, используемый для отображения текста.
    /// </summary>
    public PlainTextEditor Editor => _textControl!;

    /// <summary>
    /// Заголовок формы.
    /// </summary>
    public string Title
    {
        get => base.Text;
        set => base.Text = value;
    }

    /// <inheritdoc cref="Control.Text" />
    [AllowNull]
    public override string Text
    {
        get => _textControl?.Text ?? string.Empty;
        set
        {
            if (_textControl is not null)
            {
                _textControl.Text = value ?? string.Empty;
            }
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PlainTextForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PlainTextForm
        (
            string text
        )
        : this()
    {
        Text = text;
    }

    #endregion

    #region Private members

    private PlainTextEditor? _textControl;

    private void InitializeComponent()
    {
        Title = "Редактор плоского текста";

        _textControl = new PlainTextEditor { Dock = DockStyle.Fill };
        Controls.Add (_textControl);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление кнопки на тулбар.
    /// </summary>
    public void AddButton
        (
            ToolStripButton button
        )
    {
        Sure.NotNull (button);

        Editor.AddButton (button);
    }

    /// <summary>
    /// Демонстрация диалога с указанным текстом в блокирующем режиме.
    /// </summary>
    public static DialogResult ShowDialog
        (
            IWin32Window? owner,
            string text,
            bool maximized = false
        )
    {
        using var form = new PlainTextForm (text);
        if (maximized)
        {
            form.WindowState = FormWindowState.Maximized;
        }

        return form.ShowDialog (owner);
    }

    #endregion
}
