// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* MessageWindowResultDTO.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Avalonia.DTO;

/// <summary>
///
/// </summary>
public class MessageWindowResultDTO
{
    /// <summary>
    ///
    /// </summary>
    public MessageWindowResultDTO
        (
            string message,
            string button
        )
    {
        Message = message;
        Button = button;
    }

    /// <summary>
    /// Result text
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Clicked button
    /// </summary>
    public string Button { get; }
}
