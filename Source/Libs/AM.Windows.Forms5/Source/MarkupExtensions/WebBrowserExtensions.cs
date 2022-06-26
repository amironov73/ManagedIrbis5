// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* WebBrowserExtensions.cs -- методы расширения для WebBrowser
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="WebBrowser"/>.
/// </summary>
public static class WebBrowserExtensions
{
    #region Public methods

    /// <summary>
    /// Разрешение навигации.
    /// </summary>
    public static TWebBrowser AllowNavigation<TWebBrowser>
        (
            this TWebBrowser browser,
            bool allow
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);

        browser.AllowNavigation = allow;

        return browser;
    }

    /// <summary>
    /// Разрешение бросать документы на браузер.
    /// </summary>
    public static TWebBrowser AllowWebBrowserNavigation<TWebBrowser>
        (
            this TWebBrowser browser,
            bool allow
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);

        browser.AllowWebBrowserDrop = allow;

        return browser;
    }

    /// <summary>
    /// Поток для заливки документа в браузер.
    /// </summary>
    public static TWebBrowser DocumentStream<TWebBrowser>
        (
            this TWebBrowser browser,
            Stream stream
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (stream);

        browser.DocumentStream = stream;

        return browser;
    }

    /// <summary>
    /// HTML-текст документа для загрузки в браузер.
    /// </summary>
    public static TWebBrowser DocumentText<TWebBrowser>
        (
            this TWebBrowser browser,
            string text
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (text);

        browser.DocumentText = text;

        return browser;
    }

    /// <summary>
    /// Разрешение/запрещение контекстного меню браузера.
    /// </summary>
    public static TWebBrowser IsWebBrowserContextMenuEnabled<TWebBrowser>
        (
            this TWebBrowser browser,
            bool enabled
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);

        browser.IsWebBrowserContextMenuEnabled = enabled;

        return browser;
    }

    /// <summary>
    /// Объект для скриптинга.
    /// </summary>
    public static TWebBrowser ObjectForScripting<TWebBrowser>
        (
            this TWebBrowser browser,
            object objectForScripting
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (objectForScripting);

        browser.ObjectForScripting = objectForScripting;

        return browser;
    }

    /// <summary>
    /// Подписка на событие <see cref="WebBrowser.DocumentCompleted"/>.
    /// </summary>
    public static TWebBrowser OnDocumentCompleted<TWebBrowser>
        (
            this TWebBrowser browser,
            WebBrowserDocumentCompletedEventHandler handler
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (handler);

        browser.DocumentCompleted += handler;

        return browser;
    }

    /// <summary>
    /// Подписка на событие <see cref="WebBrowser.DocumentTitleChanged"/>.
    /// </summary>
    public static TWebBrowser OnDocumentTitleChanged<TWebBrowser>
        (
            this TWebBrowser browser,
            EventHandler handler
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (handler);

        browser.DocumentTitleChanged += handler;

        return browser;
    }

    /// <summary>
    /// Подписка на событие <see cref="WebBrowser.FileDownload"/>.
    /// </summary>
    public static TWebBrowser OnFileDownload<TWebBrowser>
        (
            this TWebBrowser browser,
            EventHandler handler
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (handler);

        browser.FileDownload += handler;

        return browser;
    }

    /// <summary>
    /// Подписка на событие <see cref="WebBrowser.Navigated"/>.
    /// </summary>
    public static TWebBrowser OnNavigated<TWebBrowser>
        (
            this TWebBrowser browser,
            WebBrowserNavigatedEventHandler handler
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (handler);

        browser.Navigated += handler;

        return browser;
    }

    /// <summary>
    /// Подписка на событие <see cref="WebBrowser.Navigating"/>.
    /// </summary>
    public static TWebBrowser OnNavigating<TWebBrowser>
        (
            this TWebBrowser browser,
            WebBrowserNavigatingEventHandler handler
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (handler);

        browser.Navigating += handler;

        return browser;
    }

    /// <summary>
    /// Подписка на событие <see cref="WebBrowser.NewWindow"/>.
    /// </summary>
    public static TWebBrowser OnNewWindow<TWebBrowser>
        (
            this TWebBrowser browser,
            CancelEventHandler handler
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (handler);

        browser.NewWindow += handler;

        return browser;
    }

    /// <summary>
    /// Подписка на событие <see cref="WebBrowser.ProgressChanged"/>.
    /// </summary>
    public static TWebBrowser OnProgressChanged<TWebBrowser>
        (
            this TWebBrowser browser,
            WebBrowserProgressChangedEventHandler handler
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (handler);

        browser.ProgressChanged += handler;

        return browser;
    }

    /// <summary>
    /// Подписка на событие <see cref="WebBrowser.StatusTextChanged"/>.
    /// </summary>
    public static TWebBrowser OnStatusTextChanged<TWebBrowser>
        (
            this TWebBrowser browser,
            EventHandler handler
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (handler);

        browser.StatusTextChanged += handler;

        return browser;
    }

    /// <summary>
    /// Подавление сообщений об ошибках в скриптах.
    /// </summary>
    public static TWebBrowser ScriptErrorsSuppressed<TWebBrowser>
        (
            this TWebBrowser browser,
            bool suppressed = true
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);

        browser.ScriptErrorsSuppressed = suppressed;

        return browser;
    }

    /// <summary>
    /// Разрешение/запрещение полос прокрутки у браузера.
    /// </summary>
    public static TWebBrowser ScrollBarsEnabled<TWebBrowser>
        (
            this TWebBrowser browser,
            bool enabled
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);

        browser.ScrollBarsEnabled  = enabled;

        return browser;
    }

    /// <summary>
    /// Задание URL для загрузки страницы.
    /// </summary>
    public static TWebBrowser Url<TWebBrowser>
        (
            this TWebBrowser browser,
            string url
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNullNorEmpty (url);

        browser.Url = new Uri (url);

        return browser;
    }

    /// <summary>
    /// Задание URL для загрузки страницы.
    /// </summary>
    public static TWebBrowser Url<TWebBrowser>
        (
            this TWebBrowser browser,
            Uri url
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);
        Sure.NotNull (url);

        browser.Url = url;

        return browser;
    }

    /// <summary>
    /// Разрешение/запрещение клавиатурных сокращений браузера.
    /// </summary>
    public static TWebBrowser WebBrowserShortcutsEnabled<TWebBrowser>
        (
            this TWebBrowser browser,
            bool enabled
        )
        where TWebBrowser: WebBrowser
    {
        Sure.NotNull (browser);

        browser.WebBrowserShortcutsEnabled = enabled;

        return browser;
    }

    #endregion
}
