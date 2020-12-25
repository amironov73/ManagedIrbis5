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

/* CopyProgressCode.cs -- the CopyProgressRoutine function should return one of the following values
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// The CopyProgressRoutine function should return
    /// one of the following values.
    /// </summary>
    public enum CopyProgressCode
    {
        /// <summary>
        /// Continue the copy operation.
        /// </summary>
        PROGRESS_CONTINUE = 0,

        /// <summary>
        /// Cancel the copy operation and delete the destination file.
        /// </summary>
        PROGRESS_CANCEL = 1,

        /// <summary>
        /// Stop the copy operation. It can be restarted at a later time.
        /// </summary>
        PROGRESS_STOP = 2,

        /// <summary>
        /// Continue the copy operation, but stop invoking
        /// CopyProgressRoutine to report progress.
        /// </summary>
        PROGRESS_QUIET = 3

    } // enum CopyProgressCode

} // namespace AM.Win32
