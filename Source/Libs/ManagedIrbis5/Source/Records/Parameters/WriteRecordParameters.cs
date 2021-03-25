// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* WriterRecordParameters.cs -- параметры сохранения записи на ИРБИС-сервере
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Параметры сохранения записи на ИРБИС-сервере.
    /// </summary>
    public sealed class WriteRecordParameters
    {
        #region Properties

        /// <summary>
        /// Запись (обязательно).
        /// </summary>
        public IRecord? Record { get; set; }

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

    } // class WriteRecordParameters

} // namespace ManagedIrbis.Infrastructure
