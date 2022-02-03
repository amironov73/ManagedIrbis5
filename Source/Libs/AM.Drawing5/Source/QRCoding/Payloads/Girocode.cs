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

/* Girocode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using static AM.Drawing.QRCoding.PayloadGenerator;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// The European Payments Council Quick Response Code
/// guidelines define the content of a QR code that
/// can be used to initiate SEPA credit transfer (SCT).
/// </summary>
public sealed class Girocode
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
        if (amount.ToInvariantString().Replace (",", ".").Contains (".") &&
            amount.ToInvariantString().Replace (",", ".").Split ('.')[1].TrimEnd ('0').Length > 2)
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

    private readonly string
        _iban, _bic, _name, _purposeOfCreditTransfer, _remittanceInformation, _messageToGirocodeUser;

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

    /// <summary>
    ///
    /// </summary>
    public enum GirocodeVersion
    {
        /// <summary>
        ///
        /// </summary>
        Version1,

        /// <summary>
        ///
        /// </summary>
        Version2
    }

    /// <summary>
    ///
    /// </summary>
    public enum TypeOfRemittance
    {
        /// <summary>
        ///
        /// </summary>
        Structured,

        /// <summary>
        ///
        /// </summary>
        Unstructured
    }

    /// <summary>
    ///
    /// </summary>
    public enum GirocodeEncoding
    {
        /// <summary>
        ///
        /// </summary>
        UTF_8,

        /// <summary>
        ///
        /// </summary>
        ISO_8859_1,

        /// <summary>
        ///
        /// </summary>
        ISO_8859_2,

        /// <summary>
        ///
        /// </summary>
        ISO_8859_4,

        /// <summary>
        ///
        /// </summary>
        ISO_8859_5,

        /// <summary>
        ///
        /// </summary>
        ISO_8859_7,

        /// <summary>
        ///
        /// </summary>
        ISO_8859_10,

        /// <summary>
        ///
        /// </summary>
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
