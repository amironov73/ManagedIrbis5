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

/* Imsi.cs -- IMSI
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
    // https://ru.wikipedia.org/wiki/IMSI
    //
    // International Mobile Subscriber Identity (IMSI) - международный
    // идентификатор мобильного абонента (индивидуальный номер абонента),
    // ассоциированный с каждым пользователем мобильной связи стандарта
    // GSM, UMTS или CDMA. При регистрации в сети аппарат абонента передаёт
    // IMSI, по которому происходит его идентификация. Во избежание перехвата,
    // этот номер посылается через сеть настолько редко (только аутентификация
    // пользователя), насколько это возможно - в тех случаях, когда
    // это возможно, вместо него посылается случайно сгенерированный TMSI.
    //
    // В системе GSM идентификатор содержится на SIM-карте в элементарном
    // файле (EF), имеющем идентификатор 6F07. Формат хранения IMSI
    // на SIM-карте описан ETSI в спецификации GSM 11.11. Кроме того,
    // IMSI используется любой мобильной сетью, соединенной с другими сетями
    // (в частности с CDMA или EVDO) таким же образом, как и в GSM сетях.
    // Этот номер связан либо непосредственно с телефоном, либо
    // с R-UIM картой (аналогом SIM карты GSM в системе CDMA).
    //
    // Длина IMSI, как правило, составляет 15 цифр, но может быть короче.
    // Например: 250-07-ХХХХХХХХХХ. Первые три цифры это MCC (Mobile
    // Country Code, мобильный код страны). В примере 250 - Россия.
    // За ним следует MNC (Mobile Network Code, код мобильной сети).
    // 07 из примера - СМАРТС. Код мобильной сети может содержать
    // две цифры по европейскому стандарту или три по североамериканскому.
    // Все последующие цифры — непосредственно идентификатор пользователя
    // MSIN (Mobile Subscriber Identification Number).
    //
    // IMSI соответствует стандарту нумерации E.212 ITU.
    //

    //
    // https://en.wikipedia.org/wiki/International_mobile_subscriber_identity
    //
    // The International Mobile Subscriber Identity or IMSI /ˈɪmziː/
    // is used to identify the user of a cellular network and is
    // a unique identification associated with all cellular networks.
    // It is stored as a 64 bit field and is sent by the phone to the network.
    // It is also used for acquiring other details of the mobile
    // in the home location register (HLR) or as locally copied in the
    // visitor location register. To prevent eavesdroppers identifying
    // and tracking the subscriber on the radio interface, the IMSI
    // is sent as rarely as possible and a randomly generated TMSI
    // is sent instead.
    //
    // The IMSI is used in any mobile network that interconnects with
    // other networks. For GSM, UMTS and LTE networks, this number
    // was provisioned in the SIM card and for cdmaOne and CDMA2000
    // networks, in the phone directly or in the R-UIM card (the CDMA
    // equivalent of the SIM card). Both cards have been superseded
    // by the UICC.
    //
    // An IMSI is usually presented as a 15 digit number but can be shorter.
    // For example, MTN South Africa's old IMSIs that are still being used
    // in the market are shown as 14 digits. The first 3 digits are
    // the mobile country code (MCC), which are followed by the mobile
    // network code (MNC), either 2 digits (European standard) or 3 digits
    // (North American standard). The length of the MNC depends on the value
    // of the MCC, and it is recommended that the length is uniform within
    // a MCC area. The remaining digits are the mobile subscription
    // identification number (MSIN) within the network's customer base
    // (mostly 10 or 9 digits depending on the MNC length).
    //
    // The IMSI conforms to the ITU E.212 numbering standard.
    //

    /// <summary>
    /// International mobile subscriber identity.
    /// </summary>
    [XmlRoot ("imsi")]
    public sealed class Imsi
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

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

            var navigator = new ValueTextNavigator(text);
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
            var verifier = new Verifier<Imsi> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Identifier);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Identifier.ToVisibleString();
        }

        #endregion
    }
}
