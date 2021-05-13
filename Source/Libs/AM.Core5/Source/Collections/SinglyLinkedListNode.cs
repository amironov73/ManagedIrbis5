// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SinglyLinkedListNode.cs -- звено односвязного списка
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Звено односвязного списка.
    /// </summary>
    public sealed class SinglyLinkedListNode<T>
    {
        #region Properties

        /// <summary>
        /// Хранимое значение.
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Ссылка на следующее звено.
        /// </summary>
        public SinglyLinkedListNode<T>? Next { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="value">Значение, которое должно храниться в звене.
        /// </param>
        public SinglyLinkedListNode
            (
                T? value
            )
        {
            Value = value;
        }

        #endregion

    } // class SinglyLinkedListNode

} // namespace AM.Collections
