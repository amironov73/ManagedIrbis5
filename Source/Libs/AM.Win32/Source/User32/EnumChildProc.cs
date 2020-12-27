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

/* EnumChildProc.cs --
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The EnumChildProc function is an application-defined callback
	/// function used with the EnumChildWindows function. It receives
	/// the child window handles.
	/// </summary>
	///
	/// <param name="hwnd">Handle to a child window of the parent window
	/// specified in EnumChildWindows.</param>
	///
	/// <param name="lParam">Specifies the application-defined value
	/// given in EnumChildWindows.</param>
	///
	/// <returns>To continue enumeration, the callback function must
	/// return TRUE; to stop enumeration, it must return FALSE.</returns>
	public delegate bool EnumChildProc
	    (
		    IntPtr hwnd,
		    IntPtr lParam
	    );

} // namespace AM.Win32
