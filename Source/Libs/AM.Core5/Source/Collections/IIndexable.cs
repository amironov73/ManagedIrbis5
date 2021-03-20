// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IIndexable.cs -- indexable object interface
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Indexable object interface.
    /// </summary>
    public interface IIndexable<T>
    {
        /// <summary>
        /// Gets item at the specified index.
        /// </summary>
        T? this[int index] { get; }

        /// <summary>
        /// Gets the count of items.
        /// </summary>
        int Count { get; }

    } // interface IIndexable

} // namespace AM.Collections
