// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IRefEnumerable.cs -- коллекция элементов, перечисляемых по ссылке
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Коллекция элементов, перечисляемых по ссылке.
    /// </summary>
    public interface IRefEnumerable<T>
    {
        /// <summary>
        /// Запрос перечислителя.
        /// </summary>
        IRefEnumerable<T> GetEnumerator();

    } // interface IRefEnumerable<T>

} // namespace AM.Collections
