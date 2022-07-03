// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ConsoleForm.cs -- форма с ConsoleControl
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Форма с <see cref="ConsoleControl"/> на ней.
/// </summary>
public sealed class ConsoleForm
    : ModalForm
{
    #region Properties

    /// <summary>
    /// Контрол-консоль.
    /// </summary>
    public ConsoleControl Console { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConsoleForm()
    {
        Console = new ConsoleControl();
        int consoleWidth = Console.Width;
        int consoleHeight = Console.Height;

        Controls.Add (Console);
        ClientSize = new Size (consoleWidth, consoleHeight);
    }

    #endregion
}
