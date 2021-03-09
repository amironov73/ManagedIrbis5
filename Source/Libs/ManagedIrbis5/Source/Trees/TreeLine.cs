// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* TreeLine.cs -- одна строка из TRE-файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Trees
{
    /// <summary>
    /// Одна строка из TRE-файла.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public sealed class TreeLine
        : IHandmadeSerializable,
        IVerifiable
    {
            #region Properties

            /// <summary>
            /// Children.
            /// </summary>
            [JsonPropertyName("children")]
            public NonNullCollection<TreeLine> Children { get; } = new();

            /// <summary>
            /// Delimiter.
            /// </summary>
            public static string? Delimiter
            {
                get => _delimiter;
                set => SetDelimiter(value);
            }

            /// <summary>
            /// Prefix.
            /// </summary>
            [JsonIgnore]
            public string? Prefix => _prefix;

            /// <summary>
            /// Suffix.
            /// </summary>
            [JsonIgnore]
            public string? Suffix => _suffix;

            /// <summary>
            /// Value.
            /// </summary>
            [JsonPropertyName("value")]
            public string? Value
            {
                get => _value;
                set => SetValue(value);
            }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public TreeLine()
            {
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public TreeLine
                (
                    string? value
                )
            {
                SetValue(value);
            }

            #endregion

            #region Private members

            private static string? _delimiter = " - ";

            private string? _prefix, _suffix, _value;

            internal int Level;

            #endregion

            #region Public methods

            /// <summary>
            /// Add child.
            /// </summary>
            public TreeLine AddChild
                (
                    string? value
                )
            {
                var result = new TreeLine(value);
                Children.Add(result);

                return result;
            }

            /// <summary>
            /// Set the delimiter.
            /// </summary>
            public static void SetDelimiter
                (
                    string? value
                )
            {
                _delimiter = value;
            } // method SetDelimiter

            /// <summary>
            /// Set the value.
            /// </summary>
            public void SetValue
                (
                    string? value
                )
            {
                _value = value;
                _prefix = null;
                _suffix = null;

                if (!string.IsNullOrEmpty(Delimiter)
                    && !ReferenceEquals(value, null)
                    && value.Length != 0)
                {
                    var parts = value.Split
                        (
                            new [] {Delimiter},
                            2,
                            StringSplitOptions.None
                        );

                    _prefix = parts[0];
                    if (parts.Length != 1)
                    {
                        _suffix = parts[1];
                    }
                }
            } // method SetValue

            #endregion

            #region IHandmadeSerializable members

            /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                Value = reader.ReadNullableString();
                reader.ReadCollection(Children);
            } // method RestoreFromStream

            /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                writer.WriteNullable(Value);
                writer.Write(Children);
            } // method SaveToStream

            #endregion

            #region IVerifiable members

            /// <inheritdoc cref="IVerifiable.Verify" />
            public bool Verify
                (
                    bool throwException
                )
            {
                var result = !string.IsNullOrEmpty(Value);

                if (result && Children.Count != 0)
                {
                    result = Children.All
                        (
                            child => child.Verify(throwException)
                        );
                }

                if (!result)
                {
                    Magna.Error
                        (
                            nameof(TreeFile) + "::" + nameof(Verify)
                            + ": verification error"
                        );

                    if (throwException)
                    {
                        throw new VerificationException();
                    }
                }

                return result;
            } // method Verify

            #endregion

    } // class TreeLine

} // namespace ManagedIrbis.Trees
