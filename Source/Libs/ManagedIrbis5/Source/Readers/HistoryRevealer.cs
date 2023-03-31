// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HistoryRevealer.cs -- построение истории книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Collections;

using JetBrains.Annotations;

using ManagedIrbis.Batch;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Построение истории книговыдачи по базе данных <c>RDR</c>
/// </summary>
[PublicAPI]
public sealed class HistoryRevealer
{
    #region Properties

    /// <summary>
    /// Подключение к серверу.
    /// </summary>
    public ISyncProvider Provider { get; }

    /// <summary>
    /// Конфигурация.
    /// </summary>
    public VisitConfiguration Configuration { get; }

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    public string Database { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HistoryRevealer
        (
            ISyncProvider provider,
            string? database = null,
            VisitConfiguration? configuration = null
        )
    {
        Sure.NotNull (provider);

        Provider = provider;
        Configuration = configuration ?? VisitConfiguration.GetDefault();
        Database = database ?? StandardDatabases.Readers;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение истории для указанного инвентарного номера.
    /// </summary>
    public VisitInfo[] RetrieveHistoryForInventory
        (
            string inventory
        )
    {
        Sure.NotNullNorEmpty (inventory);
        Provider.EnsureConnected();

        var result = new List<VisitInfo>();
        var expression = $"v{Configuration.Tag}^{Configuration.InventorySubfieldCode}: '{inventory}'";
        var parameters = new SearchParameters
        {
            Database = Provider.EnsureDatabase (Database),
            Sequential = expression
        };
        var found = Provider.Search (parameters);
        if (found.IsNullOrEmpty())
        {
            return Array.Empty<VisitInfo>();
        }

        var range = FoundItem.ToMfn (found);
        var batch = new BatchRecordReader (Provider, range, database: Database);
        foreach (var record in batch)
        {
            foreach (var field in record.EnumerateField (Configuration.Tag))
            {
                var visit = VisitInfo.Parse (field, Configuration);
                if (visit.Database.SameString (Database)
                    && visit.InventoryNumber.SameString (inventory))
                {
                    result.Add (visit);
                }
            }
        }

        result.Sort (new VisitComparer.ByDateGiven());

        return result.ToArray();
    }

    /// <summary>
    /// Построение истории для указанного штрих-кода.
    /// </summary>
    public VisitInfo[] RetrieveHistoryForBarcode
        (
            string barcode
        )
    {
        Sure.NotNullNorEmpty (barcode);
        Provider.EnsureConnected();

        var result = new List<VisitInfo>();
        var expression = $"v{Configuration.Tag}^{Configuration.BarcodeSubfieldCode}: '{barcode}'";
        var parameters = new SearchParameters
        {
            Database = Provider.EnsureDatabase (Database),
            Sequential = expression
        };
        var found = Provider.Search (parameters);
        if (found.IsNullOrEmpty())
        {
            return Array.Empty<VisitInfo>();
        }

        var range = FoundItem.ToMfn (found);
        var batch = new BatchRecordReader (Provider, range, database: Database);
        foreach (var record in batch)
        {
            foreach (var field in record.EnumerateField (Configuration.Tag))
            {
                var visit = VisitInfo.Parse (field, Configuration);
                if (visit.Database.SameString (Database)
                    && visit.Barcode.SameString (barcode))
                {
                    result.Add (visit);
                }
            }
        }

        result.Sort (new VisitComparer.ByDateGiven());

        return result.ToArray();
    }

    #endregion
}
