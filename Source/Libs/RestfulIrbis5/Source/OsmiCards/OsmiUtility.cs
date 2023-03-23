// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* OsmiUtility.cs -- полезные методы для работы с системой OSMI Cards
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Text;
using System.Web;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using CM = System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace RestfulIrbis.OsmiCards;

/// <summary>
/// Полезные методы для работы с системой OSMI Cards.
/// </summary>
[PublicAPI]
public static class OsmiUtility
{
    #region Private members

    /// <summary>
    /// Поиск указанной метки в карточке.
    /// </summary>
    private static JObject? FindLabel
        (
            JObject obj,
            string label
        )
    {
        Sure.NotNull (obj);

        var result = (JObject?) obj["values"].ThrowIfNull()
            .FirstOrDefault
                (
                    token => token["label"]?.Value<string>() == label
                );

        if (result is null)
        {
            Magna.Logger.LogInformation
                (
                    "FindLabel:: Block not found: {Label}",
                    label
                );
        }

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение карточки для указанного читателя.
    /// </summary>
    public static JObject BuildCardForReader
        (
            JObject templateObject,
            ReaderInfo reader,
            string ticket,
            DicardsConfiguration config
        )
    {
        Sure.NotNull (templateObject);
        Sure.NotNull (reader);
        Sure.NotNullNorEmpty (ticket);
        Sure.NotNull (config);

        var name = reader.FamilyName.ThrowIfNull();
        var fio = reader.FullName.ThrowIfNull();
        var result = (JObject) templateObject.DeepClone();

        JObject? block = null;
        if (!string.IsNullOrEmpty (config.FioField))
        {
            block = FindLabel (result, config.FioField);
        }

        if (block is not null)
        {
            block["value"] = fio;
        }

        block = null;
        if (!string.IsNullOrEmpty (config.CabinetField))
        {
            block = FindLabel (result, config.CabinetField);
        }

        if (block is not null)
        {
            var cabinetUrl = config.CabinetUrl;
            if (string.IsNullOrEmpty (cabinetUrl))
            {
                Magna.Logger.LogInformation
                    (
                        "BuildCardForReader:: cabinetUrl not specified"
                    );
            }
            else
            {
                block["value"] = string.Format
                    (
                        cabinetUrl,
                        UrlEncode (name),
                        UrlEncode (ticket)
                    );
            }
        }

        block = null;
        if (!string.IsNullOrEmpty (config.CatalogField))
        {
            block = FindLabel (result, config.CatalogField);
        }

        if (block is not null)
        {
            var catalogUrl = config.CatalogUrl;
            if (string.IsNullOrEmpty (catalogUrl))
            {
                Magna.Logger.LogInformation
                    (
                        "BuildCardForReader:: catalogUrl not specified"
                    );
            }
            else
            {
                block["value"] = string.Format
                    (
                        catalogUrl,
                        UrlEncode (name),
                        UrlEncode (ticket)
                    );
            }
        }

        var barcodeField = config.BarcodeField;
        if (!string.IsNullOrEmpty (barcodeField))
        {
            block = (JObject?) result[barcodeField];
            if (block is not null)
            {
                block.Property ("messageType")?.Remove();
                block.Property ("signatureType")?.Remove();
                block["message"] = ticket;
                block["signature"] = ticket;
            }
        }

        return result;
    }

    /// <summary>
    /// Убираем '-empty-'.
    /// </summary>
    public static string? NullForEmpty (this string? value) =>
        value.SameString ("-empty-") ? null : value;

    /// <summary>
    /// Полный путь до <c>dicards.json</c>.
    /// </summary>
    public static string DicardsJson() => PathUtility.MapPath ("dicards.json").ThrowIfNull();

    /// <summary>
    /// Кодирование URL в UTF-8.
    /// </summary>
    public static string UrlEncode (string text) =>
        HttpUtility.UrlEncode (text, Encoding.UTF8);

    /// <summary>
    /// Получение идентификатора читателя.
    /// </summary>
    public static string GetReaderId
        (
            Record record,
            DicardsConfiguration config
        )
    {
        Sure.NotNull (record);
        Sure.NotNull (config);

        var idTag = config.ReaderId.SafeToInt32 (30);
        var result = record.FM (idTag).ThrowIfNullOrEmpty();

        return result;
    }

    /// <summary>
    /// Получение идентификатора читателя.
    /// </summary>
    public static string GetReaderId
        (
            ReaderInfo reader,
            DicardsConfiguration config
        )
    {
        Sure.NotNull (reader);
        Sure.NotNull (config);

        var record = reader.Record.ThrowIfNull();
        var result = GetReaderId (record, config);

        return result;
    }

    #endregion
}
