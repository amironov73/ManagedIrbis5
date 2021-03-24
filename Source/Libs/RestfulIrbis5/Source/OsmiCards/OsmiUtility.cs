// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* OsmiUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Text;
using System.Web;

using AM;
using AM.IO;

using ManagedIrbis;
using ManagedIrbis.Readers;

using CM=System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    ///
    /// </summary>
    public static class OsmiUtility
    {
        #region Private members

        /// <summary>
        /// Ищем метку в карточке.
        /// </summary>
        private static /* JObject? */ object? FindLabel
            (
                // TODO: implement
                object obj,
                // JObject obj,
                string label
            )
        {
            /*

            var result = (JObject?) obj["values"].FirstOrDefault
                (
                    b => b["label"].Value<string>() == label
                );

            if (ReferenceEquals(result, null))
            {
                Magna.Info($"Block not found {label}");
            }

            return result;

            */

            throw new NotImplementedException();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build card for reader.
        /// </summary>
        public static /* JObject */ object BuildCardForReader
            (
                // TODO: implement
                object templateObject,
                // JObject templateObject,
                ReaderInfo reader,
                string ticket,
                DicardsConfiguration config
            )
        {
            /*

            var name = reader.FamilyName.ThrowIfNull("name");
            var fio = reader.FullName.ThrowIfNull("fio");

            var result = (JObject) templateObject.DeepClone();

            JObject? block = null;
            if (!string.IsNullOrEmpty(config.FioField))
            {
                block = FindLabel(result, config.FioField);
            }
            if (!ReferenceEquals(block, null))
            {
                block["value"] = fio;
            }

            block = null;
            if (!string.IsNullOrEmpty(config.CabinetField))
            {
                block = FindLabel(result, config.CabinetField);
            }
            if (!ReferenceEquals(block, null))
            {
                var cabinetUrl = config.CabinetUrl;
                if (string.IsNullOrEmpty(cabinetUrl))
                {
                    Magna.Debug("BuildCardForReader: cabinerUrl not specified!");
                }
                else
                {
                    block["value"] = string.Format
                        (
                            cabinetUrl,
                            UrlEncode(name),
                            UrlEncode(ticket)
                        );
                }
            }

            block = null;
            if (!string.IsNullOrEmpty(config.CatalogField))
            {
                block = FindLabel(result, config.CatalogField);
            }
            if (!ReferenceEquals(block, null))
            {
                var catalogUrl = config.CatalogUrl;
                if (string.IsNullOrEmpty(catalogUrl))
                {
                    Magna.Debug("BuildCardForReader: catalogUrl not specified!");
                }
                else
                {
                    block["value"] = string.Format
                        (
                            catalogUrl,
                            UrlEncode(name),
                            UrlEncode(ticket)
                        );
                }
            }

            var barcodeField = config.BarcodeField;
            if (!string.IsNullOrEmpty(barcodeField))
            {
                block = (JObject) result[barcodeField];
                if (!ReferenceEquals(block, null))
                {
                    block.Property("messageType")?.Remove();
                    block.Property("signatureType")?.Remove();
                    block["message"] = ticket;
                    block["signature"] = ticket;
                }
            }

            return result;

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Убираем '-empty-'.
        /// </summary>
        public static string? NullForEmpty
            (
                this string? value
            )
        {
            return value.SameString("-empty-")
                ? null
                : value;
        }

        /// <summary>
        /// Полный путь до <c>dicards.json</c>.
        /// </summary>
        public static string DicardsJson() => PathUtility.MapPath("dicards.json")
            .ThrowIfNull("MapPath (\"dicards.json\")");

        /// <summary>
        /// Кодирование URL в UTF-8.
        /// </summary>
        public static string UrlEncode (string text) =>
            HttpUtility.UrlEncode(text, Encoding.UTF8);

        /// <summary>
        /// Получение идентификатора читателя.
        /// </summary>
        public static string GetReaderId
            (
                Record record,
                DicardsConfiguration config
            )
        {
            var idTag = config.ReaderId.SafeToInt32(30);
            var result = record.FM(idTag).ThrowIfNull("reader.Ticket");

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
            var record = reader.Record.ThrowIfNull("reader.Record");
            var result = GetReaderId(record, config);

            return result;
        }

        #endregion
    }
}
