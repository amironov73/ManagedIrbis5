// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* SecurityUtility.cs -- вспомогательные методы для обеспечения безопасности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using AM.IO;
using AM.Text;

#endregion

#nullable enable

#pragma warning disable SYSLIB0041 // использование устаревших типов

namespace AM.Security;

/// <summary>
/// Вспомогательные методы для обеспечения безопасности.
/// </summary>
public static class SecurityUtility
{
    #region Public methods

    /// <summary>
    /// Получение сертификата для SslStream.
    /// </summary>
    public static X509Certificate GetSslCertificate()
    {
        var assembly = typeof (SecurityUtility).Assembly;
        var resourceName = "AM.Core.ArsMagnaSslSocket.cer";
        using Stream? stream = assembly.GetManifestResourceStream (resourceName);
        if (ReferenceEquals (stream, null))
        {
            throw new ArsMagnaException();
        }

        var rawData = stream.ReadToEnd();
        var result = new X509Certificate (rawData, "nH38RavkcNLr");

        // new X509Certificate();
        // result.Import (rawData);

        return result;
    }

    /// <summary>
    /// Получение сертификата для указанного субъекта
    /// </summary>
    /// Get certificate by the subject.
    public static X509Certificate GetRootCertificate
        (
            string subject
        )
    {
        Sure.NotNullNorEmpty (subject);

        using var store = new X509Store (StoreName.Root, StoreLocation.LocalMachine);
        store.Open (OpenFlags.ReadOnly);
        foreach (X509Certificate2 certificate in store.Certificates)
        {
            if (string.Compare (certificate.Subject, subject,
                    StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return certificate;
            }
        }

        throw new ArsMagnaException();
    }

    /// <summary>
    /// Простое шифрование текста до неузнаваемости.
    /// </summary>
    /// <param name="secretText">Например, строка подключения,
    /// содержащая чувствительные данные.</param>
    /// <param name="password">Пароль.</param>
    /// <returns>Зашифрованный текст.</returns>
    public static string Encrypt
        (
            string secretText,
            string password
        )
    {
        var plainBytes = Encoding.UTF8.GetBytes (secretText);
        var symmetricAlgorithm = Aes.Create();
        var passwordBytes = new Rfc2898DeriveBytes (password, new byte[16])
            .GetBytes (16);

        var encryptor = symmetricAlgorithm.CreateEncryptor
            (
                passwordBytes,
                new byte[16]
            );

        var memory = new MemoryStream();
        var crypto = new CryptoStream (memory, encryptor, CryptoStreamMode.Write);
        crypto.Write (plainBytes, 0, plainBytes.Length);
        crypto.FlushFinalBlock();
        var encryptedBytes = memory.ToArray();
        var result = Convert.ToBase64String (encryptedBytes);

        return result;
    }

    /// <summary>
    /// Расшифровка ранее зашифрованного текста.
    /// </summary>
    /// <param name="encryptedText">Текст, полученный от <see cref="Encrypt"/></param>
    /// <param name="password">Пароль.</param>
    /// <returns>Расшифрованный текст.</returns>
    public static string Decrypt
        (
            string encryptedText,
            string password
        )
    {
        var encryptedBytes = Convert.FromBase64String (encryptedText);
        var symmetricAlgorithm = Aes.Create();
        var passwordBytes = new Rfc2898DeriveBytes (password, new byte[16])
            .GetBytes (16);

        var decryptor = symmetricAlgorithm.CreateDecryptor
            (
                passwordBytes,
                new byte[16]
            );

        var memory = new MemoryStream (encryptedBytes);
        var crypto = new CryptoStream (memory, decryptor, CryptoStreamMode.Read);
        var reader = new BinaryReader (crypto);
        var decryptedBytes = reader.ReadBytes (encryptedBytes.Length);
        var result = Encoding.UTF8.GetString (decryptedBytes);

        return result;
    }

    /// <summary>
    /// Псевдошифрование в Base64.
    /// </summary>
    public static string EncryptToBase64
        (
            string text
        )
    {
        var bytes = Encoding.UTF8.GetBytes (text);
        var result = Convert.ToBase64String (bytes)
            .Replace ("=", string.Empty);

        return result;
    }

    /// <summary>
    /// Псевдорасшифровка из Base64.
    /// </summary>
    public static string DecryptFromBase64
        (
            string text
        )
    {
        while (text.Length % 4 != 0)
        {
            text += "=";
        }

        var bytes = Convert.FromBase64String (text);
        var result = Encoding.UTF8.GetString (bytes);

        return result;
    }

    /// <summary>
    /// Вычисление хеша для указанной строки.
    /// </summary>
    public static string ComputeMD5
        (
            string text
        )
    {
        var bytes = Encoding.UTF8.GetBytes (text);

        return ComputeMD5 (bytes);
    }

    /// <summary>
    /// Вычисление хеша для указанных данных.
    /// </summary>
    public static string ComputeMD5
        (
            byte[] bytes
        )
    {
        using var md5 = MD5.Create();
        var data = md5.ComputeHash (bytes);

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (data.Length * 2);
        foreach (var b in data)
        {
            builder.Append (b.ToString ("x2"));
        }

        return builder.ToString();
    }

    #endregion
}
