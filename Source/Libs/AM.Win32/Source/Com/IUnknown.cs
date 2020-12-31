﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

/* IUnknown.cs -- most fundamental COM interface
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Interface to get pointers to other interfaces on a given object, using
	/// the QueryInterface method.
	/// DO NOT CHANGE ORDER OF THESE FUNCTIONS!!!!
	/// </summary>
	[ComImport]
	[Guid ( "00000000-0000-0000-c000-000000000046" )]
	[InterfaceType ( ComInterfaceType.InterfaceIsIUnknown )]
	public interface IUnknown
	{
		/// <summary>
		/// Get a pointer to a specific interface on an object
		/// </summary>
		/// <param name="riid">GUID for the interface whose interface pointer
		/// we are seeking.</param>
		/// <param name="pVoid">Pointer where the desired interface pointer
		/// will be saved.</param>
		/// <returns>S_OK if interface is supported; E_NOINTERFACE if not.
		/// </returns>
		[PreserveSig]
		IntPtr QueryInterface
			(
				[MarshalAs ( UnmanagedType.LPStruct )]
				Guid riid,
				out IntPtr pVoid
			);

		/// <summary>
		/// Increments the reference counter to this object.
		/// </summary>
		/// <returns>New value of the reference counter.</returns>
		[PreserveSig]
		IntPtr AddRef ();

		/// <summary>
		/// Decrements the reference counter for an object.
		/// </summary>
		/// <returns>New value of the reference counter.</returns>
		[PreserveSig]
		IntPtr Release ();

	} // interface IUnknown

} // namespace AM.Win32
