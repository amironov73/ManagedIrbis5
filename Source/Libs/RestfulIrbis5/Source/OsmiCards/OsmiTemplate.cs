// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OsmiTemplate.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

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
        [CanBeNull]
        [JsonProperty("values")]
        public OsmiValue[] Values { get; set; }

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
                JObject jObject
            )
        {
            Code.NotNull(jObject, "jObject");

            OsmiTemplate result = new OsmiTemplate();

            return result;
        }

        #endregion
    }
}
