// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AbstractControl.cs -- некий абстрактный контрол, хранящий значение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.CompilerServices;

using Avalonia.Threading;

using ReactiveUI;

using AM.Avalonia.Interfaces;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Некий абстрактный контрол, хранящий значение определенного типа.
/// </summary>
public abstract class AbstractControl<T>
    : ReactiveObject, IDialogControl<T>
{
    #region Properties

    /// <inheritdoc cref="IDialogControl.Name"/>
    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IDialogControl.IsRequired"/>
    public bool IsRequired
    {
        get => _isRequired;
        set
        {
            _isRequired = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IDialogControl.IsVisible"/>
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            _isVisible = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IDialogControl.IsEnabled"/>
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IDialogControl{T}.Value"/>
    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    protected AbstractControl()
    {
        IsEnabled = true;
        IsVisible = true;
        IsRequired = false;
        _value = default!;
    }

    #endregion

    #region Private members

    private string? _name;
    private bool _isVisible;
    private bool _isEnabled;
    private bool _isRequired;
    private T _value;

    /// <summary>
    /// Вызов оповещения об изменении свойства объекта.
    /// </summary>
    protected virtual void OnPropertyChanged
        (
            [CallerMemberName] string? propertyName = null
        )
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            this.RaisePropertyChanged (propertyName);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => this.RaisePropertyChanged (propertyName)
                );
        }
    }

    #endregion
}
