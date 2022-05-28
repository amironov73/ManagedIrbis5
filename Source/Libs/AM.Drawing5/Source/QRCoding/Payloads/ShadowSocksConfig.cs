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

/* ShadowSocksConfig.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
///
/// </summary>
public class ShadowSocksConfig
    : Payload
{
    private readonly string hostname, password, methodStr;
    private readonly string? tag, parameter;
    private readonly Method method;
    private readonly int port;

    private readonly Dictionary<string, string> encryptionTexts = new ()
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
    public ShadowSocksConfig
        (
            string hostname,
            int port,
            string password,
            Method method,
            string? tag = null
        ) :
        this (hostname, port, password, method, null, tag)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="password"></param>
    /// <param name="method"></param>
    /// <param name="plugin"></param>
    /// <param name="pluginOption"></param>
    /// <param name="tag"></param>
    public ShadowSocksConfig
        (
            string hostname,
            int port,
            string password,
            Method method,
            string plugin,
            string pluginOption,
            string? tag = null
        ) :
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

    /// <summary>
    ///
    /// </summary>
    private readonly Dictionary<string, string> UrlEncodeTable = new ()
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="password"></param>
    /// <param name="method"></param>
    /// <param name="parameters"></param>
    /// <param name="tag"></param>
    /// <exception cref="ShadowSocksConfigException"></exception>
    public ShadowSocksConfig
        (
            string hostname,
            int port,
            string password,
            Method method,
            Dictionary<string, string>? parameters,
            string? tag = null
        )
    {
        this.hostname = Uri.CheckHostName (hostname) == UriHostNameType.IPv6
            ? $"[{hostname}]"
            : hostname;
        if (port < 1 || port > 65535)
        {
            throw new ShadowSocksConfigException ("Value of 'port' must be within 0 and 65535.");
        }

        this.port = port;
        this.password = password;
        this.method = method;
        methodStr = encryptionTexts[method.ToString()];
        this.tag = tag;

        if (parameters != null)
        {
            parameter =
                string.Join ("&",
                    parameters.Select (
                            kv => $"{UrlEncode (kv.Key)}={UrlEncode (kv.Value)}"
                        ).ToArray());
        }
    }

    /// <inheritdoc cref="Payload.ToString"/>
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

    /// <summary>
    ///
    /// </summary>
    public enum Method
    {
        /// <summary>
        ///
        /// </summary>
        // AEAD
        Chacha20IetfPoly1305,

        /// <summary>
        ///
        /// </summary>
        Aes128Gcm,

        /// <summary>
        ///
        /// </summary>
        Aes192Gcm,

        /// <summary>
        ///
        /// </summary>
        Aes256Gcm,

        /// <summary>
        ///
        /// </summary>
        // AEAD, not standard
        XChacha20IetfPoly1305,

        /// <summary>
        ///
        /// </summary>
        // Stream cipher
        Aes128Cfb,

        /// <summary>
        ///
        /// </summary>
        Aes192Cfb,

        /// <summary>
        ///
        /// </summary>
        Aes256Cfb,

        /// <summary>
        ///
        /// </summary>
        Aes128Ctr,

        /// <summary>
        ///
        /// </summary>
        Aes192Ctr,

        /// <summary>
        ///
        /// </summary>
        Aes256Ctr,

        /// <summary>
        ///
        /// </summary>
        Camellia128Cfb,

        /// <summary>
        ///
        /// </summary>
        Camellia192Cfb,

        /// <summary>
        ///
        /// </summary>
        Camellia256Cfb,

        /// <summary>
        ///
        /// </summary>
        Chacha20Ietf,

        /// <summary>
        ///
        /// </summary>
        // alias of Aes256Cfb
        Aes256Cb,

        /// <summary>
        ///
        /// </summary>
        // Stream cipher, not standard
        Aes128Ofb,

        /// <summary>
        ///
        /// </summary>
        Aes192Ofb,

        /// <summary>
        ///
        /// </summary>
        Aes256Ofb,

        /// <summary>
        ///
        /// </summary>
        Aes128Cfb1,

        /// <summary>
        ///
        /// </summary>
        Aes192Cfb1,

        /// <summary>
        ///
        /// </summary>
        Aes256Cfb1,

        /// <summary>
        ///
        /// </summary>
        Aes128Cfb8,

        /// <summary>
        ///
        /// </summary>
        Aes192Cfb8,

        /// <summary>
        ///
        /// </summary>
        Aes256Cfb8,

        /// <summary>
        ///
        /// </summary>
        // Stream cipher, deprecated
        Chacha20,

        /// <summary>
        ///
        /// </summary>
        BfCfb,

        /// <summary>
        ///
        /// </summary>
        Rc4Md5,

        /// <summary>
        ///
        /// </summary>
        Salsa20,

        /// <summary>
        ///
        /// </summary>
        // Not standard and not in acitve use
        DesCfb,

        /// <summary>
        ///
        /// </summary>
        IdeaCfb,

        /// <summary>
        ///
        /// </summary>
        Rc2Cfb,

        /// <summary>
        ///
        /// </summary>
        Cast5Cfb,

        /// <summary>
        ///
        /// </summary>
        Salsa20Ctr,

        /// <summary>
        ///
        /// </summary>
        Rc4,

        /// <summary>
        ///
        /// </summary>
        SeedCfb,

        /// <summary>
        ///
        /// </summary>
        Table
    }

    /// <summary>
    /// Специфичное исключение.
    /// </summary>
    public sealed class ShadowSocksConfigException
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
