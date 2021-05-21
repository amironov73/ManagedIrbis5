// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* OsmiImage.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using AM;

#endregion

#nullable enable

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    ///
    /// </summary>

    public sealed class OsmiImage
    {
        #region Properties

        /// <summary>
        /// Image type: logo, strip etc.
        /// </summary>
        [JsonPropertyName("imgType")]
        public string? ImageType { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [JsonPropertyName("imgDescription")]
        public string? Description { get; set; }

        /// <summary>
        /// Identifier.
        /// </summary>
        [JsonPropertyName("imgId")]
        public string? Id { get; set; }

        /// <summary>
        /// Usage count.
        /// </summary>
        [JsonPropertyName("usageCount")]
        public int UsageCount { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert Newtonsoft JObject to
        /// <see cref="OsmiImage"/>.
        /// </summary>
        public static OsmiImage FromJObject
            (
                // TODO: implement
                object obj
                // JObject jObject
            )
        {
            /*

            var value = jObject.ToObject<OsmiImage>();

            return value;

            */

            throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            $"{Id.ToVisibleString()} - {Description.ToVisibleString()}";

        #endregion
    }
}
