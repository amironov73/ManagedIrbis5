// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* OsmiValue.cs -- пара "ключ-значение" в карте.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// Пара "ключ-значение" в карте.
    /// </summary>
    public sealed class OsmiValue
    {
        #region Properties

        /// <summary>
        /// Label.
        /// </summary>
        [JsonPropertyName("label")]
        public string? Label { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Alternative value.
        /// </summary>
        [JsonPropertyName("altValue")]
        public string? AltValue { get; set; }

        /// <summary>
        /// Change message.
        /// </summary>
        [JsonPropertyName("changeMsg")]
        public string? ChangeMessage { get; set; }

        #endregion
    }
}
