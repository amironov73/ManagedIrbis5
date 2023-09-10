// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* WriteRecordParameters.cs -- параметры сохранения записей
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры сохранения записи/записей на ИРБИС-сервере.
    /// </summary>
    public sealed class WriteRecordParameters
    {
        #region Properties

        /// <summary>
        /// Запись (обязательно, если записываем одну).
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Много записей (обязательно, если записываем несколько).
        /// </summary>
        public Record[]? Records { get; set; }

        /// <summary>
        /// Оставить запись заблокированной?
        /// </summary>
        public bool Lock { get; set; }

        /// <summary>
        /// Актуализировать поисковый индекс?
        /// </summary>
        public bool Actualize { get; set; }

        /// <summary>
        /// Новое значение MaxMfn (устанавливает сервер).
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Не разбирать ответ сервера.
        /// </summary>
        public bool DontParse { get; set; }

        #endregion
    }
}
