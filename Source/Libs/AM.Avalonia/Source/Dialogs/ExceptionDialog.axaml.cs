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

/* ExceptionDialog.cs -- простой диалог для показа сообщения об исключении
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

#endregion

#nullable enable

namespace AM.Avalonia.Dialogs;

/// <summary>
/// Простой диалог для показа сообщения об исключении.
/// </summary>
public partial class ExceptionDialog
    : Window
{
    #region Properties

    /// <summary>
    /// Описание свойства "Сообщение".
    /// </summary>
    public static readonly StyledProperty<string?> MessageProperty
        = AvaloniaProperty.Register<ExceptionDialog, string?> (nameof (Message));

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string? Message
    {
        get => GetValue (MessageProperty);
        set => SetValue (MessageProperty, value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExceptionDialog()
    {
        AvaloniaXamlLoader.Load (this);

        DataContext = this;

#if DEBUG
        this.AttachDevTools();
#endif
    }

    #endregion

    #region Private members

    private void OkButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        Close();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Показ диалога о возникшем исключении.
    /// </summary>
    public static async Task Show
        (
            Window? owner,
            Exception exception
        )
    {
        var window = new ExceptionDialog
        {
            Message = exception.ToString()
        };

        await window.ShowDialog<bool> (owner);
    }

    #endregion
}

