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

/* Isrc.cs -- Международный стандартный номер аудио/видео записи
 * Ars Magna project, http://arsmagna.ru
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#nullable enable

namespace ManagedIrbis.Identifiers
{
    //
    // См. https://ru.wikipedia.org/wiki/ISRC
    //
    // Международный стандартный номер аудио/видео записи
    // ((ISRC),
    // определённый ISO 3901 — международный стандартный код
    // для точного определения уникальной аудио- или видеозаписи.
    // Он выделяется IFPI, уполномоченной на эти действия ISO.
    // За этот стандарт отвечает технический комитет 46,
    // подкомитет 9 ISO. Обратите внимание что ISO определяет
    // именно конкретную запись, а не песню в целом. Таким образом,
    // различные записи, редакции и ремиксы одной и той же песни будут
    // иметь различные коды ISRC.
    // Песни определяются аналогичным кодом ISWC.
    //
    // Коды ISRC выделяются национальными агентствами ISRC как
    // для частных, так и для юридических лиц. Обычно это бесплатно,
    // но национальные агентства могут взимать разумную плату
    // для покрытия издержек на эту операцию.
    //
    // Код ISRC всегда состоит из 12 символов и записывается
    // в формате "CC-XXX-YY-NNNNN" (Дефисы не являются частью кода ISRC,
    // но этот код часто пишут таким образом чтобы облегчить его чтение.)
    // Упомянутые выше четыре части означают:
    //
    // "CC" означает код страны согласно ISO 3166-1 alpha-2
    // "XXX" — трёхзначный алфавитно-цифровой регистрационный код,
    // уникальным образом определяющий организацию, которая регистрирует код.
    // Например, в Великобритании это Phonographic Performance Limited (PPL).
    // "YY" — последние две цифры года регистрации (не обязательно
    // соответствуют году, когда произведена запись)
    // "NNNNN" — уникальная последовательность из пяти цифр,
    // определяющая определённую аудиозапись.
    //
    // Например, GBEMI0300013.
    //
    // Например, запись песни "Enquanto Houver Sol",
    // выполненная бразильской группой Titãs получила код ISRC BR-BMG-03-00729:
    //
    // BR для Бразилии
    // BMG для BMG
    // 03 для 2003 года
    // 00729 уникальный идентификатор записи
    //
    // Другой пример: USPR37300012 — запись песни "Love's Theme"
    // группы Love Unlimited Orchestra.
    //
    // US-PR3-73/00012
    // US для США
    // PR3 для организации
    // 73 для 1973 года
    // 00012 уникальный идентификатор записи
    //
    // Красная книга (стандарт аудио CD) определяет кодирование
    // кодов ISRC на компакт-дисках.
    //

    /// <summary>
    /// International Standard Recording Code.
    /// </summary>
    [XmlRoot ("isrc")]
    public sealed class Isrc
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Код страны.
        /// </summary>
        [XmlElement ("country")]
        [JsonPropertyName ("country")]
        [Description ("Код страны")]
        [DisplayName ("Код страны")]
        public string? Country { get; set; }

        /// <summary>
        /// Код регистрирующей организации.
        /// </summary>
        [XmlElement ("registrant")]
        [JsonPropertyName ("registrant")]
        [Description ("Код регистрирующей организации")]
        [DisplayName ("Код регистрирующей организации")]
        public string? Registrant { get; set; }

        /// <summary>
        /// Две последние цифры года.
        /// </summary>
        [XmlElement ("year")]
        [JsonPropertyName ("year")]
        [Description ("Две последние цифры года")]
        [DisplayName ("Две последние цифры года")]
        public string? Year { get; set; }

        /// <summary>
        /// Регистрационный номер.
        /// </summary>
        [XmlElement ("number")]
        [JsonPropertyName ("number")]
        [Description ("Регистрационный номер")]
        [DisplayName ("Регистрационный номер")]
        public string? Number { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текстового представления.
        /// </summary>
        public void ParseText
            (
                ReadOnlySpan<char> text
            )
        {
            Sure.NotEmpty (text);

            var navigator = new ValueTextNavigator (text);
            navigator.SkipWhitespace();
            Country = navigator.ReadString (2).ThrowIfEmpty().ToString();
            navigator.SkipChar ('-');
            Registrant = navigator.ReadString (3).ThrowIfEmpty().ToString();
            navigator.SkipChar ('-');
            Year = navigator.ReadString (2).ThrowIfEmpty().ToString();
            navigator.SkipChar ('-');
            Number = navigator.ReadString (5).ThrowIfEmpty().ToString();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Country = reader.ReadNullableString();
            Registrant = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Number = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Country)
                .WriteNullable (Registrant)
                .WriteNullable (Year)
                .WriteNullable (Number);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Isrc> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Country)
                .Assert (Country?.Length == 2)
                .NotNullNorEmpty (Registrant)
                .Assert (Registrant?.Length == 3)
                .NotNullNorEmpty (Year)
                .Assert (Year?.Length == 2)
                .NotNullNorEmpty (Number)
                .Assert (Number?.Length == 5);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{Country}-{Registrant}-{Year}-{Number}";
        }

        #endregion
    }
}
