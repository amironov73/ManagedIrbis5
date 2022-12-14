// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ECCLevel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Skia.QrCoding;

/// <summary>
///
/// </summary>
public enum ECCLevel
{
    /// <summary>
    /// 7% may be lost before recovery is not possible
    /// </summary>
    L,
    /// <summary>
    /// 15% may be lost before recovery is not possible
    /// </summary>
    M,
    /// <summary>
    /// 25% may be lost before recovery is not possible
    /// </summary>
    Q,
    /// <summary>
    /// 30% may be lost before recovery is not possible
    /// </summary>
    H
}
