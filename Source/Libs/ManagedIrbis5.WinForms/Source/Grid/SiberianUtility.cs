// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianUtility.cs -- полезные методы для работы с гридом, ячейками, колонками и строками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Полезные методы для работы с гридом, ячейками, колонками и строками.
    /// </summary>
    public static class SiberianUtility
    {
        #region Public methods

        /// <summary>
        /// Добавление к гриду целочисленной колонки.
        /// </summary>
        public static SiberianColumn AddIntegerColumn
            (
                this SiberianGrid grid,
                string? title = null
            )
        {
            var result = grid.CreateColumn<SiberianIntegerColumn>();
            result.Title = title;

            return result;

        } // method AddIntegerColumn

        /// <summary>
        /// Добавление к гриду целочисленной колонки.
        /// </summary>
        public static SiberianColumn AddPropertyColumn
            (
                this SiberianGrid grid,
                string propertyName,
                string? title = null
            )
        {
            var result = grid.CreateColumn<SiberianPropertyColumn>();
            result.Title = title;
            result.Member = propertyName;

            return result;

        } // method AddPropertyColumn

        /// <summary>
        /// Добавление к гриду текстовой колонки.
        /// </summary>
        public static SiberianColumn AddTextColumn
            (
                this SiberianGrid grid,
                string? title = null
            )
        {
            var result = grid.CreateColumn<SiberianTextColumn>();
            result.Title = title;

            return result;

        } // method AddTextColumn

        /// <summary>
        /// Получаем колонку, которой принадлежит ячейка.
        /// </summary>
        public static SiberianColumn EnsureColumn (this SiberianCell cell) =>
            cell.Column.ThrowIfNull (nameof (cell.Column));

        /// <summary>
        /// Получаем грид, которому принадлежит ячейка.
        /// </summary>
        public static SiberianGrid EnsureGrid (this SiberianCell cell) =>
            cell.Grid.ThrowIfNull (nameof (cell.Grid));

        /// <summary>
        /// Получаем строку, которой принадлежит ячейка.
        /// </summary>
        public static SiberianRow EnsureRow (this SiberianCell cell) =>
            cell.Row.ThrowIfNull (nameof (cell.Row));

        /// <summary>
        /// Получение размеров указанного текста.
        /// </summary>
        public static void MeasureText
            (
                this SiberianGrid grid,
                string text,
                SiberianDimensions dimensions
            )
        {
            var size = dimensions.ToSize();
            var font = grid.Font;
            var flags = TextFormatFlags.Left
                        | TextFormatFlags.Top
                        | TextFormatFlags.NoPrefix;

            var result = TextRenderer.MeasureText
                (
                    text,
                    font,
                    size,
                    flags
                );

            dimensions.Width = result.Width;
            dimensions.Height = result.Height;

        } // method MeasureText

        /// <summary>
        /// Установка заголовка.
        /// </summary>
        public static T SetTitle<T>(this T column, string? title) where T : SiberianColumn
        {
            column.Title = title;
            return column;

        } // method SetFillWidth

        /// <summary>
        /// Установка относительной ширины колонки.
        /// </summary>
        public static T SetFillWidth<T>(this T column, int width) where T : SiberianColumn
        {
            column.FillWidth = width;
            return column;

        } // method SetFillWidth

        /// <summary>
        /// Установка минимальной ширины колонки.
        /// </summary>
        public static T SetMinWidth<T>(this T column, int width) where T : SiberianColumn
        {
            column.MinWidth = width;
            return column;

        } // method SetMinWidth

        /// <summary>
        /// Установка абсолютной ширины колонки.
        /// </summary>
        public static T SetWidth<T>(this T column, int width) where T : SiberianColumn
        {
            column.Width = width;
            return column;

        } // method SetWidth

        #endregion

    } // class SiberianUtility

} // namespace ManagedIrbis.WinForms.Grid
