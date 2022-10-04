// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* LanguageException.cs -- специфичное для языка исключение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace SimplestLanguage;

/// <summary>
/// Специфичное для языка исключение.
/// </summary>
public class LanguageException
    : ApplicationException
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LanguageException()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected LanguageException
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LanguageException
        (
            string? message
        )
        : base (message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LanguageException
        (
            string? message,
            Exception? innerException
        )
        : base (message, innerException)
    {
        // пустое тело конструктора
    }

    #endregion
}
