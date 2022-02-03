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

/* WiFi.cs -- Wi-Fi
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using static AM.Drawing.QRCoding.PayloadGenerator;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

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
        _ssid = escapeHexStrings && IsHexStyle (_ssid) ? "\"" + _ssid + "\"" : _ssid;
        _password = EscapeInput (password);
        _password = escapeHexStrings && IsHexStyle (_password) ? "\"" + _password + "\"" : _password;
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
