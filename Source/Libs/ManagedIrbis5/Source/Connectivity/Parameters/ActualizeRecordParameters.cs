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
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN актуализируемой записи (обязательно).
        /// 0 означает "актуализировать всю базу данных".
        /// </summary>
        public int Mfn { get; set; }

        #endregion

    } // class ActualizeRecordParameters

} // namespace ManagedIrbis.Infrastructure
