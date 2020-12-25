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

/* JobObjectHandle.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Contains handle for <see cref="WindowsJob"/>.
    /// </summary>
    public sealed class JobObjectHandle
        : SafeHandle
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public JobObjectHandle()
            : base(IntPtr.Zero, true)
        {
        }

        #endregion

        #region SafeHandle members

        /// <inheritdoc cref="SafeHandle.IsInvalid" />
        public override bool IsInvalid
        {
            get { return (handle == IntPtr.Zero); }
        }

        /// <inheritdoc cref="SafeHandle.ReleaseHandle" />
        protected override bool ReleaseHandle()
        {
            return Kernel32.CloseHandle(handle);
        }

        #endregion

    } // class JobObjectHandle

} // namespace AM.Win32
