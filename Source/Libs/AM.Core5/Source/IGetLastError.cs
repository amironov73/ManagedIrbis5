// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* IGetLastError.cs -- интерфейс получения кода ошибки
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM
{
    /// <summary>
    /// Интерфейс получения кода ошибки.
    /// </summary>
    public interface IGetLastError
    {
        /// <summary>
        /// Получение кода ошибки.
        /// </summary>
        int LastError { get; }

    } // interface IGetLastError

} // namespace AM
