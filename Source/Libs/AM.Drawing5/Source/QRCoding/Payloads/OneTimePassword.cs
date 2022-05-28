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
using System.Text;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Одноразовый пароль.
/// </summary>
public class OneTimePassword
    : Payload
{
    /// <summary>
    ///
    /// </summary>
    //https://github.com/google/google-authenticator/wiki/Key-Uri-Format
    public OneTimePasswordAuthType Type { get; set; } = OneTimePasswordAuthType.TOTP;

    /// <summary>
    ///
    /// </summary>
    public string? Secret { get; set; }

    /// <summary>
    ///
    /// </summary>
    public OneTimePasswordAuthAlgorithm AuthAlgorithm { get; set; } = OneTimePasswordAuthAlgorithm.SHA1;

    /// <summary>
    ///
    /// </summary>
    [Obsolete ("This property is obsolete, use " + nameof (AuthAlgorithm) + " instead", false)]
    public OoneTimePasswordAuthAlgorithm Algorithm
    {
        get => (OoneTimePasswordAuthAlgorithm) Enum.Parse
            (
                typeof (OoneTimePasswordAuthAlgorithm),
                AuthAlgorithm.ToString()
            );

        set => AuthAlgorithm = (OneTimePasswordAuthAlgorithm) Enum.Parse
            (
                typeof (OneTimePasswordAuthAlgorithm),
                value.ToString()
            );
    }

    /// <summary>
    ///
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int Digits { get; set; } = 6;

    /// <summary>
    ///
    /// </summary>
    public int? Counter { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public int? Period { get; set; } = 30;

    /// <summary>
    ///
    /// </summary>
    public enum OneTimePasswordAuthType
    {
        /// <summary>
        ///
        /// </summary>
        TOTP,

        /// <summary>
        ///
        /// </summary>
        HOTP,
    }

    /// <summary>
    ///
    /// </summary>
    public enum OneTimePasswordAuthAlgorithm
    {
        /// <summary>
        ///
        /// </summary>
        SHA1,

        /// <summary>
        ///
        /// </summary>
        SHA256,

        /// <summary>
        ///
        /// </summary>
        SHA512,
    }

    /// <summary>
    ///
    /// </summary>
    [Obsolete ("This enum is obsolete, use " + nameof (OneTimePasswordAuthAlgorithm) + " instead", false)]
    public enum OoneTimePasswordAuthAlgorithm
    {
        /// <summary>
        ///
        /// </summary>
        SHA1,

        /// <summary>
        ///
        /// </summary>
        SHA256,

        /// <summary>
        ///
        /// </summary>
        SHA512,
    }

    /// <inheritdoc cref="Payload.ToString"/>
    public override string ToString()
    {
        return Type switch
        {
            OneTimePasswordAuthType.TOTP => TimeToString(),
            OneTimePasswordAuthType.HOTP => HMACToString(),
            _ => throw new ArgumentOutOfRangeException()
        };
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

        var strippedSecret = Secret.Replace (" ", "");
        string? escapedIssuer = null;
        string? label = null;

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
