// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Delimiters.cs -- широко используемые разделители значений в строке
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM
{
    /// <summary>
    /// Широко используемые разделители в строке.
    /// </summary>
    public static class Delimiters
    {
        #region Public members

        /// <summary>
        /// Двоеточие.
        /// </summary>
        public static readonly char[] Colon = { ':' };

        /// <summary>
        /// Запятая.
        /// </summary>
        public static readonly char[] Comma = { ',' };

        /// <summary>
        /// Точка.
        /// </summary>
        public static readonly char[] Dot = { '.' };

        /// <summary>
        /// Точка с запятой.
        /// </summary>
        public static readonly char[] Semicolon = { ';' };

        /// <summary>
        /// Обычный пробел.
        /// </summary>
        public static readonly char[] Space = { ' ' };

        /// <summary>
        /// Горизонтальная табуляция.
        /// </summary>
        public static readonly char[] Tab = { '\t' };

        /// <summary>
        /// Вертикальная черта.
        /// </summary>
        public static readonly char[] Pipe = { '|' };

        #endregion

    } // class Delimiters

} // namespace AM
