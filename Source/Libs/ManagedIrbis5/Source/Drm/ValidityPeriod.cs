// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ValidityPeriod.cs -- период действия записи о правах доступа к ресурсу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Период действия записи о правах доступа.
    /// </summary>
    public sealed class ValidityPeriod
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 2;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "de";

        #endregion

        #region Properties

        /// <summary>
        /// Начальная дата. Подполе D.
        /// </summary>
        [SubField ('d')]
        [XmlAttribute ("from")]
        [JsonPropertyName ("from")]
        public IrbisDate? From { get; set; }

        /// <summary>
        /// Конечная дата. Подполе E.
        /// </summary>
        [SubField ('e')]
        [XmlAttribute ("till")]
        [JsonPropertyName ("till")]
        public IrbisDate? Till { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlElement ("unknown")]
        [JsonPropertyName ("unknown")]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ассоциированное поле библиографической записи <see cref="Field"/>.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Field? Field { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля библиографической записи.
        /// </summary>
        public static ValidityPeriod? ParseField
            (
                Field? field
            )
        {
            if (field is null)
            {
                return null;
            }

            var result = new ValidityPeriod
            {
                From = IrbisDate.ConvertStringToDate (field.GetFirstSubFieldValue ('d')),
                Till = IrbisDate.ConvertStringToDate (field.GetFirstSubFieldValue ('e')),
                Field = field
            };

            return result;
        }

        #endregion

    }
}
