// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* InventoryManager.cs -- менеджер инвентаризации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using AM;
using AM.Text;

using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis.Inventory
{
    /// <summary>
    /// Менеджер инвентаризации.
    /// </summary>
    public sealed class InventoryManager
    {
        #region Properties

        /// <summary>
        /// Синхронный провайдер.
        /// </summary>
        public ISyncProvider Provider { get; }

        /// <summary>
        /// Конфигурация записей.
        /// </summary>
        public RecordConfiguration RecordConfiguration { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public InventoryManager
            (
                ISyncProvider provider,
                RecordConfiguration? configuration = null
            )
        {
            Provider = provider;
            RecordConfiguration = configuration ?? RecordConfiguration.GetDefault();

        } // constructor

        #endregion

        #region Private members

        /// <summary>
        /// Проверка, начинается ли строка с указанного префикса.
        /// </summary>
        [Pure]
        private static bool SafeStarts (string? text, string prefix) =>
            !string.IsNullOrEmpty (text) && text.StartsWith (prefix);

        #endregion

        #region Public methods

        /// <summary>
        /// Извлечение экземпляров.
        /// </summary>
        public void ExtractExemplars
            (
                string place,
                IExemplarSink exemplarSink,
                IProgress<int>? progress = null
            )
        {
            Sure.NotNullNorEmpty (place);

            var searchExpression = $"\"MHR={place}\" + \"INP={place}-$\"";
            var batch = BatchRecordReader.Search (Provider, searchExpression);
            var index = 0;
            foreach (var record in batch)
            {
                progress?.Report (++index);

                var allExemplars = ExemplarInfo.Parse (record, RecordConfiguration.ExemplarTag);
                var goodExemplars = allExemplars
                    .Where (e =>
                        (
                            e.Place.SameString (place)
                                || e.RealPlace.SameString (place)
                                || SafeStarts (e.CheckedDate, place)
                            )
                            && !e.Status.SameString (ExemplarStatus.Bound)
                        );

                exemplarSink.AddExemplars (goodExemplars);
            }

        } // method ExtractExemplars

        #endregion


    } // class InventoryManager

} // namespace ManagerIrbis.Inventory
