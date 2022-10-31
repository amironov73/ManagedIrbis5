// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IAvaloniaApplicationBuilder.cs -- интерфейс построителя приложения для Avalonia UI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;

#endregion

namespace AM.Avalonia.AppServices;

/// <summary>
/// Интерфейс построителя приложения для Avalonia UI.
/// </summary>
public interface IAvaloniaApplicationBuilder
{
    /// <summary>
    /// Создание главного окна для приложения.
    /// </summary>
    Window CreateMainWindow
        (
            AvaloniaApplication application
        );
}
