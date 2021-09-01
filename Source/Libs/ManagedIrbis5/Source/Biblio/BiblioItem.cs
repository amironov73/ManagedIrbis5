// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BiblioItem.cs -- элемент библиографического указателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using AM;
using AM.Collections;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Элемент библиографического указателя, например,
    /// библиографическое описание книги или
    /// строчка в алфавитном указателе.
    /// </summary>
    public sealed class BiblioItem
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Глава, которой принадлежит элемент.
        /// </summary>
        public BiblioChapter? Chapter { get; set; }

        /// <summary>
        /// Порядковый номер в главе.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Библиографическая запись (опционально).
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Библиографическое описание (опционально).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Используется для упорядочивания элементов в главе
        /// (опционально).
        /// </summary>
        public string? Order { get; set; }

        /// <summary>
        /// Соответствующие термины поискового словаря.
        /// </summary>
        public NonNullCollection<BiblioTerm> Terms { get; private set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public BiblioItem()
        {
            Terms = new NonNullCollection<BiblioTerm>();

        } // constructor

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BiblioItem> verifier
                = new Verifier<BiblioItem>(this, throwOnError);

            // TODO do something

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new ValueStringBuilder();
            result.Append(Order);
            result.AppendLine();
            result.Append(Description);

            return result.ToString();

        } // method ToString

        #endregion

    } // class BiblioItem

} // namespace ManagedIrbis.Biblio
