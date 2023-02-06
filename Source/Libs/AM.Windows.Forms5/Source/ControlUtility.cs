// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ControlUtility.cs -- полезные методы для работы с контролами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Полезные методы для работы с контролами.
/// </summary>
public static class ControlUtility
{
    #region Public methods

    /// <summary>
    /// Нахождение контрола, который имеет фокус ввода.
    /// </summary>
    /// <remarks>Borrowed from StackOverflow:
    /// http://stackoverflow.com/questions/435433/what-is-the-preferred-way-to-find-focused-control-in-winforms-app
    /// </remarks>
    public static Control? FindFocusedControl
        (
            this Control? control
        )
    {
        var container = control as IContainerControl;
        while (container is not null)
        {
            control = container.ActiveControl;
            container = control as IContainerControl;
        }

        return control;
    }

    /// <summary>
    /// Вызов указанного действия <paramref name="action"/>
    /// строго в потоке пользовательского интерфейса
    /// для указанного контрола <paramref name="control"/>.
    /// </summary>
    /// <param name="control">Контрол, для которого
    /// выполняется действие.</param>
    /// <param name="action">Требуемое действие.</param>
    public static void InvokeIfRequired
        (
            this Control? control,
            MethodInvoker? action
        )
    {
        if (control is null || action is null)
        {
            return;
        }

        var counter = 0;

        // When the form, thus the control, isn't visible yet,
        // InvokeRequired returns false, resulting still
        // in a cross-thread exception.
        while (!control.Visible
               && counter < 20)
        {
            Application.DoEvents();
            Thread.Sleep (10);
            counter++;
        }

        if (!control.Visible && counter >= 20)
        {
            return;
        }

        if (control.InvokeRequired)
        {
            control.Invoke (action);
        }
        else
        {
            action();
        }
    }

    #endregion
}
