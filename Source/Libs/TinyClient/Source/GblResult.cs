// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* GblResult.cs -- резульат исполнения глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Результат исполнения глобальной корректировки.
    /// </summary>
    public sealed class GblResult
    {
        #region Properties

        /// <summary>
        /// Момент начала обработки.
        /// </summary>
        public DateTime TimeStarted { get; set; }

        /// <summary>
        /// Всего времени затрачено (с момента начала обработки).
        /// </summary>
        public TimeSpan TimeElapsed { get; set; }

        /// <summary>
        /// Отменено пользователем.
        /// </summary>
        public bool Canceled { get; set; }

        /// <summary>
        /// Исключение (если возникло).
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// Предполагалось обработать записей.
        /// </summary>
        public int RecordsSupposed { get; set; }

        /// <summary>
        /// Обработано записей.
        /// </summary>
        public int RecordsProcessed { get; set; }

        /// <summary>
        /// Успешно обработано записей.
        /// </summary>
        public int RecordsSucceeded { get; set; }

        /// <summary>
        /// Ошибок при обработке записей.
        /// </summary>
        public int RecordsFailed { get; set; }

        /// <summary>
        /// Результаты для каждой записи.
        /// </summary>
        public GblProtocolLine[]? Protocol { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Получение пустого результата
        /// (например, для пополения его в дальнейшем).
        /// </summary>
        public static GblResult GetEmptyResult()
        {
            var result = new GblResult
            {
                TimeStarted = DateTime.Now,
                TimeElapsed = new TimeSpan (0)
            };

            return result;
        }

        /// <summary>
        /// Слияние с другим результатом.
        /// </summary>
        public void MergeResult
            (
                GblResult intermediateResult
            )
        {
            if (intermediateResult.Canceled)
            {
                Canceled = intermediateResult.Canceled;
            }

            if (!ReferenceEquals (intermediateResult.Exception, null))
            {
                Exception = intermediateResult.Exception;
            }

            RecordsProcessed += intermediateResult.RecordsProcessed;
            RecordsFailed += intermediateResult.RecordsFailed;
            RecordsSucceeded += intermediateResult.RecordsSucceeded;
            Protocol ??= Array.Empty<GblProtocolLine>();
            var otherLines
                = intermediateResult.Protocol ?? Array.Empty<GblProtocolLine>();
            Protocol = Private.Merge (Protocol, otherLines);
        }

        /// <summary>
        /// Parse server response.
        /// </summary>
        public void Parse
            (
                Response response
            )
        {
            Protocol = GblProtocolLine.Decode (response);
            RecordsProcessed = Protocol.Length;
            RecordsSucceeded = Protocol.Count (line => line.Success);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"Records processed: {RecordsProcessed}, Canceled: {Canceled}";
        }

        #endregion
    }
}
