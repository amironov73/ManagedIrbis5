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
using System.Threading;
using System.Windows.Forms;

using AM.AppServices;

using Microsoft.Extensions.Logging;

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
        : base(args)
    {
        MainForm = mainForm!;
    }

    #endregion

    #region MagnaApplication members

    /// <inheritdoc cref="MagnaApplication.ActualRun"/>
    protected override int ActualRun()
    {
        Application.ThreadException += HandleThreadException;

        try
        {
            Application.SetCompatibleTextRenderingDefault (false);
            Application.SetHighDpiMode (HighDpiMode.SystemAware);
            Application.EnableVisualStyles();

            // ReSharper disable NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract

            MainForm ??= CreateMainForm();
            MainForm.ShowVersionInfoInTitle();

            // ReSharper restore NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract

            VisualInitialization();
            Application.Run (MainForm);
            VisualShutdown();
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Exception occurred");
            ShowException (exception);
            return 1;
        }

        return MainForm.ReturnCode;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание главной формы приложения.
    /// </summary>
    public virtual MainForm CreateMainForm()
    {
        return new MainForm();
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

    #endregion
}
