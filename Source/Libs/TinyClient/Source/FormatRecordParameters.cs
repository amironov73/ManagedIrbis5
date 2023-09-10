// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* FormatRecordParameters.cs -- параметры форматирования записи
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры форматирования записи на ИРБИС-сервере.
    /// </summary>
    public sealed class FormatRecordParameters
    {
        #region Properties

        /// <summary>
        /// Сюда помещается результат.
        /// </summary>
        public SomeValues<string> Result { get; set; }

        /// <summary>
        /// Имя базы данных (опционально).
        /// Если не указано, используется текущая база данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Спецификация формата (обязательно).
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// MFN записи, подлежащей форматированию.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// MFN записей, подлежащих форматированию.
        /// </summary>
        public int[]? Mfns { get; set; }

        /// <summary>
        /// Запись, подлежащая форматированию.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Записи подлежащие форматированию.
        /// </summary>
        public Record[]? Records { get; set; }

        #endregion
    }
}
