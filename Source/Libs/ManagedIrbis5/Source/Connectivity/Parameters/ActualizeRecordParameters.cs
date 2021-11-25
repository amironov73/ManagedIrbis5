// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ActualizeRecordParameters.cs -- параметры актуализации записи на ИРБИС-сервере
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Параметры актуализации записи на ИРБИС-сервере.
    /// </summary>
    public sealed class ActualizeRecordParameters
    {
        #region Properties

        /// <summary>
        /// Имя базы данных (опционально).
        /// Если не указано, используется текущая база данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN актуализируемой записи (обязательно).
        /// 0 означает "актуализировать всю базу данных".
        /// </summary>
        public int Mfn { get; set; }

        #endregion
    }
}
