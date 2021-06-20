// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TextUtility.cs -- различные методы для работы с текстом
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Text
{
    /// <summary>
    /// Различные методы для работы с текстом.
    /// </summary>
    public static class TextUtility
    {
        #region Public methods

        /// <summary>
        /// Determine kind of the text.
        /// </summary>
        public static TextKind DetermineTextKind
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return TextKind.PlainText;
            }

            if (text.StartsWith("{") || text.EndsWith("}"))
            {
                return TextKind.RichText;
            }

            if (text.StartsWith("<") || text.EndsWith(">"))
            {
                return TextKind.Html;
            }

            var curly = text.Contains("{") && text.Contains("}");
            var angle = text.Contains("<") && text.Contains(">");

            if (curly && !angle)
            {
                return TextKind.RichText;
            }
            if (angle && !curly)
            {
                return TextKind.Html;
            }

            return TextKind.PlainText;

        } // method DetermineTextKind

        #endregion

    } // class TextUtility

} // namespace AM.Text
