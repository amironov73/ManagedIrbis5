// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* OrderResult.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace BeriChitai.Data;

public sealed class OrderResult
{
    public bool Ok { get; set; }

    public string? Message { get; set; }
}
