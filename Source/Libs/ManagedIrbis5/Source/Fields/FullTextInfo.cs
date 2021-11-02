// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FullTextInfo.cs -- сведения о полном тексте документа
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
    //
    // Начиная с версии 2018.1
    //

    //
    // В структуру БД ЭК ИРБИС64+ по сравнению с ИРБИС64
    // введено одно новое поле, предназначенное для описания
    // ПОЛНОГО ТЕКСТА исходного документа (не путать
    // с полем 951 – ВНЕШНИЙ ОБЪЕКТ, которое сохраняется
    // в структуре БД ЭК в прежнем виде).
    // Метка поля – 955.
    // Поле – НЕПОВТОРЯЮЩЕЕСЯ.
    // Включает в себя следующие подполя:
    // A – имя файла полного текста с расширением PDF
    // (расширение указывается обязательно);
    // B – идентификатор записи права доступа (см. ниже);
    // N – количество физических страниц полного текста
    // (формируется системой автоматически).
    //
    // Файл полного текста должен быть распознанным
    // (т.е. иметь текстовый слой) PDF-файлом, поддающимся
    // разбиению на страницы. Имя файла не может содержать
    // символы «запятая», «кавычки», двойные подчеркивания,
    // квадратные и фигурные скобки. Максимальная длина
    // имени файла - 64 символа. Не рекомендуется использовать
    // в именах файлов кириллические символы. Файл полного
    // текста должен находиться по пути, который указан в
    // 11 строке параметрического файла <имя_БД>.par или
    // в подпапках по этому пути – в последнем случае
    // в подполе А 955 поля необходимо указывать относительный
    // путь, начинающийся со слэша (все слэши в относительном
    // пути должны быть ОБРАТНЫМИ). Имя файла должно быть
    // УНИКАЛЬНЫМ в рамках одной БД.
    //

    /// <summary>
    /// Сведения о полном тексте документа (поле 955).
    /// </summary>
    public sealed class FullTextInfo
        : IHandmadeSerializable,
          IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abnt";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 955;

        #endregion

        #region Properties

        /// <summary>
        /// Текст для ссылки. Подполе t.
        /// </summary>
        [SubField('t')]
        [XmlAttribute("display-text")]
        [JsonPropertyName("displayText")]
        [Description("Текст для ссылки")]
        [DisplayName("Текст для ссылки")]
        public string? DisplayText { get; set; }

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
        /// Количество страниц. Подполе n.
        /// </summary>
        [SubField('n')]
        [XmlAttribute("page-count")]
        [JsonPropertyName("pageCount")]
        [Description("Количество страниц")]
        [DisplayName("Количество страниц")]
        public int? PageCount { get; set; }

        /// <summary>
        /// Идентификатор записи права доступа. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("access-rights")]
        [JsonPropertyName("accessRights")]
        [Description("Идентификатор записи права доступа")]
        [DisplayName("Идентификатор записи права доступа")]
        public string? AccessRights { get; set; }

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
        public FullTextInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FullTextInfo
            (
                string? fileName
            )
        {
            FileName = fileName;
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
                .SetSubFieldValue ('t', DisplayText)
                .SetSubFieldValue ('a', FileName)
                .SetSubFieldValue ('n', PageCount)
                .SetSubFieldValue ('b', AccessRights);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static FullTextInfo Parse
            (
                Field field
            )
        {
            FullTextInfo result = new FullTextInfo
            {
                DisplayText = field.GetFirstSubFieldValue('t'),
                FileName = field.GetFirstSubFieldValue('a'),
                PageCount = Map.ToInt32(field, 'n'),
                AccessRights = field.GetFirstSubFieldValue('b'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static FullTextInfo[] Parse
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
                .AddNonEmpty ('t', FileName)
                .AddNonEmpty ('a', FileName)
                .AddNonEmpty ('n', PageCount.ToString())
                .AddNonEmpty ('b', AccessRights);

            return result;

        } // method ToField

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            DisplayText = reader.ReadNullableString();
            FileName = reader.ReadNullableString();
            AccessRights = reader.ReadNullableString();
            PageCount = null;

            if (reader.ReadBoolean())
            {
                PageCount = reader.ReadPackedInt32();
            }

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(DisplayText)
                .WriteNullable(FileName)
                .WriteNullable(AccessRights);
            writer.Write(PageCount.HasValue);

            if (PageCount.HasValue)
            {
                writer.WritePackedInt32(PageCount.Value);
            }

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<FullTextInfo> verifier
                = new Verifier<FullTextInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(FileName),
                    "FileName"
                );

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => FileName.ToVisibleString();

        #endregion

    } // class FullTextInfo

} // namespace ManagedIrbis.Fields
