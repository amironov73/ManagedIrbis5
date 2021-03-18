// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* OsmiUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#if !UAP

#region Using directives

using System.Linq;
using System.Text;

using AM;



using ManagedIrbis.Readers;

using Newtonsoft.Json.Linq;

#if !ANDROID && !UAP && !NETCORE

using System.Web;
using AM.IO;
using ManagedIrbis;
using CM=System.Configuration.ConfigurationManager;

#endif

#endregion

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
        [CanBeNull]
        private static JObject FindLabel
            (
                JObject obj,
                string label
            )
        {
            var result = (JObject) obj["values"].FirstOrDefault
                (
                    b => b["label"].Value<string>() == label
                );

            if (ReferenceEquals(result, null))
            {
                Log.Info($"Block not found {label}");
            }

            return result;
        }

        #endregion

        #region Public methods

#if !ANDROID && !UAP && !NETCORE

        /// <summary>
        /// Build card for reader.
        /// </summary>
        public static JObject BuildCardForReader
            (
                JObject templateObject,
                ReaderInfo reader,
                string ticket,
                DicardsConfiguration config
            )
        {
            Code.NotNull(templateObject, nameof(templateObject));
            Code.NotNull(reader, nameof(reader));

            var name = reader.FamilyName.ThrowIfNull("name");
            var fio = reader.FullName.ThrowIfNull("fio");

            var result = (JObject) templateObject.DeepClone();

            JObject block = null;
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
                    Log.Debug("BuildCardForReader: cabinerUrl not specified!");
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
                    Log.Debug("BuildCardForReader: catalogUrl not specified!");
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
        }

        /// <summary>
        /// Убираем '-empty-'.
        /// </summary>
        [CanBeNull]
        public static string NullForEmpty
            (
                [CanBeNull] this string value
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

#endif

        #endregion
    }
}

#endif
