// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Product.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// <para>Библиографическая запись на издание содержит библиографическую
    /// и коммерческую информацию о нем.</para>
    /// <list type="bullet">
    /// <item>регистрационный номер записи, код доступности издания,
    /// причину удаления записи и сведения о составителе записи;</item>
    /// <item>международный стандартный номер издания или его эквивалент;</item>
    /// <item>сведения о материальной конструкции издания;</item>
    /// <item>сведения о многочастном издании (многотомном, комплектном,
    /// комбинированном издании или серийном издании в целом);</item>
    /// <item>заглавие издания и сведения, относящиеся к заглавию;</item>
    /// <item>сведения о лицах и организациях, принимавших участие
    /// в подготовке издания;</item>
    /// <item>сведения о конференции, съезде и т. п., материалы которых
    /// содержит издание;</item>
    /// <item>сведения о переиздании;</item>
    /// <item>сведения о языке текста издания;</item>
    /// <item>количественные характеристики издания;</item>
    /// <item>классификационные индексы и названия рубрик таблиц классификации;</item>
    /// <item>сведения о читательском адресе издания;</item>
    /// <item>аннотацию и другой сопроводительный текст;</item>
    /// <item>ссылки на интернет-ресурсы, где помещено изображение,
    /// другие файлы, связанные с изданием;</item>
    /// <item>сведения об издателе;</item>
    /// <item>место издания;</item>
    /// <item>дату издания;</item>
    /// <item>сведения о тираже издания.</item>
    /// </list>
    /// </summary>
    /// <remarks>Элементы библиографического описания приводят в соответствии
    /// с ГОСТ 7.1.</remarks>
    [XmlRoot ("Product")]
    public sealed class Product
    {
        #region Properties

        /// <summary>
        /// Сведения о международном стандартном номере издания или его эквиваленте.
        /// </summary>
        [ShortTag ("productidentifier")]
        [XmlElement ("ProductIdentifier")]
        public ProductId? Id { get; set; }

        /// <summary>
        /// <para>Регистрационный номер записи служит для ее идентификации
        /// при информационном обмене.</para>
        /// <para>Данный номер не должен меняться для записей на одно
        /// и то же издание. Может содержать буквы и цифры, название
        /// Интернет-ресурса, на котором зарегистрировано издание.</para>
        /// <para>Регистрационный номер записи должен содержать не более
        /// 100 символов.</para>
        /// </summary>
        /// <remarks>Примеры: "bookchamber. ш. 11 -15548",
        /// "2014-389006".</remarks>
        [ShortTag ("a001")]
        [XmlElement ("RecordReference")]
        [DisplayName ("Регистрационный номер записи")]
        public string? RecordReference { get; set; }

        /// <summary>
        /// Код доступности издания.
        /// См. <see cref="NotificationType"/>.
        /// </summary>
        [ShortTag ("a002")]
        [XmlElement ("NotificationType")]
        [DisplayName ("Код доступности издания")]
        public string? NotificationType { get; set; }

        /// <summary>
        /// Причина удаления записи формулируется в свободной форме.
        /// Объяснение причины удаления записи не должно превышать 100 символов.
        /// </summary>
        /// <remarks>Пример: "Удаление дубликата записи".</remarks>
        [ShortTag ("a199")]
        [XmlElement ("DeletionText")]
        [DisplayName ("Причина удаления записи")]
        public string? DeletionText { get; set; }

        /// <summary>
        /// Код, указывающий на вид организации, создавшей запись.
        /// См. <see cref="ManagedIrbis.Onix.RecordSourceType"/>
        /// </summary>
        [ShortTag ("a194")]
        [XmlElement ("RecordSourceType")]
        [DisplayName ("Вид организации, создавшей запись")]
        public string? RecordSourceType { get; set; }

        /// <summary>
        /// Идентификационный номер организации, создавшей запись,
        /// содержит ее ИНН.
        /// </summary>
        [ShortTag ("recordsourceidentifier")]
        [XmlElement ("RecordSourceIdentifier")]
        [DisplayName ("Идентификационный номер организации")]
        public string? RecordSourceIdentifier { get; set; }

        /// <summary>
        /// Название организации, создавшей запись, должно включать
        /// не более 100 символов.
        /// </summary>
        /// <remarks>
        /// Пример: "Издательство Московского государственного университета".
        /// </remarks>
        [ShortTag ("197")]
        [XmlElement ("RecordSourceName")]
        [DisplayName ("Название организации, создавшей запись")]
        public string? RecordSourceName { get; set; }

        #endregion
    }
}
