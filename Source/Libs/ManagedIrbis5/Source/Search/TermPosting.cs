// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TermPosting.cs -- постинг термина
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Постинг термина.
    /// </summary>
    public sealed class TermPosting
    {
        #region Properties

        /// <summary>
        /// MFN записи с искомым термом.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Тег поля с искомым термом.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Количество повторений.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Результат форматирования.
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static TermPosting[] Parse
            (
                Response response
            )
        {
            var result = new List<TermPosting>();
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split('#', 5);
                if (parts.Length < 4)
                {
                    break;
                }

                var item = new TermPosting
                {
                    Mfn = int.Parse(parts[0]),
                    Tag = int.Parse(parts[1]),
                    Occurrence = int.Parse(parts[2]),
                    Count = int.Parse(parts[3]),
                    Text = parts.Length == 5 ? parts[4] : string.Empty
                };
                result.Add(item);
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            return $"{Mfn}#{Tag}#{Occurrence}#{Count}#{Text}";
        }

        #endregion
    }
}
