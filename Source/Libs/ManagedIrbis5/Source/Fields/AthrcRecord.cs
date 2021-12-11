// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AthrcRecord.cs -- запись в базе данных ATHRC
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Запись в базе данных ATHRC.
    /// </summary>
    public sealed class AthrcRecord
    {
        #region Properties

        /// <summary>
        /// Основное (унифицированное) наименование коллектива/мероприятия.
        /// Поле 210.
        /// </summary>
        [Field (210)]
        public object? MainTitle { get; set; }

        /// <summary>
        /// Ссылки типа СМ.
        /// Поле 410.
        /// </summary>
        [Field (410)]
        public object[]? See { get; set; }

        /// <summary>
        /// Ссылки типа СМ. ТАКЖЕ.
        /// Поле 510.
        /// </summary>
        [Field (510)]
        public object[]? SeeAlso { get; set; }

        /// <summary>
        /// Связанные основные наименования
        /// коллектива/мероприятия на других языках.
        /// Поле 710.
        /// </summary>
        [Field (710)]
        public object[]? LinkedTitles { get; set; }

        /// <summary>
        /// Вид коллектива/мероприятия.
        /// Поле 900.
        /// </summary>
        [Field (900)]
        public object? CollectiveKind { get; set; }

        /// <summary>
        /// Информационное примечание.
        /// Поле 300.
        /// </summary>
        [Field (300)]
        public object[]? Notes { get; set; }

        /// <summary>
        /// Текстовое ссылочное примечание "см. также".
        /// Поле 305.
        /// </summary>
        [Field (305)]
        public object[]? SeeAlsoNote { get; set; }

        /// <summary>
        /// Текстовое ссылочное примечание "см.".
        /// Поле 310.
        /// </summary>
        [Field (310)]
        public object? SeeNote { get; set; }

        /// <summary>
        /// Примечания об области применения.
        /// Поле 330.
        /// </summary>
        [Field (330)]
        public object[]? UsageNotes { get; set; }

        /// <summary>
        /// Источник составления записи.
        /// Поле 801.
        /// </summary>
        [Field (801)]
        public object[]? InformationSources { get; set; }

        /// <summary>
        /// Источник, в котором выявлена информ. о заголовке.
        /// Поле 810.
        /// </summary>
        [Field (810)]
        public object[]? IdentificationSources { get; set; }

        /// <summary>
        /// Информации об использовании основного наименования коллектива/мероприятия.
        /// Поле 820.
        /// </summary>
        [Field (820)]
        public object[]? UsageInformation { get; set; }

        /// <summary>
        /// Пример,приведенный в примечании.
        /// Поле 825.
        /// </summary>
        [Field (825)]
        public object[]? Examples { get; set; }

        /// <summary>
        /// Общее примечание каталогизатора.
        /// Поле 830.
        /// </summary>
        [Field (830)]
        public object[]? CataloguerNotes { get; set; }

        /// <summary>
        /// Информация об исключении основного наименования коллектива/мероприятия.
        /// Поле 835.
        /// </summary>
        [Field (835)]
        public object[]? ExclusionInformation { get; set; }

        /// <summary>
        /// Правила каталогизации и предметизации.
        /// Поле 152.
        /// </summary>
        [Field (152)]
        public object? CataloguingRules { get; set; }

        /// <summary>
        /// Ссылка-внешний объект.
        /// Поле 951.
        /// </summary>
        [Field (951)]
        public object[]? ExternalObject { get; set; }

        /// <summary>
        /// Внутренний двоичный ресурс.
        /// Поле 953.
        /// </summary>
        [Field (953)]
        public object[]? BinaryResource { get; set; }

        /// <summary>
        /// Проверку на дублетность производить.
        /// Поле 905.
        /// </summary>
        [Field (905)]
        public object? Settings { get; set; }

        /// <summary>
        /// Каталогизатор, дата.
        /// Поле 907.
        /// </summary>
        [Field (907)]
        public object[]? Technology { get; set; }

        /// <summary>
        /// Имя рабочего листа.
        /// Поле 920.
        /// </summary>
        [Field (920)]
        public string? Worksheet { get; set; }

        #endregion
    }
}
