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

/* KnownTags.cs -- известные системе метки полей
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.BibTex
{
    //
    // Каждая запись содержит некоторый список стандартных полей
    // (можно вводить любые другие поля, которые просто игнорируются
    // стандартными программами).
    //

    /// <summary>
    /// Известные системе метки полей.
    /// </summary>
    public static class KnownTags
    {
        #region Constants

        /// <summary>
        /// Адрес издателя (обычно просто город, но может быть полным
        /// адресом для малоизвестных издателей).
        /// </summary>
        public const string Address = "address";

        /// <summary>
        /// Аннотация для библиографической записи.
        /// </summary>
        public const string Annote = "annote";

        /// <summary>
        /// В JabRef: аннотация для библиографической записи.
        /// </summary>
        public const string Abstract = "abstract";

        /// <summary>
        /// Наименование книги, содержащей данную работу.
        /// </summary>
        public const string BookTitle = "booktitle";

        /// <summary>
        /// Номер и/или заглавие главы.
        /// </summary>
        public const string Chapter = "chapter";

        /// <summary>
        /// Ключ кросс-ссылки (позволяет использовать другую библио-запись
        /// в качестве названия, например, сборника трудов).
        /// </summary>
        public const string CrossReference = "crossref";

        /// <summary>
        /// Издание (полная строка, например, «1-е, стереотипное»).
        /// </summary>
        public const string Edition = "edition";

        /// <summary>
        /// Имена редакторов (оформление аналогично авторам).
        /// </summary>
        public const string Editor = "editor";

        /// <summary>
        /// A specification of an electronic publication, often
        /// a preprint or a technical report.
        /// </summary>
        public const string Eprint = "eprint";

        /// <summary>
        /// Способ публикации, если нестандартный.
        /// </summary>
        public const string HowPublished = "howpublished";

        /// <summary>
        /// Институт, вовлечённый в публикацию, необязательно издатель.
        /// </summary>
        public const string Institution = "institution";

        /// <summary>
        /// Название журнала, содержащего статью.
        /// </summary>
        public const string Journal = "journal";

        /// <summary>
        /// Скрытое ключевое поле, задающее порядок сортировки
        /// (если "author" и "editor" не заданы).
        /// </summary>
        public const string Key = "key";

        /// <summary>
        /// Месяц публикации (может содержать дату).
        /// Если не опубликовано - создания.
        /// </summary>
        public const string Month = "month";

        /// <summary>
        /// Любые заметки в произвольном формате.
        /// </summary>
        public const string Note = "note";

        /// <summary>
        /// Номер журнала.
        /// </summary>
        public const string Number = "number";

        /// <summary>
        /// Организатор конференции.
        /// </summary>
        public const string Organization = "organization";

        /// <summary>
        /// Номера страниц, разделённые запятыми или двойным дефисом.
        /// Для книги - общее количество страниц.
        /// </summary>
        public const string Pages = "pages";

        /// <summary>
        /// Издатель.
        /// </summary>
        public const string Publisher = "publisher";

        /// <summary>
        /// Институт, в котором защищалась диссертация.
        /// </summary>
        public const string School = "school";

        /// <summary>
        /// Серия, в которой вышла книга.
        /// </summary>
        public const string Series = "series";

        /// <summary>
        /// Заглавие (название) работы.
        /// </summary>
        public const string Title = "title";

        /// <summary>
        /// Тип отчёта, например "Заметки исследователя".
        /// </summary>
        public const string Type = "type";

        /// <summary>
        /// WWW-адрес.
        /// </summary>
        public const string Url = "url";

        /// <summary>
        /// Том журнала или книги.
        /// </summary>
        public const string Volume = "volume";

        /// <summary>
        /// Год публикации (если не опубликовано - создания).
        /// </summary>
        public const string Year = "year";

        #endregion

        #region Properties

        /// <summary>
        /// Все вышеперечисленные теги.
        /// </summary>
        public static string[] All =
        {
            "address", "annote", "abstract", "author", "booktitle",
            "chapter", "crossref", "edition", "editor", "eprint",
            "howpublished", "institution", "journal", "key", "month",
            "note", "number", "organization", "pages", "publisher",
            "school", "series", "title", "type", "url", "volume", "year"
        };

        #endregion

    } // class KnownTags

} // namespace ManagedIrbis.BibTex
