// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* HttpExceptionArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Web;

/// <summary>
/// The event args for an exception.
/// </summary>
public class HttpExceptionArgs
{
    /// <summary>
    /// The builder used to create the request.
    /// </summary>
    public IRequestBuilder Builder { get; }

    /// <summary>
    /// The error that was thrown.
    /// </summary>
    public Exception Error { get; }

    /// <summary>
    /// Whether to abort logging of this exception.
    /// </summary>
    public bool AbortLogging { get; set; }

    internal HttpExceptionArgs(IRequestBuilder builder, Exception ex)
    {
        Builder = builder;
        Error = ex;
    }
}
