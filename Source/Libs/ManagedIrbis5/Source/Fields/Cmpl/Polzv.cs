// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Polzv.cs -- данные пользователя ИРБИС в базе данных CMPL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

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
    [XmlElement ("accost")]
    [JsonPropertyName ("accost")]
    [Description ("Обращение для карточки-заказа на книги")]
    [DisplayName ("Обращение для карточки-заказа на книги")]
    public object? Accost { get; set; }

    /// <summary>
    /// Подпись для карточки-заказа на книги.
    /// </summary>
    [Field (13)]
    [XmlElement ("signature")]
    [JsonPropertyName ("signature")]
    [Description ("Подпись для карточки-заказа на книги")]
    [DisplayName ("Подпись для карточки-заказа на книги")]
    public Signature? Signature { get; set; }

    /// <summary>
    /// Отправитель для письма-заказа на книги.
    /// </summary>
    [Field (15)]
    [XmlElement ("sender")]
    [JsonPropertyName ("sender")]
    [Description ("Отправитель")]
    [DisplayName ("Отправитель")]
    public Sender? Sender { get; set; }

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
    [XmlElement ("max-order")]
    [JsonPropertyName ("max-order")]
    [Description ("Максимальный номер заказа книг")]
    [DisplayName ("Максимальный номер заказа книг")]
    public string? MaxOrder { get; set; }

    /// <summary>
    /// Максимальный номер акта индивидуального учета.
    /// </summary>
    [Field (80)]
    [XmlElement ("max-act1")]
    [JsonPropertyName ("max-act1")]
    [Description ("Максимальный номер акта индивидуального учета")]
    [DisplayName ("Максимальный номер акта индивидуального учета")]
    public string? MaxAct1 { get; set; }

    /// <summary>
    /// Максимальный код организации.
    /// </summary>
    [Field (81)]
    [XmlElement ("max-organization")]
    [JsonPropertyName ("max-organization")]
    [Description ("Максимальный код организации")]
    [DisplayName ("Максимальный код организации")]
    public string? MaxOrganization { get; set; }

    /// <summary>
    /// Максимальный номер записи в Книге суммарного учета поступлений.
    /// </summary>
    [Field (88)]
    [XmlElement ("max-ksu1")]
    [JsonPropertyName ("max-ksu1")]
    [Description ("Максимальный номер записи в книге суммарного учета поступлений")]
    [DisplayName ("Максимальный номер записи в книге суммарного учета поступлений")]
    public string? MaxKsu1 { get; set; }

    /// <summary>
    /// Максимальный инвентарный номер.
    /// </summary>
    [Field (910)]
    [XmlElement ("max-inventory")]
    [JsonPropertyName ("max-inventory")]
    [Description ("Максимальный инвентарный номер")]
    [DisplayName ("Максимальный инвентарный номер")]
    public string? MaxInventory { get; set; }

    /// <summary>
    /// Максимальный номер записи в Книге суммарного учета выбытия.
    /// </summary>
    [Field (888)]
    [XmlElement ("max-ksu2")]
    [JsonPropertyName ("max-ksu2")]
    [Description ("Максимальный номер записи в книге суммарного учета выбытия")]
    [DisplayName ("Максимальный номер записи в книге суммарного учета выбытия")]
    public string? MaxKsu2 { get; set; }

    /// <summary>
    /// Максимальный номер акта передачи выбывших книг.
    /// </summary>
    [Field (800)]
    [XmlElement ("max-act2")]
    [JsonPropertyName ("max-act2")]
    [Description ("Максимальный номер акта передачи выбывших книг")]
    [DisplayName ("Максимальный номер акта передачи выбывших книг")]
    public string? MaxAct2 { get; set; }

    /// <summary>
    /// Шифр документа в базе. Поле 903
    /// </summary>
    [Field (903)]
    [XmlElement ("index")]
    [JsonPropertyName ("index")]
    [Description ("Шифр в базе")]
    [DisplayName ("Шифр в базе")]
    public string? Index { get; set; }

    #endregion
}
