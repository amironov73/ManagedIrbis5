// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Polzv.cs -- данные пользователя ИРБИС в базе данных CMPL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Данные пользователя ИРБИС в базе данных CMPL.
    /// </summary>
    public sealed class Polzv
    {
        #region Properties

        /// <summary>
        /// Обращение для карточки-заказа на книги.
        /// </summary>
        [Field (12)]
        public object? Accost { get; set; }

        /// <summary>
        /// Подпись для карточки-заказа на книги.
        /// </summary>
        [Field (13)]
        public object? Signature { get; set; }

        /// <summary>
        /// Отправитель для письма-заказа на книги.
        /// </summary>
        [Field (15)]
        public object? Sender { get; set; }

        /// <summary>
        /// Директор (организации) и гл.бух. - для подписей.
        /// </summary>
        [Field (14)]
        [XmlElement ("director")]
        [JsonPropertyName ("director")]
        [Description ("Директор и главный бухгалтер")]
        [DisplayName ("Директор и главный бухгалтер")]
        public DirectorInfo? Director { get; set; }

        /// <summary>
        /// Максимальный номер заказа книг.
        /// </summary>
        [Field (30)]
        public string? MaxOrder { get; set; }

        /// <summary>
        /// Максимальный номер акта индивидуального учета.
        /// </summary>
        [Field (80)]
        public string? MaxAct1 { get; set; }

        /// <summary>
        /// Максимальный код организации.
        /// </summary>
        [Field (81)]
        public string? MaxOrganization { get; set; }

        /// <summary>
        /// Максимальный номер записи в Книге суммарного учета поступлений.
        /// </summary>
        [Field (88)]
        public string? MaxKsu1 { get; set; }

        /// <summary>
        /// Максимальный инвентарный номер
        /// </summary>
        [Field (910)]
        public string? MaxInventory { get; set; }

        /// <summary>
        /// Максимальный номер записи в Книге суммарного учета выбытия.
        /// </summary>
        [Field (888)]
        public string? MaxKsu2 { get; set; }

        /// <summary>
        /// Максимальный номер акта передачи выбывших книг.
        /// </summary>
        [Field (800)]
        public string? MaxAct2 { get; set; }

        /// <summary>
        /// Шифр документа в базе. Поле 903
        /// </summary>
        [Field (903)]
        public string? Index { get; set; }

        #endregion
    }
}
