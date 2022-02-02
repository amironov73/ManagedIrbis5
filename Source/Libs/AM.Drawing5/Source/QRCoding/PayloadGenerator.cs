// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* PayloadGenerator.cs -- генератор полезной нагрузки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Генератор полезной нагрузки.
/// </summary>
public static class PayloadGenerator
{
    /// <summary>
    /// Абстрактная полезная нагрузка.
    /// </summary>
    public abstract class Payload
    {
        #region Properties

        /// <summary>
        /// Версия.
        /// </summary>
        public virtual int Version => -1;

        /// <summary>
        /// Уровень коррекции ошибок.
        /// </summary>
        public virtual QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;

        /// <summary>
        /// Extended Channel Interpretation.
        /// </summary>
        public virtual QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Default;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public abstract override string ToString();

        #endregion
    }

    /// <summary>
    /// Wi-Fi.
    /// </summary>
    public class WiFi
        : Payload
    {
        #region Construction

        /// <summary>
        /// Generates a WiFi payload. Scanned by a QR Code scanner app, the device will connect to the WiFi.
        /// </summary>
        /// <param name="ssid">SSID of the WiFi network</param>
        /// <param name="password">Password of the WiFi network</param>
        /// <param name="authenticationMode">Authentification mode (WEP, WPA, WPA2)</param>
        /// <param name="isHiddenSSID">Set flag, if the WiFi network hides its SSID</param>
        /// <param name="escapeHexStrings">Set flag, if ssid/password is delivered as HEX string. Note: May not be supported on iOS devices.</param>
        public WiFi
            (
                string ssid,
                string password,
                Authentication authenticationMode,
                bool isHiddenSSID = false,
                bool escapeHexStrings = true
            )
        {
            _ssid = EscapeInput (ssid);
            _ssid = escapeHexStrings && isHexStyle (_ssid) ? "\"" + _ssid + "\"" : _ssid;
            _password = EscapeInput (password);
            _password = escapeHexStrings && isHexStyle (_password) ? "\"" + _password + "\"" : _password;
            _authenticationMode = authenticationMode.ToString();
            _isHiddenSsid = isHiddenSSID;
        }

        #endregion

        #region Private members

        private readonly string _ssid, _password, _authenticationMode;
        private readonly bool _isHiddenSsid;

        #endregion

        #region Object members

        /// <inheritdoc cref="Payload.ToString"/>
        public override string ToString()
        {
            return $"WIFI:T:{_authenticationMode};S:{_ssid};P:{_password};{(_isHiddenSsid ? "H:true" : string.Empty)};";
        }

        #endregion

        /// <summary>
        /// Метод аутентификации.
        /// </summary>
        public enum Authentication
        {
            /// <summary>
            /// WEP.
            /// </summary>
            WEP,

            /// <summary>
            /// WPA.
            /// </summary>
            WPA,

            /// <summary>
            /// Без пароля.
            /// </summary>
            NoPassword
        }
    }

    /// <summary>
    /// Электронная почта.
    /// </summary>
    public class Mail
        : Payload
    {
        #region Construction

        /// <summary>
        /// Creates an email payload with subject and message/text
        /// </summary>
        /// <param name="mailReceiver">Receiver's email address</param>
        /// <param name="subject">Subject line of the email</param>
        /// <param name="message">Message content of the email</param>
        /// <param name="encoding">Payload encoding type. Choose dependent on your QR Code scanner app.</param>
        public Mail
            (
                string? mailReceiver = null,
                string? subject = null,
                string? message = null,
                MailEncoding encoding = MailEncoding.MAILTO
            )
        {
            _mailReceiver = mailReceiver;
            _subject = subject;
            _message = message;
            _encoding = encoding;
        }

        #endregion

        #region Private members

        private readonly string? _mailReceiver, _subject, _message;
        private readonly MailEncoding _encoding;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var returnVal = string.Empty;
            switch (_encoding)
            {
                case MailEncoding.MAILTO:
                    var parts = new List<string>();
                    if (!string.IsNullOrEmpty (_subject))
                        parts.Add ("subject=" + Uri.EscapeDataString (_subject));
                    if (!string.IsNullOrEmpty (_message))
                        parts.Add ("body=" + Uri.EscapeDataString (_message));
                    var queryString = parts.Any() ? $"?{string.Join ("&", parts.ToArray())}" : "";
                    returnVal = $"mailto:{_mailReceiver}{queryString}";
                    break;

                case MailEncoding.MATMSG:
                    returnVal = $"MATMSG:TO:{_mailReceiver};SUB:{EscapeInput (_subject)};BODY:{EscapeInput (_message)};;";
                    break;

                case MailEncoding.SMTP:
                    returnVal = $"SMTP:{_mailReceiver}:{EscapeInput (_subject, true)}:{EscapeInput (_message, true)}";
                    break;
            }

            return returnVal;
        }

        #endregion

        /// <summary>
        /// Схема кодирования.
        /// </summary>
        public enum MailEncoding
        {
            /// <summary>
            /// Mailto:
            /// </summary>
            MAILTO,

            /// <summary>
            /// MATMSG.
            /// </summary>
            MATMSG,

            /// <summary>
            /// SMTP.
            /// </summary>
            SMTP
        }
    }

    public class SMS : Payload
    {
        private readonly string number, subject;
        private readonly SMSEncoding encoding;

        /// <summary>
        /// Creates a SMS payload without text
        /// </summary>
        /// <param name="number">Receiver phone number</param>
        /// <param name="encoding">Encoding type</param>
        public SMS (string number, SMSEncoding encoding = SMSEncoding.SMS)
        {
            this.number = number;
            subject = string.Empty;
            this.encoding = encoding;
        }

        /// <summary>
        /// Creates a SMS payload with text (subject)
        /// </summary>
        /// <param name="number">Receiver phone number</param>
        /// <param name="subject">Text of the SMS</param>
        /// <param name="encoding">Encoding type</param>
        public SMS (string number, string subject, SMSEncoding encoding = SMSEncoding.SMS)
        {
            this.number = number;
            this.subject = subject;
            this.encoding = encoding;
        }

        public override string ToString()
        {
            var returnVal = string.Empty;
            switch (encoding)
            {
                case SMSEncoding.SMS:
                    var queryString = string.Empty;
                    if (!string.IsNullOrEmpty (subject))
                        queryString = $"?body={Uri.EscapeDataString (subject)}";
                    returnVal = $"sms:{number}{queryString}";
                    break;
                case SMSEncoding.SMS_iOS:
                    var queryStringiOS = string.Empty;
                    if (!string.IsNullOrEmpty (subject))
                        queryStringiOS = $";body={Uri.EscapeDataString (subject)}";
                    returnVal = $"sms:{number}{queryStringiOS}";
                    break;
                case SMSEncoding.SMSTO:
                    returnVal = $"SMSTO:{number}:{subject}";
                    break;
            }

            return returnVal;
        }

        public enum SMSEncoding
        {
            SMS,
            SMSTO,
            SMS_iOS
        }
    }

    public class MMS : Payload
    {
        private readonly string number, subject;
        private readonly MMSEncoding encoding;

        /// <summary>
        /// Creates a MMS payload without text
        /// </summary>
        /// <param name="number">Receiver phone number</param>
        /// <param name="encoding">Encoding type</param>
        public MMS (string number, MMSEncoding encoding = MMSEncoding.MMS)
        {
            this.number = number;
            subject = string.Empty;
            this.encoding = encoding;
        }

        /// <summary>
        /// Creates a MMS payload with text (subject)
        /// </summary>
        /// <param name="number">Receiver phone number</param>
        /// <param name="subject">Text of the MMS</param>
        /// <param name="encoding">Encoding type</param>
        public MMS (string number, string subject, MMSEncoding encoding = MMSEncoding.MMS)
        {
            this.number = number;
            this.subject = subject;
            this.encoding = encoding;
        }

        public override string ToString()
        {
            var returnVal = string.Empty;
            switch (encoding)
            {
                case MMSEncoding.MMSTO:
                    var queryStringMmsTo = string.Empty;
                    if (!string.IsNullOrEmpty (subject))
                        queryStringMmsTo = $"?subject={Uri.EscapeDataString (subject)}";
                    returnVal = $"mmsto:{number}{queryStringMmsTo}";
                    break;
                case MMSEncoding.MMS:
                    var queryStringMms = string.Empty;
                    if (!string.IsNullOrEmpty (subject))
                        queryStringMms = $"?body={Uri.EscapeDataString (subject)}";
                    returnVal = $"mms:{number}{queryStringMms}";
                    break;
            }

            return returnVal;
        }

        public enum MMSEncoding
        {
            MMS,
            MMSTO
        }
    }

    /// <summary>
    /// Геолокация.
    /// </summary>
    public class Geolocation
        : Payload
    {
        /// <summary>
        /// Generates a geo location payload. Supports raw location (GEO encoding) or Google Maps link (GoogleMaps encoding)
        /// </summary>
        /// <param name="latitude">Latitude with . as splitter</param>
        /// <param name="longitude">Longitude with . as splitter</param>
        /// <param name="encoding">Encoding type - GEO or GoogleMaps</param>
        public Geolocation
            (
                string latitude,
                string longitude,
                GeolocationEncoding encoding = GeolocationEncoding.GEO
            )
        {
            _latitude = latitude.Replace (",", ".");
            _longitude = longitude.Replace (",", ".");
            _encoding = encoding;
        }

        #region Private members

        private readonly string _latitude, _longitude;
        private readonly GeolocationEncoding _encoding;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            switch (_encoding)
            {
                case GeolocationEncoding.GEO:
                    return $"geo:{_latitude},{_longitude}";

                case GeolocationEncoding.GoogleMaps:
                    return $"http://maps.google.com/maps?q={_latitude},{_longitude}";

                default:
                    return "geo:";
            }
        }

        #endregion

        /// <summary>
        /// Схема кодирования геолокации.
        /// </summary>
        public enum GeolocationEncoding
        {
            /// <summary>
            /// GEO.
            /// </summary>
            GEO,

            /// <summary>
            /// Google maps.
            /// </summary>
            GoogleMaps
        }
    }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public class PhoneNumber
        : Payload
    {
        #region Construction

        /// <summary>
        /// Generates a phone call payload
        /// </summary>
        /// <param name="number">Phonenumber of the receiver</param>
        public PhoneNumber
            (
                string number
            )
        {
            Sure.NotNullNorEmpty (number);

            _number = number;
        }

        #endregion

        #region Private members

        private readonly string _number;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"tel:{_number}";
        }

        #endregion
    }

    public class SkypeCall : Payload
    {
        private readonly string skypeUsername;

        /// <summary>
        /// Generates a Skype call payload
        /// </summary>
        /// <param name="skypeUsername">Skype username which will be called</param>
        public SkypeCall (string skypeUsername)
        {
            this.skypeUsername = skypeUsername;
        }

        public override string ToString()
        {
            return $"skype:{skypeUsername}?call";
        }
    }

    /// <summary>
    /// URL.
    /// </summary>
    public class Url
        : Payload
    {
        #region Construction

        /// <summary>
        /// Generates a link. If not given, http/https protocol will be added.
        /// </summary>
        /// <param name="url">Link url target</param>
        public Url
            (
                string url
            )
        {
            Sure.NotNullNorEmpty (url);

            _url = url;
        }

        #endregion

        #region Private members

        private readonly string _url;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return (!_url.StartsWith ("http") ? "http://" + _url : _url);
        }

        #endregion
    }

    /// <summary>
    /// Сообщение WhatsApp.
    /// </summary>
    public class WhatsAppMessage
        : Payload
    {
        #region Construction

        /// <summary>
        /// Let's you compose a WhatApp message and send it the receiver number.
        /// </summary>
        /// <param name="number">Receiver phone number where the <number> is a full phone number in international format.
        /// Omit any zeroes, brackets, or dashes when adding the phone number in international format.
        /// Use: 1XXXXXXXXXX | Don't use: +001-(XXX)XXXXXXX
        /// </param>
        /// <param name="message">The message</param>
        public WhatsAppMessage
            (
                string number,
                string message
            )
        {
            Sure.NotNull (number);
            Sure.NotNullNorEmpty (message);

            _number = number;
            _message = message;
        }

        /// <summary>
        /// Let's you compose a WhatApp message. When scanned the user is asked to choose a contact who will receive the message.
        /// </summary>
        /// <param name="message">The message</param>
        public WhatsAppMessage
            (
                string message
            )
        {
            Sure.NotNullNorEmpty (message);

            _number = string.Empty;
            _message = message;
        }

        #endregion

        #region Private members

        private readonly string _number, _message;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var cleanedPhone = Regex.Replace (_number, @"^[0+]+|[ ()-]", string.Empty);
            return ($"https://wa.me/{cleanedPhone}?text={Uri.EscapeDataString (_message)}");
        }

        #endregion
    }


    public class Bookmark : Payload
    {
        private readonly string url, title;

        /// <summary>
        /// Generates a bookmark payload. Scanned by an QR Code reader, this one creates a browser bookmark.
        /// </summary>
        /// <param name="url">Url of the bookmark</param>
        /// <param name="title">Title of the bookmark</param>
        public Bookmark (string url, string title)
        {
            this.url = EscapeInput (url);
            this.title = EscapeInput (title);
        }

        public override string ToString()
        {
            return $"MEBKM:TITLE:{title};URL:{url};;";
        }
    }

    /// <summary>
    /// Контактные данные.
    /// </summary>
    public class ContactData
        : Payload
    {
        #region Construction

        /// <summary>
        /// Generates a vCard or meCard contact dataset
        /// </summary>
        /// <param name="outputType">Payload output type</param>
        /// <param name="firstname">The firstname</param>
        /// <param name="lastname">The lastname</param>
        /// <param name="nickname">The displayname</param>
        /// <param name="phone">Normal phone number</param>
        /// <param name="mobilePhone">Mobile phone</param>
        /// <param name="workPhone">Office phone number</param>
        /// <param name="email">E-Mail address</param>
        /// <param name="birthday">Birthday</param>
        /// <param name="website">Website / Homepage</param>
        /// <param name="street">Street</param>
        /// <param name="houseNumber">Housenumber</param>
        /// <param name="city">City</param>
        /// <param name="stateRegion">State or Region</param>
        /// <param name="zipCode">Zip code</param>
        /// <param name="country">Country</param>
        /// <param name="addressOrder">The address order format to use</param>
        /// <param name="note">Memo text / notes</param>
        /// <param name="org">Organisation/Company</param>
        /// <param name="orgTitle">Organisation/Company Title</param>
        public ContactData
            (
                ContactOutputType outputType,
                string firstname,
                string lastname,
                string? nickname = null,
                string? phone = null,
                string? mobilePhone = null,
                string? workPhone = null,
                string? email = null,
                DateTime? birthday = null,
                string? website = null,
                string? street = null,
                string? houseNumber = null,
                string? city = null,
                string? zipCode = null,
                string? country = null,
                string? note = null,
                string? stateRegion = null,
                AddressOrder addressOrder = AddressOrder.Default,
                string? org = null,
                string? orgTitle = null
            )
        {
            _firstname = firstname;
            _lastname = lastname;
            _nickname = nickname;
            _org = org;
            _orgTitle = orgTitle;
            _phone = phone;
            _mobilePhone = mobilePhone;
            _workPhone = workPhone;
            _email = email;
            _birthday = birthday;
            _website = website;
            _street = street;
            _houseNumber = houseNumber;
            _city = city;
            _stateRegion = stateRegion;
            _zipCode = zipCode;
            _country = country;
            _addressOrder = addressOrder;
            _note = note;
            _outputType = outputType;
        }

        #endregion

        #region Private members

        private readonly string _firstname;
        private readonly string _lastname;
        private readonly string? _nickname;
        private readonly string? _org;
        private readonly string? _orgTitle;
        private readonly string? _phone;
        private readonly string? _mobilePhone;
        private readonly string? _workPhone;
        private readonly string? _email;
        private readonly DateTime? _birthday;
        private readonly string? _website;
        private readonly string? _street;
        private readonly string? _houseNumber;
        private readonly string? _city;
        private readonly string? _zipCode;
        private readonly string? _stateRegion;
        private readonly string? _country;
        private readonly string? _note;
        private readonly ContactOutputType _outputType;
        private readonly AddressOrder _addressOrder;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var payload = string.Empty;
            if (_outputType == ContactOutputType.MeCard)
            {
                payload += "MECARD+\r\n";
                if (!string.IsNullOrEmpty (_firstname) && !string.IsNullOrEmpty (_lastname))
                {
                    payload += $"N:{_lastname}, {_firstname}\r\n";
                }
                else if (!string.IsNullOrEmpty (_firstname) || !string.IsNullOrEmpty (_lastname))
                {
                    payload += $"N:{_firstname}{_lastname}\r\n";
                }

                if (!string.IsNullOrEmpty (_org))
                {
                    payload += $"ORG:{_org}\r\n";
                }

                if (!string.IsNullOrEmpty (_orgTitle))
                {
                    payload += $"TITLE:{_orgTitle}\r\n";
                }

                if (!string.IsNullOrEmpty (_phone))
                {
                    payload += $"TEL:{_phone}\r\n";
                }

                if (!string.IsNullOrEmpty (_mobilePhone))
                {
                    payload += $"TEL:{_mobilePhone}\r\n";
                }

                if (!string.IsNullOrEmpty (_workPhone))
                {
                    payload += $"TEL:{_workPhone}\r\n";
                }

                if (!string.IsNullOrEmpty (_email))
                {
                    payload += $"EMAIL:{_email}\r\n";
                }

                if (!string.IsNullOrEmpty (_note))
                {
                    payload += $"NOTE:{_note}\r\n";
                }

                if (_birthday != null)
                {
                    payload += $"BDAY:{((DateTime)_birthday).ToString ("yyyyMMdd")}\r\n";
                }

                var addressString = string.Empty;
                if (_addressOrder == AddressOrder.Default)
                {
                    addressString = $"ADR:,,{(!string.IsNullOrEmpty (_street) ? _street + " " : "")}{(!string.IsNullOrEmpty (_houseNumber) ? _houseNumber : "")},{(!string.IsNullOrEmpty (_zipCode) ? _zipCode : "")},{(!string.IsNullOrEmpty (_city) ? _city : "")},{(!string.IsNullOrEmpty (_stateRegion) ? _stateRegion : "")},{(!string.IsNullOrEmpty (_country) ? _country : "")}\r\n";
                }
                else
                {
                    addressString = $"ADR:,,{(!string.IsNullOrEmpty (_houseNumber) ? _houseNumber + " " : "")}{(!string.IsNullOrEmpty (_street) ? _street : "")},{(!string.IsNullOrEmpty (_city) ? _city : "")},{(!string.IsNullOrEmpty (_stateRegion) ? _stateRegion : "")},{(!string.IsNullOrEmpty (_zipCode) ? _zipCode : "")},{(!string.IsNullOrEmpty (_country) ? _country : "")}\r\n";
                }

                payload += addressString;
                if (!string.IsNullOrEmpty (_website))
                {
                    payload += $"URL:{_website}\r\n";
                }

                if (!string.IsNullOrEmpty (_nickname))
                {
                    payload += $"NICKNAME:{_nickname}\r\n";
                }

                payload = payload.Trim (new char[] { '\r', '\n' });
            }
            else
            {
                var version = _outputType.ToString().Substring (5);
                if (version.Length > 1)
                {
                    version = version.Insert (1, ".");
                }
                else
                {
                    version += ".0";
                }

                payload += "BEGIN:VCARD\r\n";
                payload += $"VERSION:{version}\r\n";

                payload += $"N:{(!string.IsNullOrEmpty (_lastname) ? _lastname : "")};{(!string.IsNullOrEmpty (_firstname) ? _firstname : "")};;;\r\n";
                payload += $"FN:{(!string.IsNullOrEmpty (_firstname) ? _firstname + " " : "")}{(!string.IsNullOrEmpty (_lastname) ? _lastname : "")}\r\n";
                if (!string.IsNullOrEmpty (_org))
                {
                    payload += $"ORG:" + _org + "\r\n";
                }

                if (!string.IsNullOrEmpty (_orgTitle))
                {
                    payload += $"TITLE:" + _orgTitle + "\r\n";
                }

                if (!string.IsNullOrEmpty (_phone))
                {
                    payload += $"TEL;";
                    if (_outputType == ContactOutputType.VCard21)
                    {
                        payload += $"HOME;VOICE:{_phone}";
                    }
                    else if (_outputType == ContactOutputType.VCard3)
                    {
                        payload += $"TYPE=HOME,VOICE:{_phone}";
                    }
                    else
                    {
                        payload += $"TYPE=home,voice;VALUE=uri:tel:{_phone}";
                    }

                    payload += "\r\n";
                }

                if (!string.IsNullOrEmpty (_mobilePhone))
                {
                    payload += $"TEL;";
                    if (_outputType == ContactOutputType.VCard21)
                    {
                        payload += $"HOME;CELL:{_mobilePhone}";
                    }
                    else if (_outputType == ContactOutputType.VCard3)
                    {
                        payload += $"TYPE=HOME,CELL:{_mobilePhone}";
                    }
                    else
                    {
                        payload += $"TYPE=home,cell;VALUE=uri:tel:{_mobilePhone}";
                    }

                    payload += "\r\n";
                }

                if (!string.IsNullOrEmpty (_workPhone))
                {
                    payload += $"TEL;";
                    if (_outputType == ContactOutputType.VCard21)
                    {
                        payload += $"WORK;VOICE:{_workPhone}";
                    }
                    else if (_outputType == ContactOutputType.VCard3)
                    {
                        payload += $"TYPE=WORK,VOICE:{_workPhone}";
                    }
                    else
                    {
                        payload += $"TYPE=work,voice;VALUE=uri:tel:{_workPhone}";
                    }

                    payload += "\r\n";
                }


                payload += "ADR;";
                if (_outputType == ContactOutputType.VCard21)
                {
                    payload += "HOME;PREF:";
                }
                else if (_outputType == ContactOutputType.VCard3)
                {
                    payload += "TYPE=HOME,PREF:";
                }
                else
                {
                    payload += "TYPE=home,pref:";
                }

                var addressString = string.Empty;
                if (_addressOrder == AddressOrder.Default)
                {
                    addressString =
                        $";;{(!string.IsNullOrEmpty (_street) ? _street + " " : "")}{(!string.IsNullOrEmpty (_houseNumber) ? _houseNumber : "")};{(!string.IsNullOrEmpty (_zipCode) ? _zipCode : "")};{(!string.IsNullOrEmpty (_city) ? _city : "")};{(!string.IsNullOrEmpty (_stateRegion) ? _stateRegion : "")};{(!string.IsNullOrEmpty (_country) ? _country : "")}\r\n";
                }
                else
                {
                    addressString =
                        $";;{(!string.IsNullOrEmpty (_houseNumber) ? _houseNumber + " " : "")}{(!string.IsNullOrEmpty (_street) ? _street : "")};{(!string.IsNullOrEmpty (_city) ? _city : "")};{(!string.IsNullOrEmpty (_stateRegion) ? _stateRegion : "")};{(!string.IsNullOrEmpty (_zipCode) ? _zipCode : "")};{(!string.IsNullOrEmpty (_country) ? _country : "")}\r\n";
                }

                payload += addressString;

                if (_birthday != null)
                {
                    payload += $"BDAY:{((DateTime)_birthday).ToString ("yyyyMMdd")}\r\n";
                }

                if (!string.IsNullOrEmpty (_website))
                {
                    payload += $"URL:{_website}\r\n";
                }

                if (!string.IsNullOrEmpty (_email))
                {
                    payload += $"EMAIL:{_email}\r\n";
                }

                if (!string.IsNullOrEmpty (_note))
                {
                    payload += $"NOTE:{_note}\r\n";
                }

                if (_outputType != ContactOutputType.VCard21 && !string.IsNullOrEmpty (_nickname))
                {
                    payload += $"NICKNAME:{_nickname}\r\n";
                }

                payload += "END:VCARD";
            }

            return payload;
        }

        #endregion

        /// <summary>
        /// Possible output types. Either vCard 2.1, vCard 3.0, vCard 4.0 or MeCard.
        /// </summary>
        public enum ContactOutputType
        {
            /// <summary>
            /// MeCard.
            /// </summary>
            MeCard,

            /// <summary>
            /// VCard 2.1.
            /// </summary>
            VCard21,

            /// <summary>
            /// VCard 3.
            /// </summary>
            VCard3,

            /// <summary>
            /// VCard 4.
            /// </summary>
            VCard4
        }


        /// <summary>
        /// define the address format
        /// Default: European format, ([Street] [House Number] and [Postal Code] [City]
        /// Reversed: North American and others format ([House Number] [Street] and [City] [Postal Code])
        /// </summary>
        public enum AddressOrder
        {
            Default,
            Reversed
        }
    }

    /// <summary>
    /// Адрес криптовалюты.
    /// </summary>
    public class BitcoinLikeCryptoCurrencyAddress
        : Payload
    {
        #region Construction

        /// <summary>
        /// Generates a Bitcoin like cryptocurrency payment payload. QR Codes with this payload can open a payment app.
        /// </summary>
        /// <param name="currencyType">Bitcoin like cryptocurrency address of the payment receiver</param>
        /// <param name="address">Bitcoin like cryptocurrency address of the payment receiver</param>
        /// <param name="amount">Amount of coins to transfer</param>
        /// <param name="label">Reference label</param>
        /// <param name="message">Referece text aka message</param>
        public BitcoinLikeCryptoCurrencyAddress
            (
                BitcoinLikeCryptoCurrencyType currencyType,
                string address,
                double? amount,
                string? label = null,
                string? message = null
            )
        {
            _currencyType = currencyType;
            _address = address;

            if (!string.IsNullOrEmpty (label))
            {
                _label = Uri.EscapeDataString (label);
            }

            if (!string.IsNullOrEmpty (message))
            {
                _message = Uri.EscapeDataString (message);
            }

            _amount = amount;
        }

        #endregion

        #region Private members

        private readonly BitcoinLikeCryptoCurrencyType _currencyType;
        private readonly string? _address, _label, _message;
        private readonly double? _amount;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            string? query = null;

            var queryValues = new KeyValuePair<string, string?>[]
            {
                new (nameof (_label), _label),
                new (nameof (_message), _message),
                new (nameof (_amount),
                    _amount.HasValue ? _amount.Value.ToString ("#.########", CultureInfo.InvariantCulture) : null)
            };

            if (queryValues.Any (keyPair => !string.IsNullOrEmpty (keyPair.Value)))
            {
                query = "?" + string.Join ("&", queryValues
                    .Where (keyPair => !string.IsNullOrEmpty (keyPair.Value))
                    .Select (keyPair => $"{keyPair.Key}={keyPair.Value}")
                    .ToArray());
            }

            return $"{Enum.GetName (typeof (BitcoinLikeCryptoCurrencyType), _currencyType).ToLower()}:{_address}{query}";
        }

        #endregion

        /// <summary>
        /// Криптовалюты.
        /// </summary>
        public enum BitcoinLikeCryptoCurrencyType
        {
            /// <summary>
            /// Bitcoin.
            /// </summary>
            Bitcoin,

            /// <summary>
            /// BitcoinCash.
            /// </summary>
            BitcoinCash,

            /// <summary>
            /// Litecoin.
            /// </summary>
            Litecoin
        }
    }

    /// <summary>
    /// Адрес Bitcoin.
    /// </summary>
    public class BitcoinAddress
        : BitcoinLikeCryptoCurrencyAddress
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BitcoinAddress
            (
                string address,
                double? amount,
                string? label = null,
                string? message = null
            )
            : base (BitcoinLikeCryptoCurrencyType.Bitcoin, address, amount, label, message)
        {
        }

        #endregion
    }

    public class BitcoinCashAddress : BitcoinLikeCryptoCurrencyAddress
    {
        public BitcoinCashAddress (string address, double? amount, string label = null, string message = null)
            : base (BitcoinLikeCryptoCurrencyType.BitcoinCash, address, amount, label, message)
        {
        }
    }

    public class LitecoinAddress : BitcoinLikeCryptoCurrencyAddress
    {
        public LitecoinAddress (string address, double? amount, string label = null, string message = null)
            : base (BitcoinLikeCryptoCurrencyType.Litecoin, address, amount, label, message)
        {
        }
    }

    public class SwissQrCode : Payload
    {
        //Keep in mind, that the ECC level has to be set to "M" when generating a SwissQrCode!
        //SwissQrCode specification:
        //    - (de) https://www.paymentstandards.ch/dam/downloads/ig-qr-bill-de.pdf
        //    - (en) https://www.paymentstandards.ch/dam/downloads/ig-qr-bill-en.pdf
        //Changes between version 1.0 and 2.0: https://www.paymentstandards.ch/dam/downloads/change-documentation-qrr-de.pdf

        #region Construction

        /// <summary>
        /// Generates the payload for a SwissQrCode v2.0. (Don't forget to use ECC-Level=M, EncodingMode=UTF-8 and to set the Swiss flag icon to the final QR code.)
        /// </summary>
        /// <param name="iban">IBAN object</param>
        /// <param name="currency">Currency (either EUR or CHF)</param>
        /// <param name="creditor">Creditor (payee) information</param>
        /// <param name="reference">Reference information</param>
        /// <param name="additionalInformation"></param>
        /// <param name="debitor">Debitor (payer) information</param>
        /// <param name="amount">Amount</param>
        /// <param name="requestedDateOfPayment">Requested date of debitor's payment</param>
        /// <param name="ultimateCreditor">Ultimate creditor information (use only in consultation with your bank - for future use only!)</param>
        /// <param name="alternativeProcedure1">Optional command for alternative processing mode - line 1</param>
        /// <param name="alternativeProcedure2">Optional command for alternative processing mode - line 2</param>
        public SwissQrCode
            (
                Iban iban,
                Currency currency,
                Contact creditor, Reference reference,
                AdditionalInformation? additionalInformation = null,
                Contact? debitor = null,
                decimal? amount = null,
                DateTime? requestedDateOfPayment = null,
                Contact? ultimateCreditor = null,
                string? alternativeProcedure1 = null,
                string? alternativeProcedure2 = null
            )
        {
            _iban = iban;

            _creditor = creditor;
            _ultimateCreditor = ultimateCreditor;

            _additionalInformation =
                additionalInformation != null ? additionalInformation : new AdditionalInformation();

            if (amount != null && amount.ToString().Length > 12)
                throw new SwissQrCodeException ("Amount (including decimals) must be shorter than 13 places.");
            _amount = amount;

            _currency = currency;
            _requestedDateOfPayment = requestedDateOfPayment;
            _debitor = debitor;

            if (iban.IsQrIban && reference.RefType != Reference.ReferenceType.QRR)
            {
                throw new SwissQrCodeException
                    (
                        "If QR-IBAN is used, you have to choose \"QRR\" as reference type!"
                    );
            }

            if (!iban.IsQrIban && reference.RefType == Reference.ReferenceType.QRR)
            {
                throw new SwissQrCodeException
                    (
                        "If non QR-IBAN is used, you have to choose either \"SCOR\" or \"NON\" as reference type!"
                    );
            }

            _reference = reference;

            if (alternativeProcedure1 != null && alternativeProcedure1.Length > 100)
            {
                throw new SwissQrCodeException
                    (
                        "Alternative procedure information block 1 must be shorter than 101 chars."
                    );
            }

            _alternativeProcedure1 = alternativeProcedure1;
            if (alternativeProcedure2 != null && alternativeProcedure2.Length > 100)
            {
                throw new SwissQrCodeException
                    (
                        "Alternative procedure information block 2 must be shorter than 101 chars."
                    );
            }

            _alternativeProcedure2 = alternativeProcedure2;
        }

        #endregion

        #region Private members

        private readonly string _br = "\r\n";
        private readonly string _alternativeProcedure1, _alternativeProcedure2;
        private readonly Iban _iban;
        private readonly decimal? _amount;
        private readonly Contact _creditor, _ultimateCreditor, _debitor;
        private readonly Currency _currency;
        private readonly DateTime? _requestedDateOfPayment;
        private readonly Reference _reference;
        private readonly AdditionalInformation _additionalInformation;

        #endregion

        /// <summary>
        /// Дополнительная информация.
        /// </summary>
        public class AdditionalInformation
        {
            #region Properties

            public string UnstructureMessage => !string.IsNullOrEmpty (_unstructuredMessage) ? _unstructuredMessage.Replace ("\n", "") : null;

            public string BillInformation => !string.IsNullOrEmpty (_billInformation) ? _billInformation.Replace ("\n", "") : null;

            public string Trailer => _trailer;

            #endregion

            #region Construction

            /// <summary>
            /// Creates an additional information object. Both parameters are optional and must be shorter than 141 chars in combination.
            /// </summary>
            /// <param name="unstructuredMessage">Unstructured text message</param>
            /// <param name="billInformation">Bill information</param>
            public AdditionalInformation
                (
                    string? unstructuredMessage = null,
                    string? billInformation = null
                )
            {
                if (((unstructuredMessage != null ? unstructuredMessage.Length : 0) +
                     (billInformation != null ? billInformation.Length : 0)) > 140)
                {
                    throw new SwissQrCodeAdditionalInformationException (
                        "Unstructured message and bill information must be shorter than 141 chars in total/combined.");
                }

                _unstructuredMessage = unstructuredMessage;
                _billInformation = billInformation;
                _trailer = "EPD";
            }

            #endregion

            #region Private members

            private readonly string? _unstructuredMessage, _billInformation, _trailer;

            #endregion

            /// <summary>
            /// Специфичное исключение.
            /// </summary>
            public class SwissQrCodeAdditionalInformationException
                : Exception
            {
                #region Construction

                /// <summary>
                /// Конструктор по умолчанию.
                /// </summary>
                public SwissQrCodeAdditionalInformationException()
                {
                }

                /// <summary>
                /// Конструктор.
                /// </summary>
                public SwissQrCodeAdditionalInformationException
                    (
                        string message
                    )
                    : base (message)
                {
                }

                /// <summary>
                /// Конструктор.
                /// </summary>
                public SwissQrCodeAdditionalInformationException
                    (
                        string message,
                        Exception inner
                    )
                    : base (message, inner)
                {
                }

                #endregion
            }
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public class Reference
        {
            /// <summary>
            /// Creates a reference object which must be passed to the SwissQrCode instance
            /// </summary>
            /// <param name="referenceType">Type of the reference (QRR, SCOR or NON)</param>
            /// <param name="reference">Reference text</param>
            /// <param name="referenceTextType">Type of the reference text (QR-reference or Creditor Reference)</param>
            public Reference
                (
                    ReferenceType referenceType,
                    string? reference = null,
                    ReferenceTextType? referenceTextType = null
                )
            {
                _referenceType = referenceType;
                _referenceTextType = referenceTextType;

                if (referenceType == ReferenceType.NON && reference != null)
                {
                    throw new SwissQrCodeReferenceException (
                        "Reference is only allowed when referenceType not equals \"NON\"");
                }

                if (referenceType != ReferenceType.NON && reference != null && referenceTextType == null)
                {
                    throw new SwissQrCodeReferenceException
                        (
                            "You have to set an ReferenceTextType when using the reference text."
                        );
                }

                if (referenceTextType == ReferenceTextType.QrReference && reference != null && (reference.Length > 27))
                {
                    throw new SwissQrCodeReferenceException ("QR-references have to be shorter than 28 chars.");
                }

                if (referenceTextType == ReferenceTextType.QrReference && reference != null &&
                    !Regex.IsMatch (reference, @"^[0-9]+$"))
                {
                    throw new SwissQrCodeReferenceException ("QR-reference must exist out of digits only.");
                }

                if (referenceTextType == ReferenceTextType.QrReference && reference != null &&
                    !ChecksumMod10 (reference))
                {
                    throw new SwissQrCodeReferenceException ("QR-references is invalid. Checksum error.");
                }

                if (referenceTextType == ReferenceTextType.CreditorReferenceIso11649 && reference != null &&
                    (reference.Length > 25))
                {
                    throw new SwissQrCodeReferenceException
                        (
                            "Creditor references (ISO 11649) have to be shorter than 26 chars."
                        );
                }

                _reference = reference;
            }

            #region Private members

            private readonly ReferenceType _referenceType;
            private readonly string? _reference;
            private readonly ReferenceTextType? _referenceTextType;

            #endregion

            public ReferenceType RefType => _referenceType;

            public string ReferenceText => !string.IsNullOrEmpty (_reference) ? _reference.Replace ("\n", "") : null;

            /// <summary>
            /// Reference type. When using a QR-IBAN you have to use either "QRR" or "SCOR"
            /// </summary>
            public enum ReferenceType
            {
                QRR,
                SCOR,
                NON
            }

            public enum ReferenceTextType
            {
                QrReference,
                CreditorReferenceIso11649
            }

            /// <summary>
            /// Специфичное исключение.
            /// </summary>
            public class SwissQrCodeReferenceException
                : Exception
            {
                #region Construction

                /// <summary>
                /// Конструктор по умолчанию.
                /// </summary>
                public SwissQrCodeReferenceException()
                {
                }

                /// <summary>
                /// Конструктор.
                /// </summary>
                public SwissQrCodeReferenceException
                    (
                        string message
                    )
                    : base (message)
                {
                }

                /// <summary>
                /// Конструктор.
                /// </summary>
                public SwissQrCodeReferenceException
                    (
                        string message,
                        Exception inner
                    )
                    : base (message, inner)
                {
                }

                #endregion
            }
        }

        public class Iban
        {
            private string iban;
            private IbanType ibanType;

            /// <summary>
            /// IBAN object with type information
            /// </summary>
            /// <param name="iban">IBAN</param>
            /// <param name="ibanType">Type of IBAN (normal or QR-IBAN)</param>
            public Iban (string iban, IbanType ibanType)
            {
                if (ibanType == IbanType.Iban && !IsValidIban (iban))
                    throw new SwissQrCodeIbanException ("The IBAN entered isn't valid.");
                if (ibanType == IbanType.QrIban && !IsValidQRIban (iban))
                    throw new SwissQrCodeIbanException ("The QR-IBAN entered isn't valid.");
                if (!iban.StartsWith ("CH") && !iban.StartsWith ("LI"))
                    throw new SwissQrCodeIbanException ("The IBAN must start with \"CH\" or \"LI\".");
                this.iban = iban;
                this.ibanType = ibanType;
            }

            public bool IsQrIban => ibanType == IbanType.QrIban;

            public override string ToString()
            {
                return iban.Replace ("-", "").Replace ("\n", "").Replace (" ", "");
            }

            public enum IbanType
            {
                Iban,
                QrIban
            }

            /// <summary>
            /// Специфичное исключение.
            /// </summary>
            public class SwissQrCodeIbanException
                : Exception
            {
                #region Construction

                /// <summary>
                /// Конструктор по умолчанию.
                /// </summary>
                public SwissQrCodeIbanException()
                {
                }

                /// <summary>
                /// Конструктор.
                /// </summary>
                public SwissQrCodeIbanException
                    (
                        string message
                    )
                    : base (message)
                {
                }

                /// <summary>
                /// Конструктор.
                /// </summary>
                public SwissQrCodeIbanException
                    (
                        string message,
                        Exception inner
                    )
                    : base (message, inner)
                {
                }

                #endregion
            }
        }

        /// <summary>
        /// Контакт.
        /// </summary>
        public class Contact
        {
            private static readonly HashSet<string> twoLetterCodes = ValidTwoLetterCodes();
            private string br = "\r\n";
            private string name, streetOrAddressline1, houseNumberOrAddressline2, zipCode, city, country;
            private AddressType adrType;

            /// <summary>
            /// Contact type. Can be used for payee, ultimate payee, etc. with address in structured mode (S).
            /// </summary>
            /// <param name="name">Last name or company (optional first name)</param>
            /// <param name="zipCode">Zip-/Postcode</param>
            /// <param name="city">City name</param>
            /// <param name="country">Two-letter country code as defined in ISO 3166-1</param>
            /// <param name="street">Streetname without house number</param>
            /// <param name="houseNumber">House number</param>
            [Obsolete ("This constructor is deprecated. Use WithStructuredAddress instead.")]
            public Contact (string name, string zipCode, string city, string country, string street = null,
                string houseNumber = null) : this (name, zipCode, city, country, street, houseNumber,
                AddressType.StructuredAddress)
            {
            }


            /// <summary>
            /// Contact type. Can be used for payee, ultimate payee, etc. with address in combined mode (K).
            /// </summary>
            /// <param name="name">Last name or company (optional first name)</param>
            /// <param name="country">Two-letter country code as defined in ISO 3166-1</param>
            /// <param name="addressLine1">Adress line 1</param>
            /// <param name="addressLine2">Adress line 2</param>
            [Obsolete ("This constructor is deprecated. Use WithCombinedAddress instead.")]
            public Contact (string name, string country, string addressLine1, string addressLine2) : this (name, null,
                null, country, addressLine1, addressLine2, AddressType.CombinedAddress)
            {
            }

            public static Contact WithStructuredAddress (string name, string zipCode, string city, string country,
                string street = null, string houseNumber = null)
            {
                return new Contact (name, zipCode, city, country, street, houseNumber, AddressType.StructuredAddress);
            }

            public static Contact WithCombinedAddress (string name, string country, string addressLine1,
                string addressLine2)
            {
                return new Contact (name, null, null, country, addressLine1, addressLine2, AddressType.CombinedAddress);
            }


            private Contact (string name, string zipCode, string city, string country, string streetOrAddressline1,
                string houseNumberOrAddressline2, AddressType addressType)
            {
                //Pattern extracted from https://qr-validation.iso-payments.ch as explained in https://github.com/codebude/QRCoder/issues/97
                var charsetPattern =
                    @"^([a-zA-Z0-9\.,;:'\ \+\-/\(\)?\*\[\]\{\}\\`´~ ]|[!""#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*$";

                adrType = addressType;

                if (string.IsNullOrEmpty (name))
                    throw new SwissQrCodeContactException ("Name must not be empty.");
                if (name.Length > 70)
                    throw new SwissQrCodeContactException ("Name must be shorter than 71 chars.");
                if (!Regex.IsMatch (name, charsetPattern))
                    throw new SwissQrCodeContactException (
                        $"Name must match the following pattern as defined in pain.001: {charsetPattern}");
                this.name = name;

                if (AddressType.StructuredAddress == adrType)
                {
                    if (!string.IsNullOrEmpty (streetOrAddressline1) && (streetOrAddressline1.Length > 70))
                        throw new SwissQrCodeContactException ("Street must be shorter than 71 chars.");
                    if (!string.IsNullOrEmpty (streetOrAddressline1) &&
                        !Regex.IsMatch (streetOrAddressline1, charsetPattern))
                        throw new SwissQrCodeContactException (
                            $"Street must match the following pattern as defined in pain.001: {charsetPattern}");
                    this.streetOrAddressline1 = streetOrAddressline1;

                    if (!string.IsNullOrEmpty (houseNumberOrAddressline2) && houseNumberOrAddressline2.Length > 16)
                        throw new SwissQrCodeContactException ("House number must be shorter than 17 chars.");
                    this.houseNumberOrAddressline2 = houseNumberOrAddressline2;
                }
                else
                {
                    if (!string.IsNullOrEmpty (streetOrAddressline1) && (streetOrAddressline1.Length > 70))
                        throw new SwissQrCodeContactException ("Address line 1 must be shorter than 71 chars.");
                    if (!string.IsNullOrEmpty (streetOrAddressline1) &&
                        !Regex.IsMatch (streetOrAddressline1, charsetPattern))
                        throw new SwissQrCodeContactException (
                            $"Address line 1 must match the following pattern as defined in pain.001: {charsetPattern}");
                    this.streetOrAddressline1 = streetOrAddressline1;

                    if (string.IsNullOrEmpty (houseNumberOrAddressline2))
                        throw new SwissQrCodeContactException (
                            "Address line 2 must be provided for combined addresses (address line-based addresses).");
                    if (!string.IsNullOrEmpty (houseNumberOrAddressline2) && (houseNumberOrAddressline2.Length > 70))
                        throw new SwissQrCodeContactException ("Address line 2 must be shorter than 71 chars.");
                    if (!string.IsNullOrEmpty (houseNumberOrAddressline2) &&
                        !Regex.IsMatch (houseNumberOrAddressline2, charsetPattern))
                        throw new SwissQrCodeContactException (
                            $"Address line 2 must match the following pattern as defined in pain.001: {charsetPattern}");
                    this.houseNumberOrAddressline2 = houseNumberOrAddressline2;
                }

                if (AddressType.StructuredAddress == adrType)
                {
                    if (string.IsNullOrEmpty (zipCode))
                        throw new SwissQrCodeContactException ("Zip code must not be empty.");
                    if (zipCode.Length > 16)
                        throw new SwissQrCodeContactException ("Zip code must be shorter than 17 chars.");
                    if (!Regex.IsMatch (zipCode, charsetPattern))
                        throw new SwissQrCodeContactException (
                            $"Zip code must match the following pattern as defined in pain.001: {charsetPattern}");
                    this.zipCode = zipCode;

                    if (string.IsNullOrEmpty (city))
                        throw new SwissQrCodeContactException ("City must not be empty.");
                    if (city.Length > 35)
                        throw new SwissQrCodeContactException ("City name must be shorter than 36 chars.");
                    if (!Regex.IsMatch (city, charsetPattern))
                        throw new SwissQrCodeContactException (
                            $"City name must match the following pattern as defined in pain.001: {charsetPattern}");
                    this.city = city;
                }
                else
                {
                    this.zipCode = this.city = string.Empty;
                }

                if (!IsValidTwoLetterCode (country))
                    throw new SwissQrCodeContactException (
                        "Country must be a valid \"two letter\" country code as defined by  ISO 3166-1, but it isn't.");

                this.country = country;
            }

            private static bool IsValidTwoLetterCode (string code) => twoLetterCodes.Contains (code);

            private static HashSet<string> ValidTwoLetterCodes()
            {
                string[] codes = new string[]
                {
                    "AF", "AL", "DZ", "AS", "AD", "AO", "AI", "AQ", "AG", "AR", "AM", "AW", "AU", "AT", "AZ", "BS",
                    "BH", "BD", "BB", "BY", "BE", "BZ", "BJ", "BM", "BT", "BO", "BQ", "BA", "BW", "BV", "BR", "IO",
                    "BN", "BG", "BF", "BI", "CV", "KH", "CM", "CA", "KY", "CF", "TD", "CL", "CN", "CX", "CC", "CO",
                    "KM", "CG", "CD", "CK", "CR", "CI", "HR", "CU", "CW", "CY", "CZ", "DK", "DJ", "DM", "DO", "EC",
                    "EG", "SV", "GQ", "ER", "EE", "SZ", "ET", "FK", "FO", "FJ", "FI", "FR", "GF", "PF", "TF", "GA",
                    "GM", "GE", "DE", "GH", "GI", "GR", "GL", "GD", "GP", "GU", "GT", "GG", "GN", "GW", "GY", "HT",
                    "HM", "VA", "HN", "HK", "HU", "IS", "IN", "ID", "IR", "IQ", "IE", "IM", "IL", "IT", "JM", "JP",
                    "JE", "JO", "KZ", "KE", "KI", "KP", "KR", "KW", "KG", "LA", "LV", "LB", "LS", "LR", "LY", "LI",
                    "LT", "LU", "MO", "MG", "MW", "MY", "MV", "ML", "MT", "MH", "MQ", "MR", "MU", "YT", "MX", "FM",
                    "MD", "MC", "MN", "ME", "MS", "MA", "MZ", "MM", "NA", "NR", "NP", "NL", "NC", "NZ", "NI", "NE",
                    "NG", "NU", "NF", "MP", "MK", "NO", "OM", "PK", "PW", "PS", "PA", "PG", "PY", "PE", "PH", "PN",
                    "PL", "PT", "PR", "QA", "RE", "RO", "RU", "RW", "BL", "SH", "KN", "LC", "MF", "PM", "VC", "WS",
                    "SM", "ST", "SA", "SN", "RS", "SC", "SL", "SG", "SX", "SK", "SI", "SB", "SO", "ZA", "GS", "SS",
                    "ES", "LK", "SD", "SR", "SJ", "SE", "CH", "SY", "TW", "TJ", "TZ", "TH", "TL", "TG", "TK", "TO",
                    "TT", "TN", "TR", "TM", "TC", "TV", "UG", "UA", "AE", "GB", "US", "UM", "UY", "UZ", "VU", "VE",
                    "VN", "VG", "VI", "WF", "EH", "YE", "ZM", "ZW", "AX"
                };
                return new HashSet<string> (codes, StringComparer.OrdinalIgnoreCase);
            }

            public override string ToString()
            {
                string contactData = $"{(AddressType.StructuredAddress == adrType ? "S" : "K")}{br}"; //AdrTp
                contactData += name.Replace ("\n", "") + br; //Name
                contactData += (!string.IsNullOrEmpty (streetOrAddressline1)
                    ? streetOrAddressline1.Replace ("\n", "")
                    : string.Empty) + br; //StrtNmOrAdrLine1
                contactData += (!string.IsNullOrEmpty (houseNumberOrAddressline2)
                    ? houseNumberOrAddressline2.Replace ("\n", "")
                    : string.Empty) + br; //BldgNbOrAdrLine2
                contactData += zipCode.Replace ("\n", "") + br; //PstCd
                contactData += city.Replace ("\n", "") + br; //TwnNm
                contactData += country + br; //Ctry
                return contactData;
            }

            public enum AddressType
            {
                StructuredAddress,
                CombinedAddress
            }

            public class SwissQrCodeContactException : Exception
            {
                public SwissQrCodeContactException()
                {
                }

                public SwissQrCodeContactException (string message)
                    : base (message)
                {
                }

                public SwissQrCodeContactException (string message, Exception inner)
                    : base (message, inner)
                {
                }
            }
        }

        public override string ToString()
        {
            //Header "logical" element
            var SwissQrCodePayload = "SPC" + _br; //QRType
            SwissQrCodePayload += "0200" + _br; //Version
            SwissQrCodePayload += "1" + _br; //Coding

            //CdtrInf "logical" element
            SwissQrCodePayload += _iban.ToString() + _br; //IBAN


            //Cdtr "logical" element
            SwissQrCodePayload += _creditor.ToString();

            //UltmtCdtr "logical" element
            //Since version 2.0 ultimate creditor was marked as "for future use" and has to be delivered empty in any case!
            SwissQrCodePayload += string.Concat (Enumerable.Repeat (_br, 7).ToArray());

            //CcyAmtDate "logical" element
            //Amoutn has to use . as decimal seperator in any case. See https://www.paymentstandards.ch/dam/downloads/ig-qr-bill-en.pdf page 27.
            SwissQrCodePayload += (_amount != null ? $"{_amount:0.00}".Replace (",", ".") : string.Empty) + _br; //Amt
            SwissQrCodePayload += _currency + _br; //Ccy

            //Removed in S-QR version 2.0
            //SwissQrCodePayload += (requestedDateOfPayment != null ?  ((DateTime)requestedDateOfPayment).ToString("yyyy-MM-dd") : string.Empty) + br; //ReqdExctnDt

            //UltmtDbtr "logical" element
            if (_debitor != null)
                SwissQrCodePayload += _debitor.ToString();
            else
                SwissQrCodePayload += string.Concat (Enumerable.Repeat (_br, 7).ToArray());


            //RmtInf "logical" element
            SwissQrCodePayload += _reference.RefType.ToString() + _br; //Tp
            SwissQrCodePayload +=
                (!string.IsNullOrEmpty (_reference.ReferenceText) ? _reference.ReferenceText : string.Empty) + _br; //Ref


            //AddInf "logical" element
            SwissQrCodePayload += (!string.IsNullOrEmpty (_additionalInformation.UnstructureMessage)
                ? _additionalInformation.UnstructureMessage
                : string.Empty) + _br; //Ustrd
            SwissQrCodePayload += _additionalInformation.Trailer + _br; //Trailer
            SwissQrCodePayload += (!string.IsNullOrEmpty (_additionalInformation.BillInformation)
                ? _additionalInformation.BillInformation
                : string.Empty) + _br; //StrdBkgInf

            //AltPmtInf "logical" element
            if (!string.IsNullOrEmpty (_alternativeProcedure1))
                SwissQrCodePayload += _alternativeProcedure1.Replace ("\n", "") + _br; //AltPmt
            if (!string.IsNullOrEmpty (_alternativeProcedure2))
                SwissQrCodePayload += _alternativeProcedure2.Replace ("\n", "") + _br; //AltPmt

            //S-QR specification 2.0, chapter 4.2.3
            if (SwissQrCodePayload.EndsWith (_br))
                SwissQrCodePayload = SwissQrCodePayload.Remove (SwissQrCodePayload.Length - _br.Length);

            return SwissQrCodePayload;
        }


        /// <summary>
        /// ISO 4217 currency codes
        /// </summary>
        public enum Currency
        {
            CHF = 756,
            EUR = 978
        }

        public class SwissQrCodeException : Exception
        {
            public SwissQrCodeException()
            {
            }

            public SwissQrCodeException (string message)
                : base (message)
            {
            }

            public SwissQrCodeException (string message, Exception inner)
                : base (message, inner)
            {
            }
        }
    }

    public class Girocode
        : Payload
    {
        #region Construction

        /// <summary>
        /// Generates the payload for a Girocode (QR-Code with credit transfer information).
        /// Attention: When using Girocode payload, QR code must be generated with ECC level M!
        /// </summary>
        /// <param name="iban">Account number of the Beneficiary. Only IBAN is allowed.</param>
        /// <param name="bic">BIC of the Beneficiary Bank.</param>
        /// <param name="name">Name of the Beneficiary.</param>
        /// <param name="amount">Amount of the Credit Transfer in Euro.
        /// (Amount must be more than 0.01 and less than 999999999.99)</param>
        /// <param name="remittanceInformation">Remittance Information (Purpose-/reference text). (optional)</param>
        /// <param name="typeOfRemittance">Type of remittance information. Either structured (e.g. ISO 11649 RF Creditor Reference) and max. 35 chars or unstructured and max. 140 chars.</param>
        /// <param name="purposeOfCreditTransfer">Purpose of the Credit Transfer (optional)</param>
        /// <param name="messageToGirocodeUser">Beneficiary to originator information. (optional)</param>
        /// <param name="version">Girocode version. Either 001 or 002. Default: 001.</param>
        /// <param name="encoding">Encoding of the Girocode payload. Default: ISO-8859-1</param>
        public Girocode
            (
                string iban,
                string bic,
                string name,
                decimal amount,
                string remittanceInformation = "",
                TypeOfRemittance typeOfRemittance = TypeOfRemittance.Unstructured,
                string purposeOfCreditTransfer = "",
                string messageToGirocodeUser = "",
                GirocodeVersion version = GirocodeVersion.Version1,
                GirocodeEncoding encoding = GirocodeEncoding.ISO_8859_1
            )
        {
            _version = version;
            _encoding = encoding;
            if (!IsValidIban (iban))
            {
                throw new GirocodeException ("The IBAN entered isn't valid.");
            }

            _iban = iban.Replace (" ", "").ToUpper();
            if (!IsValidBic (bic))
            {
                throw new GirocodeException ("The BIC entered isn't valid.");
            }

            _bic = bic.Replace (" ", "").ToUpper();
            if (name.Length > 70)
            {
                throw new GirocodeException ("(Payee-)Name must be shorter than 71 chars.");
            }

            _name = name;
            if (amount.ToString().Replace (",", ".").Contains (".") &&
                amount.ToString().Replace (",", ".").Split ('.')[1].TrimEnd ('0').Length > 2)
            {
                throw new GirocodeException ("Amount must have less than 3 digits after decimal point.");
            }

            if (amount < 0.01m || amount > 999999999.99m)
            {
                throw new GirocodeException (
                    "Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
            }

            _amount = amount;
            if (purposeOfCreditTransfer.Length > 4)
            {
                throw new GirocodeException ("Purpose of credit transfer can only have 4 chars at maximum.");
            }

            _purposeOfCreditTransfer = purposeOfCreditTransfer;
            if (typeOfRemittance == TypeOfRemittance.Unstructured && remittanceInformation.Length > 140)
            {
                throw new GirocodeException ("Unstructured reference texts have to shorter than 141 chars.");
            }

            if (typeOfRemittance == TypeOfRemittance.Structured && remittanceInformation.Length > 35)
            {
                throw new GirocodeException ("Structured reference texts have to shorter than 36 chars.");
            }

            _typeOfRemittance = typeOfRemittance;
            _remittanceInformation = remittanceInformation;
            if (messageToGirocodeUser.Length > 70)
            {
                throw new GirocodeException (
                    "Message to the Girocode-User reader texts have to shorter than 71 chars.");
            }

            _messageToGirocodeUser = messageToGirocodeUser;
        }

        #endregion

        #region Private members

        //Keep in mind, that the ECC level has to be set to "M" when generating a Girocode!
        //Girocode specification: http://www.europeanpaymentscouncil.eu/index.cfm/knowledge-bank/epc-documents/quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer/epc069-12-quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer1/

        private string br = "\n";
        private readonly string _iban, _bic, _name, _purposeOfCreditTransfer, _remittanceInformation, _messageToGirocodeUser;
        private readonly decimal _amount;
        private readonly GirocodeVersion _version;
        private readonly GirocodeEncoding _encoding;
        private readonly TypeOfRemittance _typeOfRemittance;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var girocodePayload = "BCD" + br;
            girocodePayload += ((_version == GirocodeVersion.Version1) ? "001" : "002") + br;
            girocodePayload += (int)_encoding + 1 + br;
            girocodePayload += "SCT" + br;
            girocodePayload += _bic + br;
            girocodePayload += _name + br;
            girocodePayload += _iban + br;
            girocodePayload += $"EUR{_amount:0.00}".Replace (",", ".") + br;
            girocodePayload += _purposeOfCreditTransfer + br;
            girocodePayload += ((_typeOfRemittance == TypeOfRemittance.Structured)
                ? _remittanceInformation
                : string.Empty) + br;
            girocodePayload += ((_typeOfRemittance == TypeOfRemittance.Unstructured)
                ? _remittanceInformation
                : string.Empty) + br;
            girocodePayload += _messageToGirocodeUser;

            return ConvertStringToEncoding (girocodePayload, _encoding.ToString().Replace ("_", "-"));
        }

        #endregion

        public enum GirocodeVersion
        {
            Version1,
            Version2
        }

        public enum TypeOfRemittance
        {
            Structured,
            Unstructured
        }

        public enum GirocodeEncoding
        {
            UTF_8,
            ISO_8859_1,
            ISO_8859_2,
            ISO_8859_4,
            ISO_8859_5,
            ISO_8859_7,
            ISO_8859_10,
            ISO_8859_15
        }

        /// <summary>
        /// Исключение, специфичное дле Girocode.
        /// </summary>
        public class GirocodeException
            : Exception
        {
            #region Construction

            /// <summary>
            /// Конструктор по умолчанию.
            /// </summary>
            public GirocodeException()
            {
            }

            /// <summary>
            /// Конструктор.
            /// </summary>
            public GirocodeException
                (
                    string message
                )
                : base (message)
            {
            }

            /// <summary>
            /// Конструктор.
            /// </summary>
            public GirocodeException
                (
                    string message,
                    Exception inner
                )
                : base (message, inner)
            {
            }

            #endregion
        }
    }

    public class BezahlCode : Payload
    {
        //BezahlCode specification: http://www.bezahlcode.de/wp-content/uploads/BezahlCode_TechDok.pdf

        private readonly string name,
            iban,
            bic,
            account,
            bnc,
            sepaReference,
            reason,
            creditorId,
            mandateId,
            periodicTimeunit;

        private readonly decimal amount;
        private readonly int postingKey, periodicTimeunitRotation;
        private readonly Currency currency;
        private readonly AuthorityType authority;
        private readonly DateTime executionDate, dateOfSignature, periodicFirstExecutionDate, periodicLastExecutionDate;


        /// <summary>
        /// Constructor for contact data
        /// </summary>
        /// <param name="authority">Type of the bank transfer</param>
        /// <param name="name">Name of the receiver (Empfänger)</param>
        /// <param name="account">Bank account (Kontonummer)</param>
        /// <param name="bnc">Bank institute (Bankleitzahl)</param>
        /// <param name="iban">IBAN</param>
        /// <param name="bic">BIC</param>
        /// <param name="reason">Reason (Verwendungszweck)</param>
        public BezahlCode (AuthorityType authority, string name, string account = "", string bnc = "", string iban = "",
            string bic = "", string reason = "") : this (authority, name, account, bnc, iban, bic, 0, string.Empty, 0,
            null, null, string.Empty, string.Empty, null, reason, 0, string.Empty, Currency.EUR, null, 1)
        {
        }


        /// <summary>
        /// Constructor for non-SEPA payments
        /// </summary>
        /// <param name="authority">Type of the bank transfer</param>
        /// <param name="name">Name of the receiver (Empfänger)</param>
        /// <param name="account">Bank account (Kontonummer)</param>
        /// <param name="bnc">Bank institute (Bankleitzahl)</param>
        /// <param name="amount">Amount (Betrag)</param>
        /// <param name="periodicTimeunit">Unit of intervall for payment ('M' = monthly, 'W' = weekly)</param>
        /// <param name="periodicTimeunitRotation">Intervall for payment. This value is combined with 'periodicTimeunit'</param>
        /// <param name="periodicFirstExecutionDate">Date of first periodic execution</param>
        /// <param name="periodicLastExecutionDate">Date of last periodic execution</param>
        /// <param name="reason">Reason (Verwendungszweck)</param>
        /// <param name="postingKey">Transfer Key (Textschlüssel, z.B. Spendenzahlung = 69)</param>
        /// <param name="currency">Currency (Währung)</param>
        /// <param name="executionDate">Execution date (Ausführungsdatum)</param>
        public BezahlCode (AuthorityType authority, string name, string account, string bnc, decimal amount,
            string periodicTimeunit = "", int periodicTimeunitRotation = 0, DateTime? periodicFirstExecutionDate = null,
            DateTime? periodicLastExecutionDate = null, string reason = "", int postingKey = 0,
            Currency currency = Currency.EUR, DateTime? executionDate = null) : this (authority, name, account, bnc,
            string.Empty, string.Empty, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate,
            periodicLastExecutionDate, string.Empty, string.Empty, null, reason, postingKey, string.Empty, currency,
            executionDate, 2)
        {
        }

        /// <summary>
        /// Constructor for SEPA payments
        /// </summary>
        /// <param name="authority">Type of the bank transfer</param>
        /// <param name="name">Name of the receiver (Empfänger)</param>
        /// <param name="iban">IBAN</param>
        /// <param name="bic">BIC</param>
        /// <param name="amount">Amount (Betrag)</param>
        /// <param name="periodicTimeunit">Unit of intervall for payment ('M' = monthly, 'W' = weekly)</param>
        /// <param name="periodicTimeunitRotation">Intervall for payment. This value is combined with 'periodicTimeunit'</param>
        /// <param name="periodicFirstExecutionDate">Date of first periodic execution</param>
        /// <param name="periodicLastExecutionDate">Date of last periodic execution</param>
        /// <param name="creditorId">Creditor id (Gläubiger ID)</param>
        /// <param name="mandateId">Manadate id (Mandatsreferenz)</param>
        /// <param name="dateOfSignature">Signature date (Erteilungsdatum des Mandats)</param>
        /// <param name="reason">Reason (Verwendungszweck)</param>
        /// <param name="postingKey">Transfer Key (Textschlüssel, z.B. Spendenzahlung = 69)</param>
        /// <param name="sepaReference">SEPA reference (SEPA-Referenz)</param>
        /// <param name="currency">Currency (Währung)</param>
        /// <param name="executionDate">Execution date (Ausführungsdatum)</param>
        public BezahlCode (AuthorityType authority, string name, string iban, string bic, decimal amount,
            string periodicTimeunit = "", int periodicTimeunitRotation = 0, DateTime? periodicFirstExecutionDate = null,
            DateTime? periodicLastExecutionDate = null, string creditorId = "", string mandateId = "",
            DateTime? dateOfSignature = null, string reason = "", string sepaReference = "",
            Currency currency = Currency.EUR, DateTime? executionDate = null) : this (authority, name, string.Empty,
            string.Empty, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate,
            periodicLastExecutionDate, creditorId, mandateId, dateOfSignature, reason, 0, sepaReference, currency,
            executionDate, 3)
        {
        }


        /// <summary>
        /// Generic constructor. Please use specific (non-SEPA or SEPA) constructor
        /// </summary>
        /// <param name="authority">Type of the bank transfer</param>
        /// <param name="name">Name of the receiver (Empfänger)</param>
        /// <param name="account">Bank account (Kontonummer)</param>
        /// <param name="bnc">Bank institute (Bankleitzahl)</param>
        /// <param name="iban">IBAN</param>
        /// <param name="bic">BIC</param>
        /// <param name="amount">Amount (Betrag)</param>
        /// <param name="periodicTimeunit">Unit of intervall for payment ('M' = monthly, 'W' = weekly)</param>
        /// <param name="periodicTimeunitRotation">Intervall for payment. This value is combined with 'periodicTimeunit'</param>
        /// <param name="periodicFirstExecutionDate">Date of first periodic execution</param>
        /// <param name="periodicLastExecutionDate">Date of last periodic execution</param>
        /// <param name="creditorId">Creditor id (Gläubiger ID)</param>
        /// <param name="mandateId">Manadate id (Mandatsreferenz)</param>
        /// <param name="dateOfSignature">Signature date (Erteilungsdatum des Mandats)</param>
        /// <param name="reason">Reason (Verwendungszweck)</param>
        /// <param name="postingKey">Transfer Key (Textschlüssel, z.B. Spendenzahlung = 69)</param>
        /// <param name="sepaReference">SEPA reference (SEPA-Referenz)</param>
        /// <param name="currency">Currency (Währung)</param>
        /// <param name="executionDate">Execution date (Ausführungsdatum)</param>
        /// <param name="internalMode">Only used for internal state handdling</param>
        public BezahlCode (AuthorityType authority, string name, string account, string bnc, string iban, string bic,
            decimal amount, string periodicTimeunit = "", int periodicTimeunitRotation = 0,
            DateTime? periodicFirstExecutionDate = null, DateTime? periodicLastExecutionDate = null,
            string creditorId = "", string mandateId = "", DateTime? dateOfSignature = null, string reason = "",
            int postingKey = 0, string sepaReference = "", Currency currency = Currency.EUR,
            DateTime? executionDate = null, int internalMode = 0)
        {
            //Loaded via "contact-constructor"
            if (internalMode == 1)
            {
                if (authority != AuthorityType.contact && authority != AuthorityType.contact_v2)
                    throw new BezahlCodeException (
                        "The constructor without an amount may only ne used with authority types 'contact' and 'contact_v2'.");
                if (authority == AuthorityType.contact &&
                    (string.IsNullOrEmpty (account) || string.IsNullOrEmpty (bnc)))
                    throw new BezahlCodeException (
                        "When using authority type 'contact' the parameters 'account' and 'bnc' must be set.");

                if (authority != AuthorityType.contact_v2)
                {
                    var oldFilled = (!string.IsNullOrEmpty (account) && !string.IsNullOrEmpty (bnc));
                    var newFilled = (!string.IsNullOrEmpty (iban) && !string.IsNullOrEmpty (bic));
                    if ((!oldFilled && !newFilled) || (oldFilled && newFilled))
                        throw new BezahlCodeException (
                            "When using authority type 'contact_v2' either the parameters 'account' and 'bnc' or the parameters 'iban' and 'bic' must be set. Leave the other parameter pair empty.");
                }
            }
            else if (internalMode == 2)
            {
#pragma warning disable CS0612
                if (authority != AuthorityType.periodicsinglepayment && authority != AuthorityType.singledirectdebit &&
                    authority != AuthorityType.singlepayment)
                    throw new BezahlCodeException (
                        "The constructor with 'account' and 'bnc' may only be used with 'non SEPA' authority types. Either choose another authority type or switch constructor.");
                if (authority == AuthorityType.periodicsinglepayment &&
                    (string.IsNullOrEmpty (periodicTimeunit) || periodicTimeunitRotation == 0))
                    throw new BezahlCodeException (
                        "When using 'periodicsinglepayment' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
#pragma warning restore CS0612
            }
            else if (internalMode == 3)
            {
                if (authority != AuthorityType.periodicsinglepaymentsepa &&
                    authority != AuthorityType.singledirectdebitsepa && authority != AuthorityType.singlepaymentsepa)
                    throw new BezahlCodeException (
                        "The constructor with 'iban' and 'bic' may only be used with 'SEPA' authority types. Either choose another authority type or switch constructor.");
                if (authority == AuthorityType.periodicsinglepaymentsepa &&
                    (string.IsNullOrEmpty (periodicTimeunit) || periodicTimeunitRotation == 0))
                    throw new BezahlCodeException (
                        "When using 'periodicsinglepaymentsepa' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
            }

            this.authority = authority;

            if (name.Length > 70)
                throw new BezahlCodeException ("(Payee-)Name must be shorter than 71 chars.");
            this.name = name;

            if (reason.Length > 27)
                throw new BezahlCodeException ("Reasons texts have to be shorter than 28 chars.");
            this.reason = reason;

            var oldWayFilled = (!string.IsNullOrEmpty (account) && !string.IsNullOrEmpty (bnc));
            var newWayFilled = (!string.IsNullOrEmpty (iban) && !string.IsNullOrEmpty (bic));

            //Non-SEPA payment types
#pragma warning disable CS0612
            if (authority == AuthorityType.periodicsinglepayment || authority == AuthorityType.singledirectdebit ||
                authority == AuthorityType.singlepayment || authority == AuthorityType.contact ||
                (authority == AuthorityType.contact_v2 && oldWayFilled))
            {
#pragma warning restore CS0612
                if (!Regex.IsMatch (account.Replace (" ", ""), @"^[0-9]{1,9}$"))
                    throw new BezahlCodeException ("The account entered isn't valid.");
                this.account = account.Replace (" ", "").ToUpper();
                if (!Regex.IsMatch (bnc.Replace (" ", ""), @"^[0-9]{1,9}$"))
                    throw new BezahlCodeException ("The bnc entered isn't valid.");
                this.bnc = bnc.Replace (" ", "").ToUpper();

                if (authority != AuthorityType.contact && authority != AuthorityType.contact_v2)
                {
                    if (postingKey < 0 || postingKey >= 100)
                        throw new BezahlCodeException ("PostingKey must be within 0 and 99.");
                    this.postingKey = postingKey;
                }
            }

            //SEPA payment types
            if (authority == AuthorityType.periodicsinglepaymentsepa ||
                authority == AuthorityType.singledirectdebitsepa || authority == AuthorityType.singlepaymentsepa ||
                (authority == AuthorityType.contact_v2 && newWayFilled))
            {
                if (!IsValidIban (iban))
                    throw new BezahlCodeException ("The IBAN entered isn't valid.");
                this.iban = iban.Replace (" ", "").ToUpper();
                if (!IsValidBic (bic))
                    throw new BezahlCodeException ("The BIC entered isn't valid.");
                this.bic = bic.Replace (" ", "").ToUpper();

                if (authority != AuthorityType.contact_v2)
                {
                    if (sepaReference.Length > 35)
                        throw new BezahlCodeException ("SEPA reference texts have to be shorter than 36 chars.");
                    this.sepaReference = sepaReference;

                    if (!string.IsNullOrEmpty (creditorId) && !Regex.IsMatch (creditorId.Replace (" ", ""),
                            @"^[a-zA-Z]{2,2}[0-9]{2,2}([A-Za-z0-9]|[\+|\?|/|\-|:|\(|\)|\.|,|']){3,3}([A-Za-z0-9]|[\+|\?|/|\-|:|\(|\)|\.|,|']){1,28}$"))
                        throw new BezahlCodeException ("The creditorId entered isn't valid.");
                    this.creditorId = creditorId;
                    if (!string.IsNullOrEmpty (mandateId) && !Regex.IsMatch (mandateId.Replace (" ", ""),
                            @"^([A-Za-z0-9]|[\+|\?|/|\-|:|\(|\)|\.|,|']){1,35}$"))
                        throw new BezahlCodeException ("The mandateId entered isn't valid.");
                    this.mandateId = mandateId;
                    if (dateOfSignature != null)
                        this.dateOfSignature = (DateTime)dateOfSignature;
                }
            }

            //Checks for all payment types
            if (authority != AuthorityType.contact && authority != AuthorityType.contact_v2)
            {
                if (amount.ToString().Replace (",", ".").Contains (".") &&
                    amount.ToString().Replace (",", ".").Split ('.')[1].TrimEnd ('0').Length > 2)
                    throw new BezahlCodeException ("Amount must have less than 3 digits after decimal point.");
                if (amount < 0.01m || amount > 999999999.99m)
                    throw new BezahlCodeException (
                        "Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
                this.amount = amount;

                this.currency = currency;

                if (executionDate == null)
                    this.executionDate = DateTime.Now;
                else
                {
                    if (DateTime.Today.Ticks > executionDate.Value.Ticks)
                        throw new BezahlCodeException ("Execution date must be today or in future.");
                    this.executionDate = (DateTime)executionDate;
                }
#pragma warning disable CS0612
                if (authority == AuthorityType.periodicsinglepayment ||
                    authority == AuthorityType.periodicsinglepaymentsepa)
#pragma warning restore CS0612
                {
                    if (periodicTimeunit.ToUpper() != "M" && periodicTimeunit.ToUpper() != "W")
                        throw new BezahlCodeException (
                            "The periodicTimeunit must be either 'M' (monthly) or 'W' (weekly).");
                    this.periodicTimeunit = periodicTimeunit;
                    if (periodicTimeunitRotation < 1 || periodicTimeunitRotation > 52)
                        throw new BezahlCodeException (
                            "The periodicTimeunitRotation must be 1 or greater. (It means repeat the payment every 'periodicTimeunitRotation' weeks/months.");
                    this.periodicTimeunitRotation = periodicTimeunitRotation;
                    if (periodicFirstExecutionDate != null)
                        this.periodicFirstExecutionDate = (DateTime)periodicFirstExecutionDate;
                    if (periodicLastExecutionDate != null)
                        this.periodicLastExecutionDate = (DateTime)periodicLastExecutionDate;
                }
            }
        }

        public override string ToString()
        {
            var bezahlCodePayload = $"bank://{authority}?";

            bezahlCodePayload += $"name={Uri.EscapeDataString (name)}&";

            if (authority != AuthorityType.contact && authority != AuthorityType.contact_v2)
            {
                //Handle what is same for all payments
#pragma warning disable CS0612
                if (authority == AuthorityType.periodicsinglepayment || authority == AuthorityType.singledirectdebit ||
                    authority == AuthorityType.singlepayment)
#pragma warning restore CS0612
                {
                    bezahlCodePayload += $"account={account}&";
                    bezahlCodePayload += $"bnc={bnc}&";
                    if (postingKey > 0)
                        bezahlCodePayload += $"postingkey={postingKey}&";
                }
                else
                {
                    bezahlCodePayload += $"iban={iban}&";
                    bezahlCodePayload += $"bic={bic}&";

                    if (!string.IsNullOrEmpty (sepaReference))
                        bezahlCodePayload += $"separeference={Uri.EscapeDataString (sepaReference)}&";

                    if (authority == AuthorityType.singledirectdebitsepa)
                    {
                        if (!string.IsNullOrEmpty (creditorId))
                            bezahlCodePayload += $"creditorid={Uri.EscapeDataString (creditorId)}&";
                        if (!string.IsNullOrEmpty (mandateId))
                            bezahlCodePayload += $"mandateid={Uri.EscapeDataString (mandateId)}&";
                        if (dateOfSignature != DateTime.MinValue)
                            bezahlCodePayload += $"dateofsignature={dateOfSignature.ToString ("ddMMyyyy")}&";
                    }
                }

                bezahlCodePayload += $"amount={amount:0.00}&".Replace (".", ",");

                if (!string.IsNullOrEmpty (reason))
                    bezahlCodePayload += $"reason={Uri.EscapeDataString (reason)}&";
                bezahlCodePayload += $"currency={currency}&";
                bezahlCodePayload += $"executiondate={executionDate.ToString ("ddMMyyyy")}&";
#pragma warning disable CS0612
                if (authority == AuthorityType.periodicsinglepayment ||
                    authority == AuthorityType.periodicsinglepaymentsepa)
                {
                    bezahlCodePayload += $"periodictimeunit={periodicTimeunit}&";
                    bezahlCodePayload += $"periodictimeunitrotation={periodicTimeunitRotation}&";
                    if (periodicFirstExecutionDate != DateTime.MinValue)
                        bezahlCodePayload +=
                            $"periodicfirstexecutiondate={periodicFirstExecutionDate.ToString ("ddMMyyyy")}&";
                    if (periodicLastExecutionDate != DateTime.MinValue)
                        bezahlCodePayload +=
                            $"periodiclastexecutiondate={periodicLastExecutionDate.ToString ("ddMMyyyy")}&";
                }
#pragma warning restore CS0612
            }
            else
            {
                //Handle what is same for all contacts
                if (authority == AuthorityType.contact)
                {
                    bezahlCodePayload += $"account={account}&";
                    bezahlCodePayload += $"bnc={bnc}&";
                }
                else if (authority == AuthorityType.contact_v2)
                {
                    if (!string.IsNullOrEmpty (account) && !string.IsNullOrEmpty (bnc))
                    {
                        bezahlCodePayload += $"account={account}&";
                        bezahlCodePayload += $"bnc={bnc}&";
                    }
                    else
                    {
                        bezahlCodePayload += $"iban={iban}&";
                        bezahlCodePayload += $"bic={bic}&";
                    }
                }

                if (!string.IsNullOrEmpty (reason))
                    bezahlCodePayload += $"reason={Uri.EscapeDataString (reason)}&";
            }

            return bezahlCodePayload.Trim ('&');
        }

        /// <summary>
        /// ISO 4217 currency codes
        /// </summary>
        public enum Currency
        {
            /// <summary>
            ///
            /// </summary>
            AED = 784,

            /// <summary>
            ///
            /// </summary>
            AFN = 971,

            /// <summary>
            ///
            /// </summary>
            ALL = 008,

            /// <summary>
            ///
            /// </summary>
            AMD = 051,

            /// <summary>
            ///
            /// </summary>
            ANG = 532,

            /// <summary>
            ///
            /// </summary>
            AOA = 973,

            /// <summary>
            ///
            /// </summary>
            ARS = 032,

            /// <summary>
            ///
            /// </summary>
            AUD = 036,

            /// <summary>
            ///
            /// </summary>
            AWG = 533,

            /// <summary>
            ///
            /// </summary>
            AZN = 944,

            /// <summary>
            ///
            /// </summary>
            BAM = 977,

            /// <summary>
            ///
            /// </summary>
            BBD = 052,

            /// <summary>
            ///
            /// </summary>
            BDT = 050,

            /// <summary>
            ///
            /// </summary>
            BGN = 975,

            /// <summary>
            ///
            /// </summary>
            BHD = 048,

            /// <summary>
            ///
            /// </summary>
            BIF = 108,

            /// <summary>
            ///
            /// </summary>
            BMD = 060,

            /// <summary>
            ///
            /// </summary>
            BND = 096,

            /// <summary>
            ///
            /// </summary>
            BOB = 068,

            /// <summary>
            ///
            /// </summary>
            BOV = 984,

            /// <summary>
            ///
            /// </summary>
            BRL = 986,

            /// <summary>
            ///
            /// </summary>
            BSD = 044,

            /// <summary>
            ///
            /// </summary>
            BTN = 064,

            /// <summary>
            ///
            /// </summary>
            BWP = 072,

            /// <summary>
            ///
            /// </summary>
            BYR = 974,

            /// <summary>
            ///
            /// </summary>
            BZD = 084,

            /// <summary>
            ///
            /// </summary>
            CAD = 124,

            /// <summary>
            ///
            /// </summary>
            CDF = 976,

            /// <summary>
            ///
            /// </summary>
            CHE = 947,

            /// <summary>
            ///
            /// </summary>
            CHF = 756,

            /// <summary>
            ///
            /// </summary>
            CHW = 948,

            /// <summary>
            ///
            /// </summary>
            CLF = 990,

            /// <summary>
            ///
            /// </summary>
            CLP = 152,

            /// <summary>
            ///
            /// </summary>
            CNY = 156,

            /// <summary>
            ///
            /// </summary>
            COP = 170,

            /// <summary>
            ///
            /// </summary>
            COU = 970,

            /// <summary>
            ///
            /// </summary>
            CRC = 188,

            /// <summary>
            ///
            /// </summary>
            CUC = 931,

            /// <summary>
            ///
            /// </summary>
            CUP = 192,

            /// <summary>
            ///
            /// </summary>
            CVE = 132,

            /// <summary>
            ///
            /// </summary>
            CZK = 203,

            /// <summary>
            ///
            /// </summary>
            DJF = 262,

            /// <summary>
            ///
            /// </summary>
            DKK = 208,

            /// <summary>
            ///
            /// </summary>
            DOP = 214,

            /// <summary>
            ///
            /// </summary>
            DZD = 012,

            /// <summary>
            ///
            /// </summary>
            EGP = 818,

            /// <summary>
            ///
            /// </summary>
            ERN = 232,

            /// <summary>
            ///
            /// </summary>
            ETB = 230,

            /// <summary>
            ///
            /// </summary>
            EUR = 978,

            /// <summary>
            ///
            /// </summary>
            FJD = 242,

            /// <summary>
            ///
            /// </summary>
            FKP = 238,

            /// <summary>
            ///
            /// </summary>
            GBP = 826,

            /// <summary>
            ///
            /// </summary>
            GEL = 981,

            /// <summary>
            ///
            /// </summary>
            GHS = 936,

            /// <summary>
            ///
            /// </summary>
            GIP = 292,

            /// <summary>
            ///
            /// </summary>
            GMD = 270,

            /// <summary>
            ///
            /// </summary>
            GNF = 324,

            /// <summary>
            ///
            /// </summary>
            GTQ = 320,

            /// <summary>
            ///
            /// </summary>
            GYD = 328,

            /// <summary>
            ///
            /// </summary>
            HKD = 344,

            /// <summary>
            ///
            /// </summary>
            HNL = 340,

            /// <summary>
            ///
            /// </summary>
            HRK = 191,

            /// <summary>
            ///
            /// </summary>
            HTG = 332,

            /// <summary>
            ///
            /// </summary>
            HUF = 348,

            /// <summary>
            ///
            /// </summary>
            IDR = 360,

            /// <summary>
            ///
            /// </summary>
            ILS = 376,

            /// <summary>
            ///
            /// </summary>
            INR = 356,

            /// <summary>
            ///
            /// </summary>
            IQD = 368,

            /// <summary>
            ///
            /// </summary>
            IRR = 364,

            /// <summary>
            ///
            /// </summary>
            ISK = 352,

            /// <summary>
            ///
            /// </summary>
            JMD = 388,

            /// <summary>
            ///
            /// </summary>
            JOD = 400,

            /// <summary>
            ///
            /// </summary>
            JPY = 392,

            /// <summary>
            ///
            /// </summary>
            KES = 404,

            /// <summary>
            ///
            /// </summary>
            KGS = 417,

            /// <summary>
            ///
            /// </summary>
            KHR = 116,

            /// <summary>
            ///
            /// </summary>
            KMF = 174,

            /// <summary>
            ///
            /// </summary>
            KPW = 408,

            /// <summary>
            ///
            /// </summary>
            KRW = 410,

            /// <summary>
            ///
            /// </summary>
            KWD = 414,

            /// <summary>
            ///
            /// </summary>
            KYD = 136,

            /// <summary>
            ///
            /// </summary>
            KZT = 398,

            /// <summary>
            ///
            /// </summary>
            LAK = 418,

            /// <summary>
            ///
            /// </summary>
            LBP = 422,

            /// <summary>
            ///
            /// </summary>
            LKR = 144,

            /// <summary>
            ///
            /// </summary>
            LRD = 430,

            /// <summary>
            ///
            /// </summary>
            LSL = 426,

            /// <summary>
            ///
            /// </summary>
            LYD = 434,

            /// <summary>
            ///
            /// </summary>
            MAD = 504,

            /// <summary>
            ///
            /// </summary>
            MDL = 498,

            /// <summary>
            ///
            /// </summary>
            MGA = 969,

            /// <summary>
            ///
            /// </summary>
            MKD = 807,

            /// <summary>
            ///
            /// </summary>
            MMK = 104,

            /// <summary>
            ///
            /// </summary>
            MNT = 496,

            /// <summary>
            ///
            /// </summary>
            MOP = 446,

            /// <summary>
            ///
            /// </summary>
            MRO = 478,

            /// <summary>
            ///
            /// </summary>
            MUR = 480,

            /// <summary>
            ///
            /// </summary>
            MVR = 462,

            /// <summary>
            ///
            /// </summary>
            MWK = 454,

            /// <summary>
            ///
            /// </summary>
            MXN = 484,

            /// <summary>
            ///
            /// </summary>
            MXV = 979,

            /// <summary>
            ///
            /// </summary>
            MYR = 458,

            /// <summary>
            ///
            /// </summary>
            MZN = 943,

            /// <summary>
            ///
            /// </summary>
            NAD = 516,

            /// <summary>
            ///
            /// </summary>
            NGN = 566,

            /// <summary>
            ///
            /// </summary>
            NIO = 558,

            /// <summary>
            ///
            /// </summary>
            NOK = 578,

            /// <summary>
            ///
            /// </summary>
            NPR = 524,

            /// <summary>
            ///
            /// </summary>
            NZD = 554,

            /// <summary>
            ///
            /// </summary>
            OMR = 512,

            /// <summary>
            ///
            /// </summary>
            PAB = 590,

            /// <summary>
            ///
            /// </summary>
            PEN = 604,

            /// <summary>
            ///
            /// </summary>
            PGK = 598,

            /// <summary>
            ///
            /// </summary>
            PHP = 608,

            /// <summary>
            ///
            /// </summary>
            PKR = 586,

            /// <summary>
            ///
            /// </summary>
            PLN = 985,

            /// <summary>
            ///
            /// </summary>
            PYG = 600,

            /// <summary>
            ///
            /// </summary>
            QAR = 634,

            /// <summary>
            ///
            /// </summary>
            RON = 946,

            /// <summary>
            ///
            /// </summary>
            RSD = 941,

            /// <summary>
            ///
            /// </summary>
            RUB = 643,

            /// <summary>
            ///
            /// </summary>
            RWF = 646,

            /// <summary>
            ///
            /// </summary>
            SAR = 682,

            /// <summary>
            ///
            /// </summary>
            SBD = 090,

            /// <summary>
            ///
            /// </summary>
            SCR = 690,

            /// <summary>
            ///
            /// </summary>
            SDG = 938,

            /// <summary>
            ///
            /// </summary>
            SEK = 752,

            /// <summary>
            ///
            /// </summary>
            SGD = 702,

            /// <summary>
            ///
            /// </summary>
            SHP = 654,

            /// <summary>
            ///
            /// </summary>
            SLL = 694,

            /// <summary>
            ///
            /// </summary>
            SOS = 706,

            /// <summary>
            ///
            /// </summary>
            SRD = 968,

            /// <summary>
            ///
            /// </summary>
            SSP = 728,

            /// <summary>
            ///
            /// </summary>
            STD = 678,

            /// <summary>
            ///
            /// </summary>
            SVC = 222,

            /// <summary>
            ///
            /// </summary>
            SYP = 760,

            /// <summary>
            ///
            /// </summary>
            SZL = 748,

            /// <summary>
            ///
            /// </summary>
            THB = 764,

            /// <summary>
            ///
            /// </summary>
            TJS = 972,

            /// <summary>
            ///
            /// </summary>
            TMT = 934,

            /// <summary>
            ///
            /// </summary>
            TND = 788,

            /// <summary>
            ///
            /// </summary>
            TOP = 776,

            /// <summary>
            ///
            /// </summary>
            TRY = 949,

            /// <summary>
            ///
            /// </summary>
            TTD = 780,

            /// <summary>
            ///
            /// </summary>
            TWD = 901,

            /// <summary>
            ///
            /// </summary>
            TZS = 834,

            /// <summary>
            ///
            /// </summary>
            UAH = 980,

            /// <summary>
            ///
            /// </summary>
            UGX = 800,

            /// <summary>
            ///
            /// </summary>
            USD = 840,

            /// <summary>
            ///
            /// </summary>
            USN = 997,

            /// <summary>
            ///
            /// </summary>
            UYI = 940,

            /// <summary>
            ///
            /// </summary>
            UYU = 858,

            /// <summary>
            ///
            /// </summary>
            UZS = 860,

            /// <summary>
            ///
            /// </summary>
            VEF = 937,

            /// <summary>
            ///
            /// </summary>
            VND = 704,

            /// <summary>
            ///
            /// </summary>
            VUV = 548,

            /// <summary>
            ///
            /// </summary>
            WST = 882,

            /// <summary>
            ///
            /// </summary>
            XAF = 950,

            /// <summary>
            ///
            /// </summary>
            XAG = 961,

            /// <summary>
            ///
            /// </summary>
            XAU = 959,

            /// <summary>
            ///
            /// </summary>
            XBA = 955,

            /// <summary>
            ///
            /// </summary>
            XBB = 956,

            /// <summary>
            ///
            /// </summary>
            XBC = 957,

            /// <summary>
            ///
            /// </summary>
            XBD = 958,

            /// <summary>
            ///
            /// </summary>
            XCD = 951,

            /// <summary>
            ///
            /// </summary>
            XDR = 960,

            /// <summary>
            ///
            /// </summary>
            XOF = 952,

            /// <summary>
            ///
            /// </summary>
            XPD = 964,

            /// <summary>
            ///
            /// </summary>
            XPF = 953,

            /// <summary>
            ///
            /// </summary>
            XPT = 962,

            /// <summary>
            ///
            /// </summary>
            XSU = 994,

            /// <summary>
            ///
            /// </summary>
            XTS = 963,

            /// <summary>
            ///
            /// </summary>
            XUA = 965,

            /// <summary>
            ///
            /// </summary>
            XXX = 999,

            /// <summary>
            ///
            /// </summary>
            YER = 886,

            /// <summary>
            ///
            /// </summary>
            ZAR = 710,

            /// <summary>
            ///
            /// </summary>
            ZMW = 967,

            /// <summary>
            ///
            /// </summary>
            ZWL = 932
        }


        /// <summary>
        /// Operation modes of the BezahlCode
        /// </summary>
        public enum AuthorityType
        {
            /// <summary>
            /// Single payment (Überweisung)
            /// </summary>
            [Obsolete] singlepayment,

            /// <summary>
            /// Single SEPA payment (SEPA-Überweisung)
            /// </summary>
            singlepaymentsepa,

            /// <summary>
            /// Single debit (Lastschrift)
            /// </summary>
            [Obsolete] singledirectdebit,

            /// <summary>
            /// Single SEPA debit (SEPA-Lastschrift)
            /// </summary>
            singledirectdebitsepa,

            /// <summary>
            /// Periodic payment (Dauerauftrag)
            /// </summary>
            [Obsolete] periodicsinglepayment,

            /// <summary>
            /// Periodic SEPA payment (SEPA-Dauerauftrag)
            /// </summary>
            periodicsinglepaymentsepa,

            /// <summary>
            /// Contact data
            /// </summary>
            contact,

            /// <summary>
            /// Contact data V2
            /// </summary>
            contact_v2
        }

        public class BezahlCodeException : Exception
        {
            public BezahlCodeException()
            {
            }

            public BezahlCodeException (string message)
                : base (message)
            {
            }

            public BezahlCodeException (string message, Exception inner)
                : base (message, inner)
            {
            }
        }
    }

    /// <summary>
    /// Событие в календаре.
    /// </summary>
    public class CalendarEvent
        : Payload
    {
        /// <summary>
        /// Generates a calender entry/event payload.
        /// </summary>
        /// <param name="subject">Subject/title of the calender event</param>
        /// <param name="description">Description of the event</param>
        /// <param name="location">Location (lat:long or address) of the event</param>
        /// <param name="start">Start time of the event</param>
        /// <param name="end">End time of the event</param>
        /// <param name="allDayEvent">Is it a full day event?</param>
        /// <param name="encoding">Type of encoding (universal or iCal)</param>
        public CalendarEvent
            (
                string subject,
                string description,
                string location,
                DateTime start,
                DateTime end,
                bool allDayEvent,
                EventEncoding encoding = EventEncoding.Universal
            )
        {
            _subject = subject;
            _description = description;
            _location = location;
            _encoding = encoding;
            var dtFormat = allDayEvent ? "yyyyMMdd" : "yyyyMMddTHHmmss";
            _start = start.ToString (dtFormat);
            _end = end.ToString (dtFormat);
        }

        #region Private members

        private readonly string _subject, _description, _location, _start, _end;
        private readonly EventEncoding _encoding;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var vEvent = $"BEGIN:VEVENT{Environment.NewLine}";
            vEvent += $"SUMMARY:{_subject}{Environment.NewLine}";
            vEvent += !string.IsNullOrEmpty (_description) ? $"DESCRIPTION:{_description}{Environment.NewLine}" : "";
            vEvent += !string.IsNullOrEmpty (_location) ? $"LOCATION:{_location}{Environment.NewLine}" : "";
            vEvent += $"DTSTART:{_start}{Environment.NewLine}";
            vEvent += $"DTEND:{_end}{Environment.NewLine}";
            vEvent += "END:VEVENT";

            if (_encoding == EventEncoding.iCalComplete)
            {
                vEvent = $@"BEGIN:VCALENDAR{Environment.NewLine}VERSION:2.0{Environment.NewLine}{vEvent}{Environment.NewLine}END:VCALENDAR";
            }

            return vEvent;
        }

        #endregion

        public enum EventEncoding
        {
            iCalComplete,
            Universal
        }
    }

    /// <summary>
    /// Одноразовый пароль.
    /// </summary>
    public class OneTimePassword
        : Payload
    {
        //https://github.com/google/google-authenticator/wiki/Key-Uri-Format
        public OneTimePasswordAuthType Type { get; set; } = OneTimePasswordAuthType.TOTP;
        public string Secret { get; set; }

        public OneTimePasswordAuthAlgorithm AuthAlgorithm { get; set; } = OneTimePasswordAuthAlgorithm.SHA1;

        [Obsolete ("This property is obsolete, use " + nameof (AuthAlgorithm) + " instead", false)]
        public OoneTimePasswordAuthAlgorithm Algorithm
        {
            get =>
                (OoneTimePasswordAuthAlgorithm)Enum.Parse (typeof (OoneTimePasswordAuthAlgorithm),
                    AuthAlgorithm.ToString());
            set =>
                AuthAlgorithm =
                    (OneTimePasswordAuthAlgorithm)Enum.Parse (typeof (OneTimePasswordAuthAlgorithm), value.ToString());
        }

        public string Issuer { get; set; }
        public string Label { get; set; }
        public int Digits { get; set; } = 6;
        public int? Counter { get; set; } = null;
        public int? Period { get; set; } = 30;

        public enum OneTimePasswordAuthType
        {
            TOTP,
            HOTP,
        }

        public enum OneTimePasswordAuthAlgorithm
        {
            SHA1,
            SHA256,
            SHA512,
        }

        [Obsolete ("This enum is obsolete, use " + nameof (OneTimePasswordAuthAlgorithm) + " instead", false)]
        public enum OoneTimePasswordAuthAlgorithm
        {
            SHA1,
            SHA256,
            SHA512,
        }

        public override string ToString()
        {
            switch (Type)
            {
                case OneTimePasswordAuthType.TOTP:
                    return TimeToString();

                case OneTimePasswordAuthType.HOTP:
                    return HMACToString();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Note: Issuer:Label must only contain 1 : if either of the Issuer or the Label has a : then it is invalid.
        // Defaults are 6 digits and 30 for Period
        private string HMACToString()
        {
            var sb = new StringBuilder ("otpauth://hotp/");
            ProcessCommonFields (sb);
            var actualCounter = Counter ?? 1;
            sb.Append ("&counter=" + actualCounter);
            return sb.ToString();
        }

        private string TimeToString()
        {
            if (Period == null)
            {
                throw new Exception ("Period must be set when using OneTimePasswordAuthType.TOTP");
            }

            var sb = new StringBuilder ("otpauth://totp/");

            ProcessCommonFields (sb);

            if (Period != 30)
            {
                sb.Append ("&period=" + Period);
            }

            return sb.ToString();
        }

        private void ProcessCommonFields (StringBuilder sb)
        {
            if (string.IsNullOrWhiteSpace (Secret))
            {
                throw new Exception ("Secret must be a filled out base32 encoded string");
            }

            string strippedSecret = Secret.Replace (" ", "");
            string escapedIssuer = null;
            string label = null;

            if (!string.IsNullOrWhiteSpace (Issuer))
            {
                if (Issuer.Contains (":"))
                {
                    throw new Exception ("Issuer must not have a ':'");
                }

                escapedIssuer = Uri.EscapeDataString (Issuer);
            }

            if (!string.IsNullOrWhiteSpace (Label) && Label.Contains (":"))
            {
                throw new Exception ("Label must not have a ':'");
            }

            if (Label != null && Issuer != null)
            {
                label = Issuer + ":" + Label;
            }
            else if (Issuer != null)
            {
                label = Issuer;
            }

            if (label != null)
            {
                sb.Append (label);
            }

            sb.Append ("?secret=" + strippedSecret);

            if (escapedIssuer != null)
            {
                sb.Append ("&issuer=" + escapedIssuer);
            }

            if (Digits != 6)
            {
                sb.Append ("&digits=" + Digits);
            }
        }
    }

    public class ShadowSocksConfig : Payload
    {
        private readonly string hostname, password, tag, methodStr, parameter;
        private readonly Method method;
        private readonly int port;

        private Dictionary<string, string> encryptionTexts = new Dictionary<string, string>()
        {
            { "Chacha20IetfPoly1305", "chacha20-ietf-poly1305" },
            { "Aes128Gcm", "aes-128-gcm" },
            { "Aes192Gcm", "aes-192-gcm" },
            { "Aes256Gcm", "aes-256-gcm" },

            { "XChacha20IetfPoly1305", "xchacha20-ietf-poly1305" },

            { "Aes128Cfb", "aes-128-cfb" },
            { "Aes192Cfb", "aes-192-cfb" },
            { "Aes256Cfb", "aes-256-cfb" },
            { "Aes128Ctr", "aes-128-ctr" },
            { "Aes192Ctr", "aes-192-ctr" },
            { "Aes256Ctr", "aes-256-ctr" },
            { "Camellia128Cfb", "camellia-128-cfb" },
            { "Camellia192Cfb", "camellia-192-cfb" },
            { "Camellia256Cfb", "camellia-256-cfb" },
            { "Chacha20Ietf", "chacha20-ietf" },

            { "Aes256Cb", "aes-256-cfb" },

            { "Aes128Ofb", "aes-128-ofb" },
            { "Aes192Ofb", "aes-192-ofb" },
            { "Aes256Ofb", "aes-256-ofb" },
            { "Aes128Cfb1", "aes-128-cfb1" },
            { "Aes192Cfb1", "aes-192-cfb1" },
            { "Aes256Cfb1", "aes-256-cfb1" },
            { "Aes128Cfb8", "aes-128-cfb8" },
            { "Aes192Cfb8", "aes-192-cfb8" },
            { "Aes256Cfb8", "aes-256-cfb8" },

            { "Chacha20", "chacha20" },
            { "BfCfb", "bf-cfb" },
            { "Rc4Md5", "rc4-md5" },
            { "Salsa20", "salsa20" },

            { "DesCfb", "des-cfb" },
            { "IdeaCfb", "idea-cfb" },
            { "Rc2Cfb", "rc2-cfb" },
            { "Cast5Cfb", "cast5-cfb" },
            { "Salsa20Ctr", "salsa20-ctr" },
            { "Rc4", "rc4" },
            { "SeedCfb", "seed-cfb" },
            { "Table", "table" }
        };

        /// <summary>
        /// Generates a ShadowSocks proxy config payload.
        /// </summary>
        /// <param name="hostname">Hostname of the ShadowSocks proxy</param>
        /// <param name="port">Port of the ShadowSocks proxy</param>
        /// <param name="password">Password of the SS proxy</param>
        /// <param name="method">Encryption type</param>
        /// <param name="tag">Optional tag line</param>
        public ShadowSocksConfig (string hostname, int port, string password, Method method, string tag = null) :
            this (hostname, port, password, method, null, tag)
        {
        }

        public ShadowSocksConfig (string hostname, int port, string password, Method method, string plugin,
            string pluginOption, string tag = null) :
            this (hostname, port, password, method, new Dictionary<string, string>
            {
                ["plugin"] = plugin + (
                    string.IsNullOrEmpty (pluginOption)
                        ? ""
                        : $";{pluginOption}"
                )
            }, tag)
        {
        }

        private Dictionary<string, string> UrlEncodeTable = new Dictionary<string, string>
        {
            [" "] = "+",
            ["\0"] = "%00",
            ["\t"] = "%09",
            ["\n"] = "%0a",
            ["\r"] = "%0d",
            ["\""] = "%22",
            ["#"] = "%23",
            ["$"] = "%24",
            ["%"] = "%25",
            ["&"] = "%26",
            ["'"] = "%27",
            ["+"] = "%2b",
            [","] = "%2c",
            ["/"] = "%2f",
            [":"] = "%3a",
            [";"] = "%3b",
            ["<"] = "%3c",
            ["="] = "%3d",
            [">"] = "%3e",
            ["?"] = "%3f",
            ["@"] = "%40",
            ["["] = "%5b",
            ["\\"] = "%5c",
            ["]"] = "%5d",
            ["^"] = "%5e",
            ["`"] = "%60",
            ["{"] = "%7b",
            ["|"] = "%7c",
            ["}"] = "%7d",
            ["~"] = "%7e",
        };

        private string UrlEncode (string i)
        {
            string j = i;
            foreach (var kv in UrlEncodeTable)
            {
                j = j.Replace (kv.Key, kv.Value);
            }

            return j;
        }

        public ShadowSocksConfig (string hostname, int port, string password, Method method,
            Dictionary<string, string> parameters, string tag = null)
        {
            this.hostname = Uri.CheckHostName (hostname) == UriHostNameType.IPv6
                ? $"[{hostname}]"
                : hostname;
            if (port < 1 || port > 65535)
                throw new ShadowSocksConfigException ("Value of 'port' must be within 0 and 65535.");
            this.port = port;
            this.password = password;
            this.method = method;
            methodStr = encryptionTexts[method.ToString()];
            this.tag = tag;

            if (parameters != null)
                parameter =
                    string.Join ("&",
                        parameters.Select (
                                kv => $"{UrlEncode (kv.Key)}={UrlEncode (kv.Value)}"
                            ).ToArray());
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty (parameter))
            {
                var connectionString = $"{methodStr}:{password}@{hostname}:{port}";
                var connectionStringEncoded = Convert.ToBase64String (Encoding.UTF8.GetBytes (connectionString));
                return $"ss://{connectionStringEncoded}{(!string.IsNullOrEmpty (tag) ? $"#{tag}" : string.Empty)}";
            }

            var authString = $"{methodStr}:{password}";
            var authStringEncoded = Convert.ToBase64String (Encoding.UTF8.GetBytes (authString))
                .Replace ('+', '-')
                .Replace ('/', '_')
                .TrimEnd ('=');
            return
                $"ss://{authStringEncoded}@{hostname}:{port}/?{parameter}{(!string.IsNullOrEmpty (tag) ? $"#{tag}" : string.Empty)}";
        }

        public enum Method
        {
            // AEAD
            Chacha20IetfPoly1305,
            Aes128Gcm,
            Aes192Gcm,
            Aes256Gcm,

            // AEAD, not standard
            XChacha20IetfPoly1305,

            // Stream cipher
            Aes128Cfb,
            Aes192Cfb,
            Aes256Cfb,
            Aes128Ctr,
            Aes192Ctr,
            Aes256Ctr,
            Camellia128Cfb,
            Camellia192Cfb,
            Camellia256Cfb,
            Chacha20Ietf,

            // alias of Aes256Cfb
            Aes256Cb,

            // Stream cipher, not standard
            Aes128Ofb,
            Aes192Ofb,
            Aes256Ofb,
            Aes128Cfb1,
            Aes192Cfb1,
            Aes256Cfb1,
            Aes128Cfb8,
            Aes192Cfb8,
            Aes256Cfb8,

            // Stream cipher, deprecated
            Chacha20,
            BfCfb,
            Rc4Md5,
            Salsa20,

            // Not standard and not in acitve use
            DesCfb,
            IdeaCfb,
            Rc2Cfb,
            Cast5Cfb,
            Salsa20Ctr,
            Rc4,
            SeedCfb,
            Table
        }

        /// <summary>
        /// Специфичное исключение.
        /// </summary>
        public class ShadowSocksConfigException
            : Exception
        {
            #region Construction

            /// <summary>
            /// Конструктор по умолчанию.
            /// </summary>
            public ShadowSocksConfigException()
            {
            }

            /// <summary>
            /// Конструктор.
            /// </summary>
            public ShadowSocksConfigException
                (
                    string message
                )
                : base (message)
            {
            }

            /// <summary>
            /// Конструктор.
            /// </summary>
            public ShadowSocksConfigException
                (
                    string message,
                    Exception inner
                )
                : base (message, inner)
            {
            }

            #endregion
        }
    }

    /// <summary>
    /// Транзакция Monero.
    /// </summary>
    public class MoneroTransaction
        : Payload
    {
        #region Construction

        /// <summary>
        /// Creates a monero transaction payload
        /// </summary>
        /// <param name="address">Receiver's monero address</param>
        /// <param name="txAmount">Amount to transfer</param>
        /// <param name="txPaymentId">Payment id</param>
        /// <param name="recipientName">Receipient's name</param>
        /// <param name="txDescription">Reference text / payment description</param>
        public MoneroTransaction
            (
                string address,
                float? txAmount = null,
                string? txPaymentId = null,
                string? recipientName = null,
                string? txDescription = null
            )
        {
            if (string.IsNullOrEmpty (address))
            {
                throw new MoneroTransactionException ("The address is mandatory and has to be set.");
            }

            _address = address;
            if (txAmount != null && txAmount <= 0)
            {
                throw new MoneroTransactionException ("Value of 'txAmount' must be greater than 0.");
            }

            _txAmount = txAmount;
            _txPaymentId = txPaymentId;
            _recipientName = recipientName;
            _txDescription = txDescription;
        }

        #endregion

        #region Private members

        private readonly string? _address, _txPaymentId, _recipientName, _txDescription;
        private readonly float? _txAmount;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var moneroUri = $"monero://{_address}{(!string.IsNullOrEmpty (_txPaymentId) || !string.IsNullOrEmpty (_recipientName) || !string.IsNullOrEmpty (_txDescription) || _txAmount != null ? "?" : string.Empty)}";
            moneroUri += (!string.IsNullOrEmpty (_txPaymentId)
                ? $"tx_payment_id={Uri.EscapeDataString (_txPaymentId)}&"
                : string.Empty);
            moneroUri += (!string.IsNullOrEmpty (_recipientName)
                ? $"recipient_name={Uri.EscapeDataString (_recipientName)}&"
                : string.Empty);
            moneroUri += (_txAmount != null ? $"tx_amount={_txAmount.ToString().Replace (",", ".")}&" : string.Empty);
            moneroUri += (!string.IsNullOrEmpty (_txDescription)
                ? $"tx_description={Uri.EscapeDataString (_txDescription)}"
                : string.Empty);
            return moneroUri.TrimEnd ('&');
        }

        #endregion


        public class MoneroTransactionException : Exception
        {
            public MoneroTransactionException()
            {
            }

            public MoneroTransactionException (string message)
                : base (message)
            {
            }

            public MoneroTransactionException (string message, Exception inner)
                : base (message, inner)
            {
            }
        }
    }

    public class SlovenianUpnQr : Payload
    {
        //Keep in mind, that the ECC level has to be set to "M", version to 15 and ECI to EciMode.Iso8859_2 when generating a SlovenianUpnQr!
        //SlovenianUpnQr specification: https://www.upn-qr.si/uploads/files/NavodilaZaProgramerjeUPNQR.pdf

        private string _payerName = "";
        private string _payerAddress = "";
        private string _payerPlace = "";
        private string _amount = "";
        private string _code = "";
        private string _purpose = "";
        private string _deadLine = "";
        private string _recipientIban = "";
        private string _recipientName = "";
        private string _recipientAddress = "";
        private string _recipientPlace = "";
        private string _recipientSiModel = "";
        private string _recipientSiReference = "";

        public override int Version => 15;

        public override QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;

        public override QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Iso8859_2;

        private string LimitLength (string value, int maxLength)
        {
            return (value.Length <= maxLength) ? value : value.Substring (0, maxLength);
        }

        public SlovenianUpnQr (string payerName, string payerAddress, string payerPlace, string recipientName,
            string recipientAddress, string recipientPlace, string recipientIban, string description, double amount,
            string recipientSiModel = "SI00", string recipientSiReference = "", string code = "OTHR") :
            this (payerName, payerAddress, payerPlace, recipientName, recipientAddress, recipientPlace, recipientIban,
                description, amount, null, recipientSiModel, recipientSiReference, code)
        {
        }

        public SlovenianUpnQr (string payerName, string payerAddress, string payerPlace, string recipientName,
            string recipientAddress, string recipientPlace, string recipientIban, string description, double amount,
            DateTime? deadline, string recipientSiModel = "SI99", string recipientSiReference = "",
            string code = "OTHR")
        {
            _payerName = LimitLength (payerName.Trim(), 33);
            _payerAddress = LimitLength (payerAddress.Trim(), 33);
            _payerPlace = LimitLength (payerPlace.Trim(), 33);
            _amount = FormatAmount (amount);
            _code = LimitLength (code.Trim().ToUpper(), 4);
            _purpose = LimitLength (description.Trim(), 42);
            _deadLine = (deadline == null) ? "" : deadline?.ToString ("dd.MM.yyyy");
            _recipientIban = LimitLength (recipientIban.Trim(), 34);
            _recipientName = LimitLength (recipientName.Trim(), 33);
            _recipientAddress = LimitLength (recipientAddress.Trim(), 33);
            _recipientPlace = LimitLength (recipientPlace.Trim(), 33);
            _recipientSiModel = LimitLength (recipientSiModel.Trim().ToUpper(), 4);
            _recipientSiReference = LimitLength (recipientSiReference.Trim(), 22);
        }


        private string FormatAmount (double amount)
        {
            int _amt = (int)Math.Round (amount * 100.0);
            return String.Format ("{0:00000000000}", _amt);
        }

        private int CalculateChecksum()
        {
            int _cs = 5 + _payerName.Length; //5 = UPNQR constant Length
            _cs += _payerAddress.Length;
            _cs += _payerPlace.Length;
            _cs += _amount.Length;
            _cs += _code.Length;
            _cs += _purpose.Length;
            _cs += _deadLine.Length;
            _cs += _recipientIban.Length;
            _cs += _recipientName.Length;
            _cs += _recipientAddress.Length;
            _cs += _recipientPlace.Length;
            _cs += _recipientSiModel.Length;
            _cs += _recipientSiReference.Length;
            _cs += 19;
            return _cs;
        }

        public override string ToString()
        {
            var _sb = new StringBuilder();
            _sb.Append ("UPNQR");
            _sb.Append ('\n').Append ('\n').Append ('\n').Append ('\n').Append ('\n');
            _sb.Append (_payerName).Append ('\n');
            _sb.Append (_payerAddress).Append ('\n');
            _sb.Append (_payerPlace).Append ('\n');
            _sb.Append (_amount).Append ('\n').Append ('\n').Append ('\n');
            _sb.Append (_code.ToUpper()).Append ('\n');
            _sb.Append (_purpose).Append ('\n');
            _sb.Append (_deadLine).Append ('\n');
            _sb.Append (_recipientIban.ToUpper()).Append ('\n');
            _sb.Append (_recipientSiModel).Append (_recipientSiReference).Append ('\n');
            _sb.Append (_recipientName).Append ('\n');
            _sb.Append (_recipientAddress).Append ('\n');
            _sb.Append (_recipientPlace).Append ('\n');
            _sb.AppendFormat ("{0:000}", CalculateChecksum()).Append ('\n');
            return _sb.ToString();
        }
    }


    /// <summary>
    /// Российский платежный QR-код.
    /// </summary>
    public class RussiaPaymentOrder
        : Payload
    {
        // Specification of RussianPaymentOrder
        //https://docs.cntd.ru/document/1200110981
        //https://roskazna.gov.ru/upload/iblock/5fa/gost_r_56042_2014.pdf
        //https://sbqr.ru/standard/files/standart.pdf

        // Specification of data types described in the above standard
        // https://gitea.sergeybochkov.com/bochkov/emuik/src/commit/d18f3b550f6415ea4a4a5e6097eaab4661355c72/template/ed

        // Tool for QR validation
        // https://www.sbqr.ru/validator/index.html

        //base
        private CharacterSets characterSet;
        private MandatoryFields mFields;
        private OptionalFields oFields;
        private string separator = "|";

        private RussiaPaymentOrder()
        {
            mFields = new MandatoryFields();
            oFields = new OptionalFields();
        }

        /// <summary>
        /// Generates a RussiaPaymentOrder payload
        /// </summary>
        /// <param name="name">Name of the payee (Наименование получателя платежа)</param>
        /// <param name="personalAcc">Beneficiary account number (Номер счета получателя платежа)</param>
        /// <param name="bankName">Name of the beneficiary's bank (Наименование банка получателя платежа)</param>
        /// <param name="BIC">BIC (БИК)</param>
        /// <param name="correspAcc">Box number / account payee's bank (Номер кор./сч. банка получателя платежа)</param>
        /// <param name="optionalFields">An (optional) object of additional fields</param>
        /// <param name="characterSet">Type of encoding (default UTF-8)</param>
        public RussiaPaymentOrder
            (
                string name,
                string personalAcc,
                string bankName,
                string BIC,
                string correspAcc,
                OptionalFields? optionalFields = null,
                CharacterSets characterSet = CharacterSets.utf_8
            )
            : this()
        {
            this.characterSet = characterSet;
            mFields.Name = ValidateInput (name, "Name", @"^.{1,160}$");
            mFields.PersonalAcc = ValidateInput (personalAcc, "PersonalAcc", @"^[1-9]\d{4}[0-9ABCEHKMPTX]\d{14}$");
            mFields.BankName = ValidateInput (bankName, "BankName", @"^.{1,45}$");
            mFields.BIC = ValidateInput (BIC, "BIC", @"^\d{9}$");
            mFields.CorrespAcc = ValidateInput (correspAcc, "CorrespAcc", @"^[1-9]\d{4}[0-9ABCEHKMPTX]\d{14}$");

            if (optionalFields != null)
            {
                oFields = optionalFields;
            }
        }

        /// <summary>
        /// Returns payload as string.
        /// </summary>
        /// <remarks>⚠ Attention: If CharacterSets was set to windows-1251 or koi8-r you should use ToBytes() instead of ToString() and pass the bytes to CreateQrCode()!</remarks>
        /// <returns></returns>
        public override string ToString()
        {
            var cp = characterSet.ToString().Replace ("_", "-");
            var bytes = ToBytes();

            Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding (cp).GetString (bytes);
        }

        /// <summary>
        /// Returns payload as byte[].
        /// </summary>
        /// <remarks>Should be used if CharacterSets equals windows-1251 or koi8-r</remarks>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            //Calculate the seperator
            separator = DetermineSeparator();

            //Create the payload string
            string ret = $"ST0001" + ((int)characterSet).ToString() + //(separator != "|" ? separator : "") +
                         $"{separator}Name={mFields.Name}" +
                         $"{separator}PersonalAcc={mFields.PersonalAcc}" +
                         $"{separator}BankName={mFields.BankName}" +
                         $"{separator}BIC={mFields.BIC}" +
                         $"{separator}CorrespAcc={mFields.CorrespAcc}";

            //Add optional fields, if filled
            var optionalFieldsList = GetOptionalFieldsAsList();
            if (optionalFieldsList.Count > 0)
                ret += $"|{string.Join ("|", optionalFieldsList.ToArray())}";
            ret += separator;

            //Encode return string as byte[] with correct CharacterSet
            Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);
            var cp = characterSet.ToString().Replace ("_", "-");
            byte[] bytesOut = Encoding.Convert (Encoding.UTF8, Encoding.GetEncoding (cp), Encoding.UTF8.GetBytes (ret));
            if (bytesOut.Length > 300)
                throw new RussiaPaymentOrderException (
                    $"Data too long. Payload must not exceed 300 bytes, but actually is {bytesOut.Length} bytes long. Remove additional data fields or shorten strings/values.");
            return bytesOut;
        }


        /// <summary>
        /// Determines a valid separator
        /// </summary>
        /// <returns></returns>
        private string DetermineSeparator()
        {
            // See chapter 5.2.1 of Standard (https://sbqr.ru/standard/files/standart.pdf)

            var mandatoryValues = GetMandatoryFieldsAsList();
            var optionalValues = GetOptionalFieldsAsList();

            // Possible candidates for field separation
            var separatorCandidates = new string[]
            {
                "|", "#", ";", ":", "^", "_", "~", "{", "}", "!", "#", "$", "%", "&", "(", ")", "*", "+", ",", "/", "@"
            };
            foreach (var sepCandidate in separatorCandidates)
            {
                if (!mandatoryValues.Any (x => x.Contains (sepCandidate)) &&
                    !optionalValues.Any (x => x.Contains (sepCandidate)))
                    return sepCandidate;
            }

            throw new RussiaPaymentOrderException ("No valid separator found.");
        }

        /// <summary>
        /// Takes all optional fields that are not null and returns their string represantion
        /// </summary>
        /// <returns>A List of strings</returns>
        private List<string> GetOptionalFieldsAsList()
        {
            return oFields.GetType().GetProperties()
                .Where (field => field.GetValue (oFields, null) != null)
                .Select (field =>
                {
                    var objValue = field.GetValue (oFields, null);
                    var value = field.PropertyType.Equals (typeof (DateTime?))
                        ? ((DateTime)objValue).ToString ("dd.MM.yyyy")
                        : objValue.ToString();
                    return $"{field.Name}={value}";
                })
                .ToList();
        }


        /// <summary>
        /// Takes all mandatory fields that are not null and returns their string represantion
        /// </summary>
        /// <returns>A List of strings</returns>
        private List<string> GetMandatoryFieldsAsList()
        {
            return mFields.GetType().GetFields()
                .Where (field => field.GetValue (mFields) != null)
                .Select (field =>
                {
                    var objValue = field.GetValue (mFields);
                    var value = field.FieldType.Equals (typeof (DateTime?))
                        ? ((DateTime)objValue).ToString ("dd.MM.yyyy")
                        : objValue.ToString();
                    return $"{field.Name}={value}";
                })
                .ToList();
        }

        /// <summary>
        /// Validates a string against a given Regex pattern. Returns input if it matches the Regex expression (=valid) or throws Exception in case there's a mismatch
        /// </summary>
        /// <param name="input">String to be validated</param>
        /// <param name="fieldname">Name/descriptor of the string to be validated</param>
        /// <param name="pattern">A regex pattern to be used for validation</param>
        /// <param name="errorText">An optional error text. If null, a standard error text is generated</param>
        /// <returns>Input value (in case it is valid)</returns>
        private static string ValidateInput (string input, string fieldname, string pattern, string errorText = null)
        {
            return ValidateInput (input, fieldname, new string[] { pattern }, errorText);
        }

        /// <summary>
        /// Validates a string against one or more given Regex patterns. Returns input if it matches all regex expressions (=valid) or throws Exception in case there's a mismatch
        /// </summary>
        /// <param name="input">String to be validated</param>
        /// <param name="fieldname">Name/descriptor of the string to be validated</param>
        /// <param name="patterns">An array of regex patterns to be used for validation</param>
        /// <param name="errorText">An optional error text. If null, a standard error text is generated</param>
        /// <returns>Input value (in case it is valid)</returns>
        private static string ValidateInput (string input, string fieldname, string[] patterns, string errorText = null)
        {
            if (input == null)
                throw new RussiaPaymentOrderException ($"The input for '{fieldname}' must not be null.");
            foreach (var pattern in patterns)
            {
                if (!Regex.IsMatch (input, pattern))
                    throw new RussiaPaymentOrderException (errorText ??
                                                           $"The input for '{fieldname}' ({input}) doesn't match the pattern {pattern}");
            }

            return input;
        }

        private class MandatoryFields
        {
            public string Name;
            public string PersonalAcc;
            public string BankName;
            public string BIC;
            public string CorrespAcc;
        }

        /// <summary>
        /// Необязательные поля.
        /// </summary>
        public class OptionalFields
        {
            private string _sum;

            /// <summary>
            /// Payment amount, in kopecks (FTI’s Amount.)
            /// <para>Сумма платежа, в копейках</para>
            /// </summary>
            public string Sum
            {
                get => _sum;
                set => _sum = ValidateInput (value, "Sum", @"^\d{1,18}$");
            }

            private string _purpose;

            /// <summary>
            /// Payment name (purpose)
            /// <para>Наименование платежа (назначение)</para>
            /// </summary>
            public string Purpose
            {
                get => _purpose;
                set => _purpose = ValidateInput (value, "Purpose", @"^.{1,160}$");
            }

            private string _payeeInn;

            /// <summary>
            /// Payee's INN (Resident Tax Identification Number; Text, up to 12 characters.)
            /// <para>ИНН получателя платежа</para>
            /// </summary>
            public string PayeeINN
            {
                get => _payeeInn;
                set => _payeeInn = ValidateInput (value, "PayeeINN", @"^.{1,12}$");
            }

            private string _payerInn;

            /// <summary>
            /// Payer's INN (Resident Tax Identification Number; Text, up to 12 characters.)
            /// <para>ИНН плательщика</para>
            /// </summary>
            public string PayerINN
            {
                get => _payerInn;
                set => _payerInn = ValidateInput (value, "PayerINN", @"^.{1,12}$");
            }

            private string _drawerStatus;

            /// <summary>
            /// Status compiler payment document
            /// <para>Статус составителя платежного документа</para>
            /// </summary>
            public string DrawerStatus
            {
                get => _drawerStatus;
                set => _drawerStatus = ValidateInput (value, "DrawerStatus", @"^.{1,2}$");
            }

            private string _kpp;

            /// <summary>
            /// KPP of the payee (Tax Registration Code; Text, up to 9 characters.)
            /// <para>КПП получателя платежа</para>
            /// </summary>
            public string KPP
            {
                get => _kpp;
                set => _kpp = ValidateInput (value, "KPP", @"^.{1,9}$");
            }

            private string _cbc;

            /// <summary>
            /// CBC
            /// <para>КБК</para>
            /// </summary>
            public string CBC
            {
                get => _cbc;
                set => _cbc = ValidateInput (value, "CBC", @"^.{1,20}$");
            }

            private string _oktmo;

            /// <summary>
            /// All-Russian classifier territories of municipal formations
            /// <para>Общероссийский классификатор территорий муниципальных образований</para>
            /// </summary>
            public string OKTMO
            {
                get => _oktmo;
                set => _oktmo = ValidateInput (value, "OKTMO", @"^.{1,11}$");
            }

            private string _paytReason;

            /// <summary>
            /// Basis of tax payment
            /// <para>Основание налогового платежа</para>
            /// </summary>
            public string PaytReason
            {
                get => _paytReason;
                set => _paytReason = ValidateInput (value, "PaytReason", @"^.{1,2}$");
            }

            private string _taxPeriod;

            /// <summary>
            /// Taxable period
            /// <para>Налоговый период</para>
            /// </summary>
            public string TaxPeriod
            {
                get => _taxPeriod;
                set => _taxPeriod = ValidateInput (value, "ТaxPeriod", @"^.{1,10}$");
            }

            private string _docNo;

            /// <summary>
            /// Document number
            /// <para>Номер документа</para>
            /// </summary>
            public string DocNo
            {
                get => _docNo;
                set => _docNo = ValidateInput (value, "DocNo", @"^.{1,15}$");
            }

            /// <summary>
            /// Document date
            /// <para>Дата документа</para>
            /// </summary>
            public DateTime? DocDate { get; set; }

            private string _taxPaytKind;

            /// <summary>
            /// Payment type
            /// <para>Тип платежа</para>
            /// </summary>
            public string TaxPaytKind
            {
                get => _taxPaytKind;
                set => _taxPaytKind = ValidateInput (value, "TaxPaytKind", @"^.{1,2}$");
            }

            /**************************************************************************
             * The following fiels are no further specified in the standard
             * document (https://sbqr.ru/standard/files/standart.pdf) thus there
             * is no addition input validation implemented.
             * **************************************************************************/

            /// <summary>
            /// Payer's surname
            /// <para>Фамилия плательщика</para>
            /// </summary>
            public string? LastName { get; set; }

            /// <summary>
            /// Payer's name
            /// <para>Имя плательщика</para>
            /// </summary>
            public string? FirstName { get; set; }

            /// <summary>
            /// Payer's patronymic
            /// <para>Отчество плательщика</para>
            /// </summary>
            public string? MiddleName { get; set; }

            /// <summary>
            /// Payer's address
            /// <para>Адрес плательщика</para>
            /// </summary>
            public string? PayerAddress { get; set; }

            /// <summary>
            /// Personal account of a budget recipient
            /// <para>Лицевой счет бюджетного получателя</para>
            /// </summary>
            public string? PersonalAccount { get; set; }

            /// <summary>
            /// Payment document index
            /// <para>Индекс платежного документа</para>
            /// </summary>
            public string? DocIdx { get; set; }

            /// <summary>
            /// Personal account number in the personalized accounting system in the Pension Fund of the Russian Federation - SNILS
            /// <para>№ лицевого счета в системе персонифицированного учета в ПФР - СНИЛС</para>
            /// </summary>
            public string? PensAcc { get; set; }

            /// <summary>
            /// Number of contract
            /// <para>Номер договора</para>
            /// </summary>
            public string? Contract { get; set; }

            /// <summary>
            /// Personal account number of the payer in the organization (in the accounting system of the PU)
            /// <para>Номер лицевого счета плательщика в организации (в системе учета ПУ)</para>
            /// </summary>
            public string? PersAcc { get; set; }

            /// <summary>
            /// Apartment number
            /// <para>Номер квартиры</para>
            /// </summary>
            public string? Flat { get; set; }

            /// <summary>
            /// Phone number
            /// <para>Номер телефона</para>
            /// </summary>
            public string? Phone { get; set; }

            /// <summary>
            /// DUL payer type
            /// <para>Вид ДУЛ плательщика</para>
            /// </summary>
            public string? PayerIdType { get; set; }

            /// <summary>
            /// DUL number of the payer
            /// <para>Номер ДУЛ плательщика</para>
            /// </summary>
            public string? PayerIdNum { get; set; }

            /// <summary>
            /// FULL NAME. child / student
            /// <para>Ф.И.О. ребенка/учащегося</para>
            /// </summary>
            public string? ChildFio { get; set; }

            /// <summary>
            /// Date of birth
            /// <para>Дата рождения</para>
            /// </summary>
            public DateTime? BirthDate { get; set; }

            /// <summary>
            /// Due date / Invoice date
            /// <para>Срок платежа/дата выставления счета</para>
            /// </summary>
            public string? PaymTerm { get; set; }

            /// <summary>
            /// Payment period
            /// <para>Период оплаты</para>
            /// </summary>
            public string? PaymPeriod { get; set; }

            /// <summary>
            /// Payment type
            /// <para>Вид платежа</para>
            /// </summary>
            public string? Category { get; set; }

            /// <summary>
            /// Service code / meter name
            /// <para>Код услуги/название прибора учета</para>
            /// </summary>
            public string? ServiceName { get; set; }

            /// <summary>
            /// Metering device number
            /// <para>Номер прибора учета</para>
            /// </summary>
            public string? CounterId { get; set; }

            /// <summary>
            /// Meter reading
            /// <para>Показание прибора учета</para>
            /// </summary>
            public string? CounterVal { get; set; }

            /// <summary>
            /// Notification, accrual, account number
            /// <para>Номер извещения, начисления, счета</para>
            /// </summary>
            public string? QuittId { get; set; }

            /// <summary>
            /// Date of notification / accrual / invoice / resolution (for traffic police)
            /// <para>Дата извещения/начисления/счета/постановления (для ГИБДД)</para>
            /// </summary>
            public DateTime? QuittDate { get; set; }

            /// <summary>
            /// Institution number (educational, medical)
            /// <para>Номер учреждения (образовательного, медицинского)</para>
            /// </summary>
            public string? InstNum { get; set; }

            /// <summary>
            /// Kindergarten / school class number
            /// <para>Номер группы детсада/класса школы</para>
            /// </summary>
            public string? ClassNum { get; set; }

            /// <summary>
            /// Full name of the teacher, specialist providing the service
            /// <para>ФИО преподавателя, специалиста, оказывающего услугу</para>
            /// </summary>
            public string? SpecFio { get; set; }

            /// <summary>
            /// Insurance / additional service amount / Penalty amount (in kopecks)
            /// <para>Сумма страховки/дополнительной услуги/Сумма пени (в копейках)</para>
            /// </summary>
            public string? AddAmount { get; set; }

            /// <summary>
            /// Resolution number (for traffic police)
            /// <para>Номер постановления (для ГИБДД)</para>
            /// </summary>
            public string? RuleId { get; set; }

            /// <summary>
            /// Enforcement Proceedings Number
            /// <para>Номер исполнительного производства</para>
            /// </summary>
            public string? ExecId { get; set; }

            /// <summary>
            /// Type of payment code (for example, for payments to Rosreestr)
            /// <para>Код вида платежа (например, для платежей в адрес Росреестра)</para>
            /// </summary>
            public string? RegType { get; set; }

            /// <summary>
            /// Unique accrual identifier
            /// <para>Уникальный идентификатор начисления</para>
            /// </summary>
            public string? UIN { get; set; }

            /// <summary>
            /// The technical code recommended by the service provider. Maybe used by the receiving organization to call the appropriate processing IT system.
            /// <para>Технический код, рекомендуемый для заполнения поставщиком услуг. Может использоваться принимающей организацией для вызова соответствующей обрабатывающей ИТ-системы.</para>
            /// </summary>
            public TechCode? TechCode { get; set; }
        }

        /// <summary>
        /// (List of values of the technical code of the payment)
        /// <para>Перечень значений технического кода платежа</para>
        /// </summary>
        public enum TechCode
        {
            /// <summary>
            /// Мобильная связь и стационарный телефон.
            /// </summary>
            Мобильная_связь_стационарный_телефон = 01,

            /// <summary>
            /// Коммунальные услуги, ЖКХ.
            /// </summary>
            Коммунальные_услуги_ЖКХAFN = 02,

            /// <summary>
            /// ГИБДД, налоги, пошлины, бюджетные платежи.
            /// </summary>
            ГИБДД_налоги_пошлины_бюджетные_платежи = 03,

            /// <summary>
            /// Охранные услуги.
            /// </summary>
            Охранные_услуги = 04,

            /// <summary>
            /// Услуги, оказываемые УФМС.
            /// </summary>
            Услуги_оказываемые_УФМС = 05,

            /// <summary>
            /// Пенсионный фонд России.
            /// </summary>
            ПФР = 06,

            /// <summary>
            /// Погашение кредитов.
            /// </summary>
            Погашение_кредитов = 07,

            /// <summary>
            /// Образовательные учреждения.
            /// </summary>
            Образовательные_учреждения = 08,

            /// <summary>
            /// Интернет и ТВ.
            /// </summary>
            Интернет_и_ТВ = 09,

            /// <summary>
            /// Электронные деньги.
            /// </summary>
            Электронные_деньги = 10,

            /// <summary>
            /// Отдых и путешествия.
            /// </summary>
            Отдых_и_путешествия = 11,

            /// <summary>
            /// Инвестиции и страхование.
            /// </summary>
            Инвестиции_и_страхование = 12,

            /// <summary>
            /// Спорт и здоровье.
            /// </summary>
            Спорт_и_здоровье = 13,

            /// <summary>
            /// Благотворительные и общественные организации.
            /// </summary>
            Благотворительные_и_общественные_организации = 14,

            /// <summary>
            /// Прочие услуги.
            /// </summary>
            Прочие_услуги = 15
        }

        /// <summary>
        /// Набор символов.
        /// </summary>
        public enum CharacterSets
        {
            /// <summary>
            /// Кириллица Windows.
            /// </summary>
            windows_1251 = 1, // Encoding.GetEncoding("windows-1251")

            /// <summary>
            /// UTF-8.
            /// </summary>
            utf_8 = 2, // Encoding.UTF8

            /// <summary>
            /// КОИ8-Р.
            /// </summary>
            koi8_r = 3 // Encoding.GetEncoding("koi8-r")
        }

        /// <summary>
        /// Специфичное для российских платежей исключение.
        /// </summary>
        public class RussiaPaymentOrderException
            : Exception
        {
            #region Construction

            /// <summary>
            /// Конструктор.
            /// </summary>
            public RussiaPaymentOrderException
                (
                    string message
                )
                : base (message)
            {
            }

            #endregion
        }
    }


    private static bool IsValidIban (string iban)
    {
        //Clean IBAN
        var ibanCleared = iban.ToUpper().Replace (" ", "").Replace ("-", "");

        //Check for general structure
        var structurallyValid = Regex.IsMatch (ibanCleared, @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]?){16,30}$");

        //Check IBAN checksum
        var checksumValid = false;
        var sum = $"{ibanCleared.Substring (4)}{ibanCleared.Substring (0, 4)}".ToCharArray().Aggregate ("",
            (current, c) => current + (char.IsLetter (c) ? (c - 55).ToString() : c.ToString()));
        int m = 0;
        for (int i = 0; i < (int)Math.Ceiling ((sum.Length - 2) / 7d); i++)
        {
            var offset = (i == 0 ? 0 : 2);
            var start = i * 7 + offset;
            var n = (i == 0 ? "" : m.ToString()) + sum.Substring (start, Math.Min (9 - offset, sum.Length - start));
            if (!int.TryParse (n, NumberStyles.Any, CultureInfo.InvariantCulture, out m))
                break;
            m = m % 97;
        }

        checksumValid = m == 1;
        return structurallyValid && checksumValid;
    }

    private static bool IsValidQRIban (string iban)
    {
        var foundQrIid = false;
        try
        {
            var ibanCleared = iban.ToUpper().Replace (" ", "").Replace ("-", "");
            var possibleQrIid = Convert.ToInt32 (ibanCleared.Substring (4, 5));
            foundQrIid = possibleQrIid >= 30000 && possibleQrIid <= 31999;
        }
        catch
        {
        }

        return IsValidIban (iban) && foundQrIid;
    }

    private static bool IsValidBic (string bic)
    {
        return Regex.IsMatch (bic.Replace (" ", ""), @"^([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)$");
    }


    private static string ConvertStringToEncoding (string message, string encoding)
    {
        Encoding iso = Encoding.GetEncoding (encoding);
        Encoding utf8 = Encoding.UTF8;
        byte[] utfBytes = utf8.GetBytes (message);
        byte[] isoBytes = Encoding.Convert (utf8, iso, utfBytes);
        return iso.GetString (isoBytes, 0, isoBytes.Length);
    }

    private static string EscapeInput (string inp, bool simple = false)
    {
        char[] forbiddenChars = { '\\', ';', ',', ':' };
        if (simple)
        {
            forbiddenChars = new char[1] { ':' };
        }

        foreach (var c in forbiddenChars)
        {
            inp = inp.Replace (c.ToString(), "\\" + c);
        }

        return inp;
    }


    public static bool ChecksumMod10 (string digits)
    {
        if (string.IsNullOrEmpty (digits) || digits.Length < 2)
            return false;
        int[] mods = new int[] { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };

        int remainder = 0;
        for (int i = 0; i < digits.Length - 1; i++)
        {
            var num = Convert.ToInt32 (digits[i]) - 48;
            remainder = mods[(num + remainder) % 10];
        }

        var checksum = (10 - remainder) % 10;
        return checksum == Convert.ToInt32 (digits[digits.Length - 1]) - 48;
    }

    private static bool isHexStyle (string inp)
    {
        return (Regex.IsMatch (inp, @"\A\b[0-9a-fA-F]+\b\Z") || Regex.IsMatch (inp, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"));
    }
}
