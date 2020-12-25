// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* StdHandles.cs -- standard handles
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// Standard handles.
    /// </summary>
    public enum StdHandles
        : uint
    {
        /// <summary>
        /// Handle to the standard input device.
        /// Initially, this is a handle to the console input buffer,
        /// CONIN$.
        /// </summary>
        STD_INPUT_HANDLE = unchecked((uint)-10),

        /// <summary>
        /// Handle to the standard output device.
        /// Initially, this is a handle to the console screen
        /// buffer, CONOUT$.
        /// </summary>
        STD_OUTPUT_HANDLE = unchecked((uint)-11),

        /// <summary>
        /// Handle to the standard error device.
        /// Initially, this is a handle to the console screen buffer,
        /// CONOUT$.
        /// </summary>
        STD_ERROR_HANDLE = unchecked((uint)-12)

    } // enum StdHandles

} // namespace AM.Win32
