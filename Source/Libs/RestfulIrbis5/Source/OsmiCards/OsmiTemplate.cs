// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* OsmiTemplate.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    ///
    /// </summary>
    public sealed class OsmiTemplate
    {
        #region Properties

        /// <summary>
        /// Values.
        /// </summary>
        [JsonPropertyName("values")]
        public OsmiValue[]? Values { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert <see cref="JObject"/> to
        /// <see cref="OsmiTemplate"/>.
        /// </summary>
        public static OsmiTemplate FromJObject
            (
                // TODO: implement
                object obj
                // JObject jObject
            )
        {
            var result = new OsmiTemplate();

            // TODO implement

            return result;
        }

        #endregion
    }
}
