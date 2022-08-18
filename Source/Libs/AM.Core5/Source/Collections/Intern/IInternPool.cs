// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IInternPool.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Collections.Intern;

/// <summary>
///
/// </summary>
public interface IInternPool
{
    /// <summary>
    ///
    /// </summary>
    long Added { get; }

    /// <summary>
    ///
    /// </summary>
    long Considered { get; }

    /// <summary>
    ///
    /// </summary>
    int Count { get; }

    /// <summary>
    ///
    /// </summary>
    long Deduped { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool Contains (string item);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    string Intern (ReadOnlySpan<char> value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    string? Intern (string? value);

    /// <summary>
    ///
    /// </summary>
    /// <param name="asciiValue"></param>
    /// <returns></returns>
    string InternAscii (ReadOnlySpan<byte> asciiValue);

    /// <summary>
    ///
    /// </summary>
    /// <param name="utf8Value"></param>
    /// <returns></returns>
    string InternUtf8 (ReadOnlySpan<byte> utf8Value);

#if NET5_0 || NETCOREAPP3_1
        string Intern(char[] value) => Intern(new ReadOnlySpan<char>(value));
        string InternAscii(byte[] asciiValue) => InternAscii(new ReadOnlySpan<byte>(asciiValue));
        string InternUtf8(byte[] utf8Value) => InternUtf8(new ReadOnlySpan<byte>(utf8Value));
#endif
}
