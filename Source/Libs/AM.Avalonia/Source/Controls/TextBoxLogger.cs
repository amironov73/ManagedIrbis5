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

/* TextBoxLogger.cs -- логгер, пишущий в текстовый бокс
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Logging;

using Avalonia.Controls;
using Avalonia.Threading;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Логгер, пишущий в текстовый бокс.
/// </summary>
public sealed class TextBoxLogger
    : IDisposable
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextBoxLogger
        (
            TextBox textBox
        )
    {
        _textBox = textBox;
        NlogInterceptor.RegisterHandler (_Handler);
    }

    #endregion

    #region Private members

    private readonly TextBox _textBox;

    private void _Handler (object? sender, string text)
    {
        Dispatcher.UIThread.Post
            (
                message =>
                {
                    _textBox.Text += message + _textBox.NewLine;
                    _textBox.CaretIndex = int.MaxValue;
                },
                text
            );
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        NlogInterceptor.UnregisterHandler (_Handler);
    }

    #endregion
}
