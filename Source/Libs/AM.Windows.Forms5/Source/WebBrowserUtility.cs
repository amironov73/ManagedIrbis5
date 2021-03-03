// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* WebBrowserUtility.cs -- вспомогательные методы для контрола WebBrowser
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Вспомогательные методы для контрола <see cref="WebBrowser"/>.
    /// </summary>
    public static class WebBrowserUtility
    {
        #region Public methods

        /// <summary>
        /// Очистка окна браузера.
        /// </summary>
        public static void ClearBrowser
            (
                this WebBrowser browser
            )
        {
            if (!browser.Disposing && !browser.IsDisposed)
            {
                browser.Navigate("about:blank");
            }
        } // method ClearBrowser

        /// <summary>
        /// Передача текста в браузер.
        /// </summary>
        public static void SetBrowserText
            (
                this WebBrowser browser,
                string? html
            )
        {
            if (!browser.Disposing && !browser.IsDisposed)
            {
                browser.DocumentText = html ?? string.Empty;
            }
        } // method SetBrowserText

        /// <summary>
        /// Первоначальная инициализация браузера.
        /// </summary>
        public static async Task<bool> PrepareBrowser
            (
                this WebBrowser browser
            )
        {
            if (browser.Disposing || browser.IsDisposed)
            {
                return false;
            }

            for (var i = 0; i < 2; i++)
            {
                try
                {
                    browser.Navigate("about:blank");
                    while (browser.IsBusy)
                    {
                        Application.DoEvents();
                    }

                    browser.DocumentText = "&nbsp;";

                    await ApplicationUtility.IdleDelay();
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            nameof(WebBrowserUtility) + "::" + nameof(PrepareBrowser),
                            exception
                        );
                    return false;
                }
            }

            return true;
        } // method PrepareBrowser

        #endregion

    } // class WebBrowserUtility

} // namespace AM.Windows.Forms
