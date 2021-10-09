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

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Период действия записи о правах доступа.
    /// </summary>
    public class ValidityPeriod
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "de";

        #endregion

        #region Properties

        /// <summary>
        /// Начальная дата. Подполе d.
        /// </summary>
        [SubField ('d')]
        [XmlAttribute ("from")]
        [JsonPropertyName ("from")]
        public IrbisDate? From { get; set; }

        /// <summary>
        /// Конечная дата. Подполе e.
        /// </summary>
        [SubField ('e')]
        [XmlAttribute ("till")]
        [JsonPropertyName ("till")]
        public IrbisDate? Till { get; set; }

        /// <summary>
        /// Associated <see cref="Field"/>.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Field? Field { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static ValidityPeriod? Parse
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

        } // method Parse

        #endregion

    } // class ValidityPeriod

} // namespace ManagedIrbis.Drm
