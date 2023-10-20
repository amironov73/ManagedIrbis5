// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IChatMessage.cs -- общий интерфейс сообщений чата
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace GptForMe;

/// <summary>
/// Общий интерфейс сообщений чата.
/// </summary>
[PublicAPI]
public interface IChatMessage
{
    /// <summary>
    /// Содержимое сообщения.
    /// </summary>
    string? MessageContent { get; set; }
}
