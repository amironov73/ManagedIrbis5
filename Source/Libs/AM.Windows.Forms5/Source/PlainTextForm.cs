// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable VirtualMemberCallInConstructor

/* PlainTextForm.cs -- форма для демонстрации простого текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Форма для демонстрации пользователю
/// некоторого простого текста, например,
/// лицензионного соглашения или простейшего отчета.
/// </summary>
public sealed partial class PlainTextForm
    : Form
{
    #region Properties

    /// <summary>
    /// Текстовый редактор, используемый для отображения текста.
    /// </summary>
    public PlainTextEditor Editor => _textControl;

    /// <inheritdoc cref="Control.Text" />
    public override string? Text
    {
        get => _textControl?.Text;
        set
        {
            if (!ReferenceEquals (_textControl, null))
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
            string? text
        )
        : this()
    {
        Text = text;
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
            string? text,
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
