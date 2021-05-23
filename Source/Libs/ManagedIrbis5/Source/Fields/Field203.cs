// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Field203.cs -- поддержка поля 203
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// В связи с ГОСТ 7.0.100-2018 введено 203 поле.
    /// Это поле содержит подполя: вид содержания,
    /// средства доступа, характеристика содержания.
    /// </summary>
    public sealed class Field203
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 203;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "12345678abcdefgikloptuvwxyz";

        #endregion

        #region Properties

        /// <summary>
        /// Вид содержания. Подполя A, B, D, E, F, G, I, K, L.
        /// </summary>
        public ReadOnlyMemory<char>[]? ContentType { get; set; }

        /// <summary>
        /// Средства доступа. Подполя C, 1, 2, 3, 4, 5, 6, 7, 8
        /// </summary>
        public ReadOnlyMemory<char>[]? Access { get; set; }

        /// <summary>
        /// Характеристика содержания.
        /// Подполя O, P, U, Y, T, R, W, Q, X, V, Z
        /// </summary>
        public ReadOnlyMemory<char>[]? ContentDescription { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static Field203 Parse
            (
                Field field
            )
        {
            var result = new Field203
            {
                ContentType = Sequence.FromItems
                    (
                        field.GetFirstSubFieldValue('a'),
                        field.GetFirstSubFieldValue('b'),
                        field.GetFirstSubFieldValue('d'),
                        field.GetFirstSubFieldValue('e'),
                        field.GetFirstSubFieldValue('f'),
                        field.GetFirstSubFieldValue('g'),
                        field.GetFirstSubFieldValue('i'),
                        field.GetFirstSubFieldValue('k'),
                        field.GetFirstSubFieldValue('l')
                    )
                    .NonEmptyLines()
                    .ToArray(),

                Access = Sequence.FromItems
                    (
                        field.GetFirstSubFieldValue('c'),
                        field.GetFirstSubFieldValue('1'),
                        field.GetFirstSubFieldValue('2'),
                        field.GetFirstSubFieldValue('3'),
                        field.GetFirstSubFieldValue('4'),
                        field.GetFirstSubFieldValue('5'),
                        field.GetFirstSubFieldValue('6'),
                        field.GetFirstSubFieldValue('7'),
                        field.GetFirstSubFieldValue('8')
                    )
                    .NonEmptyLines()
                    .ToArray(),

                ContentDescription = Sequence.FromItems
                    (
                        field.GetFirstSubFieldValue('o'),
                        field.GetFirstSubFieldValue('p'),
                        field.GetFirstSubFieldValue('u'),
                        field.GetFirstSubFieldValue('y'),
                        field.GetFirstSubFieldValue('t'),
                        field.GetFirstSubFieldValue('r'),
                        field.GetFirstSubFieldValue('w'),
                        field.GetFirstSubFieldValue('q'),
                        field.GetFirstSubFieldValue('x'),
                        field.GetFirstSubFieldValue('v'),
                        field.GetFirstSubFieldValue('z')
                    )
                    .NonEmptyLines()
                    .ToArray()
            };

            return result;

        } // method Parse

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            // TODO: check whether this code actually works
            ContentType = reader.ReadNullableReadOnlyMemoryArray();
            Access = reader.ReadNullableReadOnlyMemoryArray();
            ContentDescription = reader.ReadNullableReadOnlyMemoryArray();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(ContentType)
                .WriteNullable(Access)
                .WriteNullable(ContentDescription);

        } // method SaveToStream

        #endregion

    } // class Field203

} // namespace ManagedIrbis.Fields
