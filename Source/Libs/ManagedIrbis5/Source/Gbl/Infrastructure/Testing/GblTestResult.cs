// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* GblTestResult.cs -- результат прогона одного теста глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Testing
{
    /// <summary>
    /// Результат прогона одного теста глобальной корректировки.
    /// </summary>
    public sealed class GblTestResult
    {
        #region Properties

        /// <summary>
        /// Продолжительность прогона данного теста.
        /// </summary>
        [JsonPropertyName ("duration")]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Тест провалился?
        /// </summary>
        [JsonPropertyName ("failed")]
        public bool Failed { get; set; }

        /// <summary>
        /// Момент завершения теста.
        /// </summary>
        [JsonPropertyName ("finish")]
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// Имя теста.
        /// </summary>
        [JsonPropertyName ("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Описание теста в свободной форме.
        /// </summary>
        [JsonPropertyName ("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Момент запуска теста на исполнение.
        /// </summary>
        [JsonPropertyName ("start")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Дамп входных данных.
        /// </summary>
        [JsonPropertyName ("input")]
        public string? Input { get; set; }

        /// <summary>
        /// Дамп разобранной программы.
        /// </summary>
        [JsonPropertyName ("ast")]
        public string? Ast { get; set; }

        /// <summary>
        /// Ожидаемый результат.
        /// </summary>
        [JsonPropertyName ("expected")]
        public string? Expected { get; set; }

        /// <summary>
        /// Фактически полученный результат.
        /// </summary>
        [JsonPropertyName ("output")]
        public string? Output { get; set; }

        /// <summary>
        /// Текст сообщения об ошибке (если есть).
        /// </summary>
        [JsonPropertyName ("exception")]
        public string? Exception { get; set; }

        #endregion

    } // class GblTestResult

} // namespace ManagedIrbis.Gbl.Infrastructure.Testing
