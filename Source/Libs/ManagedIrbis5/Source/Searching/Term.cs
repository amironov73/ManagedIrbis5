// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Term.cs -- термин в поисковом словаре
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Термин в поисковом словаре.
    /// </summary>
    [XmlRoot("term")]
    [DebuggerDisplay("{Count} {Text}")]
    public class Term
        : IEquatable<Term>,
        IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Количество ссылок.
        /// </summary>
        [XmlAttribute("count")]
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Поисковый термин.
        /// </summary>
        [XmlAttribute("text")]
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the <see cref="Term"/>.
        /// </summary>
        public Term Clone()
        {
            return (Term) MemberwiseClone();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static Term[] Parse
            (
                Response response
            )
        {
            var result = new List<Term>();
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split('#', 2);
                var item = new Term
                {
                    Count = int.Parse(parts[0]),
                    Text = parts.Length == 2 ? parts[1] : string.Empty
                };
                result.Add(item);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Удаляет префиксы с терминов.
        /// </summary>
        public static Term[] TrimPrefix
            (
                ICollection<Term> terms,
                string prefix
            )
        {
            var prefixLength = prefix.Length;
            var result = new List<Term>(terms.Count);
            if (prefixLength == 0)
            {
                foreach (var term in terms)
                {
                    result.Add(term.Clone());
                }
            }
            else
            {
                foreach (var term in terms)
                {
                    var item = term.Text;
                    if (!string.IsNullOrEmpty(item) && item.StartsWith(prefix))
                    {
                        item = item.Substring(prefixLength);
                    }
                    var clone = term.Clone();
                    clone.Text = item;
                    result.Add(clone);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public virtual void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Count = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public virtual void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Count)
                .WriteNullable(Text);
        }

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals(Term? other)
            => Text?.Equals(other?.Text) ?? false;

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Term>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Text, "text")
                .Assert(Count >= 0, "Count");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString"/>
        public override string ToString()
        {
            return $"{Count}#{Text.ToVisibleString()}";
        }

        #endregion
    }
}
