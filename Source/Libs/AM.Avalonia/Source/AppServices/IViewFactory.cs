// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IViewFactory.cs -- фабрика для создания и уничтожения View
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;

#endregion

#nullable enable

namespace AM.Avalonia.AppServices;

/// <summary>
/// Фабрика для создания и уничтожения View.
/// </summary>
public interface IViewFactory
{
    /// <summary>
    /// Создание View.
    /// </summary>
    T CreateView<T>()
        where T: Control, new();

    /// <summary>
    /// Уничтожение View.
    /// </summary>
    void DestroyView (Control view);
}
