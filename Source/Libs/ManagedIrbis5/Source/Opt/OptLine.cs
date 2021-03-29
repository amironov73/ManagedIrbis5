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
using System.Diagnostics;
using System.IO;

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
    [DebuggerDisplay("{" + nameof(Key) + "} {" + nameof(Value) + "}")]
    public sealed class OptLine
        : IHandmadeSerializable
    {
            #region Properties

            /// <summary>
            /// Ключ.
            /// </summary>
            public string? Key { get; set; }

            /// <summary>
            /// Значение.
            /// </summary>
            public string? Value { get; set; }

            #endregion

            #region Public methods

            /// <summary>
            /// Сравнение строки с ключом.
            /// </summary>
            public bool Compare ( string? text ) =>
                OptUtility.CompareString(Key.ThrowIfNull("Key"), text);

            /// <summary>
            /// Разбор строки.
            /// </summary>
            public static OptLine? Parse
                (
                    string line
                )
            {
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }

                line = line.Trim();
                if (string.IsNullOrEmpty(line))
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
            } // method Parse

            #endregion

            #region IHandmadeSerializable

            /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                Key = reader.ReadNullableString();
                Value = reader.ReadNullableString();
            } // method RestoreFromStream

            /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                writer.WriteNullable(Key);
                writer.WriteNullable(Value);
            } // method SaveToStream

            #endregion

            #region Object members

            /// <inheritdoc cref="object.ToString" />
            public override string ToString() => $"{Key} {Value}";

            #endregion

    } // class OptLine

} // namespace ManagedIrbis.Opt
