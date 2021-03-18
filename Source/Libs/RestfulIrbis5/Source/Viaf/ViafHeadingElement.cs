// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ViafHeadingElement.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Json;



using Newtonsoft.Json.Linq;

#endregion

// ReSharper disable StringLiteralTypo

namespace RestfulIrbis.Viaf
{
    /// <summary>
    ///
    /// </summary>

    public class ViafHeadingElement
    {
        #region Properties

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the object.
        /// </summary>
        public static ViafHeadingElement Parse
            (
                JObject obj
            )
        {
            return new ViafHeadingElement
            {
            };
        }

        /// <summary>
        /// Parse the array.
        /// </summary>
        public static ViafHeadingElement[] Parse
            (
                [CanBeNull] JArray array
            )
        {
            if (ReferenceEquals(array, null))
            {
                return new ViafHeadingElement[0];
            }

            ViafHeadingElement[] result = new ViafHeadingElement[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                result[i] = Parse((JObject)array[i]);
            }

            return result;
        }

        #endregion
    }
}
