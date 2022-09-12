// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Pdf.IO;

/// <summary>
/// Determines the type of the password.
/// </summary>
public enum PasswordValidity
{
    /// <summary>
    /// Password is neither user nor owner password.
    /// </summary>
    Invalid,

    /// <summary>
    /// Password is user password.
    /// </summary>
    UserPassword,

    /// <summary>
    /// Password is owner password.
    /// </summary>
    OwnerPassword
}
