// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* TestResult.cs -- результат прогона одного теста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Результат прогона одного теста.
    /// </summary>
    public sealed class TestResult
    {
        #region Properties

        /// <summary>
        /// Продолжительность выполнения теста.
        /// </summary>
        [JsonPropertyName ("duration")]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Тест провален?
        /// </summary>
        [JsonPropertyName ("failed")]
        public bool Failed { get; set; }

        /// <summary>
        /// Тест проигнорирован?
        /// </summary>
        [JsonPropertyName ("ignored")]
        public bool Ignored { get; set; }

        /// <summary>
        /// Момент окончания прогона.
        /// </summary>
        [JsonPropertyName ("finish")]
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// Имя теста.
        /// </summary>
        [JsonPropertyName ("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Описание (берется из файла с описанием).
        /// </summary>
        [JsonPropertyName ("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Момент начала прогона.
        /// </summary>
        [JsonPropertyName ("start")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Исходный код тестируемого скрипта.
        /// </summary>
        [JsonPropertyName ("source")]
        public string? Source { get; set; }

        /// <summary>
        /// Входные данные (если есть).
        /// </summary>
        [JsonPropertyName ("input")]
        public string? Input { get; set; }

        /// <summary>
        /// Ожидаемые выходные данные.
        /// </summary>
        [JsonPropertyName ("expected")]
        public string? Expected { get; set; }

        /// <summary>
        /// Фактические выходные данные.
        /// </summary>
        [JsonPropertyName ("output")]
        public string? Output { get; set; }

        /// <summary>
        /// Исключение (если было).
        /// </summary>
        [JsonPropertyName ("exception")]
        public string? Exception { get; set; }

        #endregion
    }
}
