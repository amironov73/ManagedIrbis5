// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EasyExcel.cs -- легко и просто формируем таблицу Excel
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using DevExpress.Spreadsheet;

#endregion

#nullable enable

namespace AM.Windows.DevExpress
{
    /// <summary>
    /// Легко и просто формируем таблицу Excel
    /// </summary>
    public sealed class EasyExcel
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Книга Excel.
        /// </summary>
        public Workbook Workbook { get; }

        /// <summary>
        /// Активный лист книги Excel.
        /// </summary>
        public Worksheet Worksheet { get; private set; }

        /// <summary>
        /// Текущая строка.
        /// </summary>
        public Row CurrentRow => Worksheet.Rows[_currentRow];

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public EasyExcel()
        {
            Workbook = new Workbook();
            Worksheet = Workbook.Worksheets[0];
        }

        #endregion

        #region Private members

        private int _currentRow, _currentColumn;

        #endregion

        #region Public methods

        /// <summary>
        /// Замораживаем все строки с начала до текущей (включая).
        /// </summary>
        public void FreezeRows()
        {
            Worksheet.FreezeRows (_currentRow);
        }

        /// <summary>
        /// Получение ячейки.
        /// </summary>
        public Cell GetCell
            (
                int row,
                int column
            )
        {
            Sure.NonNegative (row);
            Sure.NonNegative (column);

            return Worksheet.Cells[row, column];
        }

        /// <summary>
        /// Получение диапазона ячеек, считая от текущей.
        /// </summary>
        public CellRange GetRange
            (
                int columns
            )
        {
            return Worksheet.Range.FromLTRB
                (
                    _currentColumn,
                    _currentRow,
                    _currentColumn + columns,
                    _currentRow
                );
        }

        /// <summary>
        /// Получение строки ячеек.
        /// </summary>
        public Row GetRow
            (
                int row
            )
        {
            Sure.NonNegative (row);

            return Worksheet.Rows[row];
        }

        /// <summary>
        /// Переход к ячейке с указанными координатами.
        /// </summary>
        public void Goto
            (
                int row,
                int column = 0
            )
        {
            _currentRow = row;
            _currentColumn = column;
        }

        /// <summary>
        /// Загрузка из файла.
        /// </summary>
        public void Load
            (
                string fileName
            )
        {
            Sure.FileExists (fileName);

            Workbook.LoadDocument (fileName);
            Worksheet = Workbook.Worksheets[0];
        }

        /// <summary>
        /// Объединение ячеек.
        /// </summary>
        public CellRange MergeCells
            (
                int columns
            )
        {
            var range = GetRange (columns);
            range.Merge();

            return range;
        }

        /// <summary>
        /// Переход на новую строку.
        /// </summary>
        public void NewLine
            (
                int delta = 1
            )
        {
            _currentRow += delta;
            _currentColumn = 0;
        }

        /// <summary>
        /// Сохранение в файл.
        /// </summary>
        public void SaveAs
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty (fileName);

            Workbook.SaveDocument (fileName, DocumentFormat.Xlsx);
        }

        /// <summary>
        /// Пропуск указанного количества ячеек по горизонтали.
        /// </summary>
        public void Skip
            (
                int delta = 1
            )
        {
            _currentColumn += delta;
        }

        /// <summary>
        /// Вывод денежного числа в текущую колонку.
        /// Переход к следующей колонке.
        /// </summary>
        public Cell WriteDecimal
            (
                decimal value
            )
        {
            var result = GetCell (_currentRow, _currentColumn);
            _currentColumn++;
            result.Value = value;

            return result;
        }

        /// <summary>
        /// Вывод числа с плавающей точкой в текущую колонку.
        /// Переход к следующей колонке.
        /// </summary>
        public Cell WriteDouble
            (
                double value
            )
        {
            var result = GetCell (_currentRow, _currentColumn);
            _currentColumn++;
            result.Value = value;

            return result;
        }

        /// <summary>
        /// Вывод целого числа в текущую колонку.
        /// Переход к следующей колонке.
        /// </summary>
        public Cell WriteInt32 (int value)
        {
            var result = GetCell (_currentRow, _currentColumn);
            _currentColumn++;
            result.Value = value;

            return result;
        }

        /// <summary>
        /// Вывод текста в указанную колонку.
        /// </summary>
        public Cell WriteText (int column, string text)
        {
            var result = GetCell (_currentRow, column);
            result.Value = text;

            return result;
        }

        /// <summary>
        /// Вывод текста в текущую колонку.
        /// Переход к следующей колонке.
        /// </summary>
        public Cell WriteText (string text)
        {
            var result = GetCell (_currentRow, _currentColumn);
            _currentColumn++;
            result.Value = text;

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Workbook.Dispose();
        }

        #endregion
    }
}
