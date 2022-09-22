// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InputLanguageAwareControl.cs -- контрол, умеющий менять язык ввода по запросу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Контрол, умеющий менять язык ввода по запросу.
/// </summary>
public class InputLanguageAwareControl
    : Control
{
    #region Control members

    /// <inheritdoc cref="Control.WndProc"/>
    protected override void WndProc
        (
            ref Message message
        )
    {
        if (!InputLanguageUtility.HandleWmInputLanguageRequest (ref message))
        {
            base.WndProc (ref message);
        }
    }

    #endregion
}
