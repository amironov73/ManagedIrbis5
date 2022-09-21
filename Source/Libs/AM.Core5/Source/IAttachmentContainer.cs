// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IAttachmentContainer.cs -- список прикрепленных данных
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM;

/// <summary>
/// Контейнер для прикрепленных данных.
/// </summary>
public interface IAttachmentContainer
{
    /// <summary>
    /// Список прикрепленных данных.
    /// </summary>
    BinaryAttachment[] ListAttachments();
}
