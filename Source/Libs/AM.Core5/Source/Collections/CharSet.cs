// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CharacterSet.cs -- множество символов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM.Runtime;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Character set.
    /// </summary>
    [DebuggerDisplay("{" + nameof(ToString) + "()}")]
    public sealed class CharSet
        : IHandmadeSerializable,
        IEnumerable<char>,
        IEquatable<CharSet>
    {
        #region Constants

        /// <summary>
        /// Default capacity of <see cref="CharSet"/>.
        /// </summary>
        public const int DefaultCapacity = 0x10000;

        #endregion

        #region Nested classes

        private class CharSetEnumerator
            : IEnumerator<char>
        {
            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public CharSetEnumerator
                (
                    CharSet charSet
                )
            {
                _data = charSet.ToArray();
                _index = -1;
            }

            #endregion

            #region Private members

            private readonly char[] _data;

            private int _index;

            #endregion

            #region IEnumerator members

            /// <inheritdoc cref="IDisposable.Dispose" />
            public void Dispose()
            {
                // Nothing to do here
            }

            /// <inheritdoc cref="IEnumerator{T}.Current" />
            char IEnumerator<char>.Current => _data[_index];

            /// <inheritdoc cref="IEnumerator.Current" />
            public object Current => _data[_index];

            /// <inheritdoc cref="IEnumerator.MoveNext" />
            public bool MoveNext()
            {
                _index++;
                if (_index >= _data.Length)
                {
                    return false;
                }

                return true;
            }

            /// <inheritdoc cref="IEnumerator.Reset" />
            public void Reset()
            {
                _index = -1;
            }

            #endregion
        }

        /*
        /// <summary>
        /// Converter for JSON serialization.
        /// </summary>
        public class CharSetConverter
            : JsonConverter
        {
            #region Constructor

            /// <summary>
            /// Constructor.
            /// </summary>
            public CharSetConverter(params Type[] types)
            {
                _types = types;
            }

            #endregion

            #region Private members

            private readonly Type[] _types;

            #endregion

            #region JsonConverter members

            /// <inheritdoc cref="JsonConverter.WriteJson" />
            public override void WriteJson
                (
                    JsonWriter writer,
                    object value,
                    JsonSerializer serializer
                )
            {
                CharSet charSet = (CharSet)value;

                JObject o = new JObject();
                o.AddFirst(new JProperty
                    (
                        "charset",
                        charSet.ToString()
                    ));

                o.WriteTo(writer);
            }

            /// <inheritdoc cref="JsonConverter.ReadJson" />
            [ExcludeFromCodeCoverage]
            public override object ReadJson
                (
                    JsonReader reader,
                    Type objectType,
                    object existingValue,
                    JsonSerializer serializer
                )
            {
                Log.Error
                    (
                        "CharSet::ReadJson: "
                        + "not implemented"
                    );

                throw new NotImplementedException();
            }

            /// <inheritdoc cref="JsonConverter.CanConvert" />
            public override bool CanConvert
                (
                    Type objectType
                )
            {
                return _types.Contains(objectType);
            }

            #endregion
        }
        */

        #endregion

        #region Properties

        ///<summary>
        /// Indexer.
        ///</summary>
        public bool this[char index]
        {
            [DebuggerStepThrough]
            get
            {
                return _data[index];
            }
            [DebuggerStepThrough]
            set
            {
                _data[index] = value;
            }
        }

        /// <summary>
        /// Count of elements.
        /// </summary>
        public int Count
        {
            get
            {
                int result = 0;
                for (int i = 0; i < _data.Length; i++)
                {
                    if (_data[i])
                    {
                        result++;
                    }
                }
                return result;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet()
            : this(DefaultCapacity)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                int capacity
            )
        {
            Sure.Positive(capacity, nameof(capacity));

            _data = new BitArray(capacity);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                BitArray data
            )
        {
            _data = new BitArray(data);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public CharSet
            (
                CharSet other
            )
        {
            Sure.NotNull(other, nameof(other));

            _data = new BitArray(other._data);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                IEnumerable<char> characters
            )
            : this()
        {
            Sure.NotNull((object?)characters, nameof(characters));

            foreach (char c in characters)
            {
                Add(c);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                params char[] characters
            )
            : this()
        {
            Add(characters);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                string characters
            )
            : this()
        {
            Sure.NotNull(characters, nameof(characters));

            Add(characters);
        }

        #endregion

        #region Private members

        private readonly BitArray _data;

        private void _Add(char[] ch, bool val)
        {
            for (int i = 0; i < ch.Length; i++)
            {
                _data[ch[i]] = val;
            }
        }

        private void _Add(string s, bool val)
        {
            int len1 = s.Length - 1;
            int len2 = s.Length - 2;
            int i = 0;
            for (; i < len1; i++)
            {
                if (s[i] == '\\')
                {
                    _data[s[i + 1]] = val;
                    i++;
                }
                else if (s[i + 1] == '-')
                {
                    if (i >= len2)
                    {
                        Magna.Error
                            (
                                "CharSet::_Add: "
                                + "too long"
                            );

                        throw new ArgumentException();
                    }
                    for (int c = s[i]; c <= s[i + 2]; c++)
                    {
                        _data[c] = val;
                    }
                    i += 2;
                }
                else
                {
                    _data[s[i]] = val;
                }
            }
            for (; i < s.Length; i++)
            {
                if (s[i] == '\\')
                {
                    Magna.Error
                        (
                            "CharSet::_Add: "
                            + "unexpected '\\'"
                        );

                    throw new ArgumentException();
                }
                _data[s[i]] = val;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add a character.
        /// </summary>
        public CharSet Add
            (
                char character
            )
        {
            _data[character] = true;

            return this;
        }

        /// <summary>
        /// Add some characters.
        /// </summary>
        public CharSet Add
            (
                params char[] range
            )
        {
            _Add(range, true);

            return this;
        }

        /// <summary>
        /// Add some characters.
        /// </summary>
        public CharSet Add
            (
                string range
            )
        {
            Sure.NotNull(range, nameof(range));

            _Add(range, true);

            return this;
        }

        /// <summary>
        /// Add range of characters.
        /// </summary>
        public CharSet AddRange
            (
                char from,
                char to
            )
        {
            for (char c = from; c <= to; c++)
            {
                Add(c);
            }

            return this;
        }

        /// <summary>
        /// Logical multiplication.
        /// </summary>
        public CharSet And
            (
                CharSet other
            )
        {
            Sure.NotNull(other, nameof(other));

            CharSet result = Clone();
            result._data.And(other._data);

            return result;
        }

        /// <summary>
        /// Check the string for characters not included in the charset.
        /// </summary>
        public bool CheckText
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (!this[text[i]])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Clear the <see cref="CharSet"/>.
        /// </summary>
        public CharSet Clear()
        {
            return SetAll(false);
        }

        /// <summary>
        /// Create clone of the <see cref="CharSet"/>.
        /// </summary>
        public CharSet Clone()
        {
            return new CharSet(this);
        }

        /// <summary>
        /// Determines whether the <see cref="CharSet"/>
        /// contains given character.
        /// </summary>
        public bool Contains
            (
                char c
            )
        {
            return _data[c];
        }

        /// <summary>
        /// Invert.
        /// </summary>
        public CharSet Not()
        {
            CharSet result = Clone();
            result._data.Not();

            return result;
        }

        /// <summary>
        /// Logical addition.
        /// </summary>
        public CharSet Or
            (
                CharSet other
            )
        {
            Sure.NotNull(other, nameof(other));

            CharSet result = Clone();
            result._data.Or(other._data);

            return result;
        }

        /// <summary>
        /// Remove a character.
        /// </summary>
        public CharSet Remove
            (
                char character
            )
        {
            _data[character] = false;

            return this;
        }

        /// <summary>
        /// Remove some characters.
        /// </summary>
        public CharSet Remove
            (
                params char[] range
            )
        {
            _Add(range, false);

            return this;
        }

        /// <summary>
        /// Remove some characters.
        /// </summary>
        public CharSet Remove
            (
                string range
            )
        {
            Sure.NotNull(range, nameof(range));

            _Add(range, false);

            return this;
        }

        /// <summary>
        /// Remove range of characters.
        /// </summary>
        public CharSet RemoveRange
            (
                char from,
                char to
            )
        {
            for (char c = from; c <= to; c++)
            {
                Remove(c);
            }

            return this;
        }

        /// <summary>
        /// Set all characters to the <paramref name="state"/>.
        /// </summary>
        public CharSet SetAll
            (
                bool state
            )
        {
            _data.SetAll(state);

            return this;
        }

        /// <summary>
        /// Create array of characters.
        /// </summary>
        public char[] ToArray()
        {
            int length = _data.Length / 8 + 1;
            List<char> result = new List<char>(length);

            for (int i = 0; i < _data.Length; i++)
            {
                if (_data[i])
                {
                    result.Add((char)i);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Logical XOR.
        /// </summary>
        public CharSet Xor
            (
                CharSet other
            )
        {
            Sure.NotNull(other, nameof(other));

            CharSet result = new CharSet(_data.Xor(other._data));

            return result;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Logical addition.
        /// </summary>
        public static CharSet operator +
            (
                CharSet left,
                CharSet right
            )
        {
            Sure.NotNull(left, nameof(left));
            Sure.NotNull(right, nameof(right));

            return left.Or(right);
        }

        /// <summary>
        /// Logical addition.
        /// </summary>
        public static CharSet operator +
            (
                CharSet left,
                string right
            )
        {
            Sure.NotNull(left, "left");
            Sure.NotNull(right, "right");

            CharSet result = new CharSet(left);

            return result.Add(right);
        }

        /// <summary>
        /// Logical addition.
        /// </summary>
        public static CharSet operator +
            (
                CharSet left,
                char right
            )
        {
            Sure.NotNull(left, "left");

            CharSet result = new CharSet(left);

            return result.Add(right);
        }

        /// <summary>
        /// Logical subtraction.
        /// </summary>
        public static CharSet operator -
            (
                CharSet left,
                CharSet right
            )
        {
            Sure.NotNull(left, "left");
            Sure.NotNull(right, "right");

            return left.And(right.Not());
        }

        /// <summary>
        /// Logical subtraction.
        /// </summary>
        public static CharSet operator -
            (
                CharSet left,
                string right
            )
        {
            Sure.NotNull(left, "left");
            Sure.NotNull(right, "right");

            CharSet result = new CharSet(left);

            return result.Remove(right);
        }

        /// <summary>
        /// Logical subtraction.
        /// </summary>
        public static CharSet operator -
            (
                CharSet left,
                char right
            )
        {
            Sure.NotNull(left, "left");

            CharSet result = new CharSet(left);

            return result.Remove(right);
        }

        /// <summary>
        /// Logical multiplication.
        /// </summary>
        public static CharSet operator *
            (
                CharSet left,
                CharSet right
            )
        {
            Sure.NotNull(left, "left");
            Sure.NotNull(right, "right");

            return left.And(right);
        }

        /// <summary>
        /// Logical multiplication.
        /// </summary>
        public static CharSet operator *
            (
                CharSet left,
                string right
            )
        {
            Sure.NotNull(left, "left");
            Sure.NotNull(right, "right");

            return left.And(new CharSet(right));
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull(reader, "reader");

            Clear();
            string text = reader.ReadString();
            Add(text);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, "writer");

            string text = ToString();
            writer.Write(text);
        }

        #endregion

        #region IEnumerable<char> members

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<char> GetEnumerator()
        {
            return new CharSetEnumerator(this);
        }

        #endregion

        #region IEquatable<CharSet> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals
            (
                CharSet? other
            )
        {
            Sure.NotNull(other, nameof(other));

            other = other.ThrowIfNull(nameof(other));

            return BitArrayUtility.AreEqual
                (
                    _data,
                    other._data
                );
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return new string(ToArray());
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            int result = 0;

            unchecked
            {
                for (int i = 0; i < _data.Length; i++)
                {
                    if (_data[i])
                    {
                        result = result * 17 + i + 1;
                    }
                }
            }

            return result;
        }

        /// <inheritdoc cref="object.Equals(object?)" />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            CharSet? charset = obj as CharSet;
            if (ReferenceEquals(charset, null))
            {
                if (obj is string)
                {
                    charset = new CharSet((string)obj);

                    return BitArrayUtility.AreEqual(_data, charset._data);
                }
                return false;
            }
            if (ReferenceEquals(charset, this))
            {
                return true;
            }


            return BitArrayUtility.AreEqual(_data, charset._data);
        }

        #endregion
    }
}
