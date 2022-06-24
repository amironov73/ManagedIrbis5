// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlLinkClickedException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Entities;

/// <summary>
/// Exception thrown when client code subscribed to LinkClicked event thrown exception.
/// </summary>
public sealed class HtmlLinkClickedException 
    : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
    /// </summary>
    public HtmlLinkClickedException()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error. </param>
    public HtmlLinkClickedException(string message)
        : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception. </param><param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
    public HtmlLinkClickedException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
