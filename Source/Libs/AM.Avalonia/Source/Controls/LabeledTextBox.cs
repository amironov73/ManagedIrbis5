// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* LabeledTextBox.cs -- текстовый бокс с меткой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Текстовый бокс с меткой.
/// </summary>
public sealed class LabeledTextBox
    : TextBox
{
    #region Properties

    /// <summary>
    /// Описание свойства "метка".
    /// </summary>
    public static readonly StyledProperty<object?> LabelProperty
        = AvaloniaProperty.Register<LabeledTextBox, object?> (nameof (Label));

    /// <summary>
    /// Собственно метка.
    /// </summary>
    public object? Label
    {
        get => GetValue (LabelProperty);
        set => SetValue (LabelProperty, value);
    }

    #endregion
}
