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

/* EnumWindowsProc.cs --
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The EnumWindowsProc function is an application-defined callback
	/// function used with the EnumWindows or EnumDesktopWindows function.
	/// It receives top-level window handles.
	/// </summary>
	///
	/// <param name="hwnd">Handle to a top-level window.</param>
	///
	/// <param name="lParam">Specifies the application-defined value given
	/// in EnumWindows or EnumDesktopWindows.</param>
	///
	/// <returns>To continue enumeration, the callback function must
	/// return TRUE; to stop enumeration, it must return FALSE.</returns>
	public delegate bool EnumWindowsProc
	    (
		    IntPtr hwnd,
		    IntPtr lParam
	    );

} // namespace AM.Win32
