// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Term.cs -- термин в поисковом словаре
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Термин в поисковом словаре.
    /// </summary>
    [DebuggerDisplay ("{Count} {Text}")]
    public class Term
        : IEquatable<Term>
    {
        #region Properties

        /// <summary>
        /// Количество ссылок.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Поисковый термин.
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование термина <see cref="Term"/>.
        /// </summary>
        public Term Clone() => (Term)MemberwiseClone();

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
                if (string.IsNullOrEmpty (line))
                {
                    break;
                }

                var parts = line.Split (Constants.NumberSign, 2);
                var item = new Term
                {
                    Count = int.Parse (parts[0]),
                    Text = parts.Length == 2 ? parts[1] : string.Empty
                };
                result.Add (item);
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
            var result = new List<Term> (terms.Count);
            if (prefixLength == 0)
            {
                foreach (var term in terms)
                {
                    result.Add (term.Clone());
                }
            }
            else
            {
                foreach (var term in terms)
                {
                    var item = term.Text;
                    if (!ReferenceEquals (item, null) && item.StartsWith (prefix))
                    {
                        item = item.Substring (prefixLength);
                    }

                    var clone = term.Clone();
                    clone.Text = item;
                    result.Add (clone);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals (Term? other)
            => Text?.Equals (other?.Text) ?? false;

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString"/>
        public override string ToString() => $"{Count}#{Text.ToVisibleString()}";

        #endregion
    }
}
