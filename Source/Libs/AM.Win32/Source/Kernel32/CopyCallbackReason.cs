// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CopyCallbackReason.cs -- reason that CopyProgressRoutine was called
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// Reason that CopyProgressRoutine was called.
    /// </summary>
    public enum CopyCallbackReason
    {
        /// <summary>
        /// Another part of the data file was copied.
        /// </summary>
        CALLBACK_CHUNK_FINISHED = 0x00000000,

        /// <summary>
        /// Another stream was created and is about to be copied.
        /// This is the callback reason given when the callback
        /// routine is first invoked.
        /// </summary>
        CALLBACK_STREAM_SWITCH = 0x00000001

    } // enum CopyCallbackReason

} // namespace AM.Win32
