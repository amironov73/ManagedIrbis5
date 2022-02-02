// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DataTooLongException.cs -- исключение, возбуждаемое при превышении допустимой длины данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Исключение, возбуждаемое при превышении допустимой длины данных.
/// </summary>
public sealed class DataTooLongException
    : ApplicationException
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DataTooLongException
        (
            string eccLevel,
            string encodingMode,
            int maxSizeByte
        )
        : base
            (
                $"The given payload exceeds the maximum size of the QR code standard. The maximum size allowed for the choosen paramters (ECC level={eccLevel}, EncodingMode={encodingMode}) is {maxSizeByte} byte."
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DataTooLongException
        (
            string eccLevel,
            string encodingMode,
            int version,
            int maxSizeByte
        )
        : base
            (
                $"The given payload exceeds the maximum size of the QR code standard. The maximum size allowed for the choosen paramters (ECC level={eccLevel}, EncodingMode={encodingMode}, FixedVersion={version}) is {maxSizeByte} byte."
            )
    {
        // пустое тело конструктора
    }

    #endregion
}
