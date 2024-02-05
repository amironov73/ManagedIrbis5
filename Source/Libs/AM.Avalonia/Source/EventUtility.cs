// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* EventUtility.cs -- полезные методы для работы с событиями в Avalonia
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia.Interactivity;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia;

/// <summary>
/// Полезные методы для работы с событиями в Avalonia.
/// </summary>
[PublicAPI]
public static class EventUtility
{
    #region Public methods

    /// <summary>
    /// Пустые аргументы для обработчика события.
    /// </summary>
    public static readonly RoutedEventArgs EmptyRoutedEventArgs = new ();

    /// <summary>
    /// Пустой обработчик события.
    /// </summary>
    public static EventHandler<RoutedEventArgs> EmptyRoutedEventHandler => (_, _) => { };

    #endregion
}
