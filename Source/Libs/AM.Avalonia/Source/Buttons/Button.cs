// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* Button.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reactive.Subjects;
using System.Windows.Input;

using ReactiveUI;

using AM.Avalonia.Interfaces;

#endregion

#nullable enable

namespace AM.Avalonia.Buttons;

/// <summary>
///
/// </summary>
public class Button
    : ReactiveObject, ICommand, IButton
{
    #region IButton

    public string Name
    {
        get => name;
        set => this.RaiseAndSetIfChanged (ref name, value);
    }

    private string name;

    public bool IsVisible
    {
        get => isVisible;
        set => this.RaiseAndSetIfChanged (ref isVisible, value);
    }

    private bool isVisible;

    public bool IsEnabled
    {
        get => isEnabled;
        set => this.RaiseAndSetIfChanged (ref isEnabled, value);
    }

    private bool isEnabled;

    public bool IsDefault
    {
        get => isDefault;
        set => this.RaiseAndSetIfChanged (ref isDefault, value);
    }

    private bool isDefault;

    public bool IsCancel
    {
        get => isCancel;
        set => this.RaiseAndSetIfChanged (ref isCancel, value);
    }

    private bool isCancel;

    public IObservable<ButtonArgs> OnClick => onClickSubject;
    public IObservable<ButtonArgs> Clicked => clickedSubject;

    private readonly Subject<ButtonArgs> onClickSubject;
    private readonly Subject<ButtonArgs> clickedSubject;

    #endregion

    #region ICommand

    public bool CanExecute (object parameter)
    {
        return IsEnabled && IsVisible;
    }

    public void Execute (object parameter)
    {
        var buttonArgs = new ButtonArgs (this);
        onClickSubject?.OnNext (buttonArgs);
        clickedSubject?.OnNext (buttonArgs);
    }

    public event EventHandler CanExecuteChanged;

    protected virtual void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke (this, EventArgs.Empty);
    }

    #endregion

    #region Equals

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        var button = obj as Button;
        return button != null && (button.GetType() == GetType() && Equals (button.Name, Name));
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return GetType().GetHashCode() ^ Name.GetHashCode();
    }

    public static bool operator == (Button button1, Button button2)
    {
        if (ReferenceEquals (button1, null))
        {
            return ReferenceEquals (button2, null);
        }

        return button1.Equals (button2);
    }

    public static bool operator != (Button button1, Button button2)
    {
        return !(button1 == button2);
    }

    #endregion

    public Button()
    {
        IsEnabled = true;
        IsVisible = true;
        onClickSubject = new Subject<ButtonArgs>();
        clickedSubject = new Subject<ButtonArgs>();
    }
}
