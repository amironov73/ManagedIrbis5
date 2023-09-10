// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TermPosting.cs -- постинг термина
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
    /// Постинг термина.
    /// </summary>
    [DebuggerDisplay ("[{Mfn}] {Tag} {Occurrence} {Count} {Text}")]
    public sealed class TermPosting
        : IEquatable<TermPosting>
    {
        #region Properties

        /// <summary>
        /// MFN записи с искомым термом.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Тег поля с искомым термом.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Количество повторений.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Результат форматирования.
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование постинга <see cref="TermPosting"/>.
        /// </summary>
        public TermPosting Clone()
        {
            return (TermPosting) MemberwiseClone();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static TermPosting[] Parse
            (
                Response response
            )
        {
            var result = new List<TermPosting>();
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty (line))
                {
                    break;
                }

                var parts = line.Split (Constants.NumberSign, 5);
                if (parts.Length < 4)
                {
                    break;
                }

                var item = new TermPosting
                {
                    Mfn = int.Parse (parts[0]),
                    Tag = int.Parse (parts[1]),
                    Occurrence = int.Parse (parts[2]),
                    Count = int.Parse (parts[3]),
                    Text = parts.Length == 5 ? parts[4] : string.Empty
                };
                result.Add (item);
            }

            return result.ToArray();
        }

        #endregion

        #region IEquatable members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals (TermPosting? other)
        {
            if (other is not null)
            {
                return Mfn == other.Mfn
                       && Tag == other.Tag
                       && Occurrence == other.Occurrence
                       && Count == other.Count
                       && Text == other.Text;
            }

            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString() => $"{Mfn}#{Tag}#{Occurrence}#{Count}#{Text}";

        #endregion
    }
}
