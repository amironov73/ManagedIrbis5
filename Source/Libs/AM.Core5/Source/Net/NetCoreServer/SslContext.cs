// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* SslContext.cs --
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

#endregion

#nullable enable

namespace NetCoreServer;

/// <summary>
/// SSL context
/// </summary>
public class SslContext
{
    /// <summary>
    /// Initialize SSL context with default protocols
    /// </summary>
    public SslContext()
        : this (SslProtocols.Tls12)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initialize SSL context with given protocols
    /// </summary>
    /// <param name="protocols">SSL protocols</param>
    public SslContext
        (
            SslProtocols protocols
        )
    {
        Protocols = protocols;
    }

    /// <summary>
    /// Initialize SSL context with given protocols and certificate
    /// </summary>
    /// <param name="protocols">SSL protocols</param>
    /// <param name="certificate">SSL certificate</param>
    public SslContext
        (
            SslProtocols protocols,
            X509Certificate certificate
        )
        : this (protocols, certificate, null)
    {
        // пустое тело конструктора
    }
    /// <summary>
    /// Initialize SSL context with given protocols, certificate and validation callback
    /// </summary>
    /// <param name="protocols">SSL protocols</param>
    /// <param name="certificate">SSL certificate</param>
    /// <param name="certificateValidationCallback">SSL certificate</param>
    public SslContext
        (
            SslProtocols protocols,
            X509Certificate certificate,
            RemoteCertificateValidationCallback? certificateValidationCallback
        )
    {
        Protocols = protocols;
        Certificate = certificate;
        CertificateValidationCallback = certificateValidationCallback;
    }

    /// <summary>
    /// Initialize SSL context with given protocols and certificates collection
    /// </summary>
    /// <param name="protocols">SSL protocols</param>
    /// <param name="certificates">SSL certificates collection</param>
    public SslContext
        (
            SslProtocols protocols,
            X509Certificate2Collection certificates
        )
        : this (protocols, certificates, null)
    {
        // пустое тело конструктора
    }
    /// <summary>
    /// Initialize SSL context with given protocols, certificates collection and validation callback
    /// </summary>
    /// <param name="protocols">SSL protocols</param>
    /// <param name="certificates">SSL certificates collection</param>
    /// <param name="certificateValidationCallback">SSL certificate</param>
    public SslContext
        (
            SslProtocols protocols,
            X509Certificate2Collection certificates,
            RemoteCertificateValidationCallback? certificateValidationCallback
        )
    {
        Protocols = protocols;
        Certificates = certificates;
        CertificateValidationCallback = certificateValidationCallback;
    }

    /// <summary>
    /// SSL protocols
    /// </summary>
    public SslProtocols Protocols { get; set; }
    /// <summary>
    /// SSL certificate
    /// </summary>
    public X509Certificate? Certificate { get; set; }
    /// <summary>
    /// SSL certificates collection
    /// </summary>
    public X509Certificate2Collection? Certificates { get; set; }
    /// <summary>
    /// SSL certificate validation callback
    /// </summary>
    public RemoteCertificateValidationCallback? CertificateValidationCallback { get; set; }

    /// <summary>
    /// Is the client is asked for a certificate for authentication.
    /// Note that this is only a request - if no certificate is provided, the server still accepts the connection request.
    /// </summary>
    public bool ClientCertificateRequired { get; set; }
}
