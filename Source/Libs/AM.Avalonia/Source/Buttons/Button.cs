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
// ReSharper disable UnusedMember.Global
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

    /// <summary>
    /// Имя кнопки.
    /// </summary>
    public string? Name
    {
        get => name;
        set => this.RaiseAndSetIfChanged (ref name, value);
    }

    private string? name;

    /// <summary>
    /// Признак видимости.
    /// </summary>
    public bool IsVisible
    {
        get => isVisible;
        set => this.RaiseAndSetIfChanged (ref isVisible, value);
    }

    private bool isVisible;

    /// <summary>
    /// Кнопка разрешена?
    /// </summary>
    public bool IsEnabled
    {
        get => isEnabled;
        set => this.RaiseAndSetIfChanged (ref isEnabled, value);
    }

    private bool isEnabled;

    /// <summary>
    /// Кнопка по умолчанию.
    /// </summary>
    public bool IsDefault
    {
        get => isDefault;
        set => this.RaiseAndSetIfChanged (ref isDefault, value);
    }

    private bool isDefault;

    /// <summary>
    /// Срабатывает по <c>Esc</c>?
    /// </summary>
    public bool IsCancel
    {
        get => isCancel;
        set => this.RaiseAndSetIfChanged (ref isCancel, value);
    }

    private bool isCancel;

    /// <summary>
    ///
    /// </summary>
    public IObservable<ButtonArgs> OnClick => onClickSubject;

    /// <summary>
    ///
    /// </summary>
    public IObservable<ButtonArgs> Clicked => clickedSubject;

    private readonly Subject<ButtonArgs> onClickSubject;
    private readonly Subject<ButtonArgs> clickedSubject;

    #endregion

    #region ICommand

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute (object? parameter)
    {
        return IsEnabled && IsVisible;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute (object? parameter)
    {
        var buttonArgs = new ButtonArgs (this);
        onClickSubject?.OnNext (buttonArgs);
        clickedSubject?.OnNext (buttonArgs);
    }

    /// <summary>
    ///
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    ///
    /// </summary>
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

    /// <summary>
    /// Оператор равенства.
    /// </summary>
    /// <param name="button1"></param>
    /// <param name="button2"></param>
    /// <returns></returns>
    public static bool operator == (Button button1, Button button2)
    {
        if (ReferenceEquals (button1, null))
        {
            return ReferenceEquals (button2, null);
        }

        return button1.Equals (button2);
    }

    /// <summary>
    /// Оператор неравенства.
    /// </summary>
    /// <param name="button1"></param>
    /// <param name="button2"></param>
    /// <returns></returns>
    public static bool operator != (Button button1, Button button2)
    {
        return !(button1 == button2);
    }

    #endregion

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Button()
    {
        IsEnabled = true;
        IsVisible = true;
        onClickSubject = new Subject<ButtonArgs>();
        clickedSubject = new Subject<ButtonArgs>();
    }
}
