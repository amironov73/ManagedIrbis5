// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* IteratorUtility.cs -- вспомогательные методы для работы с итераторами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Вспомогательные методы для работы с итераторами.
    /// </summary>
    public static class IteratorUtility
    {
        #region Public methods

        /// <summary>
        /// Итератор, установленный на начало массива.
        /// </summary>
        public static ArrayIterator<T> Begin<T>(T[] array)
            where T: unmanaged
            => new (array);

        /// <summary>
        /// Итератор, установленный сразу за концом массива
        /// </summary>
        public static ArrayIterator<T> End<T>(T[] array)
            where T: unmanaged
            => new (array, array.Length);


        #endregion

    } // class IteratorUtility

} // namespace AM.Collections
