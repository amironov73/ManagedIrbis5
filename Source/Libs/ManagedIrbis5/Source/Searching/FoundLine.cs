// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* FoundLine.cs -- строчка в списке найденных документов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Строчка в списке найденных документов.
    /// </summary>
    public sealed class FoundLine
    {
        #region Properties

        /// <summary>
        /// Порядковый номер.
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// Материализовано?
        /// </summary>
        public bool Materialized { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Запись.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Иконка.
        /// </summary>
        public object? Icon { get; set; }

        /// <summary>
        /// Выбрано пользователем.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Краткое библиографическое описание.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Для сортировки списка.
        /// </summary>
        public string? Sort { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Загружаем найденные записи с сервера.
        /// </summary>
        public static FoundLine[] Read
            (
                ISyncProvider connection,
                string format,
                IEnumerable<int> found
            )
        {
            var array = found.ToArray();
            var length = array.Length;
            var result = new FoundLine[length];
            var formatted = connection.FormatRecords(array, format);
            if (formatted is null)
            {
                return Array.Empty<FoundLine>();
            }

            for (var i = 0; i < length; i++)
            {
                var item = new FoundLine
                {
                    Mfn = array[i],
                    Description = formatted[i]
                };
                result[i] = item;
            }

            return result;

        } // method Read

        #endregion

    } // class FoundLine

} // namespace ManagedIrbis
