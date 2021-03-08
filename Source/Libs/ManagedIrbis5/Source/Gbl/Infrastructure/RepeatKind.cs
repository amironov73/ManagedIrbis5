// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RepeatKind.cs -- различные варианты повторения
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Различные варианты повторения.
    /// </summary>
    public enum RepeatKind
    {
        /// <summary>
        /// All the repeats.
        /// </summary>
        All,

        /// <summary>
        /// By format.
        /// </summary>
        ByFormat,

        /// <summary>
        /// Last repeat.
        /// </summary>
        Last,

        /// <summary>
        /// Explicit specified repeat.
        /// </summary>
        Explicit

    } // enum RepeatKind

} // namespace ManagedIrbis.Gbl.Infrastructure
