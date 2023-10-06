// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* GroupBox.cs -- группа контролов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Группа контролов (аналог WinForms).
/// </summary>
public class GroupBox
    : HeaderedContentControl
{
    #region Static construction

    /// <summary>
    /// override some metadata:
    /// Focusable: false
    /// TabNavigation: None
    /// </summary>
    static GroupBox()
    {
        FocusableProperty.OverrideMetadata<GroupBox>
            (
                new StyledPropertyMetadata<bool> (false)
            );
        KeyboardNavigation.TabNavigationProperty.OverrideMetadata<GroupBox>
            (
                new StyledPropertyMetadata<KeyboardNavigationMode> (KeyboardNavigationMode.None)
            );
    }

    #endregion

    #region StyledElement members

    /// <inheritdoc cref="StyledElement.StyleKeyOverride"/>
    protected override Type StyleKeyOverride => typeof (HeaderedContentControl);

    #endregion
}
