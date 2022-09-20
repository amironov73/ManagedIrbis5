// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RoutedEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows;

#endregion

#nullable enable

namespace AM.Windows.HtmlRenderer;

/// <summary>
/// Handler for HTML renderer routed events.
/// </summary>
/// <param name="args">the event arguments object</param>
/// <typeparam name="T">the type of the routed events args data</typeparam>
public delegate void RoutedEventHandler<T>
    (
        object? sender,
        RoutedEventArgs<T> args
    )
    where T : class;

/// <summary>
/// HTML Renderer routed event arguments containing event data.
/// </summary>
public sealed class RoutedEventArgs<T>
    : RoutedEventArgs
    where T : class
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="routedEvent"></param>
    /// <param name="data"></param>
    public RoutedEventArgs
        (
            RoutedEvent routedEvent,
            T data
        )
        : base (routedEvent)
    {
        Sure.NotNull (data);

        Data = data;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="routedEvent"></param>
    /// <param name="source"></param>
    /// <param name="data"></param>
    public RoutedEventArgs
        (
            RoutedEvent routedEvent,
            object source,
            T data
        )
        : base (routedEvent, source)
    {
        Sure.NotNull (data);

        Data = data;
    }

    #endregion

    /// <summary>
    /// the argument data of the routed event
    /// </summary>
    public T Data { get; }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return string.Create (null, $"RoutedEventArgs({Data})");
    }
}
