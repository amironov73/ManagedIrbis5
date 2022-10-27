// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* LogBox.cs -- текстовый бокс для вывода логов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text.Output;

using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Текстовый бокс для вывода логов.
/// </summary>
public sealed class LogBox
    : TextBox, IStyleable
{
    #region Properties

    /// <summary>
    /// Устройство вывода.
    /// </summary>
    public AbstractOutput Output { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LogBox()
    {
        IsReadOnly = true;
        TextWrapping = TextWrapping.Wrap;

        Output = new TextBoxOutput (this);
    }

    #endregion

    #region IStyleable members

    /// <inheritdoc cref="IStyleable.StyleKey"/>
    Type IStyleable.StyleKey => typeof (TextBox);

    #endregion

    #region Public methods

    /// <summary>
    /// Установка устройства вывода.
    /// </summary>
    public void SetOutput
        (
            AbstractOutput output
        )
    {
        Output = output;
    }

    #endregion
}
