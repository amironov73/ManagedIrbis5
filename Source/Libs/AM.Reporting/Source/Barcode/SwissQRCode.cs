// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Reporting.Barcode
{
    /// <summary>
    /// Represents a class that contains all parameters of Swiss QR Code.
    /// </summary>
    public class QRSwissParameters
    {
        #region private fields

        #endregion

        #region public properties

        /// <summary>
        /// IBAN object
        /// </summary>
        public Iban Iban { get; set; }

        /// <summary>
        /// (either EUR or CHF)
        /// </summary>
        public Currency? Currency { get; set; }

        /// <summary>
        /// Creditor (payee) information
        /// </summary>
        public Contact Creditor { get; set; }

        /// <summary>
        /// Reference information
        /// </summary>
        public Reference Reference { get; set; }

        /// <summary>
        /// Can be null
        /// </summary>
        public AdditionalInformation AdditionalInformation { get; set; }

        /// <summary>
        /// Debitor (payer) information
        /// </summary>
        public Contact Debitor { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Optional command for alternative processing mode - line 1
        /// </summary>
        public string AlternativeProcedure1 { get; set; }

        /// <summary>
        /// Optional command for alternative processing mode - line 2
        /// </summary>
        public string AlternativeProcedure2 { get; set; }

        #endregion
    }

    public class AdditionalInformation
    {
        private string unstructuredMessage, billInformation;

        /// <summary>
        /// Creates an additional information object. Both parameters are optional and must be shorter than 141 chars in combination.
        /// </summary>
        /// <param name="unstructuredMessage">Unstructured text message</param>
        /// <param name="billInformation">Bill information</param>
        public AdditionalInformation (string unstructuredMessage, string billInformation)
        {
            var res = new MyRes ("Messages,Swiss");
            if (((unstructuredMessage != null ? unstructuredMessage.Length : 0) +
                 (billInformation != null ? billInformation.Length : 0)) > 140)
            {
                throw new SwissQrCodeException (res.Get ("SwissUnstructBillLength"));
            }

            this.unstructuredMessage = unstructuredMessage;
            this.billInformation = billInformation;
            Trailer = "EPD";
        }

        public AdditionalInformation (string addInfo)
        {
            string[] data = addInfo.Split ('\r');
            Trailer = data[1].Trim();
            unstructuredMessage = data[0].Trim();
            billInformation = data[2].Trim();
        }

        public string UnstructureMessage
        {
            get => !string.IsNullOrEmpty (unstructuredMessage) ? unstructuredMessage.Replace ("\n", "") : null;
            set => unstructuredMessage = value;
        }

        public string BillInformation
        {
            get => !string.IsNullOrEmpty (billInformation) ? billInformation.Replace ("\n", "") : null;
            set => billInformation = value;
        }

        public string Trailer { get; }
    }

    public class Reference
    {
        private string reference;


        public ReferenceType RefType { get; set; }

        public string ReferenceText
        {
            get => !string.IsNullOrEmpty (reference) ? reference.Replace ("\n", "") : null;
            set => reference = value;
        }

        public ReferenceTextType? _ReferenceTextType { get; set; }

        /// <summary>
        /// Creates a reference object which must be passed to the SwissQrCode instance
        /// </summary>
        /// <param name="referenceType">Type of the reference (QRR, SCOR or NON)</param>
        /// <param name="reference">Reference text</param>
        /// <param name="referenceTextType">Type of the reference text (QR-reference or Creditor Reference)</param>
        public Reference (ReferenceType referenceType, string reference, ReferenceTextType? referenceTextType)
        {
            var res = new MyRes ("Messages,Swiss");

            this.RefType = referenceType;
            this._ReferenceTextType = referenceTextType;

            if (referenceType == ReferenceType.NON && reference != null)
            {
                throw new SwissQrCodeException (res.Get ("SwissRefTypeNon"));
            }

            if (referenceType != ReferenceType.NON && reference != null && referenceTextType == null)
            {
                throw new SwissQrCodeException (res.Get ("SwissRefTextTypeNon"));
            }

            if (referenceTextType == ReferenceTextType.QrReference && reference != null && (reference.Length > 27))
            {
                throw new SwissQrCodeException (res.Get ("SwissRefQRLength"));
            }

            if (referenceTextType == ReferenceTextType.QrReference && reference != null &&
                !Regex.IsMatch (reference, @"^[0-9]+$"))
            {
                throw new SwissQrCodeException (res.Get ("SwissRefQRNotOnlyDigits"));
            }

            if (referenceTextType == ReferenceTextType.QrReference && reference != null && !ChecksumMod10 (reference))
            {
                throw new SwissQrCodeException (res.Get ("SwissRefQRCheckSum"));
            }

            if (referenceTextType == ReferenceTextType.CreditorReferenceIso11649 && reference != null &&
                (reference.Length > 25))
            {
                throw new SwissQrCodeException (res.Get ("SwissRefISOLength"));
            }

            this.reference = reference;
        }


        public Reference (string reference)
        {
            string[] data = reference.Split ('\r');

            switch (data[0].Trim())
            {
                case "QRR":
                    RefType = ReferenceType.QRR;
                    break;
                case "SCOR":
                    RefType = ReferenceType.SCOR;
                    break;
                case "NON":
                    RefType = ReferenceType.NON;
                    break;
            }

            this.reference = data[1].Trim();
        }

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

        public bool ChecksumMod10 (string digits)
        {
            if (string.IsNullOrEmpty (digits) || digits.Length < 2)
            {
                return false;
            }

            var mods = new int[] { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };

            var remainder = 0;
            for (var i = 0; i < digits.Length - 1; i++)
            {
                var num = Convert.ToInt32 (digits[i]) - 48;
                remainder = mods[(num + remainder) % 10];
            }

            var checksum = (10 - remainder) % 10;
            return checksum == Convert.ToInt32 (digits[digits.Length - 1]) - 48;
        }
    }

    public class Contact
    {
        private List<string> twoLetterCodes;
        private string br = "\r\n";
        private AddressType adrType;

        public string Name { get; set; }

        public string StreetOrAddressline { get; set; }

        public string HouseNumberOrAddressline { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        /// <summary>
        /// Contact type. Can be used for payee, ultimate payee, etc. with address in structured mode (S).
        /// </summary>
        /// <param name="name">Last name or company (optional first name)</param>
        /// <param name="zipCode">Zip-/Postcode</param>
        /// <param name="city">City name</param>
        /// <param name="country">Two-letter country code as defined in ISO 3166-1</param>
        /// <param name="street">Streetname without house number</param>
        /// <param name="houseNumber">House number</param>
        public Contact (string name, string zipCode, string city, string country, string street, string houseNumber) :
            this (name, zipCode, city, country, street, houseNumber, AddressType.StructuredAddress)
        {
        }

        /// <summary>
        /// Contact type. Can be used for payee, ultimate payee, etc. with address in combined mode (K).
        /// </summary>
        /// <param name="name">Last name or company (optional first name)</param>
        /// <param name="country">Two-letter country code as defined in ISO 3166-1</param>
        /// <param name="addressLine1">Adress line 1</param>
        /// <param name="addressLine2">Adress line 2</param>
        public Contact (string name, string country, string addressLine1, string addressLine2) : this (name, null, null,
            country, addressLine1, addressLine2, AddressType.CombinedAddress)
        {
        }

        private Contact (string name, string zipCode, string city, string country, string streetOrAddressline1,
            string houseNumberOrAddressline2, AddressType addressType)
        {
            twoLetterCodes = ValidTwoLetterCodes();
            var res = new MyRes ("Messages,Swiss");
            var resForms = new MyRes ("Forms,BarcodeEditor,Swiss");

            //Pattern extracted from https://qr-validation.iso-payments.ch as explained in https://github.com/codebude/QRCoder/issues/97
            var charsetPattern =
                @"^([a-zA-Z0-9\.,;:'\ \+\-/\(\)?\*\[\]\{\}\\`´~ ]|[!""#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*$";

            adrType = addressType;

            if (string.IsNullOrEmpty (name))
            {
                throw new SwissQrCodeContactException (string.Format (res.Get ("SwissEmptyProperty"),
                    resForms.Get ("Name")));
            }

            if (name.Length > 70)
            {
                throw new SwissQrCodeContactException (string.Format (res.Get ("SwissLengthMore"),
                    resForms.Get ("Name"), 71));
            }

            if (!Regex.IsMatch (name, charsetPattern))
            {
                throw new SwissQrCodeContactException (
                    string.Format (res.Get ("SwissPatternError"), resForms.Get ("Name")) + charsetPattern);
            }

            this.Name = name;

            if (AddressType.StructuredAddress == adrType)
            {
                if (!string.IsNullOrEmpty (streetOrAddressline1) && (streetOrAddressline1.Length > 70))
                {
                    throw new SwissQrCodeContactException (string.Format (res.Get ("SwissLengthMore"),
                        resForms.Get ("Street"), 71));
                }

                if (!string.IsNullOrEmpty (streetOrAddressline1) &&
                    !Regex.IsMatch (streetOrAddressline1, charsetPattern))
                {
                    throw new SwissQrCodeContactException (
                        string.Format (res.Get ("SwissPatternError"), resForms.Get ("Street")) + charsetPattern);
                }

                this.StreetOrAddressline = streetOrAddressline1;

                if (!string.IsNullOrEmpty (houseNumberOrAddressline2) && houseNumberOrAddressline2.Length > 16)
                {
                    throw new SwissQrCodeContactException (string.Format (res.Get ("SwissLengthMore"),
                        resForms.Get ("HouseNumber"), 71));
                }

                this.HouseNumberOrAddressline = houseNumberOrAddressline2;
            }
            else
            {
                if (!string.IsNullOrEmpty (streetOrAddressline1) && (streetOrAddressline1.Length > 70))
                {
                    throw new SwissQrCodeContactException (string.Format (res.Get ("SwissLengthMore"), "Address line 1",
                        71));
                }

                if (!string.IsNullOrEmpty (streetOrAddressline1) &&
                    !Regex.IsMatch (streetOrAddressline1, charsetPattern))
                {
                    throw new SwissQrCodeContactException (
                        string.Format (res.Get ("SwissPatternError"), "Address line 1") + charsetPattern);
                }

                this.StreetOrAddressline = streetOrAddressline1;

                if (string.IsNullOrEmpty (houseNumberOrAddressline2))
                {
                    throw new SwissQrCodeContactException (res.Get ("SwissAddressLine2Error"));
                }

                if (!string.IsNullOrEmpty (houseNumberOrAddressline2) && (houseNumberOrAddressline2.Length > 70))
                {
                    throw new SwissQrCodeContactException (string.Format (res.Get ("SwissLengthMore"), "Address line 2",
                        71));
                }

                if (!string.IsNullOrEmpty (houseNumberOrAddressline2) &&
                    !Regex.IsMatch (houseNumberOrAddressline2, charsetPattern))
                {
                    throw new SwissQrCodeContactException (
                        string.Format (res.Get ("SwissPatternError"), "Address line 2") + charsetPattern);
                }

                this.HouseNumberOrAddressline = houseNumberOrAddressline2;
            }

            if (AddressType.StructuredAddress == adrType)
            {
                if (string.IsNullOrEmpty (zipCode))
                {
                    throw new SwissQrCodeContactException (string.Format (res.Get ("SwissEmptyProperty"),
                        resForms.Get ("ZipCode")));
                }

                if (zipCode.Length > 16)
                {
                    throw new SwissQrCodeContactException (string.Format (res.Get ("SwissLengthMore"),
                        resForms.Get ("ZipCode"), 17));
                }

                if (!Regex.IsMatch (zipCode, charsetPattern))
                {
                    throw new SwissQrCodeContactException (
                        string.Format (res.Get ("SwissPatternError"), resForms.Get ("ZipCode")) + charsetPattern);
                }

                this.ZipCode = zipCode;

                if (string.IsNullOrEmpty (city))
                {
                    throw new SwissQrCodeContactException (string.Format (res.Get ("SwissEmptyProperty"),
                        resForms.Get ("City")));
                }

                if (city.Length > 35)
                {
                    throw new SwissQrCodeContactException (string.Format (res.Get ("SwissLengthMore"),
                        resForms.Get ("City"), 36));
                }

                if (!Regex.IsMatch (city, charsetPattern))
                {
                    throw new SwissQrCodeContactException (
                        string.Format (res.Get ("SwissPatternError"), resForms.Get ("City")) + charsetPattern);
                }

                this.City = city;
            }
            else
            {
                this.ZipCode = this.City = string.Empty;
            }

            if (!IsValidTwoLetterCode (country))
            {
                throw new SwissQrCodeContactException (res.Get ("SwissCountryTwoLetters"));
            }

            this.Country = country;
        }

        private bool IsValidTwoLetterCode (string code)
        {
            return twoLetterCodes.Contains (code);
        }

        private List<string> ValidTwoLetterCodes()
        {
            string[] codes = new string[]
            {
                "AF", "AL", "DZ", "AS", "AD", "AO", "AI", "AQ", "AG", "AR", "AM", "AW", "AU", "AT", "AZ", "BS", "BH",
                "BD", "BB", "BY", "BE", "BZ", "BJ", "BM", "BT", "BO", "BQ", "BA", "BW", "BV", "BR", "IO", "BN", "BG",
                "BF", "BI", "CV", "KH", "CM", "CA", "KY", "CF", "TD", "CL", "CN", "CX", "CC", "CO", "KM", "CG", "CD",
                "CK", "CR", "CI", "HR", "CU", "CW", "CY", "CZ", "DK", "DJ", "DM", "DO", "EC", "EG", "SV", "GQ", "ER",
                "EE", "SZ", "ET", "FK", "FO", "FJ", "FI", "FR", "GF", "PF", "TF", "GA", "GM", "GE", "DE", "GH", "GI",
                "GR", "GL", "GD", "GP", "GU", "GT", "GG", "GN", "GW", "GY", "HT", "HM", "VA", "HN", "HK", "HU", "IS",
                "IN", "ID", "IR", "IQ", "IE", "IM", "IL", "IT", "JM", "JP", "JE", "JO", "KZ", "KE", "KI", "KP", "KR",
                "KW", "KG", "LA", "LV", "LB", "LS", "LR", "LY", "LI", "LT", "LU", "MO", "MG", "MW", "MY", "MV", "ML",
                "MT", "MH", "MQ", "MR", "MU", "YT", "MX", "FM", "MD", "MC", "MN", "ME", "MS", "MA", "MZ", "MM", "NA",
                "NR", "NP", "NL", "NC", "NZ", "NI", "NE", "NG", "NU", "NF", "MP", "MK", "NO", "OM", "PK", "PW", "PS",
                "PA", "PG", "PY", "PE", "PH", "PN", "PL", "PT", "PR", "QA", "RE", "RO", "RU", "RW", "BL", "SH", "KN",
                "LC", "MF", "PM", "VC", "WS", "SM", "ST", "SA", "SN", "RS", "SC", "SL", "SG", "SX", "SK", "SI", "SB",
                "SO", "ZA", "GS", "SS", "ES", "LK", "SD", "SR", "SJ", "SE", "CH", "SY", "TW", "TJ", "TZ", "TH", "TL",
                "TG", "TK", "TO", "TT", "TN", "TR", "TM", "TC", "TV", "UG", "UA", "AE", "GB", "US", "UM", "UY", "UZ",
                "VU", "VE", "VN", "VG", "VI", "WF", "EH", "YE", "ZM", "ZW", "AX"
            };
            List<string> codesList = new List<string>();

            foreach (var str in codes)
            {
                codesList.Add (str);
            }

            return codesList;
        }

        public Contact (string contact)
        {
            string[] data = contact.Split ('\r');
            if (data[0].Trim() == "S")
            {
                adrType = AddressType.StructuredAddress;
            }
            else
            {
                adrType = AddressType.CombinedAddress;
            }

            Name = data[1].Trim();
            StreetOrAddressline = data[2].Trim();
            HouseNumberOrAddressline = data[3].Trim();
            ZipCode = data[4].Trim();
            City = data[5].Trim();
            Country = data[6].Trim();
        }

        public override string ToString()
        {
            var contactData = ""; //AdrTp
            if (AddressType.StructuredAddress == adrType)
            {
                contactData += "S";
            }
            else
            {
                contactData += "K";
            }

            contactData += br;
            contactData += Name.Replace ("\n", "") + br; //Name
            contactData += (!string.IsNullOrEmpty (StreetOrAddressline)
                ? StreetOrAddressline.Replace ("\n", "")
                : string.Empty) + br; //StrtNmOrAdrLine1
            contactData += (!string.IsNullOrEmpty (HouseNumberOrAddressline)
                ? HouseNumberOrAddressline.Replace ("\n", "")
                : string.Empty) + br; //BldgNbOrAdrLine2
            contactData += ZipCode.Replace ("\n", "") + br; //PstCd
            contactData += City.Replace ("\n", "") + br; //TwnNm
            contactData += Country + br; //Ctry
            return contactData;
        }

        public enum AddressType
        {
            StructuredAddress,
            CombinedAddress
        }

        public class SwissQrCodeContactException : SwissQrCodeException
        {
            public SwissQrCodeContactException()
            {
            }

            public SwissQrCodeContactException (string message)
                : base (message)
            {
            }

            public SwissQrCodeContactException (string message, SwissQrCodeException inner)
                : base (message, inner)
            {
            }
        }
    }

    public class Iban
    {
        public IbanType? TypeIban { get; set; }

        public string _Iban { get; set; }

        /// <summary>
        /// IBAN object with type information
        /// </summary>
        /// <param name="iban">IBAN</param>
        /// <param name="ibanType">Type of IBAN (normal or QR-IBAN)</param>
        public Iban (string iban, IbanType ibanType)
        {
            var res = new MyRes ("Messages,Swiss");
            if (ibanType == IbanType.Iban && !IsValidIban (iban))
            {
                throw new SwissQrCodeException (res.Get ("SwissIbanNotValid"));
            }

            if (ibanType == IbanType.QrIban && !IsValidQRIban (iban))
            {
                throw new SwissQrCodeException (res.Get ("SwissQRIbanNotValid"));
            }

            if (!iban.StartsWith ("CH") && !iban.StartsWith ("LI"))
            {
                throw new SwissQrCodeException ("SwissQRStartNotValid");
            }

            this._Iban = iban;
            this.TypeIban = ibanType;
        }

        public bool IsQrIban => TypeIban == IbanType.QrIban;

        public Iban (string iban)
        {
            this._Iban = iban;
        }

        public override string ToString()
        {
            return _Iban.Replace ("-", "").Replace ("\n", "").Replace (" ", "");
        }

        public enum IbanType
        {
            Iban,
            QrIban
        }

        private bool IsValidIban (string iban)
        {
            //Clean IBAN
            var ibanCleared = iban.ToUpper().Replace (" ", "").Replace ("-", "");

            //Check for general structure
            var structurallyValid = Regex.IsMatch (ibanCleared, @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]?){16,30}$");

            //Check IBAN checksum
            var charSum = (ibanCleared.Substring (4) + ibanCleared.Substring (0, 4)).ToCharArray();
            var sum = "";

            foreach (var c in charSum)
            {
                sum += (char.IsLetter (c) ? (c - 55).ToString() : c.ToString());
            }

            if (!decimal.TryParse (sum, out var sumDec))
            {
                return false;
            }

            var checksumValid = (sumDec % 97) == 1;

            return structurallyValid && checksumValid;
        }

        private bool IsValidQRIban (string iban)
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
    }

    public enum Currency
    {
        CHF = 756,
        EUR = 978
    }
}
