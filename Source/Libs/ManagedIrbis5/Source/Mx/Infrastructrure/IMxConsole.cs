// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IMxConsole.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Infrastructrure
{
    /// <summary>
    /// Generic console for MX.
    /// </summary>
    public interface IMxConsole
    {
        /// <summary>
        /// Фон консоли.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Цвет символов.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Вывод.
        /// </summary>
        void Write(string text);

        /// <summary>
        /// Вввод.
        /// </summary>
        string? ReadLine();

        /// <summary>
        /// Очистка.
        /// </summary>
        void Clear();
    }
}
