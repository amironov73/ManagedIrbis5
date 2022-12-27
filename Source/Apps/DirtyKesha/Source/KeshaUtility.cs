// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* KeshaUtility.cs -- различные полезные методы для Кеши
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Security;

#endregion

#nullable enable

namespace DirtyKesha;

/// <summary>
/// Различные полезные методы для Кеши.
/// </summary>
public static class KeshaUtility
{

    #region Public metthods

    public static string EncodeTheOrder
        (
            string order
        )
    {
        Sure.NotNullNorEmpty (order);

        return "//" + SecurityUtility.EncryptToBase64 (order);
    }

    public static string? DecodeTheOrder
        (
            string encrypted
        )
    {
        Sure.NotNullNorEmpty (encrypted);

        if (encrypted.Length < 3 || encrypted[0] != '/' || encrypted[1] != '/')
        {
            return null;
        }

        return SecurityUtility.DecryptFromBase64 (encrypted[2..]);
    }

    #endregion
}
