// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StandardWindowFactory.cs -- стандартная реализация фабрики для создания и уничтожения окон
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;

#endregion

#nullable enable

namespace AM.Avalonia.AppServices;

/// <summary>
/// Стандартная реализация фабрики для создания и уничтожения окон.
/// </summary>
public sealed class StandardWindowFactory
    : IWindowFactory
{
    #region Private members

    private AvaloniaApplication? _GetApplication() =>
        Magna.Application as AvaloniaApplication;

    #endregion

    #region IWindowFactory members

    /// <inheritdoc cref="IWindowFactory.CreateWindow{T}"/>
    public T CreateWindow<T>()
        where T: Window, new()
    {
        var result = new T();
        _GetApplication()?.Windows.Add (result);

        return result;
    }

    /// <inheritdoc cref="IWindowFactory.DestroyWindow"/>
    public void DestroyWindow
        (
            Window window
        )
    {
        Sure.NotNull (window);

        _GetApplication()?.Windows.Remove (window);
    }

    #endregion
}
