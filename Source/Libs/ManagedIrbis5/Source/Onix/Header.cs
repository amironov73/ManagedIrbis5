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

/* Header.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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
    /// <para>Заголовок сообщения содержит:</para>
    /// <list type="bullet">
    /// <item>сведения об отправителе сообщения;</item>
    /// <item>сведения об адресате сообщения;</item>
    /// <item>номер сообщения;</item>
    /// <item>дату и время создания сообщения;</item>
    /// <item>дополнительные сведения о сообщении;</item>
    /// <item>сведения о языке текста издания;</item>
    /// <item>сведения о виде цены;</item>
    /// <item>обозначение валюты (денежной единицы).</item>
    /// </list>
    /// </summary>
    public sealed class Header
    {
        #region Properties

        /// <summary>
        /// Сведения об отправителие сообщения.
        /// </summary>
        [XmlElement ("Sender")]
        public Sender? Sender { get; set; }

        /// <summary>
        /// Сведения о получателе сообщения.
        /// </summary>
        [XmlElement ("Addressee")]
        public Addressee? Addressee { get; set; }

        /// <summary>
        /// Порядковый номер сообщения используется торговыми партнерами
        /// во избежание пропусков и повторов сообщений.
        /// </summary>
        [XmlElement ("MessageNumber")]
        public string? MessageNumber { get; set; }

        /// <summary>
        /// Время отправки сообщения может содержать только дату (ГГГГММДД)
        /// или дату и время суток в 24-часовом формате (ГГГГММДДЧЧММ).
        /// </summary>
        [XmlElement ("SentDateTime")]
        public string? SentDateTime { get; set; }

        /// <summary>
        /// При необходимости приводят дополнительные сведения о сообщении.
        /// Текст с дополнительной информацией о сообщении должен содержать
        /// не более 500 символов.
        /// </summary>
        /// <remarks>Пример: "Заменяет предыдущий прайс-лист".</remarks>
        [XmlElement ("MessageNote")]
        public string? MessageNote { get; set; }

        /// <summary>
        /// Сведения о языке текста издания (изданий), указанного в сообщении,
        /// приводят, если язык не указан в описании издания. Для обозначения
        /// языка используют его код.
        /// </summary>
        [XmlElement ("DefaultLanguageOfText")]
        public string? DefaultLanguageOfText { get; set; }

        /// <summary>
        /// Код вида цены, которую приводят в сообщении.
        /// См. <see cref="PriceTypeCode"/>.
        /// </summary>
        /// <remarks>Пример: "05"</remarks>
        [XmlElement ("DefaultPriceType")]
        public string? DefaultPriceType { get; set; }

        /// <summary>
        /// Обозначение валюты (денежной единицы).
        /// </summary>
        /// <remarks>Пример: "RUR"</remarks>
        [XmlElement ("DefaultCurrencyCode")]
        public string? DefaultCurrencyCode { get; set; }

        #endregion
    }
}
