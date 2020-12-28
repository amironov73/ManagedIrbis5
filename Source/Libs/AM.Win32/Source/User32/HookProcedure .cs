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

/* HookProcedure.cs -- callback function used with the SetWindowsHookEx
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// The HookProcedure hook procedure is an application-defined
    /// or library-defined callback function used with the
    /// SetWindowsHookEx function.
    /// </summary>
    public delegate int HookProcedure
        (
            int code,
            int wParam,
            int lParam
        );

} // namespace AM.Win32
