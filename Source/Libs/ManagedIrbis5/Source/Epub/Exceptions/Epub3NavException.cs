// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Epub3NavException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public sealed class Epub3NavException
    : EpubSchemaException
{
    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Epub3NavException()
        : base (EpubSchemaFileType.EPUB3_NAV_DOCUMENT)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="message"></param>
    public Epub3NavException (string message)
        : base (message, EpubSchemaFileType.EPUB3_NAV_DOCUMENT)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public Epub3NavException (string message, Exception innerException)
        : base (message, innerException, EpubSchemaFileType.EPUB3_NAV_DOCUMENT)
    {
        // пустое тело конструктора
    }
}
