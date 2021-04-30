// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IRefEnumerator.cs -- перечисление объектов по ссылке
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Перечисление объектов по ссылке.
    /// </summary>
    public interface IRefEnumerator<T>
    {
        /// <summary>
        /// Ссылка на текущий элемент.
        /// </summary>
        ref T Current { get; }

        /// <summary>
        /// Переход к следующему элементу.
        /// </summary>
        /// <returns><c>false</c>, если перечисляемые объекты закончились.
        /// </returns>
        bool MoveNext();

    } // interface IRefEnumerator<T>

} // namespace AM.Collections
