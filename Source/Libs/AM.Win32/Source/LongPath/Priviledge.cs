// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Principal;
using System.Threading;

using Luid = AM.Win32.LongPath.NativeMethods.LUID;
using Win32Exception = System.ComponentModel.Win32Exception;
using PrivilegeNotHeldException = System.Security.AccessControl.PrivilegeNotHeldException;

#endregion

#nullable enable

#pragma warning disable CS1591

namespace AM.Win32.LongPath;

public delegate void PrivilegedCallback (object state);

/// <summary>
/// From MSDN Magazine March 2005
/// </summary>
public sealed class Privilege
{
    private static readonly LocalDataStoreSlot _tlsSlot = Thread.AllocateDataSlot();
    private static readonly HybridDictionary _privileges = new ();
    private static readonly HybridDictionary _luids = new ();
    private static readonly ReaderWriterLock _privilegeLock = new ();

    private bool _needToRevert;
    private bool _initialState;
    private bool _stateWasChanged;
    private readonly Luid _luid;
    private readonly Thread _currentThread = Thread.CurrentThread;
    private TlsContents _tlsContents;

    // ReSharper disable UnusedMember.Global
    public const string CreateToken = "SeCreateTokenPrivilege";
    public const string AssignPrimaryToken = "SeAssignPrimaryTokenPrivilege";
    public const string LockMemory = "SeLockMemoryPrivilege";
    public const string IncreaseQuota = "SeIncreaseQuotaPrivilege";
    public const string UnsolicitedInput = "SeUnsolicitedInputPrivilege";
    public const string MachineAccount = "SeMachineAccountPrivilege";
    public const string TrustedComputingBase = "SeTcbPrivilege";
    public const string Security = "SeSecurityPrivilege";
    public const string TakeOwnership = "SeTakeOwnershipPrivilege";
    public const string LoadDriver = "SeLoadDriverPrivilege";
    public const string SystemProfile = "SeSystemProfilePrivilege";
    public const string SystemTime = "SeSystemtimePrivilege";
    public const string ProfileSingleProcess = "SeProfileSingleProcessPrivilege";
    public const string IncreaseBasePriority = "SeIncreaseBasePriorityPrivilege";
    public const string CreatePageFile = "SeCreatePagefilePrivilege";
    public const string CreatePermanent = "SeCreatePermanentPrivilege";
    public const string Backup = "SeBackupPrivilege";
    public const string Restore = "SeRestorePrivilege";
    public const string Shutdown = "SeShutdownPrivilege";
    public const string Debug = "SeDebugPrivilege";
    public const string Audit = "SeAuditPrivilege";
    public const string SystemEnvironment = "SeSystemEnvironmentPrivilege";
    public const string ChangeNotify = "SeChangeNotifyPrivilege";
    public const string RemoteShutdown = "SeRemoteShutdownPrivilege";
    public const string Undock = "SeUndockPrivilege";
    public const string SyncAgent = "SeSyncAgentPrivilege";
    public const string EnableDelegation = "SeEnableDelegationPrivilege";
    public const string ManageVolume = "SeManageVolumePrivilege";
    public const string Impersonate = "SeImpersonatePrivilege";
    public const string CreateGlobal = "SeCreateGlobalPrivilege";
    public const string TrustedCredentialManagerAccess = "SeTrustedCredManAccessPrivilege";

    public const string ReserveProcessor = "SeReserveProcessorPrivilege";

    // ReSharper restore UnusedMember.Global


    //
    // This routine is a wrapper around a hashtable containing mappings
    // of privilege names to luids
    //

    [ReliabilityContract (Consistency.WillNotCorruptState, Cer.MayFail)]
    private static Luid LuidFromPrivilege (string privilege)
    {
        Luid luid;
        luid.LowPart = 0;
        luid.HighPart = 0;

        //
        // Look up the privilege LUID inside the cache
        //

        RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            _privilegeLock.AcquireReaderLock (Timeout.Infinite);

            if (_luids.Contains (privilege))
            {
                luid = (Luid)_luids[privilege];

                _privilegeLock.ReleaseReaderLock();
            }
            else
            {
                _privilegeLock.ReleaseReaderLock();

                if (false == NativeMethods.LookupPrivilegeValue (null, privilege, ref luid))
                {
                    int error = Marshal.GetLastWin32Error();

                    if (error == NativeMethods.ERROR_NOT_ENOUGH_MEMORY)
                    {
                        throw new OutOfMemoryException();
                    }
                    else if (error == NativeMethods.ERROR_ACCESS_DENIED)
                    {
                        throw new UnauthorizedAccessException (
                            "Caller does not have the rights to look up privilege local unique identifier");
                    }
                    else if (error == NativeMethods.ERROR_NO_SUCH_PRIVILEGE)
                    {
                        throw new ArgumentException (
                            string.Format ("{0} is not a valid privilege name", privilege),
                            "privilege");
                    }
                    else
                    {
                        throw new Win32Exception (error);
                    }
                }

                _privilegeLock.AcquireWriterLock (Timeout.Infinite);
            }
        }
        finally
        {
            if (_privilegeLock.IsReaderLockHeld)
            {
                _privilegeLock.ReleaseReaderLock();
            }

            if (_privilegeLock.IsWriterLockHeld)
            {
                if (!_luids.Contains (privilege))
                {
                    _luids[privilege] = luid;
                    _privileges[luid] = privilege;
                }

                _privilegeLock.ReleaseWriterLock();
            }
        }

        return luid;
    }

    private sealed class TlsContents : IDisposable
    {
        private bool disposed = false;
        private int referenceCount = 1;
        private SafeTokenHandle threadHandle = new SafeTokenHandle (IntPtr.Zero);
        private bool isImpersonating = false;

        private static SafeTokenHandle processHandle = new SafeTokenHandle (IntPtr.Zero);
        private static readonly object syncRoot = new object();

        #region Constructor and finalizer

        public TlsContents()
        {
            int error = 0;
            int cachingError = 0;
            bool success = true;

            if (processHandle.IsInvalid)
            {
                lock (syncRoot)
                {
                    if (processHandle.IsInvalid)
                    {
                        if (false == NativeMethods.OpenProcessToken (
                                NativeMethods.GetCurrentProcess(),
                                TokenAccessLevels.Duplicate,
                                ref processHandle))
                        {
                            cachingError = Marshal.GetLastWin32Error();
                            success = false;
                        }
                    }
                }
            }

            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                //
                // Open the thread token; if there is no thread token,
                // copy the process token onto the thread
                //

                if (false == NativeMethods.OpenThreadToken (
                        NativeMethods.GetCurrentThread(),
                        TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges,
                        true,
                        ref threadHandle))
                {
                    if (success == true)
                    {
                        error = Marshal.GetLastWin32Error();

                        if (error != NativeMethods.ERROR_NO_TOKEN)
                        {
                            success = false;
                        }

                        if (success == true)
                        {
                            error = 0;

                            if (false == NativeMethods.DuplicateTokenEx (
                                    processHandle,
                                    TokenAccessLevels.Impersonate | TokenAccessLevels.Query |
                                    TokenAccessLevels.AdjustPrivileges,
                                    IntPtr.Zero,
                                    NativeMethods.SecurityImpersonationLevel.Impersonation,
                                    NativeMethods.TokenType.Impersonation,
                                    ref threadHandle))
                            {
                                error = Marshal.GetLastWin32Error();
                                success = false;
                            }
                        }

                        if (success == true)
                        {
                            if (false == NativeMethods.SetThreadToken (
                                    IntPtr.Zero,
                                    threadHandle))
                            {
                                error = Marshal.GetLastWin32Error();
                                success = false;
                            }
                        }

                        if (success == true)
                        {
                            //
                            // This thread is now impersonating; it needs to be reverted to its original state
                            //

                            isImpersonating = true;
                        }
                    }
                    else
                    {
                        error = cachingError;
                    }
                }
                else
                {
                    success = true;
                }
            }
            finally
            {
                if (!success)
                {
                    Dispose();
                }
            }

            if (error == NativeMethods.ERROR_NOT_ENOUGH_MEMORY)
            {
                throw new OutOfMemoryException();
            }
            else if (error == NativeMethods.ERROR_ACCESS_DENIED ||
                     error == NativeMethods.ERROR_CANT_OPEN_ANONYMOUS)
            {
                throw new UnauthorizedAccessException ("The caller does not have the rights to perform the operation");
            }
            else if (error != 0)
            {
                throw new Win32Exception (error);
            }
        }

        ~TlsContents()
        {
            if (!disposed)
            {
                Dispose (false);
            }
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        private void Dispose (bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (threadHandle != null)
            {
                threadHandle.Dispose();
                threadHandle = null;
            }

            if (isImpersonating)
            {
                NativeMethods.RevertToSelf();
            }

            disposed = true;
        }

        #endregion

        #region Reference-counting

        public void IncrementReferenceCount()
        {
            referenceCount++;
        }

        public int DecrementReferenceCount()
        {
            int result = --referenceCount;

            if (result == 0)
            {
                Dispose();
            }

            return result;
        }

        public int ReferenceCountValue
        {
            get { return referenceCount; }
        }

        #endregion

        #region Properties

        public SafeTokenHandle ThreadHandle
        {
            get { return threadHandle; }
        }

        public bool IsImpersonating
        {
            get { return isImpersonating; }
        }

        #endregion
    }

    public Privilege (string privilegeName)
    {
        if (privilegeName == null)
        {
            throw new ArgumentNullException ("privilegeName");
        }

        _luid = LuidFromPrivilege (privilegeName);
    }

    public void Enable()
    {
        ToggleState (true);
    }

#if NOT_USED
        [ReliabilityContract( Consistency.WillNotCorruptState, Cer.MayFail )]
        public void Disable()
        {
            this.ToggleState( false );
        }
#endif

    public void Revert()
    {
        int error = 0;

        //
        // All privilege operations must take place on the same thread
        //

        if (!_currentThread.Equals (Thread.CurrentThread))
        {
            throw new InvalidOperationException ("Operation must take place on the thread that created the object");
        }

        if (!NeedToRevert)
        {
            return;
        }

        //
        // This code must be eagerly prepared and non-interruptible.
        //

        RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            //
            // The payload is entirely in the finally block
            // This is how we ensure that the code will not be
            // interrupted by catastrophic exceptions
            //
        }
        finally
        {
            bool success = true;

            try
            {
                //
                // Only call AdjustTokenPrivileges if we're not going to be reverting to self,
                // on this Revert, since doing the latter obliterates the thread token anyway
                //

                if (_stateWasChanged &&
                    (_tlsContents.ReferenceCountValue > 1 ||
                     !_tlsContents.IsImpersonating))
                {
                    var newState = new NativeMethods.TOKEN_PRIVILEGE
                    {
                        PrivilegeCount = 1,
                        Privilege =
                        {
                            Luid = _luid,
                            Attributes =
                                (_initialState
                                    ? NativeMethods.SE_PRIVILEGE_ENABLED
                                    : NativeMethods.SE_PRIVILEGE_DISABLED)
                        }
                    };

                    NativeMethods.TOKEN_PRIVILEGE previousState = new NativeMethods.TOKEN_PRIVILEGE();
                    uint previousSize = 0;

                    if (false == NativeMethods.AdjustTokenPrivileges (
                            _tlsContents.ThreadHandle,
                            false,
                            ref newState,
                            (uint)Marshal.SizeOf (previousState),
                            ref previousState,
                            ref previousSize))
                    {
                        error = Marshal.GetLastWin32Error();
                        success = false;
                    }
                }
            }
            finally
            {
                if (success)
                {
                    Reset();
                }
            }
        }

        if (error == NativeMethods.ERROR_NOT_ENOUGH_MEMORY)
        {
            throw new OutOfMemoryException();
        }
        else if (error == NativeMethods.ERROR_ACCESS_DENIED)
        {
            throw new UnauthorizedAccessException ("Caller does not have the permission to change the privilege");
        }
        else if (error != 0)
        {
            throw new Win32Exception (error);
        }
    }

    public bool NeedToRevert
    {
        get { return _needToRevert; }
    }

#if NOT_USED
        public static void RunWithPrivilege( string privilege, bool enabled, PrivilegedCallback callback, object state )
        {
            if ( callback == null )
            {
                throw new ArgumentNullException( "callback" );
            }

            Privilege p = new Privilege( privilege );

            RuntimeHelpers.PrepareConstrainedRegions();

            try
            {
                if (enabled)
                {
                    p.Enable();
                }
                else
                {
                    p.Disable();
                }

                callback(state);
            }
            catch
            {
                p.Revert();
                throw;
            }
            finally
            {
                p.Revert();
            }
        }
#endif

    private void ToggleState (bool enable)
    {
        int error = 0;

        //
        // All privilege operations must take place on the same thread
        //

        if (!_currentThread.Equals (Thread.CurrentThread))
        {
            throw new InvalidOperationException ("Operation must take place on the thread that created the object");
        }

        //
        // This privilege was already altered and needs to be reverted before it can be altered again
        //

        if (NeedToRevert)
        {
            throw new InvalidOperationException ("Must revert the privilege prior to attempting this operation");
        }

        //
        // Need to make this block of code non-interruptible so that it would preserve
        // consistency of thread oken state even in the face of catastrophic exceptions
        //

        try
        {
            //
            // The payload is entirely in the finally block
            // This is how we ensure that the code will not be
            // interrupted by catastrophic exceptions
            //
        }
        finally
        {
            try
            {
                //
                // Retrieve TLS state
                //

                _tlsContents = Thread.GetData (_tlsSlot) as TlsContents;

                if (_tlsContents == null)
                {
                    _tlsContents = new TlsContents();
                    Thread.SetData (_tlsSlot, _tlsContents);
                }
                else
                {
                    _tlsContents.IncrementReferenceCount();
                }

                var newState = new NativeMethods.TOKEN_PRIVILEGE
                {
                    PrivilegeCount = 1,
                    Privilege =
                    {
                        Luid = _luid,
                        Attributes = enable ? NativeMethods.SE_PRIVILEGE_ENABLED : NativeMethods.SE_PRIVILEGE_DISABLED
                    }
                };

                NativeMethods.TOKEN_PRIVILEGE previousState = new NativeMethods.TOKEN_PRIVILEGE();
                uint previousSize = 0;

                //
                // Place the new privilege on the thread token and remember the previous state.
                //

                if (false == NativeMethods.AdjustTokenPrivileges (
                        _tlsContents.ThreadHandle,
                        false,
                        ref newState,
                        (uint)Marshal.SizeOf (previousState),
                        ref previousState,
                        ref previousSize))
                {
                    error = Marshal.GetLastWin32Error();
                }
                else if (NativeMethods.ERROR_NOT_ALL_ASSIGNED == Marshal.GetLastWin32Error())
                {
                    error = NativeMethods.ERROR_NOT_ALL_ASSIGNED;
                }
                else
                {
                    //
                    // This is the initial state that revert will have to go back to
                    //

                    _initialState =
                        ((previousState.Privilege.Attributes & NativeMethods.SE_PRIVILEGE_ENABLED) != 0);

                    //
                    // Remember whether state has changed at all
                    //

                    _stateWasChanged = (_initialState != enable);

                    //
                    // If we had to impersonate, or if the privilege state changed we'll need to revert
                    //

                    _needToRevert = _tlsContents.IsImpersonating || _stateWasChanged;
                }
            }
            finally
            {
                if (!_needToRevert)
                {
                    Reset();
                }
            }
        }

        if (error == NativeMethods.ERROR_NOT_ALL_ASSIGNED)
        {
            throw new PrivilegeNotHeldException (_privileges[_luid] as string);
        }

        if (error == NativeMethods.ERROR_NOT_ENOUGH_MEMORY)
        {
            throw new OutOfMemoryException();
        }
        else if (error == NativeMethods.ERROR_ACCESS_DENIED ||
                 error == NativeMethods.ERROR_CANT_OPEN_ANONYMOUS)
        {
            throw new UnauthorizedAccessException ("The caller does not have the right to change the privilege");
        }
        else if (error != 0)
        {
            throw new Win32Exception (error);
        }
    }

    private void Reset()
    {
        try
        {
            // Payload is in the finally block
            // as a way to guarantee execution
        }
        finally
        {
            _stateWasChanged = false;
            _initialState = false;
            _needToRevert = false;

            if (_tlsContents != null)
            {
                if (0 == _tlsContents.DecrementReferenceCount())
                {
                    _tlsContents = null;
                    Thread.SetData (_tlsSlot, null);
                }
            }
        }
    }
}
