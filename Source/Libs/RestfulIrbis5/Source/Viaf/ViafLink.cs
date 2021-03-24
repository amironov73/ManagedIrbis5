// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ViafLink.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using AM;
using AM.Json;

#endregion

#nullable enable

namespace RestfulIrbis.Viaf
{
    /// <summary>
    ///
    /// </summary>
    public class ViafLink
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? S { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Sid { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the object.
        /// </summary>
        public static ViafLink Parse
            (
                // TODO: implement
                object obj
                // JObject obj
            )
        {
            /*

            return new ViafLink
            {
                Url = obj["#text"].NullableToString(),
                S = obj["sources"]["s"].NullableToString(),
                Sid = obj["sources"]["sid"].NullableToString()
            };

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Parse the array.
        /// </summary>
        public static ViafLink[] Parse
            (
                // TODO: implement
                object[] array
                // JArray? array
            )
        {
            /*

            if (ReferenceEquals(array, null))
            {
                return new ViafLink[0];
            }

            ViafLink[] result = new ViafLink[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                result[i] = Parse((JObject)array[i]);
            }

            return result;

            */

            throw new NotImplementedException();
        }

        #endregion
    }
}
