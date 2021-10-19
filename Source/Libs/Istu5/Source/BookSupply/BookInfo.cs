// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* BookInfo.cs -- информация об учебной книге
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;

using ManagedIrbis;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Информация об учебной книге.
    /// </summary>
    [Serializable]
    public sealed class BookInfo
    {
        #region Properties

        /// <summary>
        /// Количество экземпляров.
        /// </summary>
        [JsonPropertyName ("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// Автор(ы).
        /// </summary>
        [JsonPropertyName ("author")]
        public string? Author { get; set; }

        /// <summary>
        /// Номер карточки комлектования.
        /// </summary>
        [JsonPropertyName ("card")]
        public string? CardNumber { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [JsonPropertyName ("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Электронный учебник?
        /// </summary>
        [JsonPropertyName ("electronic")]
        public bool IsElectronic { get; set; }

        /// <summary>
        /// Порядковый номер (для упорядочения).
        /// </summary>
        [JsonIgnore]
        public int Ordinal { get; internal set; }

        /// <summary>
        /// Библиографическая запись.
        /// </summary>
        [JsonIgnore]
        public Record? Record { get; set; }

        /// <summary>
        /// Данная книга выбрана (отмечена)?
        /// </summary>
        [JsonIgnore]
        public bool Selected { get; set; }

        /// <summary>
        /// Гриф УМО.
        /// </summary>
        [JsonPropertyName ("stamp")]
        public string? Stamp { get; set; }

        /// <summary>
        /// Год издания.
        /// </summary>
        [JsonPropertyName ("year")]
        public string? Year { get; set; }

        /// <summary>
        /// Книжная серия (например, "Науки о Земле" или
        /// "Из истории мировой культуры").
        /// </summary>
        [JsonPropertyName ("series")]
        public string? Series { get; set; }

        /// <summary>
        /// Издающая организация (издательство).
        /// </summary>
        [JsonPropertyName ("publisher")]
        public string? Publisher { get; set; }

        /// <summary>
        /// Место издания (город).
        /// </summary>
        [JsonPropertyName ("city")]
        public string? City { get; set; }

        /// <summary>
        /// Номер тома (если есть).
        /// </summary>
        [JsonPropertyName ("volume")]
        public string? Volume { get; set; }

        /// <summary>
        /// Авторский знак (упорядчение на полке).
        /// </summary>
        [JsonPropertyName ("sign")]
        public string? AuthorSign { get; set; }

        /// <summary>
        /// Полочный индекс (расстановочный шифр).
        /// </summary>
        [JsonPropertyName ("shelf")]
        public string? ShelfCode { get; set; }

        #endregion

    } // class BookInfo

} // namespace Istu.BookSupply
