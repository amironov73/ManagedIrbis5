// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global

/* TwoFacedControl.cs -- двуликий контрол
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Двуликий контрол.
/// Состоит из двух контролов.
/// В обычном состоянии отображает один контрол,
/// а при получении фокуса - другой.
/// </summary>
[PublicAPI]
public class TwoFacedControl
    : Decorator
{
    #region Properties

    /// <summary>
    /// Первичный контрол, отображаемый в обычном состоянии.
    /// </summary>
    public Control Primary { get; }

    /// <summary>
    /// Вторичный контрол, отображаемый при получении фокуса.
    /// </summary>
    public Control Secondary { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TwoFacedControl
        (
            Control primary,
            Control secondary
        )
    {
        Sure.NotNull (primary);
        Sure.NotNull (secondary);

        Focusable = true;
        Primary = primary;
        Secondary = secondary;
        Secondary.LostFocus += OnSecondaryOnLostFocus;

        SetState (focused: false);
    }

    #endregion

    #region Private members

    private void OnSecondaryOnLostFocus
        (
            object? sender,
            RoutedEventArgs args
        )
    {
        SetState (focused: false);
    }

    private void SetState
        (
            bool focused
        )
    {
        if (focused)
        {
            Child = Secondary;
            Child.Focus(); // нельзя?
        }
        else
        {
            Child = Primary;
        }
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="Control.OnGotFocus"/>
    protected override void OnGotFocus
        (
            GotFocusEventArgs eventArgs
        )
    {
        base.OnGotFocus (eventArgs);
        SetState (focused: true);
    }

    #endregion
}
