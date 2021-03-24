// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ViafMainHeading.cs --
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

    public class ViafMainHeading
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string[]? Sources { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string[]? Sid { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the object.
        /// </summary>
        public static ViafMainHeading Parse
            (
                // TODO: implement
                object obj
                // JObject obj
            )
        {
            /*

            return new ViafMainHeading
            {
                Text = obj["text"].NullableToString(),
                Sources = obj["sources"]["s"].GetValues<string>(),
                Sid = obj["sources"]["sid"].GetValues<string>()
            };

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Parse the array.
        /// </summary>
        public static ViafMainHeading[] Parse
            (
                // TODO: implement
                object[] array
                // JArray? array
            )
        {
            /*

            if (ReferenceEquals(array, null))
            {
                return new ViafMainHeading[0];
            }

            ViafMainHeading[] result = new ViafMainHeading[array.Count];
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
