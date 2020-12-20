// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* HotkeyModifiers.cs -- модификаторы для горячих клавиш
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Win32
{
    /// <summary>
    /// Модификаторы для горячих клавиш.
    /// </summary>
	[Flags]
	public enum HotkeyModifiers
	{
        /// <summary>
        /// None.
        /// </summary>
		None = 0,

        /// <summary>
        /// Alt.
        /// </summary>
		Alt = 1,

        /// <summary>
        /// Control.
        /// </summary>
		Control = 2,

        /// <summary>
        /// Shift.
        /// </summary>
		Shift = 4,

        /// <summary>
        /// Win.
        /// </summary>
		Win = 8

	} // enum HotkeyModifiers

} // namespace AM.Win32
