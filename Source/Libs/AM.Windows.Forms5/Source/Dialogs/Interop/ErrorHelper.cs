// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ErrorHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms.Dialogs.Interop;

/// <summary>
///
/// </summary>
internal enum HRESULT
    : long
{
    /// <summary>
    ///
    /// </summary>
    S_FALSE = 0x0001,

    /// <summary>
    ///
    /// </summary>
    S_OK = 0x0000,

    /// <summary>
    ///
    /// </summary>
    E_INVALIDARG = 0x80070057,

    /// <summary>
    ///
    /// </summary>
    E_OUTOFMEMORY = 0x8007000E,

    /// <summary>
    ///
    /// </summary>
    ERROR_CANCELLED = 0x800704C7
}
