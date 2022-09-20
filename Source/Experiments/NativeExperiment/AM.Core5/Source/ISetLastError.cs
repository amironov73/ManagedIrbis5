// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ISetLastError.cs -- интерфейс установки кода ошибки
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM
{
    /// <summary>
    /// Интерфейс установки кода ошибки.
    /// </summary>
    public interface ISetLastError
        : IGetLastError
    {
        /// <summary>
        /// Установка кода ошибки.
        /// </summary>
        int SetLastError(int code);

    } // interface ISetLastError

} // namespace AM
