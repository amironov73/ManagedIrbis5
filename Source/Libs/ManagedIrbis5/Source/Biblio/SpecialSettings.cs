// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SpecialSettings.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>

    // [JsonConverter(typeof(SpecialSettingsConverter))]
    public sealed class SpecialSettings
    {
        #region Nested classes

        /*
        class SpecialSettingsConverter
            : JsonConverter
        {
            #region JsonConverter members

            /// <inheritdoc cref="JsonConverter.WriteJson" />
            public override void WriteJson
                (
                    JsonWriter writer,
                    object value,
                    JsonSerializer serializer
                )
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc cref="JsonConverter.ReadJson" />
            public override object ReadJson
                (
                    JsonReader reader,
                    Type objectType,
                    object existingValue,
                    JsonSerializer serializer
                )
            {
                SpecialSettings result = new SpecialSettings();
                JToken token = JToken.Load(reader);
                foreach (JProperty child in token.Children<JProperty>())
                {
                    string value = child.Value.ToString();
                    if (child.Name == "name")
                    {
                        result.Name = value;
                    }
                    else
                    {
                        result.Dictionary.Add
                            (
                                child.Name,
                                value
                            );
                    }
                }

                return result;
            }

            /// <inheritdoc cref="JsonConverter.CanConvert" />
            public override bool CanConvert
                (
                    Type objectType
                )
            {
                return true;
            }

            #endregion
        }

        */

        #endregion

        #region Properties

        /// <summary>
        /// Dictionary.
        /// </summary>
        public StringDictionary Dictionary { get; private set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string? Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SpecialSettings()
        {
            Dictionary = new StringDictionary();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get setting with specified name.
        /// </summary>
        public string? GetSetting
            (
                string name
            )
        {
            Dictionary.TryGetValue(name, out var result);

            return result;
        }

        #endregion

    }
}
