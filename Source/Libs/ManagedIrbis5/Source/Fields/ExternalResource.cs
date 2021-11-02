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

/* ExternalResource.cs -- данные о внешнем ресурсе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
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
    [DebuggerDisplay("File={FileName} Url={Url} Description={Description}")]
    public sealed class ExternalResource
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "12356abdefhiklmnptwx";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 951;

        #endregion

        #region Properties

        /// <summary>
        /// Имя файла. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("filename")]
        [JsonPropertyName("filename")]
        [Description("Имя файла")]
        [DisplayName("Имя файла")]
        public string? FileName { get; set; }

        /// <summary>
        /// URL. Подполе i.
        /// </summary>
        [SubField('i')]
        [XmlAttribute("url")]
        [JsonPropertyName("url")]
        [Description("URL")]
        [DisplayName("URL")]
        public string? Url { get; set; }

        /// <summary>
        /// Текст для ссылки. Подполе t.
        /// </summary>
        [SubField('t')]
        [XmlAttribute("description")]
        [JsonPropertyName("description")]
        [Description("Текст для ссылки")]
        [DisplayName("Текст для ссылки")]
        public string? Description { get; set; }

        /// <summary>
        /// Количество файлов. Подполе n.
        /// </summary>
        [SubField('n')]
        [XmlAttribute("fileCount")]
        [JsonPropertyName("fileCount")]
        [Description("Количество файлов")]
        [DisplayName("Количество файлов")]
        public int? FileCount { get; set; }

        /// <summary>
        /// Имя-шаблон первого файла. Подполе m.
        /// </summary>
        [SubField('m')]
        [XmlAttribute("nameTemplate")]
        [JsonPropertyName("nameTemplate")]
        [Description("Имя-шаблон первого файла")]
        [DisplayName("Имя-шаблон первого файла")]
        public string? NameTemplate { get; set; }

        /// <summary>
        /// Тип внешнего файла. Подполе h.
        /// </summary>
        [SubField('h')]
        [XmlAttribute("fileType")]
        [JsonPropertyName("fileType")]
        [Description("Тип внешнего файла")]
        [DisplayName("Тип внешнего файла")]
        public string? FileType { get; set; }

        /// <summary>
        /// Признак электронного учебника. Подполе k.
        /// </summary>
        [SubField('k')]
        [XmlAttribute("textbook")]
        [JsonPropertyName("textbook")]
        [Description("Признак электронного учебника")]
        [DisplayName("Признак электронного учебника")]
        public string? Textbook { get; set; }

        /// <summary>
        /// Уровень доступа по категориям пользователей. Подполе d.
        /// Оно же - дата начала предоставления информации.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("access")]
        [JsonPropertyName("access")]
        [Description("Уровень доступа")]
        [DisplayName("Уровень доступа")]
        public string? AccessLevel { get; set; }

        /// <summary>
        /// Доступен только в ЛВС. Подполе l.
        /// </summary>
        [SubField('l')]
        [XmlAttribute("lan")]
        [JsonPropertyName("lan")]
        [Description("Доступен только в ЛВС")]
        [DisplayName("Доступен только в ЛВС")]
        public bool LanOnly { get; set; }

        /// <summary>
        /// Дата ввода информации. Подполе 1.
        /// </summary>
        [SubField('1')]
        [XmlAttribute("inputDate")]
        [JsonPropertyName("inputDate")]
        [Description("Дата ввода информации")]
        [DisplayName("Дата ввода информации")]
        public DateTime? InputDate { get; set; }

        /// <summary>
        /// Размер файла. Подполе 2.
        /// </summary>
        [SubField('2')]
        [XmlAttribute("fileSize")]
        [JsonPropertyName("fileSize")]
        [Description("Размер файла")]
        [DisplayName("Размер файла")]
        public long FileSize { get; set; }

        /// <summary>
        /// Номер источника копии. Подполе 3.
        /// </summary>
        [SubField('3')]
        [XmlAttribute("number")]
        [JsonPropertyName("number")]
        [Description("Номер источника копии")]
        [DisplayName("Номер источника копии")]
        public string? Number { get; set; }

        /// <summary>
        /// Дата последней проверки доступности. Подполе 5.
        /// </summary>
        [SubField('5')]
        [XmlAttribute("lastCheck")]
        [JsonPropertyName("lastCheck")]
        [Description("Дата последней проверки")]
        [DisplayName("Дата последней проверки")]
        public DateTime? LastCheck { get; set; }

        /// <summary>
        /// Размеры изображения в пикселах. Подполе 6.
        /// </summary>
        [SubField('6')]
        [XmlAttribute("imageSize")]
        [JsonPropertyName("imageSize")]
        [Description("Размеры изображения")]
        [DisplayName("Размеры изображения")]
        public string? ImageSize { get; set; }

        /// <summary>
        /// ISSN. Подполе X.
        /// </summary>
        [SubField('x')]
        [XmlAttribute("issn")]
        [JsonPropertyName("issn")]
        [Description("ISSN")]
        [DisplayName("ISSN")]
        public string? Issn { get; set; }

        /// <summary>
        /// Форма представления. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("form")]
        [JsonPropertyName("form")]
        [Description("Форма представления")]
        [DisplayName("Форма представления")]
        public string? Form { get; set; }

        /// <summary>
        /// Код поставщика информации. Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlAttribute("provider")]
        [JsonPropertyName("provider")]
        [Description("Код поставщика информации")]
        [DisplayName("Код поставщика информации")]
        public string? Provider { get; set; }

        /// <summary>
        /// Цена. Подполе e.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("price")]
        [JsonPropertyName("price")]
        [Description("Цена")]
        [DisplayName("Цена")]
        public string? Price { get; set; }

        /// <summary>
        /// Шифр в БД. Подполе w.
        /// </summary>
        [SubField('w')]
        [XmlAttribute("index")]
        [JsonPropertyName("index")]
        [Description("Шифр в БД")]
        [DisplayName("Шифр в БД")]
        public string? Index { get; set; }

        /// <summary>
        /// Примечания в свободной форме. Подполе p.
        /// </summary>
        [SubField('p')]
        [XmlAttribute("remarks")]
        [JsonPropertyName("remarks")]
        [Description("Примечания в свободной форме")]
        [DisplayName("Примечания в свободной форме")]
        public string? Remarks { get; set; }

        /// <summary>
        /// Электронная библиотечная система. Подполе s.
        /// </summary>
        [SubField('s')]
        [XmlAttribute("system")]
        [JsonPropertyName("system")]
        [Description("Электронная библиотечная система")]
        [DisplayName("Электронная библиотечная система")]
        public string? System { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Поле")]
        [DisplayName("Поле")]
        public Field? Field { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Пользовательские данные")]
        [DisplayName("Пользовательские данные")]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExternalResource()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExternalResource
            (
                string? url
            )
        {
            Url = url;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the field.
        /// </summary>
        public void ApplyToField
            (
                Field field
            )
        {
            // TODO check the applying

            field
                .SetSubFieldValue ('a', FileName)
                .SetSubFieldValue ('i', Url)
                .SetSubFieldValue ('t', Description)
                .SetSubFieldValue ('m', NameTemplate)
                .SetSubFieldValue ('h', FileType)
                .SetSubFieldValue ('n', FileCount)
                .SetSubFieldValue ('k', Textbook)
                .SetSubFieldValue ('d', AccessLevel)
                .SetSubFieldValue ('l', LanOnly, "да")
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
                .SetSubFieldValue ('s', System);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static ExternalResource Parse
            (
                Field field
            )
        {
            ExternalResource result = new ExternalResource
            {
                FileName = field.GetFirstSubFieldValue('a'),
                Url = field.GetFirstSubFieldValue('i'),
                Description = field.GetFirstSubFieldValue('t'),
                FileCount = Map.ToInt32(field, 'n'),
                NameTemplate = field.GetFirstSubFieldValue('m'),
                FileType = field.GetFirstSubFieldValue('h'),
                Textbook = field.GetFirstSubFieldValue('k'),
                AccessLevel = field.GetFirstSubFieldValue('d'),
                LanOnly = Map.ToBoolean(field, 'l'),
                InputDate = Map.ToDateTime(field, '1'),
                FileSize = Map.ToInt64(field, '2'),
                Number = field.GetFirstSubFieldValue('3'),
                LastCheck = Map.ToDateTime(field, '5'),
                ImageSize = field.GetFirstSubFieldValue('6'),
                Issn = field.GetFirstSubFieldValue('x'),
                Form = field.GetFirstSubFieldValue('b'),
                Provider = field.GetFirstSubFieldValue('f'),
                Price = field.GetFirstSubFieldValue('e'),
                Index = field.GetFirstSubFieldValue('w'),
                Remarks = field.GetFirstSubFieldValue('p'),
                System = field.GetFirstSubFieldValue('s'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static ExternalResource[] Parse
            (
                Record record,
                int tag = Tag
            )
        {
            return record.Fields
                .GetField(tag)
                .Select(field => Parse(field))
                .ToArray();
        }

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        public Field ToField()
        {
            var result = new Field(Tag)
                .AddNonEmpty ('a', FileName)
                .AddNonEmpty ('i', Url)
                .AddNonEmpty ('t', Description)
                .AddNonEmpty ('m', NameTemplate)
                .AddNonEmpty ('h', FileType)
                .AddNonEmpty ('n', FileCount)
                .AddNonEmpty ('k', Textbook)
                .AddNonEmpty ('d', AccessLevel)
                .AddNonEmpty ('l', LanOnly, "да")
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
                .AddNonEmpty ('s', System);

            return result;
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
            FileCount = reader.ReadNullableInt32();
            NameTemplate = reader.ReadNullableString();
            FileType = reader.ReadNullableString();
            Textbook = reader.ReadNullableString();
            AccessLevel = reader.ReadNullableString();
            LanOnly = reader.ReadBoolean();
            InputDate = reader.ReadNullableDateTime();
            FileSize = reader.ReadPackedInt64();
            Number = reader.ReadNullableString();
            LastCheck = reader.ReadNullableDateTime();
            ImageSize = reader.ReadNullableString();
            Issn = reader.ReadNullableString();
            Form = reader.ReadNullableString();
            Provider = reader.ReadNullableString();
            Price = reader.ReadNullableString();
            Index = reader.ReadNullableString();
            Remarks = reader.ReadNullableString();
            System = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(FileName)
                .WriteNullable(Url)
                .WriteNullable(Description)
                .Write(FileCount)
                .WriteNullable(NameTemplate)
                .WriteNullable(FileType)
                .WriteNullable(Textbook)
                .WriteNullable(AccessLevel)
                .Write(LanOnly);
            writer
                .Write(InputDate)
                .WritePackedInt64(FileSize)
                .WriteNullable(Number)
                .Write(LastCheck)
                .WriteNullable(ImageSize)
                .WriteNullable(Issn)
                .WriteNullable(Form)
                .WriteNullable(Provider)
                .WriteNullable(Price)
                .WriteNullable(Index)
                .WriteNullable(Remarks)
                .WriteNullable(System);
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
                = new Verifier<ExternalResource>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(FileName)
                    ^ !string.IsNullOrEmpty(Url),
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
