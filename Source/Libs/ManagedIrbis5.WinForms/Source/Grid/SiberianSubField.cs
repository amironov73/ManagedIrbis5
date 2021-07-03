// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SiberianSubField.cs -- представление подполя записи для грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Представление подполя записи для грида.
    /// </summary>
    public class SiberianSubField
    {
        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Заголовок подполя.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Текущее значение подполя.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Оригинальное значение подполя.
        /// </summary>
        public string? OriginalValue { get; set; }

        /// <summary>
        /// Режим редактирования.
        /// </summary>
        public string? Mode { get; set; }

        /// <summary>
        /// Признак модификации значения подполя.
        /// </summary>
        public bool Modified { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Создание представления подполя из элемента рабочего листа.
        /// </summary>
        public static SiberianSubField FromWorksheetItem (WorksheetItem item) => new ()
            {
                Code = item.Tag.FirstChar(),
                Title = item.Title
            };

        #endregion

    } // class SiberianSubField

} // namespace ManagedIrbis.WinForms.Grid
