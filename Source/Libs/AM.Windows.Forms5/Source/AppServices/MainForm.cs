// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MainForm.cs -- главная форма приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms.AppServices;

/// <summary>
/// Главная форма приложения.
/// </summary>
public class MainForm
    : Form
{
    #region Properties

    /// <summary>
    /// Код возврата для приложения.
    /// </summary>
    public int ReturnCode { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MainForm()
    {
        _toolStripContainer = new ToolStripContainer();
        _menuStrip = new MenuStrip();
        _toolStrip = new ToolStrip();
        _statusStrip = new StatusStrip();

        InitializeComponent();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="action">Действие, которое необходимо выполнить
    /// сразу после инициализации формы.</param>
    public MainForm
        (
            Action action
        )
        : this()
    {
        Sure.NotNull (action);

        var timer = new Timer()
        {
            Interval = 10
        };
        timer.Tick += (_, _) =>
        {
            action();
            timer.Enabled = false;
            timer.Dispose();
        };
        timer.Enabled = true;
    }

    #endregion

    #region Private members

    private readonly ToolStripContainer _toolStripContainer;
    private readonly StatusStrip _statusStrip;
    private readonly MenuStrip _menuStrip;
    private readonly ToolStrip _toolStrip;
    private LogBox? _logBox;

    private void InitializeComponent()
    {
        _toolStripContainer.BottomToolStripPanel.SuspendLayout();
        _toolStripContainer.TopToolStripPanel.SuspendLayout();
        _toolStripContainer.SuspendLayout();
        SuspendLayout();

        // _toolStripContainer
        _toolStripContainer.BottomToolStripPanel.Controls.Add (_statusStrip);
        _toolStripContainer.Dock = DockStyle.Fill;

        // _toolStripContainer.TopToolStripPanel
        _toolStripContainer.TopToolStripPanel.Controls.Add (_menuStrip);
        _toolStripContainer.TopToolStripPanel.Controls.Add (_toolStrip);

        // MainForm
        Controls.Add (_toolStripContainer);
        MainMenuStrip = _menuStrip;

        _toolStripContainer.BottomToolStripPanel.ResumeLayout (false);
        _toolStripContainer.BottomToolStripPanel.PerformLayout();
        _toolStripContainer.TopToolStripPanel.ResumeLayout (false);
        _toolStripContainer.TopToolStripPanel.PerformLayout();
        _toolStripContainer.ResumeLayout (false);
        _toolStripContainer.PerformLayout();
        ResumeLayout (false);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание формы с выполнением действия сразу после ее инициализации.
    /// </summary>
    public MainForm Create<T>
        (
            Action<T> action,
            T arg1
        )
    {
        Sure.NotNull (action);

        var result = new MainForm();
        var timer = new Timer()
        {
            Interval = 10
        };
        timer.Tick += (_, _) =>
        {
            action (arg1);
            timer.Enabled = false;
            timer.Dispose();
        };
        timer.Enabled = true;

        return result;
    }

    /// <summary>
    /// Создание формы с выполнением действия сразу после ее инициализации.
    /// </summary>
    public MainForm Create<T1, T2>
        (
            Action<T1, T2> action,
            T1 arg1,
            T2 arg2
        )
    {
        Sure.NotNull (action);

        var result = new MainForm();
        var timer = new Timer()
        {
            Interval = 10
        };
        timer.Tick += (_, _) =>
        {
            action (arg1, arg2);
            timer.Enabled = false;
            timer.Dispose();
        };
        timer.Enabled = true;

        return result;
    }

    /// <summary>
    /// Создание формы с выполнением действия сразу после ее инициализации.
    /// </summary>
    public MainForm Create<T1, T2, T3>
        (
            Action<T1, T2, T3> action,
            T1 arg1,
            T2 arg2,
            T3 arg3
        )
    {
        Sure.NotNull (action);

        var result = new MainForm();
        var timer = new Timer()
        {
            Interval = 10
        };
        timer.Tick += (_, _) =>
        {
            action (arg1, arg2, arg3);
            timer.Enabled = false;
            timer.Dispose();
        };
        timer.Enabled = true;

        return result;
    }

    /// <summary>
    /// Добавление списка для вывода логов.
    /// </summary>
    public void AddLogBox()
    {
        if (_logBox is null)
        {
            _logBox = new LogBox
            {
                Dock = DockStyle.Fill,
            };

            var wrapper = new ToolStripHost (_logBox);
            _toolStripContainer.BottomToolStripPanel.Controls.Add (wrapper);
        }
    }

    /// <summary>
    /// Добавление элемента в тулбар.
    /// </summary>
    public ToolStripButton AddToolButton
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        var result = new ToolStripButton (text);
        _toolStrip.Items.Add (result);

        return result;
    }

    /// <summary>
    /// Добавление текста в
    /// </summary>
    public ToolStripLabel AddStatusLabel
        (
            string? text = null,
            int width = 100
        )
    {
        Sure.Positive (width);

        var result = new ToolStripLabel
        {
            Text = text,
            Width = width,
            Padding = new Padding (2)
        };
        result.Paint += (_, args) =>
        {
            var graphics = args.Graphics;
            var bounds = args.ClipRectangle;
            bounds.Inflate (-1, -1);
            graphics.DrawRectangle (Pens.Black, bounds);
        };
        _statusStrip.Items.Add (result);

        return result;
    }

    /// <summary>
    /// Добавление элемента меню.
    /// </summary>
    public ToolStripMenuItem AddMenuItem
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        var result = new ToolStripMenuItem (text);
        _menuStrip.Items.Add (result);

        return result;
    }

    /// <summary>
    /// Вывод строки в лог.
    /// </summary>
    public void WriteLog
        (
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text) && _logBox is not null)
        {
            _logBox.Output.WriteLine (text);
            Magna.Application.Logger.LogInformation (text);
        }
    }

    /// <summary>
    /// Закрытие главной формы приложения с указанным кодом возврата.
    /// </summary>
    public void Close
        (
            int returnCode
        )
    {
        ReturnCode = returnCode;

        Close();
    }

    #endregion
}

