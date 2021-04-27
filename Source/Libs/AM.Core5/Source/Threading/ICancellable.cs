// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ICancellable.cs -- общий интерфейс объекта, поддерживающего отмену операции
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Threading
{
    /// <summary>
    /// Общий интерфейс объекта, поддерживающего отмену операции.
    /// Объект также должен поддерживать очистку.
    /// </summary>
    public interface ICancellable
    {
        /// <summary>
        /// В настоящее время объект занят выполнением длительной операции?
        /// </summary>
        BusyState Busy { get; }

        /// <summary>
        /// Отмена текущей операции.
        /// </summary>
        void CancelOperation();

    } // interface ICancellable

} // namespace AM.Threading
