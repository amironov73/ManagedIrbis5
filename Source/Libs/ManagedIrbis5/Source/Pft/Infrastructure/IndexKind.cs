// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IndexKind.cs -- вид индекса в спецификации повторения
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Вид индекса в спецификации <see cref="IndexSpecification"/>.
    /// </summary>
    public enum IndexKind
    {
        /// <summary>
        /// Нет никакого индекса.
        /// </summary>
        None,

        /// <summary>
        /// Индекс задан константой-литералом.
        /// </summary>
        /// <remarks>
        /// Например: 3
        /// </remarks>
        Literal,

        /// <summary>
        /// Индекс задан выражением.
        /// </summary>
        /// <remarks>
        /// Например: $x + 1
        /// </remarks>
        Expression,

        /// <summary>
        /// Последнее повторение.
        /// </summary>
        /// <remarks>
        /// *
        /// </remarks>
        LastRepeat,

        /// <summary>
        /// Новое (вновь созданное) повторение.
        /// </summary>
        /// <remarks>
        /// +
        /// </remarks>
        NewRepeat,

        /// <summary>
        /// Текущее повторение.
        /// </summary>
        /// <remarks>
        /// .
        /// </remarks>
        CurrentRepeat,

        /// <summary>
        /// Все повторения.
        /// </summary>
        /// <remarks>
        /// -
        /// </remarks>
        AllRepeats

    } // enum IndexKind

} // namespace ManagedIrbis.Pft.Infrastructure
