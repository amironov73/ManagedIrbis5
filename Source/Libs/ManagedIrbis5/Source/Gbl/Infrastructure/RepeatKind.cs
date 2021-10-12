// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RepeatKind.cs -- различные варианты повторения поля в записи
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Различные варианты повторения поля в записи.
    /// </summary>
    public enum RepeatKind
    {
        /// <summary>
        /// Все повторения, какие есть в записи.
        /// </summary>
        All,

        /// <summary>
        /// Повторения согласно формату.
        /// </summary>
        ByFormat,

        /// <summary>
        /// Последнее повторение.
        /// </summary>
        Last,

        /// <summary>
        /// Явно заданное повтоение.
        /// </summary>
        Explicit

    } // enum RepeatKind

} // namespace ManagedIrbis.Gbl.Infrastructure
