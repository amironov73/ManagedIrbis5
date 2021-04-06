// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IAttachmentContainer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM
{
    /// <summary>
    /// Container of the attachments.
    /// </summary>
    public interface IAttachmentContainer
    {
        /// <summary>
        /// List attachments.
        /// </summary>
        BinaryAttachment[] ListAttachments();

    } // interface IAttachmentContainer

} // namespace AM
