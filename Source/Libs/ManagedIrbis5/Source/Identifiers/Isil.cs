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

/* Isil.cs -- ISIL
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
    // https://ru.wikipedia.org/wiki/International_Standard_Identifier_for_Libraries_and_Related_Organizations
    //
    // ISIL (англ. International Standard Identifier for Libraries
    // and Related Organizations) - представляет собой универсальный
    // идентификатор (условное обозначение) для библиотек и родственных
    // им организаций, присваивается национальными агентствами
    // Регистрационного комитета Международной организации
    // по стандартизации (ISO), в соответствии со стандартом
    // ISO 15511. В настоящее время используется преимущественно
    // для формирования идентификатора для документов библиотечного
    // фонда в системах автоматизации библиотек на основе технологии
    // RFID, в соответствии со стандартом ISO 28560.
    // Российским агентством Регистрационного комитета ISO
    // по присвоению кодов ISIL является ГПНТБ России.
    //
    // Структура кода ISIL
    // Код ISIL представляет собой символьный идентификатор
    // переменной длины. Максимальное число символов в идентификаторе
    // может быть 16. В качестве символов могут быть использованы
    // арабские цифры (0 — 9), не модифицированные буквы латинского
    // алфавита (a — z), а также специальные символы: «/», «-», ": ".
    // Модифицированные символы латинского алфавита, а также символы
    // других национальных алфавитов не могут использоваться
    // в составе кода. Каждый буквенный символ должен использоваться
    // без учёта регистра, что бы оставаться уникальным в нормализованном
    // представлении символов кода, в соответствии
    // со стандартом ISO/IEC-10646-1.
    //
    // Каждый код ISIL состоит из двух компонентов: префикса и уникального
    // идентификатора, разделённых символом дефиса («-»). Символ дефиса
    // является обязательным символом кода ISIL.
    //
    // Префикс представляет собой код страны, в которой находилась
    // библиотека или родственная ей организация на момент
    // присвоения ISIL. Код страны состоит из двух букв латинского
    // алфавита, присваиваемых в соответствии с ISO 3166-1 alpha-2
    // (ГОСТ 7.67-2003). Если библиотека имеет филиалы или подразделения
    // в других странах, код ISIL присваивается в соответствии
    // с расположением головного офиса.
    //
    // Уникальный идентификатор может быть цифробуквенным
    // и присваивается в соответствии с национальными системами идентификации
    // библиотек. В России для формирования кода ISIL используется
    // система условных обозначений органов научно-технической информации
    // и библиотек, входящих в состав Государственной системы
    // научно-технической информации (ГСНТИ).
    //
    // Представление кода ISIL
    // При указании кода в документах, презентациях и т. д. ему должно
    // предшествовать наименование «ISIL», отделённое от кода пробелом.
    // Например:
    //
    // ISIL RU-10010033.
    //
    // Стандарт ISO 15511 не определяет форматов хранения кода
    // в компьютерных системах и базах данных, но определяет представление
    // кода для чтения людьми как: ISIL «префикс»-«идентификатор».
    // Код ISIL должен оставаться уникальным, независимо от представления
    // в верхнем или нижнем регистре клавиатуры.
    //

    /// <summary>
    /// International Standard Identifier for Libraries and Related Organizations.
    /// </summary>
    [XmlRoot ("isil")]
    public sealed class Isil
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Префикс.
        /// </summary>
        [XmlElement ("prefix")]
        [JsonPropertyName ("prefix")]
        [Description ("Префикс")]
        [DisplayName ("Префикс")]
        public string? Prefix { get; set; }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [XmlElement ("identifier")]
        [JsonPropertyName ("identifier")]
        [Description ("Идентификатор")]
        [DisplayName ("Идентификатор")]
        public string? Identifier { get; set; }

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
            Prefix = navigator.ReadUntil ('-').Trim().ThrowIfEmpty().ToString();
            navigator.SkipChar ('-');
            Identifier = navigator.GetRemainingText().Trim().ThrowIfEmpty().ToString();
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

            Prefix = reader.ReadNullableString();
            Identifier = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Prefix)
                .WriteNullable (Identifier);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Isil> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Prefix)
                .Assert (Prefix?.Length == 2)
                .NotNullNorEmpty (Identifier);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Prefix + "-" + Identifier;
        }

        #endregion
    }
}
