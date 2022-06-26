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

using AM.Drawing.HtmlRenderer.Core.Utils;

#endregion

#nullable enable

namespace AM.Windows.HtmlRenderer;

/// <summary>
/// Handler for HTML renderer routed events.
/// </summary>
/// <param name="args">the event arguments object</param>
/// <typeparam name="T">the type of the routed events args data</typeparam>
public delegate void RoutedEventHandler<T>(object sender, RoutedEventArgs<T> args) where T : class;

/// <summary>
/// HTML Renderer routed event arguments containing event data.
/// </summary>
public sealed class RoutedEventArgs<T> 
    : RoutedEventArgs 
    where T : class
{
    /// <summary>
    /// the argument data of the routed event
    /// </summary>
    private readonly T _data;

    public RoutedEventArgs(RoutedEvent routedEvent, T data)
        : base(routedEvent)
    {
        ArgChecker.AssertArgNotNull(data, "args");
        _data = data;
    }

    public RoutedEventArgs(RoutedEvent routedEvent, object source, T data)
        : base(routedEvent, source)
    {
        ArgChecker.AssertArgNotNull(data, "args");
        _data = data;
    }

    /// <summary>
    /// the argument data of the routed event
    /// </summary>
    public T Data
    {
        get { return _data; }
    }

    public override string ToString()
    {
        return string.Format("RoutedEventArgs({0})", _data);
    }
}