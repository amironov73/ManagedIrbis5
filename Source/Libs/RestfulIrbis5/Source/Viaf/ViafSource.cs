// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ViafSource.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace RestfulIrbis.Viaf
{
    /// <summary>
    ///
    /// </summary>

    public class ViafSource
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string? Nsid { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public method

        /// <summary>
        /// Parse the object.
        /// </summary>
        public static ViafSource Parse
            (
                JObject obj
            )
        {
            return new ViafSource
            {
                Nsid = obj["@nsid"].NullableToString(),
                Text = obj["#text"].NullableToString()
            };
        }

        /// <summary>
        /// Parse the array.
        /// </summary>
        public static ViafSource[] Parse
            (
                JArray? array
            )
        {
            if (ReferenceEquals(array, null))
            {
                return new ViafSource[0];
            }

            ViafSource[] result = new ViafSource[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                result[i] = Parse((JObject)array[i]);
            }

            return result;
        }

        #endregion
    }
}
