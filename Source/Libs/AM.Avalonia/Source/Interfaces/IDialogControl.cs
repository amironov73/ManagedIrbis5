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

/* IDialogControl.cs -- интерфейс диалогового контрола
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Avalonia.Interfaces;

/// <summary>
/// Интерфейс диалогового контрола.
/// </summary>
public interface IDialogControl
    : INotifyPropertyChanged
{
    /// <summary>
    /// Имя.
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// Является ли обязательным?
    /// </summary>
    bool IsRequired { get; set; }

    /// <summary>
    /// Является ли видимым?
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// Разрешено ли взаимодействие с пользователем?
    /// </summary>
    bool IsEnabled { get; set; }
}

/// <summary>
/// Интерфейс диалогового контрола, хранящего значение опредленного типа.
/// </summary>
/// <typeparam name="T">Тип хранимого значения.</typeparam>
public interface IDialogControl<T>
    : IDialogControl
{
    /// <summary>
    /// Хранимое значение.
    /// </summary>
    T Value { get; set; }
}
