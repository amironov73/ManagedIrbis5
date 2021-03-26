// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AthrsRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Запись в базе данных ATHRS.
    /// </summary>
    public sealed class AthrsRecord
    {
        #region Properties

        /// <summary>
        /// Основная (унифицированная) предметная рубрика/подрубрики.
        /// Поле 210.
        /// </summary>
        [Field(210)]
        public object? MainTitle { get; set; }

        /// <summary>
        /// Ссылки типа СМ. (Другие формы предметной рубрики)
        /// Поле 410.
        /// </summary>
        [Field(410)]
        public object[]? See { get; set; }

        /// <summary>
        /// Ссылки типа СМ. ТАКЖЕ (Предметная рубрика)
        /// Поле 550.
        /// </summary>
        [Field(550)]
        public object[]? SeeAlsoHeadings { get; set; }

        /// <summary>
        /// Ссылки типа СМ. ТАКЖЕ (Имя лица)
        /// Поле 500.
        /// </summary>
        [Field(500)]
        public object[]? SeeAlsoNames { get; set; }

        /// <summary>
        /// Ссылки типа СМ. ТАКЖЕ (Наименование организации)
        /// Поле 520.
        /// </summary>
        [Field(520)]
        public object[]? SeeAlsoOrganizations { get; set; }

        /// <summary>
        /// Ссылки типа СМ. ТАКЖЕ (Связанные основные предметные рубрики)
        /// Поле 510.
        /// </summary>
        [Field(510)]
        public object[]? SeeAlso { get; set; }

        /// <summary>
        /// Ссылка типа СМ. ТАКЖЕ (Географическое наименование)
        /// Поле 515.
        /// </summary>
        [Field(515)]
        public object? SeeAlsoGeoName { get; set; }

        /// <summary>
        /// Информационное примечание.
        /// Поле 300.
        /// </summary>
        [Field(300)]
        public object[]? Notes { get; set; }

        /// <summary>
        /// Текстовое ссылочное примечание "см. также".
        /// Поле 305.
        /// </summary>
        [Field(305)]
        public object[]? SeeAlsoNote { get; set; }

        /// <summary>
        /// Текстовое ссылочное примечание "см.".
        /// Поле 310.
        /// </summary>
        [Field(310)]
        public object? SeeNote { get; set; }

        /// <summary>
        /// Примечания об области применения.
        /// Поле 330.
        /// </summary>
        [Field(330)]
        public object[]? UsageNotes { get; set; }

        /// <summary>
        /// ББК.
        /// Поле 689.
        /// </summary>
        [Field(689)]
        public object[]? Bbk { get; set; }

        /// <summary>
        /// УДК.
        /// Поле 675.
        /// </summary>
        [Field(675)]
        public object[]? Udk { get; set; }

        /// <summary>
        /// Источник составления записи.
        /// Поле 801.
        /// </summary>
        [Field(801)]
        public object[]? InformationSources { get; set; }

        /// <summary>
        /// Источник,в котором выявлена информация о предметной рубрике.
        /// Поле 810.
        /// </summary>
        [Field(810)]
        public object[]? IdentificationSources { get; set; }

        /// <summary>
        /// Общее примечание каталогизатора.
        /// Поле 830.
        /// </summary>
        [Field(830)]
        public object[]? CataloguerNotes { get; set; }

        /// <summary>
        /// Пример,приведенный в примечании.
        /// Поле 825.
        /// </summary>
        [Field(825)]
        public object[]? Example { get; set; }

        /// <summary>
        /// Информация об исключенном заголовке.
        /// Поле 835.
        /// </summary>
        [Field(835)]
        public object[]? ExclusionInformation { get; set; }

        /// <summary>
        /// Ссылка-внешний объект.
        /// Поле 951.
        /// </summary>
        [Field(951)]
        public object[]? ExternalObject { get; set; }

        /// <summary>
        /// Правила каталогизации и предметизации.
        /// Поле 152.
        /// </summary>
        [Field(152)]
        public object? CataguingRules { get; set; }

        /// <summary>
        /// Проверку на дублетность производить.
        /// Поле 905.
        /// </summary>
        [Field(905)]
        public object? Settings { get; set; }

        /// <summary>
        /// Каталогизатор, дата.
        /// Поле 907.
        /// </summary>
        [Field(907)]
        public object[]? Technology { get; set; }

        /// <summary>
        /// Имя рабочего листа.
        /// Поле 920.
        /// </summary>
        [Field(920)]
        public string? Worksheet { get; set; }

        #endregion
    }
}
