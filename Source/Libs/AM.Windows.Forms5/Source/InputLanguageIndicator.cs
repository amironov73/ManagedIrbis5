// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InputLanguageIndicator.cs -- индикатор языка ввода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Индикатор языка ввода.
/// </summary>
[PublicAPI]
[System.ComponentModel.DesignerCategory ("Code")]
public sealed class InputLanguageIndicator
    : Control
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public InputLanguageIndicator()
    {
        SetStyle (ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle (ControlStyles.Selectable, false);

        ForeColor = Color.White;
        BackColor = Color.Blue;

        /*

        ContextMenu = new ContextMenu();
        foreach (InputLanguage language
            in InputLanguage.InstalledInputLanguages)
        {
            CultureInfo culture = language.Culture;
            string menuText = string.Format
                (
                    "{0} {1}",
                    culture.TwoLetterISOLanguageName.ToUpperInvariant(),
                    language.LayoutName
                );
            ContextMenu.MenuItems.Add(menuText, _MenuClick);
        }

        */
    }

    #endregion

    #region Private members

    private Form? _form;

    // private void _MenuClick
    //     (
    //         object? sender,
    //         EventArgs ea
    //     )
    // {
    //     MenuItem item = (MenuItem)sender;
    //     int index = ContextMenu.MenuItems.IndexOf(item);
    //     InputLanguage.CurrentInputLanguage
    //         = InputLanguage.InstalledInputLanguages[index];
    //     Application.DoEvents();
    //     Invalidate();
    // }

    private void _InputLanguageChanged
        (
            object? sender,
            InputLanguageChangedEventArgs e
        )
    {
        Invalidate();
    }

    // private void _ShowContextMenu()
    // {
    //     ContextMenu.Show(this, new Point(0, 0));
    // }

    #endregion

    #region Control members

    /// <inheritdoc cref="Control.DefaultSize" />
    protected override Size DefaultSize
    {
        get
        {
            const int height = 22;
            const int width = height;
            return new Size (width, height);
        }
    }

    /// <inheritdoc cref="Control.Dispose(bool)" />
    protected override void Dispose
        (
            bool disposing
        )
    {
        base.Dispose (disposing);
        if (_form is not null)
        {
            _form.InputLanguageChanged -= _InputLanguageChanged;
        }
    }

    /// <inheritdoc cref="Control.OnKeyDown" />
    protected override void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        base.OnKeyDown (eventArgs);
        switch (eventArgs.KeyData)
        {
            case Keys.Space:
            case Keys.Enter:
                // _ShowContextMenu();
                break;
        }
    }

    /// <inheritdoc cref="Control.OnPaint" />
    protected override void OnPaint
        (
            PaintEventArgs eventArgs
        )
    {
        var g = eventArgs.Graphics;
        var r = ClientRectangle;
        using (var textBrush = new SolidBrush (ForeColor))
        using (var backBrush = new SolidBrush (BackColor))
        using (var format = new StringFormat())
        {
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            g.FillRectangle (backBrush, r);
            var culture = InputLanguage.CurrentInputLanguage.Culture;
            var languageCode = culture.TwoLetterISOLanguageName.ToUpperInvariant();
            g.DrawString (languageCode, Font, textBrush, r, format);
        }

        base.OnPaint (eventArgs);
    }

    /// <inheritdoc cref="Control.OnParentChanged" />
    protected override void OnParentChanged
        (
            EventArgs eventArgs
        )
    {
        base.OnParentChanged (eventArgs);
        _form = FindForm();
        if (_form is not null)
        {
            // TODO: отписать предыдущего родителя
            _form.InputLanguageChanged += _InputLanguageChanged;
        }
    }

    #endregion
}
