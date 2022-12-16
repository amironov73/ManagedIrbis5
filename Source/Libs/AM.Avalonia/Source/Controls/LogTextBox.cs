// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* LogTextBox.cs -- текстовое поле, умеющее отображать логи в реальном времени
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Logging;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;

using NLog;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Текстовое поле, умеющее отображать логи в реальном времени.
/// </summary>
public sealed class LogTextBox
    : TextBox, IStyleable
{
    #region Properties

    /// <summary>
    /// Минимальный уровень, начиная с которого происходит вывод на экран.
    /// </summary>
    public LogLevel MinLevel { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public LogTextBox()
    {
        IsReadOnly = true;
        TextWrapping = TextWrapping.Wrap;
        MinLevel = LogLevel.Info;
    }

    #endregion

    #region Private members

    /// <inheritdoc cref="StyledElement.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        MagnaTarget.Subscribe (_Handler);
    }

    /// <inheritdoc cref="Control.OnUnloaded"/>
    protected override void OnUnloaded()
    {
        MagnaTarget.Unsubscribe (_Handler);

        base.OnUnloaded();
    }

    private void _Handler
        (
            object? sender,
            LogEventInfo eventInfo
        )
    {
        if (eventInfo.Level >= MinLevel)
        {
            Dispatcher.UIThread.Post (() =>
            {
                var line = $"{eventInfo.TimeStamp}: {eventInfo.Level.ToString().ToUpperInvariant()} : {eventInfo.FormattedMessage}{NewLine}";

                Text += line;
                CaretIndex = int.MaxValue;
            });
        }
    }

    Type IStyleable.StyleKey => typeof (TextBox);

    #endregion
}
