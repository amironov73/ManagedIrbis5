// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubContainerException.cs --
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
public class EpubContainerException
    : EpubSchemaException
{
    /// <summary>
    ///
    /// </summary>
    public EpubContainerException()
        : base (EpubSchemaFileType.META_INF_CONTAINER)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    public EpubContainerException
        (
            string message
        )
        : base (message, EpubSchemaFileType.META_INF_CONTAINER)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public EpubContainerException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException, EpubSchemaFileType.META_INF_CONTAINER)
    {
        // пустое тело конструктора
    }
}
