// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MxPalette.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Infrastructrure
{
    /// <summary>
    ///
    /// </summary>
    public sealed class MxPalette
    {
        #region Properties

        /// <summary>
        /// Фон.
        /// </summary>
        [JsonPropertyName("background")]
        public ConsoleColor Background { get; set; }

        /// <summary>
        /// Цвет символов по умолчанию.
        /// </summary>
        [JsonPropertyName("foreground")]
        public ConsoleColor Foreground { get; set; }

        /// <summary>
        /// Вводимые пользователем команды.
        /// </summary>
        [JsonPropertyName("command")]
        public ConsoleColor Command { get; set; }

        /// <summary>
        /// Служебные сообщения.
        /// </summary>
        [JsonPropertyName("message")]
        public ConsoleColor Message { get; set; }

        /// <summary>
        /// Сообщения об ошибках.
        /// </summary>
        [JsonPropertyName("error")]
        public ConsoleColor Error { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Палитра по умолчанию.
        /// </summary>
        public static MxPalette GetDefaultPalette()
        {
            return new MxPalette
            {
                Background = ConsoleColor.Black,
                Foreground = ConsoleColor.Gray,
                Command = ConsoleColor.White,
                Message = ConsoleColor.Green,
                Error = ConsoleColor.Red
            };
        }

        #endregion
    }
}
