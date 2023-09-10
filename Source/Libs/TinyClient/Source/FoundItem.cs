// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* FoundItem.cs -- одна строка в ответе сервера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Одна строка в ответе сервера на поисковый запрос.
    /// </summary>
    [DebuggerDisplay ("{Mfn} {Text}")]
    public sealed class FoundItem
        : IEquatable<FoundItem>
    {
        #region Properties

        /// <summary>
        /// Текстовая часть (может отсутствовать,
        /// если не запрашивалось форматирование).
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// MFN найденной записи.
        /// </summary>
        public int Mfn { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Выбирает только MFN из найденных записей.
        /// </summary>
        public static int[] ToMfn
            (
                FoundItem[]? found
            )
        {
            if (found is null || found.Length == 0)
            {
                return Array.Empty<int>();
            }

            var result = new int[found.Length];
            for (var i = 0; i < found.Length; i++)
            {
                result[i] = found[i].Mfn;
            }

            return result;
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static FoundItem[] Parse
            (
                Response response
            )
        {
            var expected = response.ReadInteger();
            var result = new List<FoundItem> (expected);
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty (line))
                {
                    break;
                }

                var parts = line.Split (Constants.NumberSign, 2);
                var item = new FoundItem
                {
                    Mfn = Private.ParseInt32 (parts[0]),
                    Text = parts.Length == 2 ? parts[1] : string.Empty
                };
                result.Add (item);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static int[] ParseMfn
            (
                Response response
            )
        {
            var expected = response.ReadInteger();
            var result = new List<int> (expected);
            while (!response.EOT)
            {
                var line = response.ReadAnsi();
                if (string.IsNullOrEmpty (line))
                {
                    break;
                }

                var parts = line.Split (Constants.NumberSign, 2);
                var mfn = int.Parse (parts[0]);
                result.Add (mfn);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Загружаем найденные записи с сервера.
        /// </summary>
        public static FoundItem[] Read
            (
                SyncConnection connection,
                string format,
                IEnumerable<int> found
            )
        {
            var array = found.ToArray();
            var length = array.Length;
            var result = new FoundItem[length];
            var formatted = connection.FormatRecords (array, format);
            if (formatted is null)
            {
                return Array.Empty<FoundItem>();
            }

            for (var i = 0; i < length; i++)
            {
                var item = new FoundItem
                {
                    Mfn = array[i],
                    Text = formatted[i]
                };
                result[i] = item;
            }

            return result;
        }

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals (FoundItem? other)
        {
            if (ReferenceEquals (null, other)) return false;
            if (ReferenceEquals (this, other)) return true;
            return Text == other.Text && Mfn == other.Mfn;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals (object? obj) =>
            ReferenceEquals (this, obj) || obj is FoundItem other && Equals (other);

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode() =>
            unchecked (((Text != null ? Text.GetHashCode() : 0) * 397) ^ Mfn);

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"[{Mfn}] {Text.ToVisibleString()}";

        #endregion
    }
}
