// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CredentialException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Permissions;

using AM.Windows.Forms.Dialogs.Properties;

#endregion

#pragma warning disable SYSLIB0003

#nullable enable

namespace AM.Windows.Forms.Dialogs;

/// <summary>
/// The exception that is thrown when an error occurs getting credentials.
/// </summary>
/// <threadsafety instance="false" static="true" />
[Serializable]
public class CredentialException
    : Win32Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CredentialException" /> class.
    /// </summary>
    [SecurityPermission (SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public CredentialException()
        : base (Resources.CredentialError)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CredentialException" /> class with the specified error.
    /// </summary>
    /// <param name="error">The Win32 error code associated with this exception.</param>
    [SecurityPermission (SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public CredentialException (int error)
        : base (error)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CredentialException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    [SecurityPermission (SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public CredentialException (string message)
        : base (message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CredentialException" /> class with the specified error and the specified detailed description.
    /// </summary>
    /// <param name="error">The Win32 error code associated with this exception.</param>
    /// <param name="message">A detailed description of the error.</param>
    [SecurityPermission (SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public CredentialException (int error, string message)
        : base (error, message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CredentialException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">A reference to the inner exception that is the cause of the current exception.</param>
    [SecurityPermission (SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public CredentialException (string message, Exception innerException)
        : base (message, innerException)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CredentialException" /> class with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
    [SecurityPermission (SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    protected CredentialException (SerializationInfo info, StreamingContext context)
        : base (info, context)
    {
        // пустое тело конструктора
    }
}
