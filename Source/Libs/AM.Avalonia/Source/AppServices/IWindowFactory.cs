// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IWindowFactory.cs -- фабрика для создания и уничтожения окон
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;

#endregion

#nullable enable

namespace AM.Avalonia.AppServices;

/// <summary>
/// Фабрика для создания и уничтожения окон.
/// </summary>
public interface IWindowFactory
{
    /// <summary>
    /// Создание окна.
    /// </summary>
    T CreateWindow<T>()
        where T: Window, new();

    /// <summary>
    /// Уничтожение окна.
    /// </summary>
    void DestroyWindow (Window window);
}
