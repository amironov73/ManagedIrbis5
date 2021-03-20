// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MxAlias.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Mx
{
    /// <summary>
    ///
    /// </summary>
    [XmlRoot("alias")]
    public sealed class MxAlias
        : IHandmadeSerializable,
        IVerifiable,
        IEquatable<MxAlias>
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
        [XmlAttribute("value")]
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MxAlias()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MxAlias
            (
                string? name,
                string? value
            )
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the line.
        /// </summary>
        public static MxAlias Parse
            (
                string line
            )
        {
            var navigator = new TextNavigator(line);
            string name = navigator.ReadUntil('=').ToString();
            if (string.IsNullOrEmpty(name))
            {
                throw new IrbisException();
            }
            name = name.Trim();
            if (string.IsNullOrEmpty(name))
            {
                throw new IrbisException();
            }
            if (navigator.ReadChar() != '=')
            {
                throw new IrbisException();
            }
            string value = navigator.GetRemainingText().ToString();
            if (string.IsNullOrEmpty(value))
            {
                throw new IrbisException();
            }
            value = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                throw new IrbisException();
            }

            MxAlias result = new MxAlias(name, value);

            return result;
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
            Verifier<MxAlias> verifier = new Verifier<MxAlias>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, "Name")
                .NotNullNorEmpty(Value, "Value");

            return verifier.Result;
        }

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals
            (
                MxAlias? other
            )
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return Name == other.Name
                && Value == other.Value;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object? obj
            )
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            if (!(obj is MxAlias alias))
            {
                return false;
            }

            return Equals(alias);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return ReferenceEquals(Name, null)
                ? 0
                : Name.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            Name.ToVisibleString() + "=" + Value.ToVisibleString();

        #endregion
    }
}
