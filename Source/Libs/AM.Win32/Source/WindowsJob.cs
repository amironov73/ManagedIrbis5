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

/* WindowsJob.cs -- Win32 job
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Win32 job.
    /// </summary>
    /// <remarks>Borrowed from Tom DuPont:
    /// http://www.tomdupont.net/2016/02/how-to-kill-child-process-when-parent.html
    /// </remarks>
    public sealed class WindowsJob
        : IDisposable
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WindowsJob()
        {
            _handle = Kernel32.CreateJobObject(IntPtr.Zero, null);

            var info = new JobObjectBasicLimitInformation
            {
                LimitFlags = 0x2000
            };

            var extendedInfo = new JobObjectExtendedLimitInformation
            {
                BasicLimitInformation = info
            };

            var infoType = typeof(JobObjectExtendedLimitInformation);
            var length = Marshal.SizeOf(infoType);
            var extendedInfoPtr = IntPtr.Zero;

            try
            {
                extendedInfoPtr = Marshal.AllocHGlobal(length);

                Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

                var setResult = Kernel32.SetInformationJobObject
                (
                    _handle,
                    JobObjectInfoType.ExtendedLimitInformation,
                    extendedInfoPtr,
                    (uint)length
                );

                if (setResult)
                {
                    return;
                }
            }
            finally
            {
                if (extendedInfoPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(extendedInfoPtr);
                }
            }

            var lastError = Marshal.GetLastWin32Error();
            var message = "Unable to set information. Error: " + lastError;
            throw new Exception(message);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~WindowsJob()
        {
            Dispose(false);
        }

        #endregion

        #region Private members

        private readonly JobObjectHandle _handle;

        private bool _disposed;

        #endregion

        #region Public methods

        /// <summary>
        /// Add the process to the job.
        /// </summary>
        public bool AddProcess
            (
                IntPtr processHandle
            )
        {
            return Kernel32.AssignProcessToJobObject
                (
                    _handle,
                    processHandle
                );
        }

        /// <summary>
        /// Add the process by id.
        /// </summary>
        public bool AddProcess
            (
                int processId
            )
        {
            var process = Process.GetProcessById(processId);
            return AddProcess(process.Handle);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose
            (
                bool disposing
            )
        {
            if (_disposed)
            {
                return;
            }

            if (_handle != null && !_handle.IsInvalid)
            {
                _handle.Dispose();
            }

            _disposed = true;
        }

        #endregion

    } // class WindowsJob

} // namespace AM.Win32
