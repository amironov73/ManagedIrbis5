// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IGeneralContainer.cs -- контейнер с элементами пользовательского интерфейса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;

#endregion

#nullable enable

namespace AM.Avalonia.General;

/// <summary>
/// Контейнер с элементами пользовательского интерфейса.
/// Например, форма.
/// </summary>
public interface IGeneralContainer
{
    /// <summary>
    /// Тулбары.
    /// </summary>
    IGeneralItemList Toolbars { get; }

    /// <summary>
    /// Главное меню.
    /// </summary>
    IGeneralItem MainMenu { get; }

    /// <summary>
    /// Строка статуса.
    /// </summary>
    IGeneralItem StatusBar { get; }

    /// <summary>
    /// Рабочая область.
    /// </summary>
    Control? WorkingArea { get; }
}
