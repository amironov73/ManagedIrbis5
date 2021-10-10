// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* KeyDefinition.cs -- ключ сортировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Tables
{
    /// <summary>
    /// Ключ сортировки.
    /// </summary>
    public sealed class KeyDefinition
    {
        #region Properties

        /// <summary>
        /// Длина ключа.
        /// </summary>
        [XmlAttribute ("length")]
        [JsonPropertyName ("length")]
        public int Length { get; set; }

        /// <summary>
        /// Допустимы множественные значения?
        /// </summary>
        [XmlAttribute ("multiple")]
        [JsonPropertyName ("multiple")]
        public bool Multiple { get; set; }

        /// <summary>
        /// Спецификация формата.
        /// </summary>
        [XmlElement ("format")]
        [JsonPropertyName ("format")]
        public string? Format { get; set; }

        #endregion

    } // class KeyDefinition

} // namespace ManagedIrbis.Tables
