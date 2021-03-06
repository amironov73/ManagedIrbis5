﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* FormatRecordParameters.cs -- параметры форматирования записи на ИРБИС-сервере
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
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

    } // class FormatRecordParameters

} // namespace ManagedIrbis.Infrastructure
