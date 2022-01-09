// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BinaryAttachment.cs -- аттачмент к исключению
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.Contracts;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Аттачмент к исключению -- произвольные двоичные данные,
/// прикрепляемые к исключению.
/// </summary>
public sealed class BinaryAttachment
{
    #region Properties

    /// <summary>
    /// Name of the attachment.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Content of the attachment.
    /// </summary>
    public byte[]? Content { get; set; }

    #endregion

    #region Public methods

    /// <inheritdoc cref="object.ToString" />
    [Pure]
    public override string ToString()
    {
        return $"{Name.ToVisibleString()}: {Content?.Length} bytes";
    }

    #endregion
}
