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

/* RecordType.cs -- тип BibTex-записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using direct

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.BibTex
{
    //
    // Каждая запись должна быть определённого типа, описывающего
    // тип публикации. Следующие типы являются стандартными
    // и обрабатываются почти всеми стилями BibTeX (названия расположены
    // по алфавиту):
    //

    /// <summary>
    /// Тип BibTex-записи.
    /// </summary>
    public static class RecordType
    {
        #region Constants

        /// <summary>
        /// Статья из журнала.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: author, title, journal, year.
        /// Дополнительные поля: volume, number, pages, month, note, key.
        /// </remarks>
        public const string Article = "article";

        /// <summary>
        /// Определенное издание книги.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: author/editor, title, publisher, year.
        /// Дополнительные поля: volume, series, address, edition, month,
        /// note, key, pages.
        /// </remarks>
        public const string Book = "book";

        /// <summary>
        /// Печатная работа, которая не содержит имя издателя
        /// или организатора (например, самиздат).
        /// </summary>
        /// <remarks>
        /// Необходимые поля: title.
        /// Дополнительные поля: author, howpublished, address, month,
        /// year, note, key.
        /// </remarks>
        public const string Booklet = "booklet";

        /// <summary>
        /// Труды конференции.
        /// </summary>
        /// <remarks>
        /// Синоним inproceedings, оставлено для совместимости с Scribe.
        /// Необходимые поля: author, title, booktitle, year.
        /// Дополнительные поля: editor, pages, organization, publisher,
        /// address, month, note, key.
        /// </remarks>
        public const string Conference = "conference";

        /// <summary>
        /// Часть книги, возможно без названия. Может быть главой
        /// (частью, параграфом), либо диапазоном страниц.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: author/editor, title, chapter/pages,
        /// publisher, year.
        /// Дополнительные поля: volume, series, address, edition,
        /// month, note, key.
        /// </remarks>
        public const string InBook = "inbook";

        /// <summary>
        /// Часть книги, имеющая собственное название (например,
        /// статья в сборнике).
        /// </summary>
        /// <remarks>
        /// Необходимые поля: author, title, booktitle, year.
        /// Дополнительные поля: editor, pages, organization,
        /// publisher, address, month, note, key.
        /// </remarks>
        public const string InCollection = "incollection";

        /// <summary>
        /// Тезис (труд) конференции.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: author, title, booktitle, year.
        /// Дополнительные поля: editor, series, pages, organization,
        /// publisher, address, month, note, key.
        /// </remarks>
        public const string InProceedings = "inproceedings";

        /// <summary>
        /// Техническая документация.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: title.
        /// Дополнительные поля: author, organization, address, edition,
        /// month, year, note, key.
        /// </remarks>
        public const string Manual = "manual";

        /// <summary>
        /// Магистерская диссертация.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: author, title, school, year.
        /// Дополнительные поля: address, month, note, key.
        /// </remarks>
        public const string MasterThesis = "masterthesis";

        /// <summary>
        /// Использовать, если другие типы не подходят.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: нет.
        /// Дополнительные поля: author, title, howpublished, month,
        /// year, note, key.
        /// </remarks>
        public const string Misc = "misc";

        /// <summary>
        /// Кандидатская диссертация.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: author, title, school, year.
        /// Дополнительные поля: address, month, note, key.
        /// </remarks>
        public const string PhdThesis = "phdthesis";

        /// <summary>
        /// Сборник трудов (тезисов) конференции.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: title, year.
        /// Дополнительные поля: editor, publisher, organization,
        /// address, month, note, key.
        /// </remarks>
        public const string Proceedings = "proceedings";

        /// <summary>
        /// Отчёт, опубликованный организацией, обычно пронумерованный
        /// внутри серии.
        /// </summary>
        /// <remarks>
        /// Необходимые поля: author, title, institution, year.
        /// Дополнительные поля: type, number, address, month, note, key.
        /// </remarks>
        public const string TechReport = "techreport";

        /// <summary>
        /// Документ, имеющий автора и название, но формально
        /// не опубликованный (рукопись).
        /// </summary>
        /// <remarks>
        /// Необходимые поля: author, title, note.
        /// Дополнительные поля: month, year, key.
        /// </remarks>
        public const string Unpublished = "unpublished";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значение констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (RecordType));
        }

        #endregion
    }
}

