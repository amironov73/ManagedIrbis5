// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UseStringInterpolation

/* ExternalResource.cs -- данные о внешнем ресурсе, поле 951
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Данные о внешнем ресурсе (поле 951).
    /// </summary>
    [XmlRoot ("external")]
    public sealed class ExternalResource
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 951;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "1234569abcdefhiklmnptuwx";

        #endregion

        #region Properties

        /// <summary>
        /// Имя файла или имя папки при групповой ссылке. Подполе A.
        /// </summary>
        /// <remarks>
        /// *.pdf, *.doc, *.jpg, *.jpeg, *.avi, *.exe
        /// </remarks>
        [SubField ('a')]
        [XmlAttribute ("filename")]
        [JsonPropertyName ("filename")]
        [Description ("Имя файла")]
        [DisplayName ("Имя файла")]
        public string? FileName { get; set; }

        /// <summary>
        /// URL (Адрес в Internet), или Полный сетевой путь и имя файла
        /// или полный сетевой путь к папке при групповой ссылке. Подполе I.
        /// </summary>
        [SubField ('i')]
        [XmlAttribute ("url")]
        [JsonPropertyName ("url")]
        [Description ("URL")]
        [DisplayName ("URL")]
        public string? Url { get; set; }

        /// <summary>
        /// Текст для ссылки. Подполе T.
        /// </summary>
        [SubField ('t')]
        [XmlAttribute ("description")]
        [JsonPropertyName ("description")]
        [Description ("Текст для ссылки")]
        [DisplayName ("Текст для ссылки")]
        public string? Description { get; set; }

        /// <summary>
        /// Количество файлов при групповой ссылке или количество страниц
        /// для PDF-файла (для автоматического разбиения на страницы).
        /// Подполе N.
        /// </summary>
        [SubField ('n')]
        [XmlAttribute ("fileCount")]
        [JsonPropertyName ("fileCount")]
        [Description ("Количество файлов")]
        [DisplayName ("Количество файлов")]
        public string? FileCount { get; set; }

        /// <summary>
        /// Имя-шаблон первого файла при групповой ссылке. Подполе M.
        /// </summary>
        [SubField ('m')]
        [XmlAttribute ("nameTemplate")]
        [JsonPropertyName ("nameTemplate")]
        [Description ("Имя-шаблон первого файла")]
        [DisplayName ("Имя-шаблон первого файла")]
        public string? NameTemplate { get; set; }

        /// <summary>
        /// Тип внешнего файла. Подполе H.
        /// </summary>
        [SubField ('h')]
        [XmlAttribute ("fileType")]
        [JsonPropertyName ("fileType")]
        [Description ("Тип внешнего файла")]
        [DisplayName ("Тип внешнего файла")]
        public string? FileType { get; set; }

        /// <summary>
        /// Признак электронного учебника (для задачи книгообеспеченности).
        /// Подполе K.
        /// </summary>
        [SubField ('k')]
        [XmlAttribute ("textbook")]
        [JsonPropertyName ("textbook")]
        [Description ("Признак электронного учебника")]
        [DisplayName ("Признак электронного учебника")]
        public string? Textbook { get; set; }

        /// <summary>
        /// Уровень доступа по категориям пользователей при доступе через Web.
        /// Подполе D.
        /// Оно же - дата начала предоставления информации.
        /// </summary>
        /// <remarks>
        /// Если ресурс доступен всем, ничего вводить не надо.
        /// </remarks>
        [SubField ('d')]
        [XmlAttribute ("access")]
        [JsonPropertyName ("access")]
        [Description ("Уровень доступа")]
        [DisplayName ("Уровень доступа")]
        public string? AccessLevel { get; set; }

        /// <summary>
        /// Ресурс доступен только в ЛВС. Подполе L.
        /// </summary>
        [SubField ('l')]
        [XmlAttribute ("lan")]
        [JsonPropertyName ("lan")]
        [Description ("Доступен только в ЛВС")]
        [DisplayName ("Доступен только в ЛВС")]
        public string? LanOnly { get; set; }

        /// <summary>
        /// Дата ввода информации. Подполе 1.
        /// </summary>
        [SubField ('1')]
        [XmlAttribute ("inputDate")]
        [JsonPropertyName ("inputDate")]
        [Description ("Дата ввода информации")]
        [DisplayName ("Дата ввода информации")]
        public string? InputDate { get; set; }

        /// <summary>
        /// Размер файла. Подполе 2.
        /// </summary>
        [SubField ('2')]
        [XmlAttribute ("fileSize")]
        [JsonPropertyName ("fileSize")]
        [Description ("Размер файла")]
        [DisplayName ("Размер файла")]
        public string? FileSize { get; set; }

        /// <summary>
        /// Номер источника копии. Подполе 3.
        /// </summary>
        [SubField ('3')]
        [XmlAttribute ("number")]
        [JsonPropertyName ("number")]
        [Description ("Номер источника копии")]
        [DisplayName ("Номер источника копии")]
        public string? Number { get; set; }

        /// <summary>
        /// Дата последней проверки доступности. Подполе 5.
        /// </summary>
        [SubField ('5')]
        [XmlAttribute ("lastCheck")]
        [JsonPropertyName ("lastCheck")]
        [Description ("Дата последней проверки")]
        [DisplayName ("Дата последней проверки")]
        public string? LastCheck { get; set; }

        /// <summary>
        /// Размеры изображения в пикселах. Подполе 6.
        /// </summary>
        [SubField ('6')]
        [XmlAttribute ("imageSize")]
        [JsonPropertyName ("imageSize")]
        [Description ("Размеры изображения")]
        [DisplayName ("Размеры изображения")]
        public string? ImageSize { get; set; }

        /// <summary>
        /// ISSN. Подполе X.
        /// </summary>
        [SubField ('x')]
        [XmlAttribute ("issn")]
        [JsonPropertyName ("issn")]
        [Description ("ISSN")]
        [DisplayName ("ISSN")]
        public string? Issn { get; set; }

        /// <summary>
        /// Форма представления. Подполе B.
        /// </summary>
        [SubField ('b')]
        [XmlAttribute ("form")]
        [JsonPropertyName ("form")]
        [Description ("Форма представления")]
        [DisplayName ("Форма представления")]
        public string? Form { get; set; }

        /// <summary>
        /// Код поставщика информации. Подполе F.
        /// </summary>
        [SubField ('f')]
        [XmlAttribute ("provider")]
        [JsonPropertyName ("provider")]
        [Description ("Код поставщика информации")]
        [DisplayName ("Код поставщика информации")]
        public string? Provider { get; set; }

        /// <summary>
        /// Цена. Подполе E.
        /// </summary>
        [SubField ('e')]
        [XmlAttribute ("price")]
        [JsonPropertyName ("price")]
        [Description ("Цена")]
        [DisplayName ("Цена")]
        public string? Price { get; set; }

        /// <summary>
        /// Шифр в БД. Подполе W.
        /// </summary>
        [SubField ('w')]
        [XmlAttribute ("index")]
        [JsonPropertyName ("index")]
        [Description ("Шифр в БД")]
        [DisplayName ("Шифр в БД")]
        public string? Index { get; set; }

        /// <summary>
        /// Примечания в свободной форме. Подполе P.
        /// </summary>
        [SubField ('p')]
        [XmlAttribute ("remarks")]
        [JsonPropertyName ("remarks")]
        [Description ("Примечания в свободной форме")]
        [DisplayName ("Примечания в свободной форме")]
        public string? Remarks { get; set; }

        /// <summary>
        /// Электронная библиотечная система. Подполе S.
        /// </summary>
        [SubField ('s')]
        [XmlAttribute ("system")]
        [JsonPropertyName ("system")]
        [Description ("Электронная библиотечная система")]
        [DisplayName ("Электронная библиотечная система")]
        public string? System { get; set; }

        /// <summary>
        /// Правила доступа в J-ИРБИС 2.0. Подполе 9.
        /// </summary>
        [SubField ('9')]
        [XmlAttribute ("rules")]
        [JsonPropertyName ("rules")]
        [Description ("Правила доступа в J-ИРБИС")]
        [DisplayName ("Правила доступа в J-ИРБИС")]
        public string? Rules { get; set; }

        /// <summary>
        /// Режим доступа.
        /// </summary>
        /// <remarks>
        /// Режим доступа для ресурсов из локальных сетей, а также
        /// из  полнотекстовых баз данных, доступ к которым осуществляется
        /// на договорной основе, по подписке и т.п.
        /// Подполе 4.
        /// </remarks>
        [SubField ('4')]
        [XmlAttribute ("access-mode")]
        [JsonPropertyName ("accessMode")]
        [Description ("Режим доступа")]
        [DisplayName ("Режим доступа")]
        public string? AccessMode { get; set; }

        /// <summary>
        /// Номер РСУ. Подполе U.
        /// </summary>
        [SubField ('u')]
        [XmlAttribute ("rsu")]
        [JsonPropertyName ("rsu")]
        [Description ("Номер РСУ")]
        [DisplayName ("Номер РСУ")]
        public string? Rsu { get; set; }

        /// <summary>
        /// Дата окончания срока доступа (в виде ГГГГММДД).
        /// Подполе C.
        /// </summary>
        [SubField ('c')]
        [XmlAttribute ("access-expiration")]
        [JsonPropertyName ("accessExpiration")]
        [Description ("Дата окончания срока доступа")]
        [DisplayName ("Дата окончания срока доступа")]
        public string? AccessExpirationDate { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlElement ("unknown")]
        [JsonPropertyName ("unknown")]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ассоциированное поле библиографической записи.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Field? Field { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        [Description ("Пользовательские данные")]
        [DisplayName ("Пользовательские данные")]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Применение данных к указанному полю библиографической записи.
        /// </summary>
        public void ApplyTo
            (
                Field field
            )
        {
            Sure.NotNull (field);

            field
                .SetSubFieldValue ('a', FileName)
                .SetSubFieldValue ('i', Url)
                .SetSubFieldValue ('t', Description)
                .SetSubFieldValue ('n', FileCount)
                .SetSubFieldValue ('m', NameTemplate)
                .SetSubFieldValue ('h', FileType)
                .SetSubFieldValue ('k', Textbook)
                .SetSubFieldValue ('d', AccessLevel)
                .SetSubFieldValue ('l', LanOnly)
                .SetSubFieldValue ('1', InputDate)
                .SetSubFieldValue ('2', FileSize)
                .SetSubFieldValue ('3', Number)
                .SetSubFieldValue ('5', LastCheck)
                .SetSubFieldValue ('6', ImageSize)
                .SetSubFieldValue ('x', Issn)
                .SetSubFieldValue ('b', Form)
                .SetSubFieldValue ('f', Provider)
                .SetSubFieldValue ('e', Price)
                .SetSubFieldValue ('w', Index)
                .SetSubFieldValue ('p', Remarks)
                .SetSubFieldValue ('s', System)
                .SetSubFieldValue ('9', Rules)
                .SetSubFieldValue ('4', AccessMode)
                .SetSubFieldValue ('u', Rsu)
                .SetSubFieldValue ('c', AccessExpirationDate);
        }

        /// <summary>
        /// Разбор указанного поля библиографической записи.
        /// </summary>
        public static ExternalResource ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return new ExternalResource ()
            {
                FileName = field.GetFirstSubFieldValue ('a'),
                Url = field.GetFirstSubFieldValue ('i'),
                Description = field.GetFirstSubFieldValue ('t'),
                FileCount = field.GetFirstSubFieldValue ('n'),
                NameTemplate = field.GetFirstSubFieldValue ('m'),
                FileType = field.GetFirstSubFieldValue ('h'),
                Textbook = field.GetFirstSubFieldValue ('k'),
                AccessLevel = field.GetFirstSubFieldValue ('d'),
                LanOnly = field.GetFirstSubFieldValue ('l'),
                InputDate = field.GetFirstSubFieldValue ('1'),
                FileSize = field.GetFirstSubFieldValue ('2'),
                Number = field.GetFirstSubFieldValue ('3'),
                LastCheck = field.GetFirstSubFieldValue ('5'),
                ImageSize = field.GetFirstSubFieldValue ('6'),
                Issn = field.GetFirstSubFieldValue ('x'),
                Form = field.GetFirstSubFieldValue ('b'),
                Provider = field.GetFirstSubFieldValue ('f'),
                Price = field.GetFirstSubFieldValue ('e'),
                Index = field.GetFirstSubFieldValue ('w'),
                Remarks = field.GetFirstSubFieldValue ('p'),
                System = field.GetFirstSubFieldValue ('s'),
                Rules = field.GetFirstSubFieldValue ('9'),
                AccessMode = field.GetFirstSubFieldValue ('4'),
                Rsu = field.GetFirstSubFieldValue ('u'),
                AccessExpirationDate = field.GetFirstSubFieldValue ('c'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };
        }

        /// <summary>
        /// Разбор указанной библиографической записи.
        /// </summary>
        public static ExternalResource[] ParseRecord
            (
                Record record,
                int tag = Tag
            )
        {
            Sure.NotNull (record);
            Sure.Positive (tag);

            return record
                .EnumerateField (tag)
                .Select (field => ParseField (field))
                .ToArray();
        }

        /// <summary>
        /// Преобразование данных в поле библиографической записи.
        /// </summary>
        public Field ToField()
        {
            return new Field (Tag)
                .AddNonEmpty ('a', FileName)
                .AddNonEmpty ('i', Url)
                .AddNonEmpty ('t', Description)
                .AddNonEmpty ('m', NameTemplate)
                .AddNonEmpty ('h', FileType)
                .AddNonEmpty ('n', FileCount)
                .AddNonEmpty ('k', Textbook)
                .AddNonEmpty ('d', AccessLevel)
                .AddNonEmpty ('l', LanOnly)
                .AddNonEmpty ('1', InputDate)
                .AddNonEmpty ('2', FileSize)
                .AddNonEmpty ('3', Number)
                .AddNonEmpty ('5', LastCheck)
                .AddNonEmpty ('6', ImageSize)
                .AddNonEmpty ('x', Issn)
                .AddNonEmpty ('b', Form)
                .AddNonEmpty ('f', Provider)
                .AddNonEmpty ('e', Price)
                .AddNonEmpty ('w', Index)
                .AddNonEmpty ('p', Remarks)
                .AddNonEmpty ('s', System)
                .AddNonEmpty ('9', Rules)
                .AddNonEmpty ('4', AccessMode)
                .AddNonEmpty ('u', Rsu)
                .AddNonEmpty ('c', AccessExpirationDate)
                .AddRange (UnknownSubFields);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            FileName = reader.ReadNullableString();
            Url = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            FileCount = reader.ReadNullableString();
            NameTemplate = reader.ReadNullableString();
            FileType = reader.ReadNullableString();
            Textbook = reader.ReadNullableString();
            AccessLevel = reader.ReadNullableString();
            LanOnly = reader.ReadNullableString();
            InputDate = reader.ReadNullableString();
            FileSize = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            LastCheck = reader.ReadNullableString();
            ImageSize = reader.ReadNullableString();
            Issn = reader.ReadNullableString();
            Form = reader.ReadNullableString();
            Provider = reader.ReadNullableString();
            Price = reader.ReadNullableString();
            Index = reader.ReadNullableString();
            Remarks = reader.ReadNullableString();
            System = reader.ReadNullableString();
            Rules = reader.ReadNullableString();
            AccessMode = reader.ReadNullableString();
            Rsu = reader.ReadNullableString();
            AccessExpirationDate = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable (FileName)
                .WriteNullable (Url)
                .WriteNullable (Description)
                .WriteNullable (FileCount)
                .WriteNullable (NameTemplate)
                .WriteNullable (FileType)
                .WriteNullable (Textbook)
                .WriteNullable (AccessLevel)
                .WriteNullable (LanOnly)
                .WriteNullable (InputDate)
                .WriteNullable (FileSize)
                .WriteNullable (Number)
                .WriteNullable (LastCheck)
                .WriteNullable (ImageSize)
                .WriteNullable (Issn)
                .WriteNullable (Form)
                .WriteNullable (Provider)
                .WriteNullable (Price)
                .WriteNullable (Index)
                .WriteNullable (Remarks)
                .WriteNullable (System)
                .WriteNullable (Rules)
                .WriteNullable (AccessMode)
                .WriteNullable (Rsu)
                .WriteNullable (AccessExpirationDate)
                .WriteNullableArray (UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ExternalResource> verifier
                = new Verifier<ExternalResource> (this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty (FileName)
                    ^ !string.IsNullOrEmpty (Url),
                    "FileName or URL"
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "FileName: {0}, Url: {1}, Description: {2}",
                    FileName.ToVisibleString(),
                    Url.ToVisibleString(),
                    Description.ToVisibleString()
                );
        }

        #endregion

    } // class ExternalResource

} // namespace ManagedIrbis.Fields
