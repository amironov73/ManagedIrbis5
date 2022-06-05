// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* ClientLM.cs -- менеджер клиентских лицензий
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Linq;
using System.Text;

using AM;
using AM.IO;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Менеджер клиентских лицензий.
/// </summary>
public sealed class ClientLM
{
    #region Constants

    /// <summary>
    /// Соль по умолчанию.
    /// </summary>
    public const string DefaultSalt = "Ассоциация ЭБНИТ";

    #endregion

    #region Properties

    /// <summary>
    /// Кодировка символов.
    /// </summary>
    public Encoding Encoding { get; }

    /// <summary>
    /// Текущая соль.
    /// </summary>
    public string? Salt { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ClientLM()
        : this
            (
                IrbisEncoding.Ansi,
                DefaultSalt
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="encoding">Кодировка символов.</param>
    /// <param name="salt">Соль</param>
    public ClientLM
        (
            Encoding encoding,
            string? salt
        )
    {
        Sure.NotNull (encoding);

        Encoding = encoding;
        Salt = salt;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка хеша из INI-файла <see cref="IniFile"/>
    /// (работает как в ИРБИС32, так и в ИРБИС64).
    /// </summary>
    public bool CheckHash
        (
            IniFile iniFile
        )
    {
        Sure.NotNull (iniFile);

        var user = iniFile.GetValue ("Main", "User", null);
        var common = iniFile.GetValue ("Main", "Common", null);

        if (string.IsNullOrEmpty (user)
            || string.IsNullOrEmpty (common))
        {
            return false;
        }

        var hash = ComputeHash (user);

        return hash == common;
    }

    /// <summary>
    /// Вычисление хеша (работает как в ИРБИС32, так и в ИРБИС64).
    /// </summary>
    public string ComputeHash
        (
            string text
        )
    {
        Sure.NotNull (text);

        var salted = Salt + text;
        var raw = Encoding.GetBytes (salted);
        unchecked
        {
            var sum = 0;
            foreach (var one in raw)
            {
                sum += one;
            }

            raw = Encoding.GetBytes
                    (
                        sum.ToString (CultureInfo.InvariantCulture)
                    )
                .Reverse()
                .ToArray();

            for (var i = 0; i < raw.Length; i++)
            {
                raw[i] += 0x6D;
            }
        }

        var result = Encoding.GetString (raw, 0, raw.Length);

        return result;
    }

    #endregion
}
