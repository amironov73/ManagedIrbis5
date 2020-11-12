// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* OsmiCard.cs -- карточка пользователя системы OSMI Cards.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// Карточка пользователя системы OSMI Cards.
    /// </summary>
    public sealed class OsmiCard
    {
        #region Properties

        /// <summary>
        /// Массив пар "ключ-значение".
        /// </summary>
        [JsonPropertyName("values")]
        public OsmiValue[]? Values { get; set; }

        #endregion
    }
}
