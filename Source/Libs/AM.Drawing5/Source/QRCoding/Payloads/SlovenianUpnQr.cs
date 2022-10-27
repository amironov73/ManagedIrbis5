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

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

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
        return string.Format ("{0:00000000000}", _amt);
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
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("UPNQR");
        builder.Append ('\n').Append ('\n').Append ('\n').Append ('\n').Append ('\n');
        builder.Append (_payerName).Append ('\n');
        builder.Append (_payerAddress).Append ('\n');
        builder.Append (_payerPlace).Append ('\n');
        builder.Append (_amount).Append ('\n').Append ('\n').Append ('\n');
        builder.Append (_code.ToUpper()).Append ('\n');
        builder.Append (_purpose).Append ('\n');
        builder.Append (_deadLine).Append ('\n');
        builder.Append (_recipientIban.ToUpper()).Append ('\n');
        builder.Append (_recipientSiModel).Append (_recipientSiReference).Append ('\n');
        builder.Append (_recipientName).Append ('\n');
        builder.Append (_recipientAddress).Append ('\n');
        builder.Append (_recipientPlace).Append ('\n');
        builder.AppendFormat ("{0:000}", CalculateChecksum()).Append ('\n');

        return builder.ReturnShared();
    }
}
