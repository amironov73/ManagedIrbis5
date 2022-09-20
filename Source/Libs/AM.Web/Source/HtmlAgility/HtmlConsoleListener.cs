// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

using System;
using System.Diagnostics;

namespace HtmlAgilityPack;

internal class HtmlConsoleListener
    : TraceListener
{
    #region Public Methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    public override void Write
        (
            string? message
        )
    {
        if (!string.IsNullOrEmpty (message))
        {
            Write (message, "");
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="category"></param>
    public override void Write
        (
            string? message,
            string? category
        )
    {
        Console.Write ("T:" + category + ": " + message);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    public override void WriteLine
        (
            string? message
        )
    {
        Write (message + "\n");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="category"></param>
    public override void WriteLine
        (
            string? message,
            string? category
        )
    {
        Write (message + "\n", category);
    }

    #endregion
}
