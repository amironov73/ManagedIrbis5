// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* WinFormsApplication.cs -- Magna-приложение WinForms
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using AM.AppServices;

using Microsoft.Extensions.Hosting;

#endregion

#nullable enable

namespace AM.Windows.Forms.AppServices;

/// <summary>
/// Magna-приложение WinForms.
/// </summary>
public class WinFormsApplication
    : MagnaApplication
{
    #region Properties

    /// <summary>
    /// Главная форма приложения.
    /// </summary>
    public MainForm MainForm { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WinFormsApplication
        (
            string[] args,
            MainForm? mainForm = null
        )
        : base (args)
    {
        MainForm = mainForm!;
    }

    #endregion

    #region Private members

    private string? _formTitle;
    private List<Action<WinFormsApplication>>? _postConfigure;

    #endregion

    #region MagnaApplication members

    /// <inheritdoc cref="EarlyInitialization"/>
    protected override void EarlyInitialization()
    {
        base.EarlyInitialization();

        Application.SetCompatibleTextRenderingDefault (false);
        Application.SetHighDpiMode (HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.ThreadException += HandleThreadException;

    }

    /// <inheritdoc cref="FinalInitialization"/>
    protected override bool FinalInitialization()
    {
        if (!base.FinalInitialization())
        {
            return false;
        }

        var mainForm = ((MainForm?) MainForm) ?? CreateMainForm();
        mainForm.Text = _formTitle;
        MainForm = mainForm;
        MainForm.ShowVersionInfoInTitle();

        InputLanguageUtility.InstallWmInputLanguageRequestFix();

        return true;
    }

    /// <inheritdoc cref="MagnaApplication.HandleException"/>
    public override void HandleException
        (
            Exception exception
        )
    {
        base.HandleException (exception);
        ShowException (exception);
    }

    /// <summary>
    /// Запуск приложения.
    /// </summary>
    public override int Run
        (
            Func<IMagnaApplication, int> runDelegate
        )
    {
        Sure.NotNull (runDelegate);

        if (!FinalInitialization())
        {
            return int.MaxValue;
        }

        var result = int.MaxValue;
        try
        {
            ApplicationHost.Start();

            // настраиваем таймер, запускаюшмй пользовательский код
            var timer = new System.Windows.Forms.Timer { Interval = 10 };
            timer.Tick += (_, _) =>
            {
                timer.Enabled = false;
                if (_postConfigure is not null)
                {
                    foreach (var action in _postConfigure)
                    {
                        action (this);
                    }
                }

                result = runDelegate (this);
            };
            timer.Enabled = true;

            VisualInitialization();

            MainForm.FormClosed += (_, _) =>
            {
                Shutdown();
            };

            Application.Run (MainForm);
            VisualShutdown();

            // ожидаем остановки хоста
            ApplicationHost.WaitForShutdown();
            MarkAsShutdown();
        }
        catch (Exception exception)
        {
            HandleException (exception);
        }

        Cleanup();

        // глушим хост
        ApplicationHost.Dispose();
        MarkAsShutdown();

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Конфигуризрование пользовательского интерфейса после запуска.
    /// </summary>
    public WinFormsApplication PostConfigure
        (
            Action<WinFormsApplication> configureDelegate
        )
    {
        _postConfigure ??= new List<Action<WinFormsApplication>>();
        _postConfigure.Add (configureDelegate);

        return this;
    }

    /// <summary>
    /// Создание главной формы приложения.
    /// </summary>
    public virtual MainForm CreateMainForm()
    {
        return new MainForm();
    }

    /// <summary>
    /// Установка заглавия формы.
    /// </summary>
    public virtual WinFormsApplication SetTitle
        (
            string? title
        )
    {
        if (IsInitialized)
        {
            MainForm.Text = title;
        }
        else
        {
            _formTitle = title;
        }

        return this;
    }

    /// <summary>
    /// Вызывается при возникновении необработанного исключения
    /// в потоке, отличном от пользовательского интерфейса.
    /// </summary>
    public virtual void HandleThreadException
        (
            object sender,
            ThreadExceptionEventArgs e
        )
    {
        ShowException (e.Exception);
    }

    /// <summary>
    /// Вызывается при возникновении необработанного исключения.
    /// </summary>
    public virtual void ShowException
        (
            Exception exception
        )
    {
        ExceptionBox.Show (exception);
    }

    /// <summary>
    /// Визуальная инициализация.
    /// </summary>
    public virtual void VisualInitialization()
    {
        // перекрыть в потомке
    }

    /// <summary>
    /// Визуальная де-инициализация.
    /// </summary>
    public virtual void VisualShutdown()
    {
        // перекрыть в потомке
    }

    /// <summary>
    /// Вывод строки в лог.
    /// </summary>
    public void WriteLog
        (
            string text
        )
    {
        MainForm.WriteLog (text);
    }

    #endregion
}
