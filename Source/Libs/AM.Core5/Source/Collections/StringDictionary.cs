// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* StringDictionary.cs -- простой словарь "строка-строка"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Simple "string-string" <see cref="Dictionary{T1,T2}"/>
    /// with saving-restoring facility.
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class StringDictionary
        : Dictionary<string, string>,
        IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// End-of-Dictionary mark.
        /// </summary>
        public const string EndOfDictionary = "*****";

        #endregion

        #region Public methods

        /// <summary>
        /// Loads <see cref="StringDictionary"/> from
        /// the specified <see cref="StreamReader"/>.
        /// </summary>
        public static StringDictionary Load
            (
                TextReader reader
            )
        {
            Sure.NotNull(reader, nameof(reader));

            var result = new StringDictionary();
            while (true)
            {
                var key = reader.ReadLine();
                if (key is null
                    || key.StartsWith(EndOfDictionary))
                {
                    break;
                }

                var value = reader.ReadLine() ?? string.Empty;

                result.Add(key, value);
            }

            return result;
        }

        /// <summary>
        /// Loads <see cref="StringDictionary"/> from the specified file.
        /// </summary>
        public static StringDictionary Load
            (
                string fileName,
                Encoding encoding
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));
            Sure.NotNull(encoding, nameof(encoding));

            using (TextReader reader = TextReaderUtility.OpenRead
                (
                    fileName,
                    encoding
                ))
            {
                return Load(reader);
            }
        }

        /// <summary>
        /// Saves the <see cref="StringDictionary"/> with specified writer.
        /// </summary>
        public void Save
            (
                TextWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            foreach (var pair in this)
            {
                writer.WriteLine(pair.Key);
                writer.WriteLine(pair.Value);
            }

            writer.WriteLine(EndOfDictionary);
        }

        /// <summary>
        /// Saves the <see cref="StringDictionary"/> to specified file.
        /// </summary>
        public void Save
            (
                string fileName,
                Encoding encoding
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));
            Sure.NotNull(encoding, nameof(encoding));

            using (TextWriter writer = TextWriterUtility.Create
                (
                    fileName,
                    encoding
                ))
            {
                Save(writer);
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull(reader, nameof(reader));

            Clear();

            var count = reader.ReadPackedInt32();
            for (var i = 0; i < count; i++)
            {
                var key = reader.ReadString();
                var value = reader.ReadNullableString() ?? string.Empty;
                Add(key, value);
            }
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer.WritePackedInt32(Count);
            foreach (var pair in this)
            {
                writer.Write(pair.Key);
                writer.WriteNullable(pair.Value);
            }
        }

        #endregion
    }
}
