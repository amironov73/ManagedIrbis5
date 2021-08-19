// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IGeneralItem.cs -- интерфейс элемента пользовательского интерфейса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms.General
{
    /// <summary>
    /// Некий элемент пользовательского интерфейса,
    /// например, кнопка или элемент меню.
    /// </summary>
    public interface IGeneralItem
    {
        /// <summary>
        /// Уникальный идентификатор (в пределах контейнера).
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Текст на кнопке или элементе меню.
        /// </summary>
        string Caption { get; }

        /// <summary>
        /// Разрешено в настоящий момент?
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Основное действие.
        /// </summary>
        event EventHandler? Execute;

        /// <summary>
        /// Обновление состояния.
        /// </summary>
        event EventHandler? Update;

    } // interface IGeneralItem

} // namespace AM.Windows.Forms.General
