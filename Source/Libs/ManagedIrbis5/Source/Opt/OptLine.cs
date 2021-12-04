// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* OptLine.cs -- строка OPT-файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Opt
{
    /// <summary>
    /// Строка OPT-файла.
    /// </summary>
    [XmlRoot ("line")]
    public sealed class OptLine
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Ключ.
        /// </summary>
        [XmlAttribute ("key")]
        [JsonPropertyName ("key")]
        [Description ("Ключ")]
        public string? Key { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        [XmlAttribute ("value")]
        [JsonPropertyName ("value")]
        [Description ("Значение")]
        public string? Value { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Сравнение строки с ключом.
        /// </summary>
        public bool Compare (string? text)
        {
            return OptUtility.CompareString (Key.ThrowIfNull (), text);
        }

        /// <summary>
        /// Разбор строки.
        /// </summary>
        public static OptLine? Parse
            (
                string? line
            )
        {
            if (string.IsNullOrEmpty (line))
            {
                return null;
            }

            line = line.Trim();
            if (string.IsNullOrEmpty (line))
            {
                return null;
            }

            var parts = line.Split
                (
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries
                );

            if (parts.Length != 2)
            {
                return null;
            }

            var result = new OptLine
            {
                Key = parts[0],
                Value = parts[1]
            };

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Key = reader.ReadNullableString();
            Value = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer.WriteNullable (Key);
            writer.WriteNullable (Value);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<OptLine> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Key)
                .NotNullNorEmpty (Value);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"{Key.ToVisibleString()} {Value.ToVisibleString()}";
        }

        #endregion
    }
}
