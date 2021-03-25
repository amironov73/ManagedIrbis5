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

/* ReadRecordParameters.cs -- параметры чтения записи с ИРБИС-сервера
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Параметры чтения записи с ИРБИС-сервера.
    /// </summary>
    public sealed class ReadRecordParameters
    {
        #region Properties

        /// <summary>
        /// Результат помещается сюда.
        /// </summary>
        public IRecord? Record { get; set; }

        /// <summary>
        /// Имя базы данных (опционально).
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN записи (обязательно).
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Оставить запись заблокированной?
        /// </summary>
        public bool Lock { get; set; }

        /// <summary>
        /// Номер версии (опционально).
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Формат (опционально).
        /// </summary>
        public string? Format { get; set; }

        #endregion

    } // class ReadRecordParameters

} // namespace ManagedIrbis.Infrastructure
