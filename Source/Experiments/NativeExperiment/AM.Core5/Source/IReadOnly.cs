// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

/* IReadOnly -- common interface for object that can be read-only.
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM
{
    /// <summary>
    /// Common interface for object that can be read-only.
    /// </summary>
    public interface IReadOnly<out T>
    {
        /// <summary>
        /// Creates the read-only clone of the object.
        /// </summary>
        T AsReadOnly();

        /// <summary>
        /// Whether the object is read-only.
        /// </summary>
        bool ReadOnly { get; }

        /// <summary>
        /// Marks the object as read-only.
        /// </summary>
        void SetReadOnly();

        /// <summary>
        /// Throws <see cref="ReadOnlyException"/>
        /// if the object is read-only.
        /// </summary>
        void ThrowIfReadOnly();

    } // interface IReadOnly

} // namespace AM
