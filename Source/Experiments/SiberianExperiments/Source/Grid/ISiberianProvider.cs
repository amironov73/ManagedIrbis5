// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ISiberianProvider.cs -- интерфейс провайдера данных для грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Интерфейс провайдера данных для грида.
    /// </summary>
    public interface ISiberianProvider
    {
        /// <summary>
        /// Добавление данных (в конец).
        /// </summary>
        void AddData(object? data);

        /// <summary>
        /// Общая длина данных в записях.
        /// </summary>
        int DataLength { get; }

        /// <summary>
        /// Получение данных для строки с указанным индексом.
        /// </summary>
        object? GetData(int index);

        /// <summary>
        /// Обновление данных для строки с указанным индексом.
        /// </summary>
        void PutData(int index, object? data);

    } // interface ISiberianProvider

} // namespace ManagedIrbis.WinForms.Grid
