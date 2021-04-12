// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* InputLanguageUtility.cs -- манипуляции с языком ввода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Utility for <see cref="InputLanguage"/> manipulations.
    /// </summary>
    /// <remarks>Borrowed from:
    /// https://blogs.msdn.microsoft.com/snippets/2008/12/31/how-to-change-input-language-programmatically/
    /// </remarks>
    public static class InputLanguageUtility
    {
        #region Nested classes

        internal class InputLanguageMessageFilter
            : IMessageFilter
        {
            #region Properties

            public static IMessageFilter? Instance { get; set; }

            #endregion

            #region IMessageFilter members

            public bool PreFilterMessage(ref Message message)
            {
                return HandleWmInputLanguageRequest(ref message);
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// American English language.
        /// </summary>
        public static InputLanguage AmericanEnglishLanguage =>
            InputLanguage.FromCulture(new CultureInfo(0x0409))
                .ThrowIfNull("InputLanguage.FromCulture");

        /// <summary>
        /// Next installed input language.
        /// </summary>
        public static InputLanguage NextLanguage
        {
            get
            {
                var languages= InputLanguage.InstalledInputLanguages;
                var currentIndex = languages.IndexOf
                    (
                        InputLanguage.CurrentInputLanguage
                    );
                var nextIndex = currentIndex + 1;
                if (nextIndex >= languages.Count)
                {
                    nextIndex = 0;
                }
                var result = languages[nextIndex];

                return result;
            }
        }

        /// <summary>
        /// Russian language.
        /// </summary>
        public static InputLanguage RussianLanguage =>
            InputLanguage.FromCulture(new CultureInfo(0x0419))
                .ThrowIfNull("InputLanguage.FromCulture");

        #endregion

        #region Public methods

        /// <summary>
        /// Change current Input Language to a next installed language.
        /// </summary>
        public static void ChangeInputLanguage()
        {
            var languages = InputLanguage.InstalledInputLanguages;

            // Nothing to do if there is only one input language supported:
            if (languages.Count == 1)
            {
                return;
            }

            var nextLanguage = NextLanguage;
            ChangeInputLanguage(nextLanguage);
        }

        /// <summary>
        /// Changing current Input Language to a new one passed in the param.
        /// </summary>
        /// <param name="isoLanguageCode">ISO Culture name string code
        /// e.g. "en" for English</param>
        public static void ChangeInputLanguage
            (
                string isoLanguageCode
            )
        {
            // Convert ISO Culture name to InputLanguage object.
            // Be aware: if ISO is not supported
            // Exception will be invoked here
            var language = InputLanguage.FromCulture
                (
                    new CultureInfo(isoLanguageCode)
                )
                .ThrowIfNull("InputLanguage");

            ChangeInputLanguage(language);
        }
        /// <summary>
        /// Changing current Input Language to a new one passed in the param
        /// </summary>
        /// <param name="languageId">Integer Culture code
        /// e.g. 1033 for English</param>
        public static void ChangeInputLanguage
            (
                int languageId
            )
        {
            // Convert Integer Culture code to InputLanguage object.
            // Be aware: if Culture code is not supported
            // Exception will be invoked here
            var language = InputLanguage.FromCulture(new CultureInfo(languageId))
                .ThrowIfNull("language");

            ChangeInputLanguage(language);
        }

        /// <summary>
        /// Changing current Input Language to a new one passed in the param
        /// </summary>
        /// <param name="inputLanguage">New input language.</param>
        public static void ChangeInputLanguage
            (
                InputLanguage inputLanguage
            )
        {
            // Check is this Language really installed.
            // Raise exception to warn if it is not
            if (InputLanguage.InstalledInputLanguages.IndexOf(inputLanguage) == -1)
            {
                Magna.Error
                    (
                        "InputLanguageUtility::ChangeInputLanguage: "
                        + "language="
                        + inputLanguage
                        + " not installed"
                    );

                throw new ArgumentOutOfRangeException();
            }

            // InputLanguage changes here:
            InputLanguage.CurrentInputLanguage = inputLanguage;

            Magna.Trace
                (
                    "InputLanguageUtility::ChangeInputLanguage: "
                    + "language="
                    + inputLanguage
                );
        }

        /// <summary>
        /// Switch to English language.
        /// </summary>
        public static void SwitchToEnglish()
        {
            ChangeInputLanguage(AmericanEnglishLanguage);
        }

        /// <summary>
        /// Switch to Russian language.
        /// </summary>
        public static void SwitchToRussian()
        {
            ChangeInputLanguage(RussianLanguage);
        }

        /// <summary>
        /// Обрабатываем запрос на смену языка ввода.
        /// </summary>
        public static bool HandleWmInputLanguageRequest
            (
                ref Message message
            )
        {
            // WM_INPUTLANGCHANGEREQUEST
            if (message.Msg == 0x0050)
            {
                ChangeInputLanguage();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Устанавливает глобальный обработчик сообщения
        /// WM_INPUTLANGCHANGEREQUEST.
        /// </summary>
        public static void InstallWmInputLanguageRequestFix()
        {
            if (InputLanguageMessageFilter.Instance is null)
            {
                var instance = new InputLanguageMessageFilter();
                Application.AddMessageFilter(instance);
                InputLanguageMessageFilter.Instance = instance;
            }
        }

        /// <summary>
        /// Удаляет глобальный обработчик сообщения
        /// WM_INPUTLANGCHANGEREQUEST.
        /// </summary>
        public static void RemoveWmInputLanguageRequestFix()
        {
            if (InputLanguageMessageFilter.Instance is not null)
            {
                Application.RemoveMessageFilter(InputLanguageMessageFilter.Instance);
                InputLanguageMessageFilter.Instance = null;
            }
        }

        #endregion
    }
}
