// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* TermLink.cs -- связь термина
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Связь термина
    /// </summary>
    [XmlRoot("link")]
    public sealed class TermLink
        : IEquatable<TermLink>,
        IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// MFN записи с искомым термом.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonPropertyName("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Тег поля с искомым термом.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonPropertyName("tag")]
        public int Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        [XmlAttribute("occurrence")]
        [JsonPropertyName("occurrence")]
        public int Occurrence { get; set; }

        /// <summary>
        /// Смещение от начала поля.
        /// </summary>
        [XmlAttribute("index")]
        [JsonPropertyName("index")]
        public int Index { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование связи.
        /// </summary>
        public TermLink Clone()
        {
            return (TermLink) MemberwiseClone();
        }

        /// <summary>
        /// Конвертируем <see cref="TermPosting"/>
        /// в <see cref="TermLink"/>.
        /// </summary>
        public static TermLink FromPosting
            (
                TermPosting posting
            )
        {
            var result = new TermLink
            {
                Mfn = posting.Mfn,
                Tag = posting.Tag,
                Occurrence = posting.Occurrence,
                Index = posting.Count
            };

            return result;
        }

        /// <summary>
        /// Конвертируем список <see cref="TermPosting"/>
        /// в массив <see cref="TermLink"/>.
        /// </summary>
        public static TermLink[] FromPostings
            (
                IList<TermPosting> postings
            )
        {
            var result = new TermLink[postings.Count];
            for (int i = 0; i < postings.Count; i++)
            {
                result[i] = FromPosting(postings[i]);
            }

            return result;
        }

        /// <summary>
        /// Чтение ссылки из файла.
        /// </summary>
        public static TermLink Read
            (
                Stream stream
            )
        {
            var result = new TermLink
            {
                Mfn = stream.ReadInt32Network(),
                Tag = stream.ReadInt32Network(),
                Occurrence = stream.ReadInt32Network(),
                Index = stream.ReadInt32Network()
            };

            return result;
        }

        /// <summary>
        /// Convert array of <see cref="TermLink"/> into array of MFN.
        /// </summary>
        public static int[] ToMfn
            (
                IList<TermLink> links
            )
        {
            var result = new int[links.Count];
            for (var i = 0; i < links.Count; i++)
            {
                result[i] = links[i].Mfn;
            }

            return result;
        }

        /// <summary>
        /// Convert array of MFN into array of <see cref="TermLink"/>s.
        /// </summary>
        public static TermLink[] FromMfn
            (
                IList<int> array
            )
        {
            var result = new TermLink[array.Count];
            for (var i = 0; i < array.Count; i++)
            {
                result[i] = new TermLink
                {
                    Mfn = array[i]
                };
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            Tag = reader.ReadPackedInt32();
            Occurrence = reader.ReadPackedInt32();
            Index = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WritePackedInt32(Tag)
                .WritePackedInt32(Occurrence)
                .WritePackedInt32(Index);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<TermLink>(this, throwOnError);

            verifier
                .Assert(Mfn > 0, "Mfn")
                .Assert(Tag > 0, "Tag")
                .Assert(Occurrence > 0, "Occurrence")
                .Assert(Index > 0, "Index");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"[{Mfn}] {Tag}/{Occurrence} {Index}";
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
        public bool Equals
            (
                TermLink? other
            )
        {
            if (other is not null)
            {
                return Mfn == other.Mfn
                       && Tag == other.Tag
                       && Occurrence == other.Occurrence
                       && Index == other.Index;
            }

            return false;
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals
            (
                object? obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var termLink = obj as TermLink;

            return !ReferenceEquals(termLink, null)
                   && Equals(termLink);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyMemberInGetHashCode

                var hashCode = Mfn;
                hashCode = (hashCode * 397) ^ Tag;
                hashCode = (hashCode * 397) ^ Occurrence;
                hashCode = (hashCode * 397) ^ Index;

                return hashCode;
                // ReSharper restore NonReadonlyMemberInGetHashCode
            }
        }

        #endregion
    }
}
