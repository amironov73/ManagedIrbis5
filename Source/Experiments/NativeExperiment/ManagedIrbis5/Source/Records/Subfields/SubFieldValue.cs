// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* SubFieldValue.cs -- subfield value related routines
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Subfield value related routines.
/// </summary>
public static class SubFieldValue
{
    #region Properties

    /// <summary>
    /// Throw exception on verification error.
    /// </summary>
    public static bool ThrowOnVerify { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Whether the value valid.
    /// </summary>
    public static bool IsValidValue
        (
            ReadOnlySpan<char> value
        )
    {
        foreach (var c in value)
        {
            if (c == SubField.Delimiter)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// SubField value normalization.
    /// </summary>
    public static string? Normalize
        (
            string? value
        )
    {
        if (string.IsNullOrWhiteSpace (value))
        {
            return value;
        }

        var result = value.Trim();

        return result;
    }

    /// <summary>
    /// Verify subfield value.
    /// </summary>
    public static bool Verify (ReadOnlySpan<char> value) =>
        Verify (value, ThrowOnVerify);

    /// <summary>
    /// Verify subfield value.
    /// </summary>
    public static bool Verify
        (
            ReadOnlySpan<char> value,
            bool throwOnError
        )
    {
        var result = IsValidValue (value);

        if (!result)
        {
            Magna.Logger.LogDebug
                (
                    nameof (SubFieldValue) + "::" + nameof (Verify)
                    + ": {VerificationError}" ,
                    value.ToVisibleString()
                );

            if (throwOnError)
            {
                throw new VerificationException (nameof (SubField.Value));
            }
        }

        return result;
    }

    #endregion
}
