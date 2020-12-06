// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Parameter.cs -- параметр вида ИМЯ=ЗНАЧЕНИЕ.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Parameters
{
    /// <summary>
    /// Параметр вида ИМЯ=ЗНАЧЕНИЕ.
    /// </summary>
    [XmlRoot("parameter")]
    [DebuggerDisplay("{Name}={Value}")]
    public sealed class Parameter
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [XmlAttribute("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        /// <remarks>Can be <c>string.Empty</c>.</remarks>
        [XmlAttribute("value")]
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Values.
        /// </summary>
        public NonNullCollection<string> Values { get; private set; }

        #endregion

        #region Construciton

        /// <summary>
        /// Constructor.
        /// </summary>
        public Parameter()
        {
            Values = new NonNullCollection<string>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Parameter
            (
                string name,
                string? value
            )
        {
            Sure.NotNullNorEmpty(name, nameof(name));

            Name = name;
            Value = value ?? string.Empty;
            Values = new NonNullCollection<string>
            {
                Value
            };
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Name = reader.ReadNullableString();
            Value = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Name)
                .WriteNullable(Value);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Parameter>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Name, "Name")
                .NotNull(Value, "Value");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        [Pure]
        public override string ToString()
        {
            return $"{Name}={Value}";
        }

        #endregion
    }
}
