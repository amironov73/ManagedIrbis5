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

/* BezahlCode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

using static AM.Drawing.QRCoding.PayloadGenerator;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
///
/// </summary>
public sealed class BezahlCode
    : Payload
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
